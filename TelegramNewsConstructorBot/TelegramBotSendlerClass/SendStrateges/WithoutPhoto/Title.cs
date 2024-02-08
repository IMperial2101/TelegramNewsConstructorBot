using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiaNewsParserTelegramBot.TelegramBotClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using Telegram.Bot.Types.Enums;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    public class Title : PhotoConstructorForSendler, ISendNew
    {
        public async Task SendNew(TelegramBotSendler myTelegramBot, MyNew myNew, string chatId)
        {
            try
            {
                string message = MakeMessage(myNew);
                await myTelegramBot.botClient.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                await myTelegramBot.SendMessageToOwner($"Ошибка отправки сообщения: {ex.Message} - {myNew.url}\n" +
                    $"Стратегия отправки {GetSendStrategyName()}");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }   
        }
        public string GetSendStrategyName()
        {
            return "Title";
        }
        private string MakeMessage(MyNew myNew)
        {
            string message = $"*{myNew.title}*";
            message += $"\n\n{MakeSubscribeBar(myNew)}";
            return message;
        }
    }
}
