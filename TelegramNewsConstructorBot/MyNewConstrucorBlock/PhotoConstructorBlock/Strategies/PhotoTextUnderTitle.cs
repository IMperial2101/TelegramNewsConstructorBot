﻿using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock
{
    internal class PhotoTextUnderTitle : AbstractPhotoConstructor, IConstructor
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

                RectangleF rect1 = MakeRectangleWithPaddings(50, 5, 5, 5, width, height);
                Font font1 = AdjustFontSize(graphics, myNew.title, rect1);

                Color color = ColorTranslator.FromHtml("#FD7E4F"); 

                Brush brush = new SolidBrush(color);

                graphics.DrawString(myNew.title, font1, brush, rect1, stringFormat1);
                
            }
        }

        public void MakePhoto(Image image, MyNew myNew)
        {
            AddGradient(image);
            AddTextOnImage(myNew, image);
        }

        Image IConstructor.MakePhoto(Image image, MyNew myNew)
        {
            throw new NotImplementedException();
        }
    }
}
