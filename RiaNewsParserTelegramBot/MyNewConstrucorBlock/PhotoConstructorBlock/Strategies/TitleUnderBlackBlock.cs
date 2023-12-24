using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    internal class TitleUnderBlackBlock : AbstractPhotoConstructor, IConstructor, IBlackBlock
    {
        Image image;
        RectangleF TextRectangle;
        public void AddImage(Image image)
        {
            this.image = image;
            TextRectangle = MakeRectangleWithPaddings(70, 3, 5, 5, image.Width, image.Height);
        }
        public Image AddBlackBlock(Image originalImage, int blackRectangleHeight)
        {
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height + blackRectangleHeight;

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {             
                graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
                Rectangle blackRect = new Rectangle(0, originalImage.Height, newWidth, blackRectangleHeight);
                Color semiTransparentColor = Color.FromArgb(255, Color.Black); // 128 - уровень прозрачности (0 - полностью прозрачный, 255 - непрозрачный)
                using (Brush semiTransparentBrush = new SolidBrush(semiTransparentColor))
                {
                    graphics.FillRectangle(semiTransparentBrush, blackRect);
                }
            }

            return newImage;
        }

        public void AddGradient(Image image)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                int rectangleHeight = 300;
                int distanceFromBottom = (int)TextRectangle.Height;

                int rectangleY = height - distanceFromBottom - rectangleHeight;

                Rectangle gradientRect = new Rectangle(0, rectangleY, width, rectangleHeight+1);


                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    gradientRect,
                    Color.Transparent,
                    Color.FromArgb(255, 0, 0, 0),
                    LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(gradientBrush, gradientRect);
                }
            }
        }

        public void AddTextOnImage(MyNew myNew, Image image)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.Alignment = StringAlignment.Center;
                stringFormat1.LineAlignment = StringAlignment.Far;
                RectangleF TextRectangle1 = MakeRectangleWithPaddings(70, 3, 5, 5, image.Width, image.Height);
                Font font1 = AdjustFontSize(graphics, myNew.title, TextRectangle1);

                Color color = ColorTranslator.FromHtml("#FFFFFF");

                Brush brush = new SolidBrush(color);

                graphics.DrawString(myNew.title, font1, brush, TextRectangle1, stringFormat1);

            }
        }

        public Image MakePhoto(Image image, MyNew myNew)
        {
            Image imageWithBlackBlock = AddBlackBlock(image, (int)TextRectangle.Height);
            AddGradient(imageWithBlackBlock);
            AddTextOnImage(myNew, imageWithBlackBlock);
            return imageWithBlackBlock;
        }
    }
}
