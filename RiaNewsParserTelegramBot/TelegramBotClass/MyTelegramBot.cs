using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace NewsPropertyBot.TelegramBotClass
{
    public class MyTelegramBot
    {
        static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        public TelegramBotClient botClient;
        string ownerMessage;
        public MyTelegramBot()
        {
            botClient = new TelegramBotClient(MyPropertiesStatic.botToken);
            ownerMessage = $"Date: {DateTime.Now}\n" +
                           $"Exception: ";
            
        }
        public async Task SendMessageToOwner(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(MyPropertiesStatic.ownerId,$"{ownerMessage} {message}", parseMode: ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }

    }


}
