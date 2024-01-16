using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    internal class PhotoWithTitleWithDescription : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            ColorVariationsEnum colorVariations = MyColorConverter.GetRandomColorVariation();

            myNew.descriptionToSend = myNew.description[0];
            await MakePhoto(myNew, new TitleWithDescription(), colorVariations);
            try
            {
                string pathToPhoto = Path.Combine(pathToImages, myNew.photoName + "Done.png");
                string message = MakeMessage(myNew);
                if (System.IO.File.Exists(pathToPhoto))
                {
                    using FileStream fileStream = new(pathToPhoto, FileMode.Open, FileAccess.Read, FileShare.Read);
                    InputOnlineFile inputFile = new InputOnlineFile(fileStream);
                    await myTelegramBot.botClient.SendPhotoAsync(MyPropertiesStatic.channelID, inputFile,message, ParseMode.Markdown);
                }
                else
                {
                    Console.WriteLine("Файл не найден!");
                }
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
            if (myNew.secondTitle != null)
            {
                messageBuilder.AppendLine($"*{myNew.secondTitle}*");
            }
            else
            {
                messageBuilder.AppendLine($"*{myNew.title}*");
            }
            return messageBuilder.ToString();
        }
        public string GetSendStrategyName()
        {
            return "PhotoWithTitleWithDescription";
        }
    }
}
