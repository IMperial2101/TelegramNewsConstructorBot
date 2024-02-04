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
