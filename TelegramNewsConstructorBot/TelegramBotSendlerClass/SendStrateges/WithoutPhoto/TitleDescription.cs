using AngleSharp.Dom;
using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    public class TitleDescription : PhotoConstructorForSendler,ISendNew
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
            return "TitleDescription";
        }
        private string MakeMessage(MyNew myNew)
        {
            myNew.descriptionToSend = MakeDescriptionToSend(myNew);
            string message = $"*{myNew.title}*\n\n{myNew.descriptionToSend}";
            message += $"\n\n{MakeSubscribeBar(myNew)}";
            return message;
        }
    }
}
