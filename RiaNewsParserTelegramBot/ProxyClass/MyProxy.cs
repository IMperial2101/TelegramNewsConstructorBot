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
        string ip;
        int port;
        string login;
        string password;
        public MyProxy(string ip,int port,string login, string password)
        {
            this.ip = ip;
            this.port = port;
            this.login = login;
            this.password = password;
        }
        public WebProxy GetWebProxy()
        {
            WebProxy proxy = new WebProxy(ip,port);
            proxy.Credentials = new NetworkCredential(login, password);
            return proxy;
        }
    }
}
