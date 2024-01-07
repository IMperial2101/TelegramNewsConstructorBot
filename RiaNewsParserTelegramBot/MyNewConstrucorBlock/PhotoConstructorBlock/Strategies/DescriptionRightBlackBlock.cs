using NewsPropertyBot.NewClass;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    internal class DescriptionRightBlackBlock : AbstractPhotoConstructor, IConstructor
    {
        public Image MakePhoto(Image image, MyNew myNew)
        {
            MyTextPadding descriptionPadding = new MyTextPadding(7, 7, 55, 5);

            string[] colors = MyColorConverter.GetColorVariations(ColorVariationsEnum.Black_White);
            image = MakeImageWithBlackBlockAndGradient(image, colors[0], descriptionPadding.Left, descriptionPadding.Right);
            AddDescriptionBlock(image, descriptionPadding, myNew, colors[1]);

            AddDateWithBlackBlock(image, false, true, colors[0], colors[1]);

            return image;
        }

        private void AddDescriptionBlock(Image image, MyTextPadding descriptionPadding, MyNew myNew, string color)
        {
            RectangleF textRectangle = MakeRectangleWithPaddings(descriptionPadding.Top, descriptionPadding.Bottom, descriptionPadding.Left, descriptionPadding.Right, image.Width, image.Height);
            myNew.descriptionToSend = makeDescriptionToSend(myNew.description[0]);
            MyText descriptionText = new MyText(myNew.descriptionToSend, color, "Montserrat", textRectangle, StringAlignment.Far, StringAlignment.Center);

            AddTextOnImage(image, descriptionText);
        }

        private Image MakeImageWithBlackBlockAndGradient(Image originalImage, string gradientColor, int textPaddingLeft, int textPaddingRight)
        {
            int blackRectangleWidth = CalculateRectangleWidth(textPaddingLeft, textPaddingRight, originalImage.Width);
            Image imageWithBlackBlock = MakeImageWithBlackBlockWide(originalImage, blackRectangleWidth, gradientColor);
            AddGradient(imageWithBlackBlock, blackRectangleWidth, gradientColor);
            return imageWithBlackBlock;
        }

        private Image MakeImageWithBlackBlockWide(Image originalImage, int blackRectangleWidth, string gradientColor)
        {
            int newWidth = originalImage.Width + blackRectangleWidth;
            int newHeight = originalImage.Height;

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                Rectangle blackRect = new Rectangle(originalImage.Width, 0, blackRectangleWidth, newHeight);
                Color semiTransparentColor = ColorTranslator.FromHtml($"#{gradientColor}");
                using (Brush semiTransparentBrush = new SolidBrush(semiTransparentColor))
                {
                    graphics.FillRectangle(semiTransparentBrush, blackRect);
                }
                graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
            }

            return newImage;
        }

        private void AddGradient(Image image, int blackRectangleWidth, string gradientColor)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                int rectangleWidth = (int)(width * 0.3);

                Rectangle gradientRect = new Rectangle(width - blackRectangleWidth -rectangleWidth , 0, rectangleWidth, height);

                Color color = Color.Transparent;
                Color endColor = ColorTranslator.FromHtml($"#{gradientColor}");

                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    gradientRect,        
                    color,
                    endColor,
                    LinearGradientMode.Horizontal))
                {
                    graphics.FillRectangle(gradientBrush, gradientRect);
                }
            }
        }

        private int CalculateRectangleWidth(float leftPaddingPercent, float rightPaddingPercent, int width)
        {
            float leftPadding, rightPadding;

            if (leftPaddingPercent >= 100 || leftPaddingPercent <= 0)
                leftPadding = 0;
            else
                leftPadding = (leftPaddingPercent / 100) * width;

            if (rightPaddingPercent >= 100 || rightPaddingPercent <= 0)
                rightPadding = 0;
            else
                rightPadding = (rightPaddingPercent / 100) * width;

            return (int)(width - leftPadding - rightPadding);
        }
    }
}
