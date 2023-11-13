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
        public string photoUrl { get; set; }
        public List<string> description = new List<string>();
        public string url { get; set; }
    }
}
