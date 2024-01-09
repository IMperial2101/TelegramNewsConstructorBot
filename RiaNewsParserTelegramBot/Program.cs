using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using RiaNewsParserTelegramBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static async Task Main()
    {


        Random random = new Random();
        MyPropertiesStatic.MakeStaticProperties(properties);
        MyTelegramBot telegramBot = new MyTelegramBot();
        Parser parser = new Parser(telegramBot);
        MakeImagesFolder();

        
        MyNew myNew;
        myNew = await parser.ParseOneNewAsync("https://ria.ru/20240109/perevooruzhenie-1917044593.html");
        Console.WriteLine(myNew.title.Length);
        MyNewTelegramSendler telegramSendler = new MyNewTelegramSendler(telegramBot);
        if (telegramSendler.CheckNewAdjust(myNew, new SendPhotoWithTitle()))
            telegramSendler.SendNew(myNew, new SendPhotoWithTitle(), MyPropertiesStatic.channelID[0]);
        else
            Console.WriteLine($"Новость {myNew.title.Substring(0,15)} не прошла проверку");
        Console.ReadLine();
        


        //await parser.FirstParseAddLinks();
        while (true)
        {
            Console.WriteLine("Начало парсинга");
            List<MyNew> newsList = await parser.ParseNews();
            
            //выбор типа новости
            //конструирование новости
            //отправка новостей

            Console.WriteLine($"Конец, ожидание {MyPropertiesStatic.timeBetweenMainParseMinutes} минут {DateTime.Now}\n");
            await Task.Delay(TimeSpan.FromMinutes(MyPropertiesStatic.timeBetweenMainParseMinutes));
        }
        Console.ReadLine();
     
    }

    static MyProperties ReadLineProperties()
    {
        string propertiesJSON = string.Empty;
        try{
            using (StreamReader streamReader = new StreamReader("properties.txt"))
                propertiesJSON = streamReader.ReadToEnd();

            if (!string.IsNullOrEmpty(propertiesJSON))
                return JsonConvert.DeserializeObject<MyProperties>(propertiesJSON);
            else
                Console.WriteLine("Файл JSON пуст или не найден.");
        }
        catch (Exception ex){
            Console.WriteLine("Ошибка чтения файла JSON: " + ex.Message);
        }
        return null;
    }   
    static void MakeImagesFolder()
    {
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images")))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images"));
        }
    }





}
