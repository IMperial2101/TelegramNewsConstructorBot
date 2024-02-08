using AngleSharp.Dom;
using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    internal class TitleAndDescriptionPhoto : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(TelegramBotSendler myTelegramBot, MyNew myNew,string chatId)
        {

            ColorVariationsEnum colorVariations = MyColorConverter.GetRandomColorVariation();
            await MakePhoto(myNew, new TitleUnderWithDecorLine(), colorVariations);

            try
            {
                myNew.descriptionToSend = MakeDescriptionToSend(myNew);
                string pathToPhoto = Path.Combine(pathToImages, myNew.photoName + "Done.png");
                if (System.IO.File.Exists(pathToPhoto))
                {
                    string message = MakeMessage(myNew);
                    using FileStream fileStream = new(pathToPhoto, FileMode.Open, FileAccess.Read, FileShare.Read);
                    InputOnlineFile inputFile = new InputOnlineFile(fileStream);
                    await myTelegramBot.botClient.SendPhotoAsync(chatId, inputFile, message, ParseMode.Markdown);
                }
                else
                {
                    Console.WriteLine("Файл не найден!");
                }
            }
            catch(Exception ex)
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
            if (myNew.secondTitle != null)
            {
                messageBuilder.AppendLine($"*{myNew.secondTitle}*\n");
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
            messageBuilder.AppendLine(MakeSubscribeBar(myNew));

            return messageBuilder.ToString();
        }
        public string GetSendStrategyName()
        {
            return "TitleAndDescriptionPhoto";
        }

    }
}
