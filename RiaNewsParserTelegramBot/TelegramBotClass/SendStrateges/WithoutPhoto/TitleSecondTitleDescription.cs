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
    public class TitleSecondTitleDescription : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            try
            {
                myNew.descriptionToSend = MakeDescriptionToSend(myNew);
                await myTelegramBot.botClient.SendTextMessageAsync(MyPropertiesStatic.channelID, $"*{myNew.title}*_{MakeSecondTitle(myNew)}_\n\n{myNew.descriptionToSend}", ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                await myTelegramBot.SendMessageToOwner($"Ошибка отправки сообщения: {ex.Message} - {myNew.url}\n" +
                    $"Стратегия отправки {GetSendStrategyName()}");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
        }
        private string MakeSecondTitle(MyNew myNew)
        {
            if (myNew.secondTitle == null)
                return "";
            return myNew.secondTitle + "\n";
        }
        public string GetSendStrategyName()
        {
            return "TitleSecondTitleDescription";
        }
    }
}
