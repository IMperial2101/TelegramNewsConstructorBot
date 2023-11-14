using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        static List<string> GetLinksToParse(List<string> mainPageLinks, ref string lastNewLink)
        {
            if (lastNewLink == null)
            {
                Console.WriteLine("Последняя актуальная ссылка = null");
                lastNewLink = mainPageLinks[0];
                return null;
            }
            else if (lastNewLink == mainPageLinks[0])
            {
                Console.WriteLine("Новых ссылок не найдено");
                return null;
            }
            else
            {
                if (!mainPageLinks.Contains(lastNewLink))
                {
                    Console.WriteLine("Полседняя ссылка не равна null, но она не нашлась в новом списке");
                    lastNewLink = mainPageLinks[0];
                    return null;
                }
                List<string> linkstoParse = new List<string>();
                foreach (var el in mainPageLinks)
                {
                    if (el != lastNewLink)
                    {
                        linkstoParse.Add(el);
                    }
                    else
                    {
                        break;
                    }
                }
                lastNewLink = mainPageLinks[0];
                return linkstoParse;      
            }
        }
    }
}
