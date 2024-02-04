using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramNewsConstructorBot.TelegramBot.EnumsState;
using TelegramNewsConstructorBot.TelegramBot.User;

namespace TelegramNewsConstructorBot.TelegramBot.BotOnMessageReceived
{
    public class BotOnMessageReceivedClass
    {
        public BotOnMessageReceivedClass(TelegramBotClient _telegramBot, MyParser _myParser, MyNewTelegramSendler _myNewTelegramSendler)
        {
            myNewTelegramSendler = _myNewTelegramSendler;
            myParser = _myParser;
            telegramBot = _telegramBot;
            myMessages = new MyMessages(telegramBot);
            userStateSwitcher = new UserStateSwithcer(telegramBot, myMessages,myParser, myNewTelegramSendler);
            userCommandSwitcher = new UserCommandSwitcher(telegramBot, myMessages,myParser, myNewTelegramSendler);

            
        }
        MyNewTelegramSendler myNewTelegramSendler;
        MyParser myParser;
        MyMessages myMessages;
        UserStateSwithcer userStateSwitcher;
        UserCommandSwitcher userCommandSwitcher;
        TelegramBotClient telegramBot;
        Dictionary<long, MyUser> Users = new Dictionary<long, MyUser>();
        List<string> allCommands = new List<string>(new string[] { "🆕Создать новость", "⬅️Комманды" });

        private bool switchUserstate;
        public async void _BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Console.WriteLine("@{0} send {1}\n", messageEventArgs.Message.Chat.Username, messageEventArgs.Message.Text);

            var message = messageEventArgs.Message;
            var chatId = message.Chat.Id;

            if (message == null || message.Type != MessageType.Text)
                return;

            MyUser user = CheckUserAndAdd(chatId, message.Chat.Username);

            switchUserstate = true;
            if (allCommands.Contains(message.Text))
                if (!user.availableCommands.Contains(message.Text))
                {
                    telegramBot.SendTextMessageAsync(user.chatId, "Эта команда пока недоступна, в клавиатуре есть все доступные команды");
                    return;
                }
                else
                    switchUserstate = false;


            if (switchUserstate)
                userStateSwitcher.SwitchUserState(user, message,switchUserstate);
            else
                userCommandSwitcher.SwitchUserCommands(user, message);
        }
        private MyUser CheckUserAndAdd(long chatId, string username)
        {
            MyUser user;
            if (!Users.TryGetValue(chatId, out user))
            {
                user = new MyUser();
                user.chatId = chatId;
                user.telegramUserName = "@" + username;
                Users.Add(chatId, user);

            }
            return user;
        }
        
        
        
    }
}