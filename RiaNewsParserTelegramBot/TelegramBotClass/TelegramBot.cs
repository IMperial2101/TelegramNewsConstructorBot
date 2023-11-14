using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NewsPropertyBot.TelegramBotClass
{
    public class TelegramBot
    {
        static string token = "6904948865:AAFIPhq_bhiLPVoQZqrwLHHxbixXxKwtIT8";

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
                string message = $"*{myNew.title}*\n{myNew.secondTitle}";

                if (!string.IsNullOrEmpty(myNew.photoUrl))
                {
                    message += $"\n[Изображение]({myNew.photoUrl})";
                }
                int maxAbzatcCount = 3;
                if(myNew.description.Count < 3)               
                    maxAbzatcCount = myNew.description.Count-1;
                
                for(int i = 0; i < maxAbzatcCount;i++)
                {
                    message += $"\n{myNew.description[i]}\n";
                }

                await botClient.SendTextMessageAsync(channelId, message, ParseMode.Markdown);
                Console.WriteLine($"Успешно отправили новость в канал");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }

    }
    

}
