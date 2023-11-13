using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPropertyBot.XpathClass
{
    internal class XPathStrings
    {
        public readonly string AllNewLinks = "//div[@class = 'list-item']//a[contains(@class,'list-item__title')]";
        public readonly string title = "//div[@class = 'article__title']";
        public readonly string secondTitle = "//h1[@class = 'article__second-title']";
        public readonly string photoNew = "//div[@class = 'photoview__open']//img";
        public readonly string descriptionText = "//div[@class = 'article__body js-mediator-article mia-analytics']/div[@data-type = 'text']";
        
    }
}
