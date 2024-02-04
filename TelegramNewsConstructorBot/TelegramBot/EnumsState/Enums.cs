using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramNewsConstructorBot.TelegramBot.EnumsState
{
    public class Enums
    {
        public enum UserState
        {
            Start,
            EnterKey,
            Commands,

        }
        public enum UserStateCommands
        {
            Default,
            CreateNew
        }
        public enum CreateNewState
        {
            GetLink
        }

    }
}
