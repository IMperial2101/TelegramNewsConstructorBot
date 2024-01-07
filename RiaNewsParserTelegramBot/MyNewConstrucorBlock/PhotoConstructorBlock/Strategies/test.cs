using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock
{
    internal class test : AbstractPhotoConstructor, IConstructor
    {
        public void AddGradient(Image image)
        {
            int width = image.Width;
            int height = image.Height;

            // Вычисляем новые размеры с добавлением 200 пикселей снизу
            int newWidth = width;
            int newHeight = height + 200;

            // Создаем новое изображение с новыми размерами
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.Clear(Color.Transparent); // Заполняем новое изображение прозрачным цветом

                // Определяем размеры и позицию для вставки изображения с сохранением пропорций
                int destWidth, destHeight, destX, destY;

                if ((float)width / height > (float)newWidth / newHeight)
                {
                    destWidth = newWidth;
                    destHeight = (int)(height * newWidth / (float)width);
                    destX = 0;
                    destY = (newHeight - destHeight) / 2;
                }
                else
                {
                    destHeight = newHeight;
                    destWidth = (int)(width * newHeight / (float)height);
                    destX = (newWidth - destWidth) / 2;
                    destY = 0;
                }

                // Рисуем оригинальное изображение в верхней части нового изображения
                graphics.DrawImage(image, new Rectangle(destX, destY, destWidth, destHeight));

                // Рисуем черный прямоугольник внизу нового изображения
                Rectangle blackRect = new Rectangle(0, height, newWidth, 200);
                using (Brush blackBrush = new SolidBrush(Color.Black))
                {
                    graphics.FillRectangle(blackBrush, blackRect);
                }
                newImage.Save(Path.Combine(MyPropertiesStatic.imagesFolderPath, "test") + "Done.png", ImageFormat.Png);
            }

        }

        public void AddGradient(Image image, int blackRectangleHeight)
        {
            throw new NotImplementedException();
        }

        public void AddImage(Image image)
        {
            throw new NotImplementedException();
        }

        public void AddTextOnImage(MyNew myNew, Image image)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                StringFormat stringFormat1 = new StringFormat();
                stringFormat1.Alignment = StringAlignment.Center;
                stringFormat1.LineAlignment = StringAlignment.Center;

                RectangleF rect1 = MakeRectangleWithPaddings(50, 5, 5, 5, width, height);
                Font font1 = AdjustFontSize(graphics, myNew.title, rect1);

                Color color = ColorTranslator.FromHtml("#FD7E4F");

                Brush brush = new SolidBrush(color);

                graphics.DrawString(myNew.title, font1, brush, rect1, stringFormat1);

            }
        }

        public void MakePhoto(Image image, MyNew myNew)
        {
            throw new NotImplementedException();
        }

        Image IConstructor.MakePhoto(Image image, MyNew myNew)
        {
            throw new NotImplementedException();
        }
    }
}
