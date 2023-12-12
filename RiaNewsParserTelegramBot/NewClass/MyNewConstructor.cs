using RiaNewsParserTelegramBot.PropertiesClass;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using NewsPropertyBot.NewClass;
using Telegram.Bot.Types;

namespace RiaNewsParserTelegramBot.NewClass
{
    static class MyNewConstructor
    {
        static int lastParagraphCount;
        static int maxParagraphCount;
        static int maxDescripSymbCount;
        static bool sendPhotoWithText;
        static bool sendSecondTitle;
        static bool sendPhoto;
        static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        static Random random = new Random();
        static public void MakeMyNewProperties(MyNew myNew)
        {
            MakeRandomProperties();
            myNew.descriptionToSend = MakeDescriptionRandomDesign(myNew);

            if(sendPhoto)
            {
                myNew.sendPhotoBool = true;
                if (sendPhotoWithText && myNew.photoUrl != null && myNew.title != null)
                {
                    myNew.sendPhotoWithTextBool = true;
                    myNew.photoName = MakeRandomString();
                }
            }
        }

        public static async Task MakePhotoWithText(MyNew myNew)
        {
            if (myNew.sendPhotoWithTextBool)
            {
                await DownloadImage(myNew);
                await AddTextOnImage(myNew);
                myNew.photoName += "Done.png";
            }
        }
        static async Task AddTextOnImage(MyNew myNew)
        {
            using (Image image = Image.FromFile(Path.Combine(imagesFolderPath, myNew.photoName)+".png"))
            {
                int width = image.Width;
                int height = image.Height;

                using (Graphics graphics = Graphics.FromImage(image))
                {
                    Rectangle gradientRect = new Rectangle(0, 0, width, height);


                    using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                        gradientRect,
                        Color.Transparent,
                        Color.FromArgb(200, 0, 0, 0),
                        LinearGradientMode.Vertical))
                    {

                        graphics.FillRectangle(gradientBrush, gradientRect);


                        StringFormat stringFormat1 = new StringFormat();
                        stringFormat1.Alignment = StringAlignment.Center;
                        stringFormat1.LineAlignment = StringAlignment.Center;
                        float padding = 0.05f * width; // 10% от ширины изображения
                        float rectWidth = width - 2 * padding; // Ширина прямоугольника с учетом отступов

                        RectangleF rect1 = new RectangleF(padding, height / 2, rectWidth, height / 2);
                        Font font1 = AdjustFontSize(graphics, myNew.title, rect1);
                        graphics.DrawString(myNew.title, font1, Brushes.White, rect1, stringFormat1);


                        string text2 = "🔥Marshall News, вооружение🔥";
                        //string text2 = "**************************************************************************************************************";
                        using (Font font2 = new Font("Segoe UI Emoji", 100))
                        {
                            StringFormat stringFormat2 = new StringFormat();
                            stringFormat2.Alignment = StringAlignment.Center;
                            stringFormat2.LineAlignment = StringAlignment.Near;

                            RectangleF rect2 = new RectangleF(0, height * 3 / 4, width, height / 4);
                            // graphics.DrawString(text2, font2, Brushes.White, rect2, stringFormat2);
                        }
                    }
                }
                myNew.photoName += "Done.png";
                image.Save(Path.Combine(imagesFolderPath, myNew.photoName)+"Done.png", ImageFormat.Png);
            }

            Console.WriteLine($"Обработали фото {myNew.title.Substring(0, 20)}");
        }
        static Font AdjustFontSize(Graphics graphics, string text, RectangleF rect)
        {
            Font font;
            SizeF textSize;
            int fontSize = 1000;

            do
            {
                font = new Font("Montserrat", fontSize, FontStyle.Bold);


                textSize = graphics.MeasureString(text, font);

                fontSize--;
            } while (textSize.Height > rect.Height || textSize.Width / 2.5 > rect.Width);

            return font;
        }
        static string MakeDescriptionRandomDesign(MyNew myNew)
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
        static public async Task DownloadImage(MyNew myNew)
        {

            string imagePath = Path.Combine(imagesFolderPath, myNew.photoName+".png");

            HttpClient httpClient = new HttpClient();

            using var stream = await httpClient.GetStreamAsync(myNew.photoUrl);
            using var file = System.IO.File.OpenWrite(imagePath);

            await stream.CopyToAsync(file);
            Console.WriteLine($"Скачали фото {myNew.title.Substring(0, 20)}");

        }
        static string MakeRandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(5);

            for (int i = 0; i < 5; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
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
