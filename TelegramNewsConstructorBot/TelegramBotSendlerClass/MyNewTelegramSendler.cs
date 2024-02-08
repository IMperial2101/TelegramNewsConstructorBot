using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiaNewsParserTelegramBot.MyNewConstrucorBlock.PhotoConstructorBlock;

namespace RiaNewsParserTelegramBot.TelegramBotClass
{
    public class MyNewTelegramSendler
    {
        public string pathToImages = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        private ISendNew? sendStrategy;
        TelegramBotSendler myTelegramBot;
        public MyNewTelegramSendler(TelegramBotSendler myTelegramBot)
        {
            this.myTelegramBot = myTelegramBot; 
        }
        public void SetStrategy(ISendNew strategy)
        {
            sendStrategy = strategy;
        }
        public async Task SendNew(MyNew myNew, ISendNew strategy,string chatId)
        {
            
            if (myNew.photoUrl == null)
                return;
            await strategy.SendNew(myTelegramBot, myNew, chatId);
            if (myNew.photoName != null)
                DeleteImages(myNew);

        }
        private void DeleteImages(MyNew myNew)
        {

            string photoBefore = Path.Combine(pathToImages, myNew.photoName + ".png");
            string photoAfter = Path.Combine(pathToImages, myNew.photoName + "Done.png");


            if (File.Exists(photoBefore)){
                try{
                    File.Delete(photoBefore);
                    Console.WriteLine($"Файл {myNew.photoName} успешно удален.");
                }
                catch (Exception ex){
                    Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
                }
            }
            else
                Console.WriteLine($"Файл {myNew.photoName} не существует.");

            if (File.Exists(photoAfter)){
                try{
                    File.Delete(photoAfter);
                    Console.WriteLine($"Файл {myNew.photoName + "Done"} успешно удален.");
                }
                catch (Exception ex){
                    Console.WriteLine($"Ошибка при удалении файла: {ex.Message}");
                }
            }
            else
                Console.WriteLine($"Файл {myNew.photoName + "Done"} не существует.");
        }

    }
}
