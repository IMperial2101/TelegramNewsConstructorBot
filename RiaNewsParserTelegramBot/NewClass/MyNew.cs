using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPropertyBot.NewClass
{
    public class MyNew
    {
        public string title { get; set; }
        public string secondTitle { get; set; }
        public bool sendPhotoBool { get; set; } = false;
        public bool sendPhotoWithTextBool { get; set; } = false;
        public string photoUrl { get; set; }
        public string photoName { get; set; }
        public List<string> description = new List<string>();
        public string descriptionToSend { get; set; }
        public string url { get; set; }
    }
}
