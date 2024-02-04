using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using RiaNewsParserTelegramBot.TelegramBotClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithClearPhoto;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithPhotoshopPhoto;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges.WithPhotoshopPhoto.OnlyDescriptionOnPhoto;
using System.Text.RegularExpressions;
using Telegram.Bot;
using TelegramNewsConstructorBot.TelegramBot;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static async Task Main()
    {
        Random random = new Random();
        MyPropertiesStatic.MakeStaticProperties(properties);
        TelegramBotSendler telegramBot = new TelegramBotSendler();
        MyParser parser = new MyParser(telegramBot);
        PhotoConstructor photoConstructor = new PhotoConstructor();
        MyNewTelegramSendler telegramSendler = new MyNewTelegramSendler(telegramBot);
        MakeImagesFolder();


        MyTelegramBot myBot = new MyTelegramBot(new TelegramBotClient(MyPropertiesStatic.botToken),parser, telegramSendler);
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
    static string ChooseRandomSendStrategy(Dictionary<string, int> _weightSendStrategies,string lastSendStrategy)
    {
        Dictionary<string, int> weightSendStrategies = new Dictionary<string, int>(_weightSendStrategies);
        Random random = new Random();
        int totalWeight = 0;

        if(weightSendStrategies.ContainsKey(lastSendStrategy))
            weightSendStrategies.Remove(lastSendStrategy);

        foreach (var el in weightSendStrategies)
            totalWeight += el.Value;


        foreach(var el in weightSendStrategies)
        {
            if (random.Next(totalWeight) <= el.Value)
                return el.Key;
            else
                totalWeight -= el.Value;
        }
        return null;
    }
    static ISendNew ReturnISendNew(string strategy)
    {
        switch (strategy)
        {
            case "Title":
                {
                    return new Title();
                    break;
                }
            case "TitleDescription":
                {
                    return new TitleDescription();
                    break;
                }
            case "TitleSecondTitleDescription":
                {
                    return new TitleSecondTitleDescription();
                    break;
                }
            case "TitleClearPhoto":
                {
                    return new TitleClearPhoto();
                    break;
                }
            case "TitleSecondTitleClearPhoto":
                {
                    return new TitleSecondTitleClearPhoto();
                    break;
                }
            case "TitleSecondTitleDescriptionClearPhoto":
                {
                    return new TitleSecondTitleDescriptionClearPhoto();
                    break;
                }
            case "DescriptionLeftPhoto":
                {
                    return new DescriptionLeftPhoto();
                    break;
                }
            case "DescriptionRightPhoto":
                {
                    return new DescriptionRightPhoto();
                    break;
                }
            case "DescriptionUnderPhoto":
                {
                    return new DescriptionUnderPhoto();
                    break;
                }
            case "TitleAndDescriptionPhoto":
                {
                    return new TitleAndDescriptionPhoto();
                    break;
                }
            case "TitlePhoto":
                {
                    return new TitlePhoto();
                    break;
                }
            case "TitleWithDescriptionPhoto":
                {
                    return new TitleWithDescriptionPhoto();
                    break;
                }
        }
        return null;

    }





}
