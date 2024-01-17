using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock
{
    public enum ColorEnum
    {
        Black,
        White,
        Red
    }
    public enum ColorVariationsEnum
    {
        Black_White,
        Black_Purple,
        Black_PaleGreen,
        Black_PaleYellow,
        Black_GreyBlue,
        Black_OrangeLava,
        Black_Peach,
        Black_Test,
        Black_PaleBlue,
    }
    public static class MyColorConverter
    {
        
        public readonly static string black = "000000";
        public readonly static string greyDark = "1E1E1E";
        public readonly static string white = "FDFDFD";
        public readonly static string purple = "D4CCFF";
        public readonly static string paleGreen = "C2D1A7";
        public readonly static string paleYellow = "E9CC86";
        public readonly static string greyBlue = "7099A0";
        public readonly static string orangeLava = "FD6236";
        public readonly static string peach = "FB7259";
        public readonly static string paleBlue = "8870FD";

        private static readonly Dictionary<ColorVariationsEnum, string[]> ColorVariationsMap = new Dictionary<ColorVariationsEnum, string[]>
            {
                { ColorVariationsEnum.Black_White, new string[] { black, white } },
                { ColorVariationsEnum.Black_Purple, new string[] { black, purple } },
                { ColorVariationsEnum.Black_PaleGreen, new string[] { black, paleGreen } },
                { ColorVariationsEnum.Black_PaleYellow, new string[] { black, paleYellow } },
                { ColorVariationsEnum.Black_GreyBlue, new string[] { black, greyBlue } },
                { ColorVariationsEnum.Black_OrangeLava, new string[] { black, orangeLava } },
                { ColorVariationsEnum.Black_Peach, new string[] { black, peach } },
                { ColorVariationsEnum.Black_PaleBlue, new string[] { black, paleBlue } },
            };


        public static string[] GetColorVariations(ColorVariationsEnum color)
        {
            if (ColorVariationsMap.ContainsKey(color))
            {
                return ColorVariationsMap[color];
            }
            throw new ArgumentException("ColorEnum not found in ColorVariationsMap");
        }
        public static ColorVariationsEnum GetRandomColorVariation()
        {
            Random random = new Random();
            Array values = Enum.GetValues(typeof(ColorVariationsEnum));
            ColorVariationsEnum randomColor = (ColorVariationsEnum)values.GetValue(random.Next(values.Length));
            return randomColor;
        }
    }
}
