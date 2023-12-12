using NewsPropertyBot.ProxyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.PropertiesClass
{
    static class MyPropertiesStatic
    {
        public static string botToken { get; set; }
        public static string[] channelID { get; set; }
        public static string parseLink { get; set; }
        public static string useProxy { get; set; }
        public static MyProxy myProxy { get; set; }
        public static bool randomMessageDesign { get; set; }
        public static int sendPhotoPercent { get; set; }
        public static int sendPhotoWithTextRandomPercent { get; set; }
        public static int sendSecondTitleRandomPerсent { get; set; }
        public static int timeBetweenMainParseMinutes { get; set; }
        public static int minViewCount { get; set; }
        public static int timeBetweenSendMessSeconds { get; set; }
        public static string smile { get; set; }
        public static int maxParagraphCount { get; set; }
        public static int maxDescripSymbCount { get; set; }
        public static int ownerId { get; set; }
    }
}
