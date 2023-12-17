using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock
{
    public interface INewsConstructor
    {
        void MakeMyNewProperties(MyNew myNew);
        string MakeDescriptionRandomDesign(MyNew myNew);



    }
}
