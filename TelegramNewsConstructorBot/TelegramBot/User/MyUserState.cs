using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramNewsConstructorBot.TelegramBot.EnumsState;

namespace TelegramNewsConstructorBot.TelegramBot.User
{
    public class MyUserState
    {
        public Enums.UserState UserState { get; set; } = Enums.UserState.Start;
        public Enums.UserStateCommands UserStateCommands { get; set; }
        public Enums.CreateNewState CreateNewState { get; set; }
    }
}
