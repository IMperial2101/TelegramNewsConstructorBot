using NewsPropertyBot.NewClass;
using System.Drawing;

namespace RiaNewsParserTelegramBot
{
    public interface IPhotoConstructor
    {
        Task DownloadImage(MyNew myNew);
        void AddGradient(Image image, Graphics graphics);
        void AddTextOnImage(MyNew myNew, Image image, Graphics graphics);
        Font AdjustFontSize(Graphics graphics, string text, RectangleF rect);
    }
}
