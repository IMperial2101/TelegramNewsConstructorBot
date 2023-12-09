using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NewsPropertyBot.ProxyClass
{
    public  class MyProxy
    {
        public string host { get; set; }
        public int port { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public MyProxy()
        {
            
        }
        public MyProxy(string host,int port,string login, string password)
        {
            this.host = host;
            this.port = port;
            this.login = login;
            this.password = password;
        }
        public WebProxy GetWebProxy()
        {
            WebProxy proxy = new WebProxy(host,port);
            proxy.Credentials = new NetworkCredential(login, password);
            return proxy;
        }
    }
}
