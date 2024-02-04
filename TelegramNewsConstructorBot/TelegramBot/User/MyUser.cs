using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramNewsConstructorBot.TelegramBot.User
{
    public class MyUser : MyUserState
    {
        public int id { get; set; }
        public string telegramUserName { get; set; }
        public long chatId { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        
        public List<string> availableCommands = new List<string>();
    }
}
