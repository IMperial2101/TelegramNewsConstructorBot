﻿using NewsPropertyBot.NewClass;
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
    internal class TitleUnderWithDecorLine : AbstractPhotoConstructor, IPhotoConstructorStrategy
    {

        
        public Image MakePhoto(Image image, MyNew myNew, ColorVariationsEnum colorsVariation)
        {
            try
            {
                MyTextPadding mainTitlePadding = new MyTextPadding(70, 3, 5, 5);
                MyTextPadding linePadding = new MyTextPadding(60, 30, 45, 45);

                string[] colors = MyColorConverter.GetColorVariations(colorsVariation);
                image = MakeImageWithBlackBlockAndGradient(image, colors[0], mainTitlePadding.Top, mainTitlePadding.Bottom);

                AddTextBlockOnImage(image, mainTitlePadding, MakeCorrectTitle(myNew.title), colors[1]);
                AddLineBlockOnImage(image, linePadding, colors[1]);

                if (image.Height >= 1000 || image.Width >= 1900)
                    DrawGradientLines(image, linePadding, 450, 10, colors[1]);
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
        private void AddLineBlockOnImage(Image image, MyTextPadding textPadding, string color)
        {
            RectangleF lineRectangle = MakeRectangleWithPaddings(textPadding.Top, textPadding.Bottom, textPadding.Left, textPadding.Right, image.Width, image.Height);
            MyText lineUpText = new MyText(MyPropertiesStatic.decoreLineSmile, color, "Segoe UI Emoji", lineRectangle);
            AddTextOnImage(image, lineUpText);
        }
        private void AddTextBlockOnImage(Image image, MyTextPadding mainTitlePadding, string title, string textColor)
        {
            RectangleF textRectangle = MakeRectangleWithPaddings(mainTitlePadding.Top, mainTitlePadding.Bottom, mainTitlePadding.Left, mainTitlePadding.Right, image.Width, image.Height);
            MyText titleText = new MyText(title, textColor, "Montserrat", textRectangle, StringAlignment.Center, StringAlignment.Far);
            AddTextOnImage(image, titleText);
        }
        private Image MakeImageWithBlackBlockAndGradient(Image image, string gradientColor,int topPadding,int bottomPadding)
        {
            int blackRectangleHeight = CalculateRectangleHeight(topPadding, bottomPadding, image.Height);
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

                int rectangleHeight = 300;

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
       
        public void DrawGradientLines(Image image,MyTextPadding linePadding,  int lineWidth, int lineHeight, string htmlColor)
        {
            RectangleF rectangle = MakeRectangleWithPaddings(linePadding.Top, linePadding.Bottom, linePadding.Left, linePadding.Right, image.Width, image.Height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Color color = ColorTranslator.FromHtml("#"+htmlColor);

                using (LinearGradientBrush leftGradient = new LinearGradientBrush(new PointF(rectangle.Left, rectangle.Top - (rectangle.Height / 2)), new PointF(rectangle.Left - lineWidth, rectangle.Top - (rectangle.Height / 2)), color, Color.Transparent))
                {
                    graphics.FillRectangle(leftGradient, rectangle.Left - lineWidth+1, rectangle.Top + (rectangle.Height / 2), lineWidth, lineHeight);
                }

                using (LinearGradientBrush rightGradient = new LinearGradientBrush(new PointF(rectangle.Right, rectangle.Top - (rectangle.Height / 2)), new PointF(rectangle.Right + lineWidth, rectangle.Top - (rectangle.Height / 2)), color, Color.Transparent))
                {
                    graphics.FillRectangle(rightGradient, rectangle.Right , rectangle.Top + (rectangle.Height / 2), lineWidth, lineHeight);
                }

                
            }
        }
        public string GetStrategyName()
        {
            return "TitleUnderWithDecorLine";
        }





    }
}
