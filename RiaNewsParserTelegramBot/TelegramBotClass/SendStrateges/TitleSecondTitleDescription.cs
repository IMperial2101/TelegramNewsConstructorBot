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
            myNew.descriptionToSend = MakeDescriptionToSend(myNew, 2);
            myTelegramBot.botClient.SendTextMessageAsync(MyPropertiesStatic.channelID, $"*{myNew.title}*_{MakeSecondTitle(myNew)}_\n\n{myNew.descriptionToSend}", ParseMode.Markdown);
        }
        private string MakeSecondTitle(MyNew myNew)
        {
            if (myNew.secondTitle == null)
                return "";
            return myNew.secondTitle;
        }
    }
}
