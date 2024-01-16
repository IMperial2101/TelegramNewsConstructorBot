using NewsPropertyBot.NewClass;
using NewsPropertyBot.TelegramBotClass;
using RiaNewsParserTelegramBot.PropertiesClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace RiaNewsParserTelegramBot.TelegramBotClass.SendStrateges
{
    public class TitleDescription : PhotoConstructorForSendler,ISendNew
    {
        public async Task SendNew(MyTelegramBot myTelegramBot, MyNew myNew)
        {
            myNew.descriptionToSend = MakeDescriptionToSend(myNew);
            await myTelegramBot.botClient.SendTextMessageAsync(MyPropertiesStatic.channelID, $"*{myNew.title}*\n\n{myNew.descriptionToSend}", ParseMode.Markdown);
        }
        public string GetSendStrategyName()
        {
            return "TitleDescription";
        }
    }
}
