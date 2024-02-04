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
using Telegram.Bot.Types.Enums;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
     class TitlePhoto : PhotoConstructorForSendler, ISendNew
     {
        public async Task SendNew(TelegramBotSendler myTelegramBot, MyNew myNew)
        {

            await MakePhoto(myNew,new TitleUnderBlackBlock(), ColorVariationsEnum.Black_OrangeLava);
            try
            {
                string message = MakeMessage();
                string pathToPhoto = Path.Combine(pathToImages, myNew.photoName + "Done.png");
                if (System.IO.File.Exists(pathToPhoto))
                {
                    using FileStream fileStream = new(pathToPhoto, FileMode.Open, FileAccess.Read, FileShare.Read);
                    InputOnlineFile inputFile = new InputOnlineFile(fileStream);
                    await myTelegramBot.botClient.SendPhotoAsync(MyPropertiesStatic.channelID, inputFile,message,ParseMode.Markdown);
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
        public string GetSendStrategyName()
        {
            return "TitlePhoto";
        }
        private string MakeMessage()
        {
            string message = $"\n{MakeSubscribeBar()}";
            return message;
        }
    }
}
