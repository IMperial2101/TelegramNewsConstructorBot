using HtmlAgilityPack;
using RiaNewsParserTelegramBot.PropertiesClass;

namespace NewsPropertyBot.ParsingClasses
{
    public partial class MyParser
    {
       
        public async Task DownloadPageAsync(string pageUrl, HtmlDocument htmlDocument)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageUrl))
                    throw new ArgumentException("Page URL cannot be empty or null.", nameof(pageUrl));
            }
            catch (ArgumentException ex)
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
                if (htmlDocument.DocumentNode.SelectSingleNode("//html") == null)
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

    }
}
