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
    public class Title : ISendNew
    {
        public async void SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            myTelegramBot.botClient.SendTextMessageAsync(MyPropertiesStatic.channelID, $"*{myNew.title}*", ParseMode.Markdown);
        }
        public bool CheckNewAdjust(MyNew myNew)
        {
            if(myNew.title != null && myNew.title.Length < 250)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
