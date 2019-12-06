using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedInterfaces;

namespace Server
{
    [Serializable]
    public class User
    {
        public string name;
        public string url;
        public ICLibrary iclient;
        //public ClientGui guiClient;
        //public ClientScript scriptClient;

        public User(string name, string url, ICLibrary ic)
        {
            this.Name = name;
            this.Url = url;
            this.iclient = ic;
        }

        /*public User(string name, string url, ClientGui gc)
        {
            this.Name = name;
            this.Url = url;
            this.guiClient = gc;
        }

        public User(string name, string url, ClientScript sc)
        {
            this.Name = name;
            this.Url = url;
            this.scriptClient = sc;
        }*/

        public ICLibrary GetIClient()
        {
            return this.iclient;
        }
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Url
        {
            get { return url; }
            set { url = value; }
        }

    }
}
