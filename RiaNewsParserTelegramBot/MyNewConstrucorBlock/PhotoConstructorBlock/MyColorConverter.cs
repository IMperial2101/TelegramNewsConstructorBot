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
        Black_
    }
    public static class MyColorConverter
    {
        
        public static string black = "000000";
        public static string greyDark = "1E1E1E";
        public static string white = "FFFFFF";
        public static string purple = "D4CCFF";
        public static string paleGreen = "C2D1A7";
        public static string paleYellow = "E9CC86";
        public static string greyBlue = "7099A0";
        private static readonly Dictionary<ColorEnum, string> ColorMap = new Dictionary<ColorEnum, string>
                {
                { ColorEnum.Black, "000000" },
                { ColorEnum.White, "FFFFFF" },
                { ColorEnum.Red, "822B31" }
                };

        private static readonly Dictionary<ColorVariationsEnum, string[]> ColorVariationsMap = new Dictionary<ColorVariationsEnum, string[]>
            {
                { ColorVariationsEnum.Black_White, new string[] { black, white } },
                { ColorVariationsEnum.Black_Purple, new string[] { black, purple } },
                { ColorVariationsEnum.Black_PaleGreen, new string[] { black, paleGreen } },
                { ColorVariationsEnum.Black_PaleYellow, new string[] { black, paleYellow } },
                { ColorVariationsEnum.Black_GreyBlue, new string[] { black, greyBlue } },
                { ColorVariationsEnum.Black_, new string[] { black, "7099A0" } },

            };


        public static string GetColorCode(ColorEnum color)
        {
            if (ColorMap.ContainsKey(color))
            {
                return ColorMap[color];
            }
            throw new ArgumentException("ColorEnum not found in ColorMap");
        }

        public static string[] GetColorVariations(ColorVariationsEnum color)
        {
            if (ColorVariationsMap.ContainsKey(color))
            {
                return ColorVariationsMap[color];
            }
            throw new ArgumentException("ColorEnum not found in ColorVariationsMap");
        }
    }
}
