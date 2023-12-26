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
    internal class PhotoTextUnderDescription : AbstractPhotoConstructor,IConstructor
    {
        public void AddGradient(Image image)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                int width = image.Width;
                int height = image.Height;

                Rectangle gradientRect = new Rectangle(0, 0, width, height);

                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    gradientRect,
                    Color.Transparent,
                    Color.FromArgb(200, 0, 0, 0),
                    LinearGradientMode.Vertical))
                {
                    graphics.FillRectangle(gradientBrush, gradientRect);
                }
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

                RectangleF rect1 = MakeRectangleWithPaddings(50,5,5,5,width,height);
                Font font1 = AdjustFontSize(graphics, MakeDescription(myNew), rect1);
                graphics.DrawString(MakeDescription(myNew), font1, Brushes.White, rect1, stringFormat1);
            }
        }

        public void MakePhoto(Image image, MyNew myNew)
        {
            throw new NotImplementedException();
        }

        private string MakeDescription(MyNew myNew)
        {
            if (myNew.description[0].Length < 500)
                myNew.descriptionToSend = myNew.description[0];
            return myNew.descriptionToSend;
        }

        Image IConstructor.MakePhoto(Image image, MyNew myNew)
        {
            throw new NotImplementedException();
        }
    }
}
