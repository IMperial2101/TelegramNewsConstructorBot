using HtmlAgilityPack;
using NewsPropertyBot.NewClass;
using NewsPropertyBot.ProxyClass;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NewsPropertyBot.TelegramBotClass;
using static System.Net.WebRequestMethods;
using NewsPropertyBot.XpathClass;

namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        Dictionary<string, bool> currLinksForSendInChannel = new Dictionary<string, bool>();
        Dictionary<string, int> mainPageLinksWithViewsDict = new Dictionary<string, int>();
        List<string> linksToParse = new List<string>();
        HtmlDocument htmlDocumentMainPage = new HtmlDocument();
        List<string> mainPageLinks = new List<string>();
        XPathStrings xPathStrings = new XPathStrings();
        
        HttpClient httpClient;
        TelegramBot telegramBot;
        string lastNewLink = null;
        string urlToParse;
        static int minViewCount = 1000;
        

        public Parser()
        {
            httpClient = new HttpClient();
        }
        public Parser(WebProxy proxy,string url, TelegramBot telegramBot)
        {
            this.telegramBot = telegramBot;
            urlToParse = url;
            HttpClientHandler httpHandler = new HttpClientHandler();
            httpHandler.Proxy = proxy;
            httpHandler.UseDefaultCredentials = false;

            httpClient = new HttpClient(httpHandler);

            httpClient.MaxResponseContentBufferSize = int.MaxValue;
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
        }
        public async Task StartParseNews()
        {
            await ParseNewLinksFuncAsync();
            CheckMainLinksForView();
            await ParseAndSendNewsAsync();

        }
        public async Task ParsePageAsync(string pageUrl, HtmlDocument htmlDocument)
        {
            if (string.IsNullOrWhiteSpace(pageUrl))
            {
                throw new ArgumentException("Page URL cannot be empty or null.", nameof(pageUrl));
            }
            try
            {
                HttpResponseMessage response;
                response = await httpClient.GetAsync(pageUrl);
                if (!response.IsSuccessStatusCode)
                    return;
                htmlDocument.LoadHtml(await response.Content.ReadAsStringAsync());
                Console.WriteLine($"\n\nСкачали успешно страницу - {pageUrl}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
        public async Task ParseNewLinksFuncAsync()
        {
            
            try
            {
                await ParsePageAsync(urlToParse, htmlDocumentMainPage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading page: {ex.Message}");
                return;
            }
            try
            {
                HtmlNodeCollection newNewsNodes = htmlDocumentMainPage.DocumentNode.SelectNodes(xPathStrings.mainLinksDivContainer);

                if (newNewsNodes != null)
                {
                    foreach (var node in newNewsNodes)
                    {
                        try
                        {
                            mainPageLinksWithViewsDict.Add(node.SelectSingleNode(".//a[contains(@class,'list-item__title')]").GetAttributeValue("href", ""), Convert.ToInt32(node.SelectSingleNode(".//div[@class = 'list-item__views-text']").InnerText));
                        }
                        catch
                        {
                            Console.WriteLine("Две одинаковые ссылки на странице");
                        }
                    }
                }
                else{
                    Console.WriteLine("No new news nodes found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки при парсинге новых элементов
                Console.WriteLine($"Error parsing new elements: {ex.Message}");
                return;
            }
        }
        public async Task ParseAndSendNewsAsync()
        {
            var tasks = currLinksForSendInChannel
    .Where(kv => !kv.Value) // Фильтруем по значению
    .Select(kv => ParseOneNewAsync(kv.Key)) // Применяем функцию к каждой отфильтрованной ссылке
    .ToList(); // Преобразуем в список задач

            try
            {
                var results = await Task.WhenAll(tasks);

                foreach (var myNew in results)
                {
                    if (myNew != null)
                    {
                        await telegramBot.SendMyNewToChannelAsync(myNew);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении асинхронных задач: {ex.Message}");
                return;
            }
        }
        public async Task<MyNew> ParseOneNewAsync(string url)
        {
            currLinksForSendInChannel[url] = true;
            HtmlDocument htmlDocumentNew = new HtmlDocument();
            try
            {
                await ParsePageAsync(url, htmlDocumentNew);

                MyNew myNew = new MyNew();
                myNew.url = url;

                HtmlNode name = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.title);
                if (name != null)               
                    myNew.title = HttpUtility.HtmlDecode(name.InnerText.Trim());              
                else{
                    Console.WriteLine("Ошибка: Не удалось найти узел для заголовка.");
                    return null;
                }

                HtmlNode afterName = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.secondTitle);
                if (afterName != null)               
                    myNew.secondTitle = HttpUtility.HtmlDecode(afterName.InnerText.Trim());               
                else{
                    Console.WriteLine("Ошибка: Не удалось найти узел для описания после заголовка.");
                    return null;
                }

                HtmlNode photo = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.photoNew);
                if (photo != null)                
                    myNew.photoUrl = photo.GetAttributeValue("src", "");               
                else{
                    Console.WriteLine("Ошибка: Не удалось найти узел для изображения.");
                    return null;
                }

                HtmlNodeCollection descriptionAbzatc = htmlDocumentNew.DocumentNode.SelectNodes(xPathStrings.descriptionText);
                if (descriptionAbzatc != null){
                    foreach (HtmlNode abzatc in descriptionAbzatc.ToList())                   
                        myNew.description.Add(HttpUtility.HtmlDecode(abzatc.InnerText));                    
                }
                else{
                    Console.WriteLine("Ошибка: Не удалось найти узлы для описания.");
                    return null;
                }
                
                return myNew;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }
        private void CheckMainLinksForView()
        {
            foreach(var link in currLinksForSendInChannel)
            {
                if(!mainPageLinksWithViewsDict.ContainsKey(link.Key) && link.Value == true)
                {
                    currLinksForSendInChannel.Remove(link.Key);
                }
            }
            foreach(var myNew in mainPageLinksWithViewsDict)
            {

                if(!currLinksForSendInChannel.ContainsKey(myNew.Key) && myNew.Value > minViewCount)
                {
                    currLinksForSendInChannel.Add(myNew.Key,false);
                }
            }
        }

    }
}
