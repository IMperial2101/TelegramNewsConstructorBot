
using AngleSharp;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.ProxyClass;
using NewsPropertyBot.TelegramBotClass;
using System.Net;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Types;

class Program
{
    static MyProxy proxy;
    static Parser parser;
    
    static async Task Main()
    {
        TelegramBot telegramBot = new TelegramBot();
        proxy = new MyProxy("91.188.243.122",9487, "XrVHcG", "pthNrV");
        parser = new Parser(proxy.GetWebProxy(), "https://ria.ru/world/", telegramBot);


        
        while(true)
        {
            await parser.StartParseNews();
            await Task.Delay(TimeSpan.FromMinutes(5));

        }
        

        
        
       
    }



}
