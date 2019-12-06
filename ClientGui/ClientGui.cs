using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedInterfaces;
using ClientLibrary;
using ExceptionLibrary;

namespace ClientGui
{
    public class ClientGui : MarshalByRefObject
    {
        private string name;
        private string client_url;
        private string server_url;
        public IServer server;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string CUrl
        {
            get { return client_url; }
            set { client_url = value; }
        }

        public string SUrl
        {
            get { return server_url; }
            set { server_url = value; }
        }

        public ClientGui(string name)
        {
            this.Name = name;
        }
        public ClientGui(string name, string cUrl, string sUrl)
        {
            this.Name = name;
            this.CUrl = cUrl;
            this.SUrl = sUrl;
        }

        static public List<string> UrlBreakdown(string url)
        {
            List<string> breakdown = new List<string>();

            int pFrom = url.LastIndexOf(":") + ":".Length;
            int pTo = url.LastIndexOf("/");
            string url_port = url.Substring(pFrom, pTo - pFrom);

            int nameFrom = url.LastIndexOf("/") + "/".Length;
            string url_name = url.Substring(nameFrom);

            int url_index = url.IndexOf("tcp://");
            int ip_To = url.LastIndexOf(":");
            string url_ip = url.Substring(6, ip_To - 6);

            if (url_ip == "localhost")
            {
                url_ip = "127.0.0.1";

            }

            breakdown.Add(url_ip);
            breakdown.Add(url_port);
            breakdown.Add(url_name);

            return breakdown;

        }


        public void ConnectClientGui(string name, List<string> urlL, string sURL, ClientGui cc)
        {
            TcpChannel c = new TcpChannel(Int32.Parse(urlL[1]));
            ChannelServices.RegisterChannel(c, false);


            RemotingServices.Marshal(cc, urlL[2], typeof(ClientGui));

            this.server = (IServer)Activator.GetObject(typeof(IServer), sURL);
            this.client_url = $"tcp://{urlL[0]}:{urlL[1]}/{urlL[2]}";

            if (!server.CheckName(name))
            {
                MessageBox.Show($"An user with name '{name}' already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                c.StopListening(null);
                RemotingServices.Disconnect(this);
                ChannelServices.UnregisterChannel(c);
                cc = null;
                return;
            }
            try {
                server.AddUser(name, this.client_url);
            }catch(FreezedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            Form2 form2 = new Form2(name, urlL[1], this.client_url, this.server, cc);
            form2.ShowDialog();

        }




        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length != 0)
            {
                ClientGui cc = new ClientGui(args[0]);
                Console.WriteLine("INSIDE IF");
                List<string> urlL = UrlBreakdown(args[1]);
                cc.ConnectClientGui(args[0], urlL, args[2], cc);
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
