﻿using NewsPropertyBot.ProxyClass;

namespace RiaNewsParserTelegramBot.PropertiesClass
{
    static class MyPropertiesStatic
    {
        public static Dictionary<string,string> userNames { get; set; }
        public static string botToken { get; set; }
        public static string channelID { get; set; }
        public static string parseLink { get; set; }
        public static string useProxy { get; set; }
        public static MyProxy myProxy { get; set; }
        public static string adminKey { get; set; }
        public static int timeBetweenMainParseMinutes { get; set; }
        public static int minViewCount { get; set; }
        public static int timeBetweenSendMessSeconds { get; set; }
        public static string smile { get; set; }
        public static string abzatcSmile { get; set; }
        public static string decoreLineSmile { get; set; }
        public static int minParagraphCount { get; set; }
        public static int maxParagraphCount { get; set; }
        public static int maxDescripSymbCount { get; set; }
        public static int ownerId { get; set; }
        public static Dictionary<string, int> WeightSendStrategies { get; set; }
        public static void MakeStaticProperties(MyProperties properties)
        {
            userNames = properties.userNames;
            botToken = properties.botToken;
            channelID = properties.channelID;
            parseLink = properties.parseLink;
            useProxy = properties.useProxy;
            myProxy = properties.myProxy;
            adminKey = properties.adminKey;
            timeBetweenMainParseMinutes = properties.timeBetweenMainParseMinutes;
            minViewCount = properties.minViewCount;
            timeBetweenSendMessSeconds = properties.timeBetweenSendMessSeconds;
            smile = properties.smile;
            abzatcSmile = properties.abzatcSmile;
            decoreLineSmile = properties.decoreLineSmile;
            minParagraphCount = properties.minParagraphCount;
            maxParagraphCount = properties.maxParagraphCount;
            maxDescripSymbCount = properties.maxDescripSymbCount;
            ownerId = properties.ownerId;
            WeightSendStrategies = properties.WeightSendStrategies;

        }

        public static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
    }

}
