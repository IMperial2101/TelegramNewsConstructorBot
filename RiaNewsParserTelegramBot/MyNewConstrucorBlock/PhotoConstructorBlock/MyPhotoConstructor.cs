using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RiaNewsParserTelegramBot
{
    internal class MyPhotoConstructor : IPhotoConstructor
    {
        public void AddTextOnImage(MyNew myNew, Image image, Graphics graphics)
        {
            int width = image.Width;
            int height = image.Height;

            StringFormat stringFormat1 = new StringFormat();
            stringFormat1.Alignment = StringAlignment.Center;
            stringFormat1.LineAlignment = StringAlignment.Center;

            RectangleF rect1 = MakeWidhtWithPagging(5,width,height);
            Font font1 = AdjustFontSize(graphics, myNew.title, rect1);
            graphics.DrawString(myNew.title, font1, Brushes.White, rect1, stringFormat1);
        }

        public void AddGradient(Image image, Graphics graphics)
        {
            int width = image.Width;
            int height = image.Height;

            Rectangle gradientRect = new Rectangle(0, 0, width, height);

            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                gradientRect,
                Color.Transparent,
                Color.FromArgb(200, 0, 0, 0),
                LinearGradientMode.Vertical))
            {
                graphics.FillRectangle(gradientBrush, gradientRect);
            }
        }

        public Font AdjustFontSize(Graphics graphics, string text, RectangleF rect)
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

        public async Task DownloadImage(MyNew myNew)
        {
            string imagePath = Path.Combine(MyPropertiesStatic.imagesFolderPath, myNew.photoName + ".png");

            HttpClient httpClient = new HttpClient();

            using var stream = await httpClient.GetStreamAsync(myNew.photoUrl);
            using var file = System.IO.File.OpenWrite(imagePath);

            await stream.CopyToAsync(file);
            Console.WriteLine($"Скачали фото {myNew.title.Substring(0, 20)}");
        }

        private RectangleF MakeWidhtWithPagging(int paddingPercent,int width,int height)
        {
            float padding;
            float rectWidth;
            if (paddingPercent >= 100 || paddingPercent <= 0){
                padding = 0;
                rectWidth = width;
            }
            else{
                padding = (paddingPercent / 100) * width;
                rectWidth = width - 2 * padding;
            }           
            return new RectangleF(padding, height / 2, rectWidth, height / 2);
        }
}