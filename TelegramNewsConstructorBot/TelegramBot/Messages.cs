using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramNewsConstructorBot.TelegramBot.User;

namespace TelegramNewsConstructorBot.TelegramBot
{
    public class MyMessages
    {
        public MyMessages(TelegramBotClient _telegramBot)
        {
            telegramBot = _telegramBot;
        }
        TelegramBotClient telegramBot;


        public async Task SendCommandsAsync(MyUser user)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                new[]
                {
                    new KeyboardButton("🆕Создать новость")
                }
            }, true);

            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: "*Выберите команду*",
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
            FillAvaileadbleCommands(user, keyboard);

        }
        public async Task SendCancelCreateNewKeyboad(MyUser user)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                new[]
                {
                    new KeyboardButton("⬅️Комманды"),
                }
            }, true);

            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: "Можете вернуться обратно",
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
            FillAvaileadbleCommands(user, keyboard);

        }
        public async Task BackToMenuOrCreateNew(MyUser user)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                new[]
                {
                    new KeyboardButton("⬅️Комманды"),
                    new KeyboardButton("🆕Создать новость")
                }
            }, true);

            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: "Можете вернуться обратно",
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
            FillAvaileadbleCommands(user, keyboard);

        }
        public async Task ConfirmSendNew(MyUser user)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
               {
                new[]
                {
                    new KeyboardButton("❌Не отправлять")
                }
            }, true);

            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: "Можно отменить отправку и вернуться в меню",
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
            FillAvaileadbleCommands(user, keyboard);

        }
        public async Task ChooseSendStrategy(MyUser user)
        {
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("1️⃣", "TitleAndDescriptionPhoto"),
                        InlineKeyboardButton.WithCallbackData("2️⃣", "TitlePhoto"),
                        InlineKeyboardButton.WithCallbackData("3️⃣", "TitleWithDescriptionPhoto")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("4️⃣", "DescriptionUnderPhoto"),
                        InlineKeyboardButton.WithCallbackData("5️⃣", "DescriptionLeftPhoto")
                        
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("6️⃣", "TitleSecondTitleDescriptionClearPhoto"),
                        InlineKeyboardButton.WithCallbackData("7️⃣", "TitleSecondTitleClearPhoto"),
                        InlineKeyboardButton.WithCallbackData("8️⃣", "TitleClearPhoto")
                    },
                };
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

            string newTypes = $"*Заголовок на фото*\n" +
                $"1 - Заголовок на фото + описание\n" +
                $"2 - Заголовок на фото + второй заголовок\n" +
                $"3 - Заголовок и описание на фото + второй заголовок\n\n" +
                $"*Описание на фото*\n"+
                $"4 - Описание снизу на фото + заголовок\n" +
                $"5 - Описание слева на фото + заголовок\n\n" +
                $"*Чистое фото*\n" +
                $"6 - Заголовок, второй заголовок, описание\n" +
                $"7 - Заголовок,второй заголовок\n" +
                $"8 - Заголовок\n";
            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: newTypes,
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );

        }
        public async Task ConfirmSendNewCallbackQuery(MyUser user,string sendStrategy)
        {
            string confirmData = "Send_" + sendStrategy;

            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("✅Отправить", confirmData)
                    }
                    
                };
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

            string newTypes = $"Отправить новость?";
            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: newTypes,
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );

        }
        public async Task SendSavedChannel(MyUser user)
        {
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("✅Отправить", "SendSavedChannel"),
                        InlineKeyboardButton.WithCallbackData("🔁Выбрать другой", "ChooseChannelSend"),
                    }
                };
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttons);

            string newTypes = $"Есть сохраненный канал - *{user.savedChannel}*, отправить в него?";
            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: newTypes,
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );

        }
        public async Task SendClearKeyboard(MyUser user)
        {
            var keyboard = new ReplyKeyboardMarkup(Array.Empty<KeyboardButton[]>());

            await telegramBot.SendTextMessageAsync(
                chatId: user.chatId,
                text: "",
                replyMarkup: keyboard,
                parseMode: ParseMode.Markdown
            );
            FillAvaileadbleCommands(user, keyboard);

        }

        private void FillAvaileadbleCommands(MyUser user, ReplyKeyboardMarkup keyboard)
        {
            user.availableCommands.Clear();
            foreach (var commandArr in keyboard.Keyboard.ToList())
            {
                foreach (var command in commandArr)
                {
                    user.availableCommands.Add(command.Text);
                }
            }
        }
    }
}
