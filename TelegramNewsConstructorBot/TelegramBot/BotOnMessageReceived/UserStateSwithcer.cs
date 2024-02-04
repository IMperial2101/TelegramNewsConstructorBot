using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using RiaNewsParserTelegramBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using TelegramNewsConstructorBot.TelegramBot.EnumsState;
using TelegramNewsConstructorBot.TelegramBot.User;

namespace TelegramNewsConstructorBot.TelegramBot.BotOnMessageReceived
{
    internal class UserStateSwithcer
    {
        public UserStateSwithcer(TelegramBotClient _telegramBot, MyMessages _myMessages, MyParser _myParser, MyNewTelegramSendler _myNewTelegramSendler)
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
        public async void SwitchUserState(MyUser user,
                                   Message message, bool switchUserstate)
        {
            if (!switchUserstate)
                return;
            switch (user.UserState)
            {
                case Enums.UserState.Start:
                    UserState_Start(user);
                    break;
                case Enums.UserState.EnterKey:
                    UserState_EnterKey(user,message);
                    break;
                case Enums.UserState.Commands:
                    switch(user.UserStateCommands)
                    {
                        case Enums.UserStateCommands.Default:
                            UserStateCommandsDefault(user);
                            break;
                        case Enums.UserStateCommands.CreateNew:
                            switch(user.CreateNewState)
                            {
                                case Enums.CreateNewState.GetLink:
                                    CreateNewState_GetLink(user,message);
                                    break;
                            }
                            break;
                    }                
                    break;
                    
            }
        }
        private async void UserState_Start(MyUser user)
        {
            telegramBot.SendTextMessageAsync(user.chatId, "Здравствуйте, введите ключ доступа:");
            myMessages.SendClearKeyboard(user);
            user.UserState = Enums.UserState.EnterKey;
        }
        private async void UserState_EnterKey(MyUser user,Message message)
        {
            string messageText = message.Text;
            if (messageText == MyPropertiesStatic.adminKey)
            {
                await telegramBot.SendTextMessageAsync(user.chatId, "Успешная авторизация");
                user.UserState = Enums.UserState.Commands;
                myMessages.SendCommandsAsync(user);
            }
            else
            {
                await telegramBot.SendTextMessageAsync(user.chatId, "Неправильный ключ, попробуйте еще раз");
            }
        }
        private async void UserStateCommandsDefault(MyUser user)
        {
            telegramBot.SendTextMessageAsync(user.chatId, "Выберите команду на клавиатуре");
        }
        private async void UserState_ChooseCommands(MyUser user, Message message)
        {
            await telegramBot.SendTextMessageAsync(user.chatId, "Выберите команду");
        }
        private async void CreateNewState_GetLink(MyUser user,Message message)
        {
            
            string link = message.Text;
            if(!link.Contains("ria.ru"))
            {
                await telegramBot.SendTextMessageAsync(user.chatId, "Это не похоже на ссылку с РИА Новостей");
                return;
            }           
            try
            {
                MyNew myNew = await myParser.ParseOneNewAsync(link);
                myNewTelegramSendler.SendNew(myNew, new TitleAndDescriptionPhoto());
                await telegramBot.SendTextMessageAsync(user.chatId, "Новость отправлена, можете отправить еще новости");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
