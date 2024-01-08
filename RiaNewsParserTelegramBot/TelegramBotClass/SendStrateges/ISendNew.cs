using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    public interface ISendNew
    {
        public abstract void SendNew(MyTelegramBot myTelegramBot,MyNew myNew,string channelId);
        public bool CheckNewAdjust(MyNew myNew);
    }
}
