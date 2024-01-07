using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock
{
    internal class MyText
    {
        public MyText(string _text, string _color, string fontName,RectangleF _textRectangle, StringAlignment _alignment = StringAlignment.Center, StringAlignment _lineAlignment = StringAlignment.Center)
        {
            text = _text;
            color = _color;
            font = AdjustFontSize(text, fontName, _textRectangle);
            alignment = _alignment;
            lineAlignment = _lineAlignment;
            textRectangle = _textRectangle;
        }

        public MyText()
        {

        }

        public string text;
        public string color;
        public Font font;
        public StringAlignment alignment = StringAlignment.Center;
        public StringAlignment lineAlignment = StringAlignment.Center;
        public RectangleF textRectangle;
        public Font AdjustFontSize(string text, string fontName, RectangleF rect)
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
    }
}
