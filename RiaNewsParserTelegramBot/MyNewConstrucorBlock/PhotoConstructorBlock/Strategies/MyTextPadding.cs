using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    public class MyTextPadding
    {
        public MyTextPadding(int top, int bottom, int left, int right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
        public MyTextPadding()
        {

        }
        private int textPaddingTop = 0;
        private int textPaddingBottom = 0;
        private int textPaddingLeft = 0;
        private int textPaddingRight = 0;

        public int Top
        {
            get { return textPaddingTop; }
            set
            {
                if (value < 0 || value > 100)
                    textPaddingTop = 0;
                else
                    textPaddingTop = value;
            }
        }
        public int Bottom
        {
            get { return textPaddingBottom; }
            set
            {
                if (value < 0 || value > 100)
                    textPaddingBottom = 0;
                else
                    textPaddingBottom = value;
            }
        }
        public int Left
        {
            get { return textPaddingLeft; }
            set
            {
                if (value < 0 || value > 100)
                    textPaddingLeft = 0;
                else
                    textPaddingLeft = value;
            }
        }
        public int Right
        {
            get { return textPaddingRight; }
            set
            {
                if (value < 0 || value > 100)
                    textPaddingRight = 0;
                else
                    textPaddingRight = value;
            }
        }
    }
}
