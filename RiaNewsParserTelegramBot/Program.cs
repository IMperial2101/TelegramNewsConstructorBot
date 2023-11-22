
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
        int countdownTimeInSeconds = 5 * 60; // 5 минут в секундах

        


        while (true)
        {
            Console.WriteLine("Начало парсинга");
            await parser.StartParseNews();
            Console.WriteLine("Конец, ожидание 5 минут ", DateTime.Now);
            for (int i = countdownTimeInSeconds; i > 0; i--)
            {
                Console.Write($"Осталось {TimeSpan.FromSeconds(i)}"); // Пишем текст без перевода строки

                await Task.Delay(1000); // Задержка на 1 секунду (1000 миллисекунд)

                Console.SetCursorPosition(Console.CursorLeft - $"Осталось {TimeSpan.FromSeconds(i)}".Length, Console.CursorTop); // Перемещаем курсор влево
            }
        }

        Console.ReadLine();
        
        

        
        
       
    }



}
