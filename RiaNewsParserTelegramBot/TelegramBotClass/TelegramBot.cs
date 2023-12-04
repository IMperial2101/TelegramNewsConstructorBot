using NewsPropertyBot.NewClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Random random = new Random();
        int maxParagraphCount;
        int maxDescripSymbCount;
        bool sendPhoto;
        bool sendSecondTitle;
        TelegramBotClient botClient;
        string ownerMessage;
        MyProperties properties;
        int lastParagraphCount;
        public TelegramBot(MyProperties properties)
        {
            this.properties = properties;
            lastParagraphCount = properties.maxParagraphCount;
            botClient = new TelegramBotClient(properties.botToken);
            ownerMessage = $"Channel: {properties.channelID}\n" +
                           $"Date: {DateTime.Now}\n" +
                           $"Exception: ";
            
        }
        public async Task SendMessageToOwner(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(properties.ownerId, ownerMessage+= message, parseMode: ParseMode.Markdown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }
        public async Task SendMyNewToChannelAsync(MyNew myNew)
        {
            MakeRandomProperties();
            try
            {
                if(properties.randomMessageDesign)
                {
                    if (!string.IsNullOrEmpty(myNew.photoUrl) && sendPhoto)
                    {
                        // Send the photo first
                        if (sendPhoto)
                            await botClient.SendPhotoAsync(properties.channelID, new InputOnlineFile(myNew.photoUrl), caption: $"*{myNew.title}*_{(sendSecondTitle ? myNew.secondTitle : "")}_\n{(maxParagraphCount != 0 ? MakeDescriptionRandomDesign(myNew) : "\n")}[Читать полность]({myNew.url})🔗", parseMode: ParseMode.Markdown);
                    }
                    else
                    {
                        // If no photo, send text only
                        await botClient.SendTextMessageAsync(properties.channelID, $"*{myNew.title}*\n\n[Читать полность]({myNew.url})🔗", ParseMode.Markdown);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(myNew.photoUrl))
                    {
                        // Send the photo first
                        await botClient.SendPhotoAsync(properties.channelID, new InputOnlineFile(myNew.photoUrl), caption: $"*{myNew.title}*_{myNew.secondTitle}_\n{MakeDescriptionNoRandomDesign(myNew)}[Читать полность]({myNew.url})🔗", parseMode: ParseMode.Markdown);
                    }
                    else
                    {
                        // If no photo, send text only
                        string message = $"*{myNew.title}*\n{myNew.secondTitle}";
                        message += MakeDescriptionNoRandomDesign(myNew);
                        await botClient.SendTextMessageAsync(properties.channelID, message, ParseMode.Markdown);
                    }
                }
               
                Console.WriteLine($"Успешно отправили новость в канал");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }
        private string MakeDescriptionRandomDesign(MyNew myNew)
        {
            string description = "";
            if (myNew.description.Count != 0)
                description += $"\n{properties.smile}";
            if (myNew.description.Count == 1)
            {
                maxParagraphCount = 1;
            }
            else if (myNew.description.Count < maxParagraphCount)
                maxParagraphCount = myNew.description.Count - 1;

            for (int i = 0; i < maxParagraphCount; i++)
            {
                description += $"{myNew.description[i]}\n\n";
            }
            if(description.Length > maxDescripSymbCount)
            {
                description = description.Substring(0, maxDescripSymbCount) + "...\n\n";
            }
            return description;
        }
        private string MakeDescriptionNoRandomDesign(MyNew myNew)
        {
            string description = "";
            if (myNew.description.Count != 0)
                description += "\n🌎";
            if (myNew.description.Count == 1)
            {
                properties.maxParagraphCount = 1;
            }
            else if (myNew.description.Count < properties.maxParagraphCount)
                properties.maxParagraphCount = myNew.description.Count - 1;

            for (int i = 0; i < properties.maxParagraphCount; i++)
            {
                description += $"{myNew.description[i]}\n\n";
            }
            if (description.Length > properties.maxDescripSymbCount)
            {
                description = description.Substring(0, properties.maxDescripSymbCount) + "...\n";
            }
            return description;
        }

        private void MakeRandomProperties()
        {
            if (properties.randomMessageDesign)
            {
                //поработать с количеством абхацев
                while(lastParagraphCount == maxParagraphCount)
                {
                    maxParagraphCount = random.Next(0, properties.maxParagraphCount <= 1 ? 2 : properties.maxParagraphCount + 1);
                }
                lastParagraphCount = maxParagraphCount;
                maxDescripSymbCount = random.Next(properties.maxDescripSymbCount <= 50 ? 0 : properties.maxDescripSymbCount - 50, properties.maxDescripSymbCount <= 50 ? 150 : properties.maxDescripSymbCount + 50);

                if (properties.sendPhotoRandomPersent > 100 || properties.sendPhotoRandomPersent < 0)
                    sendPhoto = true;
                else
                {
                    if (random.Next(0, 100) <= properties.sendPhotoRandomPersent)
                        sendPhoto = true;
                    else
                        sendPhoto = false;
                }

                if (properties.sendSecondTitleRandomPersent > 100 || properties.sendSecondTitleRandomPersent < 0)
                    sendSecondTitle = true;
                else
                {
                    if (random.Next(0, 100) <= properties.sendSecondTitleRandomPersent)
                        sendSecondTitle = true;
                    else
                        sendSecondTitle = false;
                }
            }
            else
            {
                maxParagraphCount = properties.maxParagraphCount;
                maxDescripSymbCount = properties.maxDescripSymbCount;
                sendPhoto = true;
                sendSecondTitle = true;
            }
        }

    }


}
