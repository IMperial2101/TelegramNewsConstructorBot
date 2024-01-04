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
    internal class TestStrategyTitleUnder : AbstractPhotoConstructor, IConstructor
    {

        TextPadding mainTitlePadding = new TextPadding(70, 3, 5, 5);
        TextPadding linePadding = new TextPadding(60,30,10,10);
        public Image MakePhoto(Image image, MyNew myNew)
        {
            string[] colors = MyColorConverter.GetColorVariations(ColorVariationsEnum.Black_White);
            Image finalphoto = MakeImageWithBlackBlockAndGradient(image, colors[0],mainTitlePadding.Top,mainTitlePadding.Bottom);

            RectangleF textRectangle = MakeRectangleWithPaddings(mainTitlePadding.Top, mainTitlePadding.Bottom, mainTitlePadding.Left, mainTitlePadding.Right, finalphoto.Width, finalphoto.Height);         
            MyText titleText = new MyText(myNew.title, colors[1], "Montserrat", textRectangle, StringAlignment.Center, StringAlignment.Far);
            AddTextOnImage(finalphoto, titleText);

            RectangleF lineRectangle = MakeRectangleWithPaddings(linePadding.Top, linePadding.Bottom, linePadding.Left, linePadding.Right, finalphoto.Width, finalphoto.Height);
            MyText lineUpText = new MyText("🌍", MyColorConverter.white, "Segoe UI Emoji", lineRectangle);
            AddTextOnImage(finalphoto, lineUpText);
            DrawGradientLines(finalphoto, lineRectangle,200,10,MyColorConverter.white);


            AddDateWithBlackBlock(finalphoto, false, true);

            return finalphoto;
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


        //Нужно доделать эту функцию
        public void DrawGradientLines(Image image, RectangleF rectangle, int lineWidth, int lineHeight, string htmlColor)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Color color = ColorTranslator.FromHtml("#"+htmlColor);


                using (LinearGradientBrush leftGradient1 = new LinearGradientBrush(
                    new PointF(rectangle.Left, rectangle.Top),
                    new PointF(rectangle.Left + lineWidth, rectangle.Top),
                    Color.Transparent,
                    color))
                {
                    using (LinearGradientBrush rightGradient1 = new LinearGradientBrush(
                        new PointF(rectangle.Right, rectangle.Top / 2),
                        new PointF(rectangle.Right - lineWidth, rectangle.Top / 2),
                        color,
                        Color.Transparent))
                    {
                        graphics.FillRectangle(leftGradient1, rectangle.Left - lineWidth, rectangle.Top, lineWidth, lineHeight);
                        graphics.FillRectangle(rightGradient1, rectangle.Right, rectangle.Top, lineWidth, lineHeight);
                    }
                }
            }
        }






    }
}
