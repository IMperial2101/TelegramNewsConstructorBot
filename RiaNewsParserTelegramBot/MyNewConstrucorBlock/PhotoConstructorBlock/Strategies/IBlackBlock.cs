using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    public interface IBlackBlock
    {
        public abstract Image AddBlackBlock(Image originalImage, int blackRectangleHeight);

    }
}
