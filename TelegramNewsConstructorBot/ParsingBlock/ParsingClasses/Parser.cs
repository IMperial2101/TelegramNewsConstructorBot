using HtmlAgilityPack;
using NewsPropertyBot.NewClass;
using System.Web;
using NewsPropertyBot.TelegramBotClass;
using NewsPropertyBot.XpathClass;
using RiaNewsParserTelegramBot.PropertiesClass;


namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        Dictionary<string, bool> currLinksForSendInChannel = new Dictionary<string, bool>();
        Dictionary<string, int> mainPageLinksWithViewsDict;
        XPathStrings xPathStrings = new XPathStrings();
        HtmlDocument htmlDocumentMainPage = new HtmlDocument();        
        HttpClient httpClient;
        MyTelegramBot telegramBot;

        string parseLink;
        
        public Parser(MyTelegramBot telegramBot)
        {
            this.telegramBot = telegramBot;
            parseLink = MyPropertiesStatic.parseLink;
            HttpClientHandler httpHandler = new HttpClientHandler();
            if(MyPropertiesStatic.useProxy == "yes")
            {
                httpHandler.Proxy = MyPropertiesStatic.myProxy.GetWebProxy();
                httpHandler.UseDefaultCredentials = false;
            }
            httpClient = new HttpClient(httpHandler);

            httpClient.MaxResponseContentBufferSize = int.MaxValue;
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
        }
        public async Task<List<MyNew>> ParseNews()
        {
            await DownloadPageAsync(parseLink, htmlDocumentMainPage);
            mainPageLinksWithViewsDict = ParseNewLinks();
            switch (mainPageLinksWithViewsDict != null)
            {
                case true:
                    {
                        Console.WriteLine("Успешно скачали главную страницу");
                        RemoveNoActualLinks(mainPageLinksWithViewsDict,currLinksForSendInChannel);
                        AddNewLinksToSend(mainPageLinksWithViewsDict, currLinksForSendInChannel);
                        List<MyNew> newsList = await ParseAllNewsAsync();
                        return newsList;//явный вывод
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Не удалость скачать страницу {parseLink}");
                        await telegramBot.SendMessageToOwner($"Не удалость скачать страницу [{parseLink}]({parseLink})\n");
                        return null;
                        break;
                    }
            }

        }
        public Dictionary<string, int> ParseNewLinks()
        {
            Dictionary<string, int> mainPageLinksWithViewsDict = new Dictionary<string, int>();
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
                    telegramBot.SendMessageToOwner($"HtmlNodeCollectionIsNull [{parseLink}]({parseLink})\n");
                    return null;
                }
            }
            catch (Exception ex)
            {
                telegramBot.SendMessageToOwner($"Error parsing new elements: {ex.Message}");
                Console.WriteLine($"Error parsing new elements: {ex.Message}");
            }
            return mainPageLinksWithViewsDict;
        }
        public async Task<List<MyNew>> ParseAllNewsAsync()
        {
            var tasks = currLinksForSendInChannel
                .Where(kv => !kv.Value)
                .Select(kv => ParseOneNewAsync(kv.Key))
                .ToList();

            var results = new List<MyNew>();

            foreach (var task in tasks)
            {
                try
                {
                    var result = await task;
                    if (result != null)
                    {
                        results.Add(result);
                        Console.WriteLine($"Успешно отпарсили ссылку \"{result.title.Substring(0,20)}...");
                    }
                }
                catch (Exception ex)
                {
                    telegramBot.SendMessageToOwner($"Ошибка при выполнении асинхронной задачи парсинга: {ex.Message}");
                    Console.WriteLine($"Ошибка при выполнении асинхронной задачи парсинга: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(2));
            }

            return results;
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
                else
                {
                    name = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.titleH1);
                    if (name != null)
                        myNew.title = HttpUtility.HtmlDecode(name.InnerText.Trim());
                    else
                    {
                        telegramBot.SendMessageToOwner($"Ошибка: Не удалось найти узел для заголовка.[url]({url})");
                        Console.WriteLine("Ошибка: Не удалось найти узел для заголовка.");
                        return null;
                    }
                }
                    HtmlNode afterName = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.secondTitle);
                if (afterName != null)               
                    myNew.secondTitle = HttpUtility.HtmlDecode(afterName.InnerText.Trim());               
                else{
                    telegramBot.SendMessageToOwner($"Ошибка: Не удалось найти узел для описания после заголовка.- [url]({url})");
                    Console.WriteLine($"Ошибка: Не удалось найти узел для описания после заголовка.- {url}");
                }

                HtmlNode photo = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.photoNew);
                if (photo != null)                
                    myNew.photoUrl = photo.GetAttributeValue("src", "");               
                else{
                    telegramBot.SendMessageToOwner($"Ошибка: Не удалось найти узел для изображения.- [url]({url})");
                    Console.WriteLine($"Ошибка: Не удалось найти узел для изображения.- {url}");
                }

                HtmlNodeCollection descriptionAbzatc = htmlDocumentNew.DocumentNode.SelectNodes(xPathStrings.descriptionText);
                if (descriptionAbzatc != null){
                    foreach (HtmlNode abzatc in descriptionAbzatc.ToList())                   
                        myNew.description.Add(HttpUtility.HtmlDecode(abzatc.InnerText));
                    myNew.description[0] = makeDescriptionOnPhoto(myNew.description[0]);
                }
                else{
                    telegramBot.SendMessageToOwner($"Ошибка: Не удалось найти узлы для описания.- [url]({url})");
                    Console.WriteLine($"Ошибка: Не удалось найти узлы для описания.- {url}");
                }
                
                return myNew;
                
            }
            catch (Exception ex)
            {
                await telegramBot.SendMessageToOwner($"Ошибка: {ex.Message} - {url}");
                Console.WriteLine($"Ошибка: {ex.Message} - {url}");
                return null;
            }
        }
        public async Task FirstParseAddLinks()
        {
            await DownloadPageAsync(parseLink, htmlDocumentMainPage);
            mainPageLinksWithViewsDict = ParseNewLinks();
            switch (mainPageLinksWithViewsDict != null)
            {
                case true:
                    {
                        Console.WriteLine("Успешно скачали главную страницу");
                        AddNewLinksToSend(mainPageLinksWithViewsDict,currLinksForSendInChannel);
                        foreach (var key in currLinksForSendInChannel.Keys.ToList())
                            currLinksForSendInChannel[key] = true;

                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Не удалость скачать страницу {parseLink}");
                        await telegramBot.SendMessageToOwner($"Не удалость скачать страницу [{parseLink}]({parseLink})\n");

                        break;
                    }
            }
        }
        private string makeDescriptionOnPhoto(string description)
        {
            int indexOfDot = description.IndexOf(". ");

            if (indexOfDot != -1)
                return description.Substring(indexOfDot + 2); // +2 для включения пробела после точки
            else
                return string.Empty;
        }

    }
}
