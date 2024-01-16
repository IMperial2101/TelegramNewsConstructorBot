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
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    internal class PhotoWithTitleAndDescription : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {

            ColorVariationsEnum colorVariations = MyColorConverter.GetRandomColorVariation();
            await MakePhoto(myNew, new TitleUnderBlackBlock(), colorVariations);

            try
            {
                myNew.descriptionToSend = MakeDescriptionToSend(myNew, 2);
                string pathToPhoto = Path.Combine(pathToImages, myNew.photoName + "Done.png");
                if (System.IO.File.Exists(pathToPhoto))
                {
                    string message = MakeMessage(myNew);
                    using FileStream fileStream = new(pathToPhoto, FileMode.Open, FileAccess.Read, FileShare.Read);
                    InputOnlineFile inputFile = new InputOnlineFile(fileStream);
                    await myTelegramBot.botClient.SendPhotoAsync(MyPropertiesStatic.channelID, inputFile, message, ParseMode.Markdown);
                }
                else
                {
                    Console.WriteLine("Файл не найден!");
                }
            }
            catch(Exception ex)
            {
                await myTelegramBot.SendMessageToOwner($"Ошибка отправки сообщения: {ex.Message} - {myNew.url}");
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
            myNew.descriptionToSend = MakeDescriptionToSend(myNew, 2);
            messageBuilder.AppendLine(myNew.descriptionToSend);


            return messageBuilder.ToString();
        }
        

    }
}
