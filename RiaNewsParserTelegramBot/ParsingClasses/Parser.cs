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
using AngleSharp.Dom;

namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        Dictionary<string, bool> currLinksForSendInChannel = new Dictionary<string, bool>();
        Dictionary<string, int> mainPageLinksWithViewsDict = new Dictionary<string, int>();
        List<string> linksToParse = new List<string>();
        List<string> mainPageLinks = new List<string>();
        XPathStrings xPathStrings = new XPathStrings();
        HtmlDocument htmlDocumentMainPage = new HtmlDocument();
        string lastLink = "";
        
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
            await DownloadPageAsync(urlToParse, htmlDocumentMainPage);
            string parseNewLinksResponse = ParseNewLinks();
            switch (parseNewLinksResponse)
            {
                case "NoNewLinksFound":
                    {
                        break;
                    }
                case "Succes":
                    {
                        RemoveNoActualLinks();
                        AddNewLinksToSend();
                        var newsList = await ParseNewsAsync();
                        await SendNewsToChannelAsync(newsList);
                        break;
                    }
            }

        }
        public async Task DownloadPageAsync(string pageUrl,HtmlDocument htmlDocument)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageUrl))
                    throw new ArgumentException("Page URL cannot be empty or null.", nameof(pageUrl));
            }
            catch(ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                htmlDocument = null;
                return;
            }
            try
            {
                HttpResponseMessage response;
                response = await httpClient.GetAsync(pageUrl);
                if (response.IsSuccessStatusCode != true)
                {
                    htmlDocument = null;
                    return;
                }
                htmlDocument.LoadHtml(await response.Content.ReadAsStringAsync());
                if(htmlDocument.DocumentNode.SelectSingleNode("//html") == null)
                {
                    htmlDocument = null;
                    return;
                }
                return;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            htmlDocument = null;
            return;
        }
        public string ParseNewLinks()
        {
            try
            {               
                HtmlNodeCollection newNewsNodes = htmlDocumentMainPage.DocumentNode.SelectNodes(xPathStrings.mainLinksDivContainer);
                string firstNewLink = newNewsNodes[0].SelectSingleNode(".//a[contains(@class,'list-item__title')]").GetAttributeValue("href", "");
                if (lastLink == firstNewLink)
                {
                    return "NoNewLinksFound";
                }
                mainPageLinksWithViewsDict.Clear();
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
                    lastLink = firstNewLink;
                }
                else{
                    return "HtmlNodeCollectionIsNull";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing new elements: {ex.Message}");
            }
            return "Succes";
        }
        public async Task ParseAndSendNewsAsyncT()
        {
            var tasks = currLinksForSendInChannel
    .Where(kv => !kv.Value) 
    .Select(kv => ParseOneNewAsync(kv.Key)) 
    .ToList(); 

            try
            {
                var results = await Task.WhenAll(tasks);

                foreach (var myNew in results)
                {
                    if (myNew != null)
                    {
                        await telegramBot.SendMyNewToChannelAsync(myNew);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении асинхронных задач: {ex.Message}");
                return;
            }
        }
        public async Task<List<MyNew>> ParseNewsAsync()
        {
            var tasks = currLinksForSendInChannel
                .Where(kv => !kv.Value)
                .Select(kv => ParseOneNewAsync(kv.Key))
                .ToList();

            try
            {
                var results = await Task.WhenAll(tasks);
                return results.Where(myNew => myNew != null).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении асинхронных задач парсинга: {ex.Message}");
                return new List<MyNew>();
            }
        }
        public async Task SendNewsToChannelAsync(List<MyNew> newsList)
        {
            foreach (var myNew in newsList)
            {
                try
                {
                    await telegramBot.SendMyNewToChannelAsync(myNew);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке новости в канал: {ex.Message}");
                }
            }
        }
        public async Task<MyNew> ParseOneNewAsync(string url)
        {
            currLinksForSendInChannel[url] = true;
            HtmlDocument htmlDocumentNew = new HtmlDocument();
            try
            {
                await DownloadPageAsync(url, htmlDocumentNew);

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
        private void AddNewLinksToSend()
        {  
            foreach(var myNew in mainPageLinksWithViewsDict)
            {

                if(!currLinksForSendInChannel.ContainsKey(myNew.Key) && myNew.Value > minViewCount)
                {
                    currLinksForSendInChannel.Add(myNew.Key,false);
                }
            }
        }
        private void RemoveNoActualLinks()
        {
            foreach (var link in currLinksForSendInChannel)
            {
                if (!mainPageLinksWithViewsDict.ContainsKey(link.Key) && link.Value == true)
                {
                    currLinksForSendInChannel.Remove(link.Key);
                }
            }
        }

    }
}
