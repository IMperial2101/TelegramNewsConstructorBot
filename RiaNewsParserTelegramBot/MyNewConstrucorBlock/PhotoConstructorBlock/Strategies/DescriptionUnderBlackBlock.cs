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
    internal class DescriptionUnderBlackBlock : AbstractPhotoConstructor, IPhotoConstructorStrategy
    {

        public Image MakePhoto(Image image, MyNew myNew)
        {
            MyTextPadding descriptionPadding = new MyTextPadding(60, 3, 5, 5);

            string[] colors = MyColorConverter.GetColorVariations(ColorVariationsEnum.Black_White);
            image = MakeImageWithBlackBlockAndGradient(image, colors[0],descriptionPadding.Top,descriptionPadding.Bottom);

            AddDescriptionBlock(image, descriptionPadding, myNew, colors[1]);
            AddDateWithBlackBlock(image, false, true, colors[0], colors[1]);

            return image;
        }
        private void AddDescriptionBlock(Image image, MyTextPadding descriptionPadding,MyNew myNew,string color)
        {
            RectangleF textRectangle = MakeRectangleWithPaddings(descriptionPadding.Top, descriptionPadding.Bottom, descriptionPadding.Left, descriptionPadding.Right, image.Width, image.Height);
            myNew.descriptionToSend = myNew.description[0];
            MyText descriptionText = new MyText(myNew.descriptionToSend, color, "Montserrat", textRectangle, StringAlignment.Center, StringAlignment.Far);

            AddTextOnImage(image, descriptionText);
        }
        private Image MakeImageWithBlackBlockAndGradient(Image image, string gradientColor,int textPaddingTop,int textPaddingBottom)
        {
            int blackRectangleHeight = CalculateRectangleHeight(textPaddingTop, textPaddingBottom, image.Height);
            Image imageWithBlackBlock = MakeImageWithBlackBlock(image, blackRectangleHeight, gradientColor);
            AddGradient(imageWithBlackBlock, blackRectangleHeight, gradientColor);
            return imageWithBlackBlock;
        }
        private Image MakeImageWithBlackBlock(Image originalImage, int blackRectangleHeight, string gradientColor)
        {
            int newWidth = originalImage.Width;
            int newHeight = originalImage.Height + blackRectangleHeight;

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
                Rectangle blackRect = new Rectangle(0, originalImage.Height, newWidth, blackRectangleHeight);
                Color semiTransparentColor = ColorTranslator.FromHtml($"#{gradientColor}");
                using (Brush semiTransparentBrush = new SolidBrush(semiTransparentColor))
                {
                    graphics.FillRectangle(semiTransparentBrush, blackRect);
                }
            }

            return newImage;
        }
        private void AddGradient(Image image, int blackRectangleHeight, string gradientColor)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                int rectangleHeight = (int)(image.Height * 0.5);

                int rectangleY = height - blackRectangleHeight - rectangleHeight;

                Rectangle gradientRect = new Rectangle(0, rectangleY, width, rectangleHeight + 1);

                Color color = Color.Transparent;
                Color endColor = ColorTranslator.FromHtml($"#{gradientColor}");

                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    gradientRect,
                    color,
                    endColor,
                    LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(gradientBrush, gradientRect);
                }
            }
        }
        private int CalculateRectangleHeight(float topPaddingPercent, float bottomPaddingPercent, int height)
        {
            float topPadding, bottomPadding;

            if (topPaddingPercent >= 100 || topPaddingPercent <= 0)
                topPadding = 0;
            else
                topPadding = (topPaddingPercent / 100) * height;

            if (bottomPaddingPercent >= 100 || bottomPaddingPercent <= 0)
                bottomPadding = 0;
            else
                bottomPadding = (bottomPaddingPercent / 100) * height;

            return (int)(height - topPadding - bottomPadding);
        }





    }
}
