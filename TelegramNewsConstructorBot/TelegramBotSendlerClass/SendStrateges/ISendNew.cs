using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    public interface ISendNew
    {
        Task SendNew(TelegramBotSendler myTelegramBot, MyNew myNew);
        string GetSendStrategyName();
    }
}
