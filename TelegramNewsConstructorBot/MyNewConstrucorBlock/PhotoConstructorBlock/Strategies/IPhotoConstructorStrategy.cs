﻿using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies
{
    public interface IPhotoConstructorStrategy
    {
        Image MakePhoto(Image image, MyNew myNew, ColorVariationsEnum colors);
        string GetStrategyName();
    }
}
