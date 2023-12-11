using System.Linq;
using System.Text;
using System.Drawing;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HtmlAgilityPack;
using NewsPropertyBot.ParsingClasses;
using NewsPropertyBot.ProxyClass;
using NewsPropertyBot.TelegramBotClass;
using Newtonsoft.Json;
using RiaNewsParserTelegramBot.PropertiesClass;
using System.Net;
using System.IO;
using System.Net.Http.Json;
using System.Threading.Channels;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using System.Drawing.Imaging;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using NewsPropertyBot.NewClass;

class Program
{
    static MyProperties properties = ReadLineProperties();
    static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
    static async Task Main()
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder result = new StringBuilder(5);

        for (int i = 0; i < 5; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        string photoName = result.ToString();

        TelegramBot telegramBot = new TelegramBot(properties);
        Parser parser = new Parser(telegramBot, properties);
        MyNew myNew = await parser.ParseOneNewAsync("https://ria.ru/20231211/spetsoperatsiya-1915074390.html");

        string imageUrl = myNew.photoUrl;

        await DownloadImage(imageUrl,imagesFolderPath,$"{photoName}.png");
        AddTextOnImage($"{imagesFolderPath}\\{photoName}.png",$"{ imagesFolderPath}\\{photoName}Done.png", myNew.title);
        try
        {
            using (var stream = System.IO.File.Open($"{imagesFolderPath}\\{photoName}Done.png", FileMode.Open))
            {
                // Отправка фото через поток (Stream)
                InputOnlineFile inputPhoto = new InputOnlineFile(stream);
                await telegramBot.botClient.SendPhotoAsync(properties.ownerId, inputPhoto, "Описание фотографии");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отправке фото: {ex.Message}");
        }
        Console.WriteLine("Done");
        Console.ReadLine();

        telegramBot.SendMyNewToChannelAsync(await parser.ParseOneNewAsync("https://ria.ru/20231211/spetsoperatsiya-1914990608.html"));
        Console.ReadLine();

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
    static void MakeImagesFolder()
    {
        if (!Directory.Exists(imagesFolderPath))
        {
            Directory.CreateDirectory(imagesFolderPath);
        }
    }
    static void AddTextOnImage(string pathToImage, string pathToSave, string text)
    {
        using (Image image = Image.FromFile(pathToImage))
        {
            int width = image.Width;
            int height = image.Height;

            using (Graphics graphics = Graphics.FromImage(image))
            {
                Rectangle gradientRect = new Rectangle(0, 0, width, height);


                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    gradientRect,
                    Color.Transparent,
                    Color.FromArgb(200, 0, 0, 0),
                    LinearGradientMode.Vertical))
                {

                    graphics.FillRectangle(gradientBrush, gradientRect);
                    
                    
                    StringFormat stringFormat1 = new StringFormat();
                    stringFormat1.Alignment = StringAlignment.Center;
                    stringFormat1.LineAlignment = StringAlignment.Center;
                    float padding = 0.05f * width; // 10% от ширины изображения
                    float rectWidth = width - 2 * padding; // Ширина прямоугольника с учетом отступов

                    RectangleF rect1 = new RectangleF(padding, height/2, rectWidth, height / 2);
                    Font font1 = AdjustFontSize(graphics, text, rect1);
                    graphics.DrawString(text, font1, Brushes.White, rect1, stringFormat1);


                    string text2 = "🔥Marshall News, вооружение🔥";
                    //string text2 = "**************************************************************************************************************";
                    using (Font font2 = new Font("Segoe UI Emoji", 100))
                    {
                        StringFormat stringFormat2 = new StringFormat();
                        stringFormat2.Alignment = StringAlignment.Center;
                        stringFormat2.LineAlignment = StringAlignment.Near;

                        RectangleF rect2 = new RectangleF(0, height * 3 / 4, width, height / 4); 
                       // graphics.DrawString(text2, font2, Brushes.White, rect2, stringFormat2);
                    }
                }
            }
            image.Save(pathToSave, ImageFormat.Png);
        }

        
    }

    static Font AdjustFontSize(Graphics graphics, string text, RectangleF rect)
    {
        Font font;
        SizeF textSize;
        int fontSize = 1000; 

        do
        {
            font = new Font("Montserrat", fontSize, FontStyle.Bold); 


            textSize = graphics.MeasureString(text, font);

            fontSize--;
        } while (textSize.Height > rect.Height || textSize.Width/2.5 > rect.Width);

        return font;
    }



    static async Task DownloadImage(string imageUrl,string pathToDownload,string fileName)
    {
       
        string imagePath = Path.Combine(pathToDownload, fileName);

        HttpClient httpClient = new HttpClient();

        using var stream = await httpClient.GetStreamAsync(imageUrl);
        using var file = System.IO.File.OpenWrite(imagePath);

        await stream.CopyToAsync(file);

        
    }


}
