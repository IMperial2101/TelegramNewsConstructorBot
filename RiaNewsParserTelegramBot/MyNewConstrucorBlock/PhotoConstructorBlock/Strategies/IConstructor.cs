using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    public interface IConstructor
    {
        public abstract void AddTextOnImage(MyNew myNew, Image image);
        public abstract void AddGradient(Image image);
        public abstract Image MakePhoto(Image image, MyNew myNew);
        public abstract void AddImage(Image image);
    }
}
