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
        public BotOnMessageReceivedClass(TelegramBotClient _telegramBot, MyParser _myParser, MyNewTelegramSendler _myNewTelegramSendler, Dictionary<long, MyUser> _users)
        {
            myNewTelegramSendler = _myNewTelegramSendler;
            myParser = _myParser;
            telegramBot = _telegramBot;
            myMessages = new MyMessages(telegramBot);
            userStateSwitcher = new UserStateSwithcer(telegramBot, myMessages,myParser, myNewTelegramSendler);
            userCommandSwitcher = new UserCommandSwitcher(telegramBot, myMessages,myParser, myNewTelegramSendler);
            users = _users;
        }
        MyNewTelegramSendler myNewTelegramSendler;
        MyParser myParser;
        MyMessages myMessages;
        UserStateSwithcer userStateSwitcher;
        UserCommandSwitcher userCommandSwitcher;
        TelegramBotClient telegramBot;
        Dictionary<long, MyUser> users;
        List<string> allCommands = new List<string>(new string[] { "🆕Создать новость", "⬅️Комманды", "✅Отправить", "❌Не отправлять" });

        private bool switchUserstate;
        public async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            var chatId = message.Chat.Id;
            Console.WriteLine(chatId);
            Console.WriteLine("@{0} send {1}\n", messageEventArgs.Message.Chat.Username, messageEventArgs.Message.Text);
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
            if (!users.TryGetValue(chatId, out user))
            {
                user = new MyUser();
                user.chatId = chatId;
                user.telegramUserName = "@" + username;
                users.Add(chatId, user);

            }
            return user;
        }
        
        
        
    }
}