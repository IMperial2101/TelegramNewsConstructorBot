using NewsPropertyBot.NewClass;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System.Text;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
    static async Task Main()
    {

        MakeImagesFolder();
        MakeStaticProperties();
        TelegramBot telegramBot = new TelegramBot();
        Parser parser = new Parser(telegramBot, properties);

        /*
        MyNew myNew = await parser.ParseOneNewAsync("https://ria.ru/20231212/kanadets-1915407469.html");
        myNew.photoName = MakeRandomString();
        await MyNewConstructor.DownloadImage(myNew);
        */

        await parser.Start();       
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
    static void MakeStaticProperties()
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
    static void MakeImagesFolder()
    {
        if (!Directory.Exists(imagesFolderPath))
        {
            Directory.CreateDirectory(imagesFolderPath);
        }
    }


    static string MakeRandomString()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder result = new StringBuilder(5);

        for (int i = 0; i < 5; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        return result.ToString();
    }




}
