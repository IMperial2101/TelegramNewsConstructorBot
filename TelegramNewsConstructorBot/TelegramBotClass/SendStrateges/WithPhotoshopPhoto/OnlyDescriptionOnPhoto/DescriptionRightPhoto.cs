﻿using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiaNewsParserTelegramBot.PropertiesClass;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithPhotoshopPhoto
{
    internal class DescriptionRightPhoto : PhotoConstructorForSendler, ISendNew
    {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            ColorVariationsEnum colorVariations = MyColorConverter.GetRandomColorVariation();
            await MakePhoto(myNew, new DescriptionRightBlackBlock(), colorVariations);
            try
            {
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
                await myTelegramBot.SendMessageToOwner($"Ошибка отправки сообщения: {ex.Message} - {myNew.url}\n" +
                    $"Стратегия отправки {GetSendStrategyName()}");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        public string GetSendStrategyName()
        {
            return "DescriptionRightPhoto";
        }
        public string MakeMessage(MyNew myNew)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(MyPropertiesStatic.smile);
            messageBuilder.AppendLine($"*{myNew.title}*");
            if(myNew.secondTitle != null)
                messageBuilder.AppendLine($"_{myNew.secondTitle}_");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine(MakeSubscribeBar());
            return messageBuilder.ToString();

        }
    }
}
