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

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    public class TitleSecondTitleDescription : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(TelegramBotSendler myTelegramBot, MyNew myNew)
        {
            try
            {
                string message = MakeMessage(myNew);
                await myTelegramBot.botClient.SendTextMessageAsync(MyPropertiesStatic.channelID, message, ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                await myTelegramBot.SendMessageToOwner($"Ошибка отправки сообщения: {ex.Message} - {myNew.url}\n" +
                    $"Стратегия отправки {GetSendStrategyName()}");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
        }
        private string MakeMessage(MyNew myNew)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(MyPropertiesStatic.smile);
            messageBuilder.AppendLine($"*{myNew.title}*");
            if (myNew.secondTitle != null)
            {
                messageBuilder.AppendLine($"_{myNew.secondTitle}_\n");
            }
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
            messageBuilder.AppendLine();
            messageBuilder.AppendLine(MakeSubscribeBar());

            return messageBuilder.ToString();
        }
        private string MakeSecondTitle(MyNew myNew)
        {
            if (myNew.secondTitle == null)
                return "";
            return myNew.secondTitle + "\n";
        }
        public string GetSendStrategyName()
        {
            return "TitleSecondTitleDescription";
        }
    }
}
