using NewsPropertyBot.ParsingClasses;
using RiaNewsParserTelegramBot.TelegramBotClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramNewsConstructorBot.TelegramBot.BotOnMessageReceived;

namespace TelegramNewsConstructorBot.TelegramBot
{
    internal class MyTelegramBot
    {
        public MyTelegramBot(TelegramBotClient telegramBot,MyParser myParser,MyNewTelegramSendler myNewTelegramSendler)
        {
            this.telegramBot = telegramBot;
            telegramBot.OnMessage += new BotOnMessageReceivedClass(telegramBot,myParser, myNewTelegramSendler)._BotOnMessageReceived;
            telegramBot.StartReceiving(Array.Empty<UpdateType>());
        }
        public TelegramBotClient telegramBot;
        
    }
}
