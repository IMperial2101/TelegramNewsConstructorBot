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
    public class SendTitle : ISendNew
    {
        public async void SendNew(MyTelegramBot myTelegramBot, MyNew myNew,string channelId)
        {
            myTelegramBot.botClient.SendTextMessageAsync(channelId, $"*{myNew.title}*", ParseMode.Markdown);
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
