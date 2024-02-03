using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    internal class TitleWithDescription : AbstractPhotoConstructor, IPhotoConstructorStrategy
    {

        public Image MakePhoto(Image image, MyNew myNew, ColorVariationsEnum colorsVariation)
        {
            try
            {
                MyTextPadding descriptionPadding = new MyTextPadding(70, 3, 5, 5);
                MyTextPadding titlePadding = new MyTextPadding(50, 30, 5, 5);

                string[] colors = MyColorConverter.GetColorVariations(colorsVariation);
                image = MakeImageWithBlackBlockAndGradient(image, MyColorConverter.black, titlePadding.Top, 0);

                AddTitleBlockOnImage(image, titlePadding, MakeCorrectTitle(myNew.title), colors[1]);
                AddDescriptionBlockOnImage(image, descriptionPadding, MakeCorrectDescription(myNew.description[0]), MyColorConverter.white);


                AddDateWithBlackBlock(image, false, true, colors[0], colors[1]);
            }
            catch (Exception ex)
            {
                myTelegramBot.SendMessageToOwner($"Ошибка создания фотографии: {ex.Message} - {myNew.url}\n" +
                        $"Стратегия отправки {GetStrategyName()}");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            return image;
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
        private void AddTitleBlockOnImage(Image image,MyTextPadding titlePadding,string title,string color)
        {
            RectangleF titleRectangle = MakeRectangleWithPaddings(titlePadding.Top, titlePadding.Bottom, titlePadding.Left, titlePadding.Right, image.Width, image.Height);
            MyText titleText = new MyText(title, color, "Montserrat", titleRectangle, StringAlignment.Center, StringAlignment.Far);
            AddTextOnImage(image, titleText);
        }
        private void AddDescriptionBlockOnImage(Image image,MyTextPadding descriptionPadding,string description, string color)
        {
            RectangleF textRectangle = MakeRectangleWithPaddings(descriptionPadding.Top, descriptionPadding.Bottom, descriptionPadding.Left, descriptionPadding.Right, image.Width, image.Height);
            MyText descriptionText = new MyText(description, color, "Montserrat", textRectangle, StringAlignment.Center, StringAlignment.Near);
            AddTextOnImage(image, descriptionText);
        }
        public string GetStrategyName()
        {
            return "TitleWithDescription";
        }



    }
}
