using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static async Task Main()
    {
        Random random = new Random();
        MyPropertiesStatic.MakeStaticProperties(properties);
        TelegramBot telegramBot = new TelegramBot();
        Parser parser = new Parser(telegramBot);
        MakeImagesFolder();

        MyNew myNew;
        myNew = await parser.ParseOneNewAsync("https://ria.ru/20240104/poteri-1919702769.html");
        PhotoConstructor photoConstructor1 = new PhotoConstructor();
        photoConstructor1.MakePhoto(myNew, new DescriptionLeftBlackBlock());

        Console.ReadLine();


        List<IConstructor> strateges = new List<IConstructor>();
        strateges.Add(new DescriptionLeftBlackBlock());
        strateges.Add(new DescriptionUnderBlackBlock());
        strateges.Add(new TitleUnderBlackBlock());
        //await parser.FirstParseAddLinks();
        while (true)
        {
            Console.WriteLine("Начало парсинга");
            List<MyNew> newsList = await parser.ParseNews();
            PhotoConstructor photoConstructor = new PhotoConstructor();

            for(int i = 0; i < newsList.Count; i++)
            {
                int randomStrategyNumber = random.Next(0, strateges.Count);
                photoConstructor.MakePhoto(newsList[i], strateges[randomStrategyNumber]);
            }

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
