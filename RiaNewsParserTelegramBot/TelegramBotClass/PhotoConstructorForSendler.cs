using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.TelegramBotClass
{
    public class PhotoConstructorForSendler
    {
        public string pathToImages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        PhotoConstructor photoConstructor = new PhotoConstructor();
        public async Task MakePhoto(MyNew myNew, IPhotoConstructorStrategy constructorStrategy)
        {
            
            await photoConstructor.MakePhoto(myNew, constructorStrategy);
        }
        protected string MakeDescriptionToSend(MyNew myNew, int abzatcCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(MyPropertiesStatic.smile);

            if (abzatcCount > myNew.description.Count)
                abzatcCount = myNew.description.Count;
            for (int i = 0; i < abzatcCount; i++)
            {
                stringBuilder.AppendLine(myNew.description[i]);
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
