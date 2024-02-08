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
using TelegramNewsConstructorBot.TelegramBot.BotOnCallbackQuery;
using TelegramNewsConstructorBot.TelegramBot.User;

namespace TelegramNewsConstructorBot.TelegramBot
{
    internal class MyTelegramBot
    {
        MyNewTelegramSendler myNewTelegramSendler;
        MyParser myParser;
        MyMessages myMessages;
        UserStateSwithcer userStateSwitcher;
        UserCommandSwitcher userCommandSwitcher;
        TelegramBotClient telegramBot;
        Dictionary<long, MyUser> users = new Dictionary<long, MyUser>();
        public MyTelegramBot(TelegramBotClient telegramBot,MyParser myParser,MyNewTelegramSendler myNewTelegramSendler)
        {
            var onMessage = new BotOnMessageReceivedClass(telegramBot, myParser, myNewTelegramSendler, users);

            var onCallback = new BotOnCallbackQueryClass(telegramBot,myParser,myNewTelegramSendler,users);

            telegramBot.OnMessage += onMessage.BotOnMessageReceived;
            telegramBot.OnCallbackQuery += onCallback.BotOnCallbackQuery;


            telegramBot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine("Начало работы бота");
        }       
    }
}
