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
using Telegram.Bot.Exceptions;

namespace RiaNewsParserTelegramBot.TelegramBotClass
{
    public class PhotoConstructorForSendler
    {

        public string pathToImages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        PhotoConstructor photoConstructor = new PhotoConstructor();
        public async Task MakePhoto(MyNew myNew, IPhotoConstructorStrategy constructorStrategy,ColorVariationsEnum colorsVariation)
        {
            await photoConstructor.MakePhoto(myNew, constructorStrategy, colorsVariation);
        }
        protected string MakeDescriptionToSend(MyNew myNew)
        {
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder();

            int abzatcCount = random.Next(MyPropertiesStatic.minParagraphCount, MyPropertiesStatic.maxParagraphCount);

            if (abzatcCount > myNew.description.Count)
                abzatcCount = myNew.description.Count;
            for (int i = 0; i < abzatcCount; i++)
            {
                if (stringBuilder.Length > MyPropertiesStatic.maxDescripSymbCount)
                    break;
                if (myNew.description[i] != null || myNew.description[i].Length > 20)
                {
                    stringBuilder.Append($"{MyPropertiesStatic.abzatcSmile}  ");
                    stringBuilder.AppendLine(myNew.description[i]);
                    stringBuilder.AppendLine();
                }               
                if (stringBuilder.Length >= 800)
                {
                    stringBuilder.Length = 790;
                    stringBuilder.Append("...");
                    break;
                }

                
            }

            return stringBuilder.ToString();
        }
        public string MakeCorrectTitle(string title)
        {
            if (title.Length > 280)
                return title.Substring(0, 180) + "...";
            else
                return title;
        }
        protected string MakeSubscribeBar(MyNew myNew)
        {
            string subscribeBar = $"[Читать полностью]({myNew.url})\n\n[🔥MARHSALL.ПОДПИСАТЬСЯ🔥](https://t.me/{MyPropertiesStatic.channelID.Substring(1)})\n\n";
            return subscribeBar;

        }

    }
}
