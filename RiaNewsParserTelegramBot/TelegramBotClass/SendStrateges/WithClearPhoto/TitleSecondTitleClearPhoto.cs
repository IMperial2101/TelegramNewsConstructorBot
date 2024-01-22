using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithClearPhoto
{
    internal class TitleSecondTitleClearPhoto : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            string message = MakeMessage(myNew);
            try
            {
                await myTelegramBot.botClient.SendPhotoAsync(MyPropertiesStatic.channelID, myNew.photoUrl, message, ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                await myTelegramBot.SendMessageToOwner($"Ошибка отправки сообщения: {ex.Message} - {myNew.url}\n" +
                    $"Стратегия отправки {GetSendStrategyName()}");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        public string GetSendStrategyName()
        {
            return "TitleSecondTitleClearPhoto";
        }
        private string MakeMessage(MyNew myNew)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine(MakeCorrectTitle($"{MyPropertiesStatic.smile}*{myNew.title}*"));
            if(myNew.secondTitle != null)
                messageBuilder.AppendLine($"_{myNew.secondTitle}_");

            return messageBuilder.ToString();
        }
    }
}
