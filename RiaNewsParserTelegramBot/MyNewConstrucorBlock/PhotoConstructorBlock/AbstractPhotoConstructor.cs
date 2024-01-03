using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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
        public Font AdjustFontSize(string text,string fontName, RectangleF rect)
        {
            Font font;
            SizeF textSize;
            int fontSize = 1000;

            using (Bitmap tempBitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage(tempBitmap))
                {
                    int stringsCount;
                    while (true)
                    {
                        font = new Font(fontName, fontSize, FontStyle.Bold);

                        textSize = graphics.MeasureString(text, font, (int)rect.Width);

                        if (textSize.Height > rect.Height)
                        {
                            fontSize--;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
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

        public void AddTextOnImage(Image image,MyText myText)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.Alignment = myText.alignment;
                stringFormat1.LineAlignment = myText.lineAlignment;              

                Color color = ColorTranslator.FromHtml($"#{myText.color}");

                Brush brush = new SolidBrush(color);

                graphics.DrawString(myText.text, myText.font, brush, myText.textRectangle, stringFormat1);
            }
        }
        public void AddGradientTextOnImage(Image image, string text, string color1, string color2, RectangleF textRectangle, StringAlignment alignment, StringAlignment lineAlignment)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = alignment;
                stringFormat.LineAlignment = lineAlignment;

                Font font = AdjustFontSize(graphics, text, textRectangle);

                // Конвертируем строки с цветами в объекты Color
                Color startColor = ColorTranslator.FromHtml($"#{color1}");
                Color endColor = ColorTranslator.FromHtml($"#{color2}");

                // Создаем градиентный кисть
                LinearGradientBrush gradientBrush = new LinearGradientBrush(textRectangle, startColor, endColor, LinearGradientMode.Horizontal);

                // Настраиваем цвет текста
                Brush textBrush = new SolidBrush(Color.Black); // Черный цвет для текста

                // Создаем градиентный текст
                GraphicsPath textPath = new GraphicsPath();
                textPath.AddString(text, font.FontFamily, (int)font.Style, font.Size, textRectangle, stringFormat);

                // Рисуем градиентный текст
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.FillPath(gradientBrush, textPath);
            }
        }

        public void AddDateOnImage(Image image, bool right, bool up, string date)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                // Шрифт и размер текста
                Font font = new Font("Base 05", 50, FontStyle.Bold, GraphicsUnit.Pixel);

                // Цвет текста
                SolidBrush brush = new SolidBrush(Color.White);

                // Определение положения текста
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = right ? StringAlignment.Far : StringAlignment.Near;
                stringFormat.LineAlignment = up ? StringAlignment.Near : StringAlignment.Far;

                // Определение координат текста в зависимости от угла и расстояния от угла
                int x = right ? image.Width - 20 : 20; // Если текст справа, то отступ слева, иначе отступ справа
                int y = up ? 20 : image.Height - 20; // Если текст сверху, то отступ снизу, иначе отступ сверху

                // Наложение текста на изображение
                graphics.DrawString(date, font, brush, new PointF(x, y), stringFormat);
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
            public static string black = "000000";
            public static string greyDark = "1E1E1E";
            public static string white = "FFFFFF";
            public static string purple = "D4CCFF";
            public static string paleGreen = "C2D1A7";
            public static string paleYellow = "E9CC86";
            public static string greyBlue = "7099A0";
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