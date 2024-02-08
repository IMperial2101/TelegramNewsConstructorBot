using HtmlAgilityPack;
using NewsPropertyBot.NewClass;
using System.Web;
using NewsPropertyBot.TelegramBotClass;
using NewsPropertyBot.XpathClass;
using RiaNewsParserTelegramBot.PropertiesClass;


namespace NewsPropertyBot.ParsingClasses
{
    public partial class MyParser
    {
        Dictionary<string, bool> currLinksForSendInChannel = new Dictionary<string, bool>();
        Dictionary<string, int> mainPageLinksWithViewsDict;
        XPathStrings xPathStrings = new XPathStrings();
        HtmlDocument htmlDocumentMainPage = new HtmlDocument();        
        HttpClient httpClient;
        TelegramBotSendler telegramBot;

        string parseLink;
        
        public MyParser(TelegramBotSendler telegramBot)
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
                        Console.WriteLine("Ошибка: Не удалось найти узел для заголовка.");
                        return null;
                    }
                }
                    HtmlNode afterName = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.secondTitle);
                if (afterName != null)               
                    myNew.secondTitle = HttpUtility.HtmlDecode(afterName.InnerText.Trim());               
                else{
                    Console.WriteLine($"Ошибка: Не удалось найти узел для описания после заголовка.- {url}");
                }

                HtmlNode photo = htmlDocumentNew.DocumentNode.SelectSingleNode(xPathStrings.photoNew);
                if (photo != null)                
                    myNew.photoUrl = photo.GetAttributeValue("src", "");               
                else{
                    Console.WriteLine($"Ошибка: Не удалось найти узел для изображения.- {url}");
                }

                HtmlNodeCollection descriptionAbzatc = htmlDocumentNew.DocumentNode.SelectNodes(xPathStrings.descriptionText);
                if (descriptionAbzatc != null){
                    foreach (HtmlNode abzatc in descriptionAbzatc.ToList())                   
                        myNew.description.Add(HttpUtility.HtmlDecode(abzatc.InnerText));
                    myNew.description[0] = makeDescriptionOnPhoto(myNew.description[0]);
                }
                else{
                    Console.WriteLine($"Ошибка: Не удалось найти узлы для описания.- {url}");
                }
                
                return myNew;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message} - {url}");
                return null;
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
