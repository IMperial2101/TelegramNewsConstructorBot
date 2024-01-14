using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;

namespace RiaNewsParserTelegramBot.TelegramBotClass
{
    internal class MyNewTelegramSendler
    {
        
        private ISendNew? sendStrategy;
        MyTelegramBot myTelegramBot;
        public MyNewTelegramSendler(MyTelegramBot myTelegramBot)
        {
            this.myTelegramBot = myTelegramBot; 
        }
        public void SetStrategy(ISendNew strategy)
        {
            sendStrategy = strategy;
        }
        public async Task SendNew(MyNew myNew, ISendNew strategy)
        {
            await strategy.SendNew(myTelegramBot, myNew);
        }

    }
}
