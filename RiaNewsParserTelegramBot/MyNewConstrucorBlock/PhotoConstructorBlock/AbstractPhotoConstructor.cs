using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;


namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    internal abstract class AbstractPhotoConstructor
    {
        
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
       
        public void AddDateOnImage(Image image, bool right, bool up)
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
                graphics.DrawString(DateTime.Now.ToShortDateString(), font, brush, new PointF(x, y), stringFormat);
            }
        }
        public void AddDateWithBlackBlock(Image image, bool right, bool up, string blockColor, string dateColor)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                // Шрифт и размер текста
                Font font = new Font("Base 05", 50, FontStyle.Bold, GraphicsUnit.Pixel);

                // Цвет текста
                SolidBrush brush = new SolidBrush(ColorTranslator.FromHtml($"#{dateColor}"));

                // Получаем размер текста
                SizeF textSize = graphics.MeasureString(DateTime.Now.ToShortDateString(), font);

                // Определение координат прямоугольника в зависимости от угла
                float rectWidth = textSize.Width + 20; // Ширина прямоугольника с небольшим отступом
                float rectHeight = textSize.Height + 20; // Высота прямоугольника с небольшим отступом

                float rectX = right ? image.Width - rectWidth : 0; // Если текст справа, координата X - ширина прямоугольника, иначе 0
                float rectY = up ? 0 : image.Height - rectHeight; // Если текст сверху, координата Y - высота прямоугольника, иначе 0

                // Создаем прямоугольник для черного блока
                RectangleF blackRect = new RectangleF(rectX, rectY, rectWidth, rectHeight);

                // Рисуем черный прямоугольник за текстом
                graphics.FillRectangle(new SolidBrush(ColorTranslator.FromHtml($"#{blockColor}")), blackRect);

                // Определение координат текста внутри прямоугольника
                float textX = right ? rectX + 10 : rectX + rectWidth - 10 - textSize.Width; // Если текст справа, отступ слева, иначе отступ справа
                float textY = up ? rectY + 10 : rectY + rectHeight - 10 - textSize.Height; // Если текст сверху, отступ сверху, иначе отступ снизу

                // Наложение текста на изображение внутри прямоугольника
                graphics.DrawString(DateTime.Now.ToShortDateString(), font, brush, new PointF(textX, textY));
            }
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

        protected string makeDescriptionToSend(string description)
        {
            int indexOfDot = description.IndexOf(". "); 

            if (indexOfDot != -1)
                return description.Substring(indexOfDot + 2); // +2 для включения пробела после точки
            else
                return string.Empty;
        }

       



        
        
    }
    
}