using NewsPropertyBot.NewClass;
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
        static string token = "6904948865:AAFIPhq_bhiLPVoQZqrwLHHxbixXxKwtIT8";
        static int maxAbzatcCount = 2;
        static int maxDescriptionSymbCount = 100;
        static string channelId = "@MoscowPropetyNews";
        static TelegramBotClient botClient = new TelegramBotClient(token);
        public async Task SendMessageToChannel(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(channelId, message);
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
                    await botClient.SendPhotoAsync(channelId, new InputOnlineFile(myNew.photoUrl), caption: $"*{myNew.title}*\n_{myNew.secondTitle}_\n{MakeDescription(myNew)}[Читать полность]({myNew.url})🔗", parseMode: ParseMode.Markdown);
                }
                else
                {
                    // If no photo, send text only
                    string message = $"*{myNew.title}*\n{myNew.secondTitle}";

                    message += MakeDescription(myNew);
                    

                    await botClient.SendTextMessageAsync(channelId, message, ParseMode.Markdown);
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
                description += "\n🌎";
            if (myNew.description.Count == 1)
            {
                maxAbzatcCount = 1;
            }
            else if (myNew.description.Count < maxAbzatcCount)
                maxAbzatcCount = myNew.description.Count - 1;

            for (int i = 0; i < maxAbzatcCount; i++)
            {
                description += $"{myNew.description[i]}\n\n";
            }
            if(description.Length > maxDescriptionSymbCount)
            {
                description = description.Substring(0, maxDescriptionSymbCount) + "...\n";
            }
            return description;
        }

    }


}
