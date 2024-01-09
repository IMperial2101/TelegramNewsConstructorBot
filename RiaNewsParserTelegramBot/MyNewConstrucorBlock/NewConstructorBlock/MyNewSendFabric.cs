using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.NewConstructorBlock
{
    internal class MyNewSendFabric
    {
        static Random random = new Random();
        static int lastParagraphCount;
        static int maxParagraphCount;
        static int maxDescripSymbCount;
        static bool sendPhotoWithText;
        static bool sendSecondTitle;
        static bool sendPhoto;
        public string MakeDescriptionRandomDesign(MyNew myNew)
        {
            StringBuilder descriptionBuilder = new StringBuilder("");
            if (myNew.description.Count != 0)
                descriptionBuilder.Append($"\n{MyPropertiesStatic.smile}");
            else
                return "";
            if (myNew.description.Count == 1)
                maxParagraphCount = 1;
            else if (myNew.description.Count < maxParagraphCount)
                maxParagraphCount = myNew.description.Count - 1;

            for (int i = 0; i < maxParagraphCount; i++)
                descriptionBuilder.Append($"{myNew.description[i]}\n\n");

            if (descriptionBuilder.Length > maxDescripSymbCount)
                descriptionBuilder.Length = maxDescripSymbCount;

            return descriptionBuilder.ToString();
        }
        static private void MakeRandomProperties()
        {
            if (MyPropertiesStatic.randomMessageDesign)
            {
                while (lastParagraphCount == maxParagraphCount)
                {
                    maxParagraphCount = random.Next(0, MyPropertiesStatic.maxParagraphCount <= 1 ? 2 : MyPropertiesStatic.maxParagraphCount + 1);
                }
                lastParagraphCount = maxParagraphCount;

                maxDescripSymbCount = random.Next(MyPropertiesStatic.maxDescripSymbCount <= 50 ? 0 : MyPropertiesStatic.maxDescripSymbCount - 50, MyPropertiesStatic.maxDescripSymbCount <= 50 ? 150 : MyPropertiesStatic.maxDescripSymbCount + 50);

                if (MyPropertiesStatic.sendSecondTitleRandomPerсent > 100 || MyPropertiesStatic.sendSecondTitleRandomPerсent < 0)
                    sendSecondTitle = true;
                else
                {
                    if (random.Next(0, 100) <= MyPropertiesStatic.sendSecondTitleRandomPerсent)
                        sendSecondTitle = true;
                    else
                        sendSecondTitle = false;
                }
                if (MyPropertiesStatic.sendPhotoPercent > 100 || MyPropertiesStatic.sendPhotoPercent < 0)
                    sendPhoto = true;
                else
                {
                    if (random.Next(0, 100) <= MyPropertiesStatic.sendPhotoPercent)
                        sendPhoto = true;
                    else
                        sendPhoto = false;
                }
                if (sendPhoto && (MyPropertiesStatic.sendPhotoWithTextRandomPercent > 100 || MyPropertiesStatic.sendPhotoWithTextRandomPercent < 0))
                    sendPhotoWithText = true;
                else
                {
                    if (sendPhoto && (random.Next(0, 100) <= MyPropertiesStatic.sendPhotoWithTextRandomPercent))
                        sendPhotoWithText = true;
                    else
                        sendPhotoWithText = false;
                }

            }
            else
            {
                maxParagraphCount = MyPropertiesStatic.maxParagraphCount;
                maxDescripSymbCount = MyPropertiesStatic.maxDescripSymbCount;
                sendSecondTitle = true;
                sendPhotoWithText = true;
            }
        }

    }
}
