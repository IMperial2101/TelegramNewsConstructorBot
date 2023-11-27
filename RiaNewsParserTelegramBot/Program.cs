
using AngleSharp;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.ProxyClass;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.PropertiesClass;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static MyProxy proxy;
    static Parser parser;

    static async Task Main()
    {
        TelegramBot telegramBot = new TelegramBot(properties);
        MyProxy proxy = properties.myProxy;
        Parser parser = new Parser(telegramBot, properties);
        int timeBetweenMainParseMinutes = properties.timeBetweenMainParseMinutes;


        await parser.Start();       
    }
    static MyProperties ReadLineProperties()
    {
        string propertiesJSON = string.Empty;
        try
        {
            using (StreamReader streamReader = new StreamReader("properties.txt"))
            {
                propertiesJSON = streamReader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(propertiesJSON))
            {
                return JsonConvert.DeserializeObject<MyProperties>(propertiesJSON);
            }
            else
            {
                Console.WriteLine("Файл JSON пуст или не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка чтения файла JSON: " + ex.Message);
        }
        return null;
    }
 
}
