using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types;
using RiaNewsParserTelegramBot.PropertiesClass;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
     class PhotoWithTitle : PhotoConstructorForSendler, ISendNew
     {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            await MakePhoto(myNew,new TitleUnderBlackBlock());

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
    }
}
