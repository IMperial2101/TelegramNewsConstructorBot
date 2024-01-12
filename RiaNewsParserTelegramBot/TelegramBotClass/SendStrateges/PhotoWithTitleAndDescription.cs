﻿using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    internal class PhotoWithTitleAndDescription
    {
        string pathToImages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        public async void SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            string pathToPhoto = Path.Combine(pathToImages, myNew.photoName + "Done.png");
            if (System.IO.File.Exists(pathToPhoto))
            {
                using FileStream fileStream = new(pathToPhoto, FileMode.Open, FileAccess.Read, FileShare.Read);
                InputOnlineFile inputFile = new InputOnlineFile(fileStream);
                await myTelegramBot.botClient.SendPhotoAsync(MyPropertiesStatic.channelID, inputFile);
            }
            else
            {
                Console.WriteLine("Файл не найден!");
            }
        }
        public bool CheckNewAdjust(MyNew myNew)
        {
            string descriptionToSend = makeDescriptionToSend(myNew.description[0]);
            if (descriptionToSend == string.Empty || descriptionToSend.Length > 500)
                return false;
            string pathToPhoto = Path.Combine(pathToImages, myNew.photoName + "Done.png");
            if (System.IO.File.Exists(pathToPhoto))
                return true;
            return false;

        }
        protected string makeDescriptionToSend(string description)
        {
            int indexOfDot = description.IndexOf(". ");

            if (indexOfDot != -1)
                return description.Substring(indexOfDot + 2); // +2 для включения пробела после точки
            else
                return string.Empty;
        }
    }
}
