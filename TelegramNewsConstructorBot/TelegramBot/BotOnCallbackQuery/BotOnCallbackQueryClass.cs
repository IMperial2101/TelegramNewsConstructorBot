using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using RiaNewsParserTelegramBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithClearPhoto;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithPhotoshopPhoto.OnlyDescriptionOnPhoto;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithPhotoshopPhoto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using TelegramNewsConstructorBot.TelegramBot.BotOnMessageReceived;
using TelegramNewsConstructorBot.TelegramBot.User;
using static TelegramNewsConstructorBot.TelegramBot.EnumsState.Enums;
using System.Threading.Channels;

namespace TelegramNewsConstructorBot.TelegramBot.BotOnCallbackQuery
{
    internal class BotOnCallbackQueryClass
    {
        public BotOnCallbackQueryClass(TelegramBotClient _telegramBot, MyParser _myParser, MyNewTelegramSendler _myNewTelegramSendler, Dictionary<long, MyUser> _users)
        {
            telegramBot = _telegramBot;
            myParser = _myParser;
            myNewTelegramSendler = _myNewTelegramSendler;
            users = _users;
            myMessages = new MyMessages(telegramBot);
        }
        MyNewTelegramSendler myNewTelegramSendler;
        MyParser myParser;
        MyMessages myMessages;
        TelegramBotClient telegramBot;
        Dictionary<long, MyUser> users;
        public async void BotOnCallbackQuery(object sender,
                                             CallbackQueryEventArgs callbackQueryEventArgs)
        {
            Console.WriteLine("Thread id: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("{0} choise {1}\n", callbackQueryEventArgs.CallbackQuery.From, callbackQueryEventArgs.CallbackQuery.Data.ToString());

            var callBackQuery = callbackQueryEventArgs.CallbackQuery.Data;
            var chatId = callbackQueryEventArgs.CallbackQuery.Message.Chat.Id;

            if (callBackQuery == null)
                return;

            MyUser user = CheckUserAndAdd(chatId);
            SwitchListActions(user, callbackQueryEventArgs);

        }
        private MyUser CheckUserAndAdd(long chatId)
        {
            MyUser user;
            if (!users.TryGetValue(chatId, out user))
            {
                user = new MyUser();
                user.chatId = chatId;
                users.Add(chatId, user);

            }
            return user;
        }
        private async void SwitchListActions(MyUser user, CallbackQueryEventArgs callbackQuery)
        {
            if (user.skipCallbackData)
                return;

            string callbackData = callbackQuery.CallbackQuery.Data;
            
            if(callbackData.Contains("Send_"))
            {
                string strategy = callbackData.Substring(5);
                ISendNew sendStrategy = ReturnISendNew(strategy);
                user.ISendNew = sendStrategy;
                if(user.savedChannel != null)
                {
                    myMessages.SendSavedChannel(user);
                }
                else
                {
                    await telegramBot.SendTextMessageAsync(user.chatId, "Отправьте название канала, формата - @НАЗВАНИЕ Либо отравьте ссылку формата - https://t.me/НАЗВАНИЕ");
                    user.CreateNewState = CreateNewState.SendChannel;
                    user.skipCallbackData = true;
                }
                
            }

            switch (callbackData)
            {
                case "TitleAndDescriptionPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "TitlePhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "TitleWithDescriptionPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "DescriptionUnderPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "DescriptionLeftPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "TitleSecondTitleDescriptionClearPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "TitleSecondTitleClearPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "TitleClearPhoto":
                    await SendCheckNew(user, callbackData);
                    break;
                case "SendSavedChannel":
                    SendSavedChannel(user);
                    break;
                case "ChooseChannelSend":
                    ChooseChannelSend(user);
                    break;

            }

        }
        private async Task SendCheckNew(MyUser user,string callbackData)
        {
            try
            {
                if(user.myNew == null)
                {
                    user.myNew = await myParser.ParseOneNewAsync(user.linkNew);
                    if(user.myNew == null)
                    {
                        telegramBot.SendTextMessageAsync(user.chatId, "Не удалость скачать страницу новости, скорее всего ссылка была повреждена");
                        user.UserState = UserState.Commands;
                        myMessages.SendCommandsAsync(user);
                        return;
                    }
                }
                    
                await myNewTelegramSendler.SendNew(user.myNew, ReturnISendNew(callbackData), user.chatId.ToString());
                myMessages.ConfirmSendNewCallbackQuery(user,callbackData);
                myMessages.ChooseSendStrategy(user);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                telegramBot.SendTextMessageAsync(user.chatId, "Не удалось отправить новость попробуйте еще раз");
                user.UserState = UserState.Commands;
                myMessages.SendCommandsAsync(user);
            }
        }
        private async Task SendSavedChannel(MyUser user)
        {
            await myNewTelegramSendler.SendNew(user.myNew, user.ISendNew, user.savedChannel);
            user.CreateNewState = CreateNewState.Confirm;
            user.myNew = null;
            myMessages.BackToMenuOrCreateNew(user);
        }
        private async Task ChooseChannelSend(MyUser user)
        {
            await telegramBot.SendTextMessageAsync(user.chatId, "Отправьте название канала, формата - @НАЗВАНИЕ Либо отравьте ссылку формата - https://t.me/НАЗВАНИЕ");
            user.CreateNewState = CreateNewState.SendChannel;
            user.skipCallbackData = true;
        }
        private ISendNew ReturnISendNew(string strategy)
        {
            switch (strategy)
            {
                case "Title":
                    {
                        return new Title();
                        break;
                    }
                case "TitleDescription":
                    {
                        return new TitleDescription();
                        break;
                    }
                case "TitleSecondTitleDescription":
                    {
                        return new TitleSecondTitleDescription();
                        break;
                    }
                case "TitleClearPhoto":
                    {
                        return new TitleClearPhoto();
                        break;
                    }
                case "TitleSecondTitleClearPhoto":
                    {
                        return new TitleSecondTitleClearPhoto();
                        break;
                    }
                case "TitleSecondTitleDescriptionClearPhoto":
                    {
                        return new TitleSecondTitleDescriptionClearPhoto();
                        break;
                    }
                case "DescriptionLeftPhoto":
                    {
                        return new DescriptionLeftPhoto();
                        break;
                    }
                case "DescriptionRightPhoto":
                    {
                        return new DescriptionRightPhoto();
                        break;
                    }
                case "DescriptionUnderPhoto":
                    {
                        return new DescriptionUnderPhoto();
                        break;
                    }
                case "TitleAndDescriptionPhoto":
                    {
                        return new TitleAndDescriptionPhoto();
                        break;
                    }
                case "TitlePhoto":
                    {
                        return new TitlePhoto();
                        break;
                    }
                case "TitleWithDescriptionPhoto":
                    {
                        return new TitleWithDescriptionPhoto();
                        break;
                    }
            }
            return null;

        }
    }
}
