﻿

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
            int countdownSeconds = 600; 
            await parser.MainParseFunction();


            while (countdownSeconds > 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Оставшееся время: {TimeSpan.FromSeconds(countdownSeconds).ToString(@"mm\:ss")}");

                await Task.Delay(1000);
                countdownSeconds--;
            }
        }
        

        
        Console.ReadLine();
       
    }
    

}
