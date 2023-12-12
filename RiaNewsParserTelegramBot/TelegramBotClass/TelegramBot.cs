using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace NewsPropertyBot.TelegramBotClass
{
    public class TelegramBot
    {
        static string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        public TelegramBotClient botClient;
        string ownerMessage;
        public TelegramBot()
        {
            botClient = new TelegramBotClient(MyPropertiesStatic.botToken);
            ownerMessage = $"Date: {DateTime.Now}\n" +
                           $"Exception: ";
            
        }
        public async Task SendMessageToOwner(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(MyPropertiesStatic.ownerId,$"{ownerMessage} {message}", parseMode: ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }
        public async Task SendNewsToChannelAsync(List<MyNew> newsList)
        {
            
            foreach (var myNew in newsList)
            {
                try
                {
                    await SendMyNewToChannelAsync(myNew);
                    await Task.Delay(TimeSpan.FromSeconds(MyPropertiesStatic.timeBetweenSendMessSeconds));
                }
                catch (Exception ex)
                {
                    SendMessageToOwner($"Ошибка при отправке новости в канал: {ex.Message}");
                    Console.WriteLine($"Ошибка при отправке новости в канал: {ex.Message}");
                }
            }
            
        }
        public async Task SendMyNewToChannelAsync(MyNew myNew)
        {
            for(int i = 0;i< MyPropertiesStatic.channelID.Length;i++)
            {
                try
                {                   
                    if (MyPropertiesStatic.randomMessageDesign)
                    {
                        if(myNew.sendPhotoBool)
                        {
                            if (myNew.sendPhotoWithTextBool)
                            {

                                using (var stream = new FileStream(Path.Combine(imagesFolderPath, myNew.photoName), FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    InputOnlineFile photo = new InputOnlineFile(stream);
                                    await botClient.SendPhotoAsync(MyPropertiesStatic.channelID[i], photo, caption: myNew.descriptionToSend, parseMode: ParseMode.Markdown);
                                }
                            }
                            else
                            {
                                await botClient.SendPhotoAsync(MyPropertiesStatic.channelID[i], new InputOnlineFile(myNew.photoUrl), caption: myNew.descriptionToSend, parseMode: ParseMode.Markdown);
                            }
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(MyPropertiesStatic.channelID[i], myNew.title + myNew.secondTitle + myNew.description);
                        }
                    }
                    else
                    {
                        
                    }

                    Console.WriteLine($"Успешно отправили новость в канал");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
                }
            }
            
        }
        
        private string MakeDescriptionNoRandomDesign(MyNew myNew)
        {
            string description = "";
            if (myNew.description.Count != 0)
                description +=  $"\n{MyPropertiesStatic.smile}";
            if (myNew.description.Count == 1)
            {
                MyPropertiesStatic.maxParagraphCount = 1;
            }
            else if (myNew.description.Count < MyPropertiesStatic.maxParagraphCount)
                MyPropertiesStatic.maxParagraphCount = myNew.description.Count - 1;

            for (int i = 0; i < MyPropertiesStatic.maxParagraphCount; i++)
            {
                description += $"{myNew.description[i]}\n\n";
            }
            if (description.Length > MyPropertiesStatic.maxDescripSymbCount)
            {
                description = description.Substring(0, MyPropertiesStatic.maxDescripSymbCount) + "...\n";
            }
            return description;
        }

        

    }


}
