using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramNewsConstructorBot.TelegramBot.User;

namespace TelegramNewsConstructorBot.TelegramBot.BotOnMessageReceived
{
    public class UserCommandSwitcher
    {
        public UserCommandSwitcher(TelegramBotClient _telegramBot, MyMessages _myMessages, MyParser _myParser, MyNewTelegramSendler _myNewTelegramSendler)
        {
            myNewTelegramSendler = _myNewTelegramSendler;
            telegramBot = _telegramBot;
            myMessages = _myMessages;
            myParser = _myParser;
        }
        MyNewTelegramSendler myNewTelegramSendler;
        MyParser myParser;
        MyMessages myMessages;
        TelegramBotClient telegramBot;

        public async void SwitchUserCommands(MyUser user, Message message)
        {
            switch (message.Text)
            {
                case "🆕Создать новость":
                    MakeNew(user);
                    break;
                case "⬅️Комманды":
                    CancelMakeNew(user);
                    break;
            }
        }
        private void MakeNew(MyUser user)
        {
            telegramBot.SendTextMessageAsync(user.chatId, "*Отправьте ссылку на новость*",ParseMode.Markdown);
            user.UserStateCommands = EnumsState.Enums.UserStateCommands.CreateNew;
            user.CreateNewState = EnumsState.Enums.CreateNewState.GetLink;
            myMessages.SendCancelCreateNewKeyboad(user);
        }
        private void CancelMakeNew(MyUser user)
        {
            user.UserState = EnumsState.Enums.UserState.Commands;
            user.UserStateCommands = EnumsState.Enums.UserStateCommands.Default;
            myMessages.SendCommandsAsync(user);
        }
    }
}
