using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using static RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies.AbstractPhotoConstructor;
using System.Drawing.Drawing2D;
using System.Drawing;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;

internal class DescriptionLeftBlackBlock : AbstractPhotoConstructor, IConstructor
{
    private const int textPaddingTop = 7;
    private const int textPaddingBottom = 7;
    private const int textPaddingLeft = 5;
    private const int textPaddingRight = 55;
    
    public Image MakePhoto(Image image, MyNew myNew)
    {
        string[] colors = MyColorConverter.GetColorVariations(ColorVariationsEnum.Black_White);
        Image finalphoto = MakeImageWithBlackBlockAndGradient(image, colors[0]);

        RectangleF textRectangle = MakeRectangleWithPaddings(textPaddingTop, textPaddingBottom, textPaddingLeft, textPaddingRight, finalphoto.Width, finalphoto.Height);
        myNew.descriptionToSend = makeDescriptionToSend(myNew.description[0]);
        MyText descriptionText = new MyText(myNew.descriptionToSend, colors[1],"Montserrat",textRectangle, StringAlignment.Near,StringAlignment.Center);

        AddTextOnImage(finalphoto, descriptionText);
        AddDateWithBlackBlock(finalphoto, true, false, colors[0], colors[1]);

        return finalphoto;
    }
    private Image MakeImageWithBlackBlockAndGradient(Image image, string gradientColor)
    {
        int blackRectangleWidth = CalculateRectangleWidth(textPaddingLeft, textPaddingRight, image.Width);
        Image imageWithBlackBlock = MakeImageWithBlackBlockWide(image, blackRectangleWidth, gradientColor);
        AddGradient(imageWithBlackBlock, blackRectangleWidth, gradientColor);
        return imageWithBlackBlock;
    }
    private Image MakeImageWithBlackBlock(Image originalImage, int blackRectangleWidth, string gradientColor)
    {
        int newWidth = originalImage.Width;
        int newHeight = originalImage.Height;

        Bitmap newImage = new Bitmap(newWidth, newHeight);

        using (Graphics graphics = Graphics.FromImage(newImage))
        {
            // Рисуем фотографию
            graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));

            // Создаем черный прямоугольник, накладывающийся слева
            Rectangle blackRect = new Rectangle(0, 0, blackRectangleWidth, newHeight);
            Color semiTransparentColor = ColorTranslator.FromHtml($"#{gradientColor}");
            using (Brush semiTransparentBrush = new SolidBrush(semiTransparentColor))
            {
                graphics.FillRectangle(semiTransparentBrush, blackRect);
            }
        }

        return newImage;
    }
    private Image MakeImageWithBlackBlockWide(Image originalImage, int blackRectangleWidth, string gradientColor)
    {
        int newWidth = originalImage.Width + blackRectangleWidth;
        int newHeight = originalImage.Height;

        Bitmap newImage = new Bitmap(newWidth, newHeight);

        using (Graphics graphics = Graphics.FromImage(newImage))
        {
            Rectangle blackRect = new Rectangle(0, 0, blackRectangleWidth, newHeight);
            Color semiTransparentColor = ColorTranslator.FromHtml($"#{gradientColor}");
            using (Brush semiTransparentBrush = new SolidBrush(semiTransparentColor))
            {
                graphics.FillRectangle(semiTransparentBrush, blackRect);
            }
            graphics.DrawImage(originalImage, new Rectangle(blackRectangleWidth, 0, originalImage.Width, originalImage.Height));
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

            Rectangle gradientRect = new Rectangle(blackRectangleWidth-1, 0, rectangleWidth, height);

            Color color = Color.Transparent;
            Color endColor = ColorTranslator.FromHtml($"#{gradientColor}");

            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                gradientRect,
                endColor,
                color,
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
