using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    abstract class AbstractPhotoConstructor
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

                textSize = graphics.MeasureString(text, font);

                stringsCount = (int)(textSize.Width / rect.Width);

                if (stringsCount * textSize.Height > rect.Height)
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

    }
}
