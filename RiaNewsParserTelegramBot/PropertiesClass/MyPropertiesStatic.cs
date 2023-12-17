using NewsPropertyBot.ProxyClass;

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
        public static void MakeStaticProperties(MyProperties properties)
        {
            MyPropertiesStatic.botToken = properties.botToken;
            MyPropertiesStatic.channelID = properties.channelID;
            MyPropertiesStatic.parseLink = properties.parseLink;
            MyPropertiesStatic.useProxy = properties.useProxy;
            MyPropertiesStatic.myProxy = properties.myProxy;
            MyPropertiesStatic.randomMessageDesign = properties.randomMessageDesign;
            MyPropertiesStatic.sendPhotoPercent = properties.sendPhotoPercent;
            MyPropertiesStatic.sendPhotoWithTextRandomPercent = properties.sendPhotoWithTextRandomPercent;
            MyPropertiesStatic.sendSecondTitleRandomPerсent = properties.sendSecondTitleRandomPerсent;
            MyPropertiesStatic.timeBetweenMainParseMinutes = properties.timeBetweenMainParseMinutes;
            MyPropertiesStatic.minViewCount = properties.minViewCount;
            MyPropertiesStatic.timeBetweenSendMessSeconds = properties.timeBetweenSendMessSeconds;
            MyPropertiesStatic.smile = properties.smile;
            MyPropertiesStatic.maxParagraphCount = properties.maxParagraphCount;
            MyPropertiesStatic.maxDescripSymbCount = properties.maxDescripSymbCount;
            MyPropertiesStatic.ownerId = properties.ownerId;
        }

        public static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
    }

}
