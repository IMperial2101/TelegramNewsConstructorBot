using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace NewsPropertyBot.TelegramBotClass
{
    public class TelegramBot
    {
        string botToken;
        int maxParagraphCount;
        int maxDescripSymbCount;
        string channelID;
        string smile;
        TelegramBotClient botClient;
        public TelegramBot(MyProperties properties)
        {
            botToken = properties.botToken;
            channelID = properties.channelID;
            maxDescripSymbCount = properties.maxDescripSymbCount;
            maxParagraphCount = properties.maxParagraphCount;           
            smile = properties.smile;
            botClient = new TelegramBotClient(botToken);
        }
        public async Task SendMessageToChannel(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(channelID, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }
        public async Task SendMyNewToChannelAsync(MyNew myNew)
        {

            try
            {
                if (!string.IsNullOrEmpty(myNew.photoUrl))
                {
                    // Send the photo first
                    await botClient.SendPhotoAsync(channelID, new InputOnlineFile(myNew.photoUrl), caption: $"*{myNew.title}*_{myNew.secondTitle}_\n{MakeDescription(myNew)}[Читать полность]({myNew.url})🔗", parseMode: ParseMode.Markdown);
                }
                else
                {
                    // If no photo, send text only
                    string message = $"*{myNew.title}*\n{myNew.secondTitle}";

                    message += MakeDescription(myNew);
                    

                    await botClient.SendTextMessageAsync(channelID, message, ParseMode.Markdown);
                }

                Console.WriteLine($"Успешно отправили новость в канал");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }
        private string MakeDescription(MyNew myNew)
        {
            string description = "";
            if (myNew.description.Count != 0)
                description += $"\n{smile}";
            if (myNew.description.Count == 1)
            {
                maxParagraphCount = 1;
            }
            else if (myNew.description.Count < maxParagraphCount)
                maxParagraphCount = myNew.description.Count - 1;

            for (int i = 0; i < maxParagraphCount; i++)
            {
                description += $"{myNew.description[i]}\n\n";
            }
            if(description.Length > maxDescripSymbCount)
            {
                description = description.Substring(0, maxDescripSymbCount) + "...\n";
            }
            return description;
        }

    }


}
