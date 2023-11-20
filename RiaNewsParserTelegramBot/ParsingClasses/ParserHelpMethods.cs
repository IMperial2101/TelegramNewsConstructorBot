using HtmlAgilityPack;
using NewsPropertyBot.NewClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPropertyBot.ParsingClasses
{
    partial class Parser
    {
        
        static bool CheckViewCount(HtmlNode views)
        {
            if (views != null)
            {
                if (int.TryParse(views.InnerText, out int number))
                {
                    if (number < minViewCount)
                    {
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: Не удалось определить количество просмотров.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Ошибка: Не удалось найти количество просмотров.");
                return false;
            }
            return true;
        }

    }
}
