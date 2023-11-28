using HtmlAgilityPack;
using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        private void AddNewLinksToSend()
        {
            try
            {
                foreach (var myNew in mainPageLinksWithViewsDict)
                {

                    if (!currLinksForSendInChannel.ContainsKey(myNew.Key) && myNew.Value > properties.minViewCount)
                    {
                        currLinksForSendInChannel.Add(myNew.Key, false);
                    }
                }
            }
            catch (Exception ex)
            {
                telegramBot.SendMessageToOwner($"Ошибка в методе AddNewLinksToSend(): {ex.Message}");
            }
        }
        private void RemoveNoActualLinks()
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
            }
            catch (Exception ex)
            {
                telegramBot.SendMessageToOwner($"Ошибка в методе RemoveNoActualLinks(): {ex.Message}");
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
