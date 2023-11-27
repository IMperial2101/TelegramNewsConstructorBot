using NewsPropertyBot.ProxyClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.PropertiesClass
{
    public class MyProperties
    {
        public string botToken { get; set; }
        public string channelID { get; set; }
        public string parseLink { get; set; }
        public string useProxy { get; set; }
        public MyProxy myProxy { get; set; }
        public int timeBetweenMainParseMinutes { get; set; }
        public int minViewCount { get; set; }
        public int timeBetweenSendMessSeconds { get; set; }
        public string smile { get; set; }
        public int maxParagraphCount { get; set; }
        public int maxDescripSymbCount { get; set; }
    }
}
