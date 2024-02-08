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
using static TelegramNewsConstructorBot.TelegramBot.EnumsState.Enums;

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
                case UserState.Start:
                    UserState_Start(user);
                    break;
                case UserState.EnterKey:
                    UserState_EnterKey(user,message);
                    break;
                case UserState.Commands:
                    switch(user.UserStateCommands)
                    {
                        case UserStateCommands.Default:
                            UserStateCommandsDefault(user);
                            break;
                        case UserStateCommands.CreateNew:
                            switch(user.CreateNewState)
                            {
                                case CreateNewState.GetLink:
                                    CreateNewState_GetLink(user,message);
                                    break;
                                case CreateNewState.ChooseStrategy:
                                    CreateNewState_ChooseStrategy(user);
                                    break;
                                case CreateNewState.SendChannel:
                                    CreateNewState_SendChannel(user,message);
                                    break;
                                case CreateNewState.Confirm:
                                    CreateNewState_Confirm(user);
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
            string username = message.Chat.Username;
            if (!MyPropertiesStatic.userNames.ContainsKey(username))
            {
                await telegramBot.SendTextMessageAsync(user.chatId, "У вас нет доступа к этому боту, обратитесь к @Danilll9997 ");
                return;
            }
            string messageText = message.Text;
            if (messageText == MyPropertiesStatic.userNames[username])
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
        private async void CreateNewState_GetLink(MyUser user,Message message)
        {
            
            string link = message.Text;
            if(!link.Contains("ria.ru"))
            {
                await telegramBot.SendTextMessageAsync(user.chatId, "Это не похоже на ссылку с РИА Новостей");
                return;
            }
            user.linkNew = link;
            await telegramBot.SendTextMessageAsync(user.chatId, $"Выберите тип новости:");
            user.skipCallbackData = false;
            myMessages.ChooseSendStrategy(user);
            user.CreateNewState = Enums.CreateNewState.ChooseStrategy;
            
            
        }
        private async void CreateNewState_ChooseStrategy(MyUser user)
        {

        }
        private async void CreateNewState_SendChannel(MyUser user,Message message)
        {
            string channelId;
            if(message.Text.Contains("https://t.me/"))
            {
                channelId = "@"+message.Text.Substring(13);
            }
            else if(message.Text.Contains("@"))
            {
                channelId = message.Text;
            }
            else
            {
                telegramBot.SendTextMessageAsync(user.chatId, "Эта ссылку не подходит, попробуйте еще раз");
                return;
            }
            try
            {
                await myNewTelegramSendler.SendNew(user.myNew, user.ISendNew, channelId);
                user.CreateNewState = CreateNewState.Confirm;
                user.savedChannel = channelId;
            }
            catch(Exception ex)
            {
                user.savedChannel = null;
                Console.WriteLine($"{ex.Message}");
                telegramBot.SendTextMessageAsync(user.chatId, "Не удалось отправить новость попробуйте еще раз");
                user.UserState = UserState.Commands;
                myMessages.SendCommandsAsync(user);
            }
            user.myNew = null;
            myMessages.BackToMenuOrCreateNew(user);
        }
        private async void CreateNewState_Confirm(MyUser user)
        {
            await telegramBot.SendTextMessageAsync(user.chatId, "Вы можете отправить создать еще новость, либо вернуться в меню");
        }
    }
}
