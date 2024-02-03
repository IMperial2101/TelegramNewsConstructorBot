using HtmlAgilityPack;
using RiaNewsParserTelegramBot.PropertiesClass;

namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        private Dictionary<string, bool> AddNewLinksToSend(Dictionary<string, int> mainPageLinksWithViewsDict, Dictionary<string, bool> currLinksForSendInChannel)
        {
            try
            {
                foreach (var myNew in mainPageLinksWithViewsDict)
                {

                    if (!currLinksForSendInChannel.ContainsKey(myNew.Key) && myNew.Value > MyPropertiesStatic.minViewCount)
                    {
                        currLinksForSendInChannel.Add(myNew.Key, false);
                    }
                }
                return currLinksForSendInChannel;
            }
            catch (Exception ex)
            {
                telegramBot.SendMessageToOwner($"Ошибка в методе AddNewLinksToSend(): {ex.Message}");
                return null;
            }
        }
        private Dictionary<string, bool> RemoveNoActualLinks(Dictionary<string, int> mainPageLinksWithViewsDict, Dictionary<string, bool> currLinksForSendInChannel)
        {
            try
            {
                foreach (var link in currLinksForSendInChannel)
                {
                    if (!mainPageLinksWithViewsDict.ContainsKey(link.Key) && link.Value == true)
                    {
                        currLinksForSendInChannel.Remove(link.Key);
                    }
                }
                return currLinksForSendInChannel;
            }
            catch (Exception ex)
            {
                telegramBot.SendMessageToOwner($"Ошибка в методе RemoveNoActualLinks(): {ex.Message}");
                return null;
            }
        }
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
