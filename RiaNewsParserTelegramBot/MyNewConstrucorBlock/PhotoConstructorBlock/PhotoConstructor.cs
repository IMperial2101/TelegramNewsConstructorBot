﻿using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock.Strategies;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock
{
    public class PhotoConstructor
    {

        private MyTelegramBot myTelegramBot = new MyTelegramBot();
        private IPhotoConstructorStrategy? constructoStrategy;
        public void SetStrategyAddText(IPhotoConstructorStrategy addText)
        {
            this.constructoStrategy = addText;
        }
        public async Task MakePhoto(MyNew myNew,IPhotoConstructorStrategy strategy,ColorVariationsEnum colorsVariation)
        {
            try
            {
                myNew.photoName = MakeRandomString();
                await DownloadImage(myNew);
                using (Image image = Image.FromFile(Path.Combine(MyPropertiesStatic.imagesFolderPath, myNew.photoName) + ".png"))
                {
                    Image finalImage = strategy.MakePhoto(image, myNew, colorsVariation);
                    finalImage.Save(Path.Combine(MyPropertiesStatic.imagesFolderPath, myNew.photoName) + "Done.png", ImageFormat.Png);
                }
            }
            catch(Exception ex)
            {
                await myTelegramBot.SendMessageToOwner($"Ошибка создания фотографии: {ex.Message} - {myNew.url}\nСтратегия - {strategy.GetStrategyName}.");
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
        }
        public async Task DownloadImage(MyNew myNew)
        {
            string imagePath = Path.Combine(MyPropertiesStatic.imagesFolderPath, myNew.photoName + ".png");

            HttpClient httpClient = new HttpClient();

            using var stream = await httpClient.GetStreamAsync(myNew.photoUrl);
            using var file = System.IO.File.OpenWrite(imagePath);

            await stream.CopyToAsync(file);
            Console.WriteLine($"Скачали фото {myNew.title.Substring(0, 20)}");
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
        static public void DeletePhotoes(string fileName)
        {
            try
            {
                if (File.Exists(Path.Combine( MyPropertiesStatic.imagesFolderPath, fileName + ".png"))) // Проверяем существование файла
                {
                    File.Delete(Path.Combine(MyPropertiesStatic.imagesFolderPath, fileName + ".png")); // Удаляем файл
                    Console.WriteLine("Файл успешно удален.");
                }
                else
                {
                    Console.WriteLine("Файл не существует.");
                }
                if (File.Exists(Path.Combine(MyPropertiesStatic.imagesFolderPath, fileName + "Done.png"))) // Проверяем существование файла
                {
                    File.Delete(Path.Combine(MyPropertiesStatic.imagesFolderPath, fileName + "Done.png")); // Удаляем файл
                    Console.WriteLine("Файл успешно удален.");
                }
                else
                {
                    Console.WriteLine("Файл не существует.");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Ошибка удаления файла: {e.Message}");
            }
        }

    }
}
