using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithClearPhoto
{
    internal class TitleSecondTitleDescriptionClearPhoto : PhotoConstructorForSendler, ISendNew
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
            return "TitleClearPhoto";
        }
        private string MakeMessage(MyNew myNew)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine(MakeCorrectTitle($"{MyPropertiesStatic.smile}*{myNew.title}*"));
            if (myNew.secondTitle != null)
                messageBuilder.AppendLine($"_{myNew.secondTitle}_");
            messageBuilder.AppendLine();
            myNew.descriptionToSend = MakeDescriptionToSend(myNew);

            // Используем регулярное выражение для поиска текста внутри кавычек
            string pattern = "\"([^\"]*)\"";
            MatchCollection matches = Regex.Matches(myNew.descriptionToSend, pattern);

            // Заменяем найденные кавычки на подчеркивания
            foreach (Match match in matches)
            {
                string textInsideQuotes = match.Groups[1].Value;
                string replacement = $"*\"{textInsideQuotes}\"*";
                myNew.descriptionToSend = myNew.descriptionToSend.Replace(match.Value, replacement);
            }

            messageBuilder.AppendLine(myNew.descriptionToSend);

            return messageBuilder.ToString();
        }
    }
}
