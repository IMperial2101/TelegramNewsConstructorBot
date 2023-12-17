using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.PropertiesClass;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static async Task Main()
    {

        MakeImagesFolder();
        MyPropertiesStatic.MakeStaticProperties(properties);
        TelegramBot telegramBot = new TelegramBot();
        Parser parser = new Parser(telegramBot);

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
    
    static void MakeImagesFolder()
    {
        if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images")))
        {
            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images"));
        }
    }





}
