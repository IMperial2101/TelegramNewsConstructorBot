using System.Drawing;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    internal abstract class AbstractPhotoConstructor
    {
        public Font AdjustFontSize(Graphics graphics, string text, RectangleF rect)
        {
            Font font;
            SizeF textSize;
            int fontSize = 1000;

            int stringsCount;
            while (true)
            {
                font = new Font("Montserrat", fontSize, FontStyle.Bold);

                textSize = graphics.MeasureString(text, font, (int)rect.Width);

                if (textSize.Height > rect.Height)
                {
                    fontSize--;
                }
                else
                    break;
            }

            return font;
        }

        public RectangleF MakeRectangleWithPaddings(float topPaddingPercent, float bottomPaddingPercent, float leftPaddingPercent, float rightPaddingPercent, int width, int height)
        {
            float topPadding, bottomPadding, leftPadding, rightPadding;
            float rectWidth, rectHeight;

            if (topPaddingPercent >= 100 || topPaddingPercent <= 0)
                topPadding = 0;
            else
                topPadding = (topPaddingPercent / 100) * height;

            if (bottomPaddingPercent >= 100 || bottomPaddingPercent <= 0)
                bottomPadding = 0;
            else
                bottomPadding = (bottomPaddingPercent / 100) * height;

            if (leftPaddingPercent >= 100 || leftPaddingPercent <= 0)
                leftPadding = 0;
            else
                leftPadding = (leftPaddingPercent / 100) * width;

            if (rightPaddingPercent >= 100 || rightPaddingPercent <= 0)
                rightPadding = 0;
            else
                rightPadding = (rightPaddingPercent / 100) * width;

            rectWidth = width - leftPadding - rightPadding;
            rectHeight = height - topPadding - bottomPadding;

            return new RectangleF(leftPadding, topPadding, rectWidth, rectHeight);
        }

        public void AddTextOnImage(Image image, string text, string textColor, RectangleF textRectangle,StringAlignment alignment, StringAlignment lineAligment )
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.Alignment = alignment;
                stringFormat1.LineAlignment = lineAligment;

                Font font = AdjustFontSize(graphics, text, textRectangle);

                Color color = ColorTranslator.FromHtml($"#{textColor}");

                Brush brush = new SolidBrush(color);

                graphics.DrawString(text, font, brush, textRectangle, stringFormat1);
            }
        }

        public enum ColorEnum
        {
            Black,
            White,
            Red
        }
        public enum ColorVariationsEnum
        {
            Black_White,
            Black_Purple,
            Black_PaleGreen,
            Black_PaleYellow,
            Black_GreyBlue,
            Black_
        }
        public static class ColorConverter
        {
            static string black = "000000";
            static string greyDark = "1E1E1E";
            static string white = "FFFFFF";
            static string purple = "D4CCFF";
            static string paleGreen = "C2D1A7";
            static string paleYellow = "E9CC86";
            static string greyBlue = "7099A0";
            private static readonly Dictionary<ColorEnum, string> ColorMap = new Dictionary<ColorEnum, string>
                {
                { ColorEnum.Black, "000000" },
                { ColorEnum.White, "FFFFFF" },
                { ColorEnum.Red, "822B31" }
                };

            private static readonly Dictionary<ColorVariationsEnum, string[]> ColorVariationsMap = new Dictionary<ColorVariationsEnum, string[]>
            {
                { ColorVariationsEnum.Black_White, new string[] { black, white } },
                { ColorVariationsEnum.Black_Purple, new string[] { black, purple } },
                { ColorVariationsEnum.Black_PaleGreen, new string[] { black, paleGreen } },
                { ColorVariationsEnum.Black_PaleYellow, new string[] { black, paleYellow } },
                { ColorVariationsEnum.Black_GreyBlue, new string[] { black, greyBlue } },
                { ColorVariationsEnum.Black_, new string[] { black, "7099A0" } },

            };


            public static string GetColorCode(ColorEnum color)
            {
                if (ColorMap.ContainsKey(color))
                {
                    return ColorMap[color];
                }
                throw new ArgumentException("ColorEnum not found in ColorMap");
            }

            public static string[] GetColorVariations(ColorVariationsEnum color)
            {
                if (ColorVariationsMap.ContainsKey(color))
                {
                    return ColorVariationsMap[color];
                }
                throw new ArgumentException("ColorEnum not found in ColorVariationsMap");
            }
        }
    }
}