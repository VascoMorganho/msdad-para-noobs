using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;
using SharedInterfaces;
using ExceptionLibrary;

namespace ClientGui
{
    public partial class Form1 : Form
    {

        IServer s = null;
        ClientGui cc = null;

        public static Form1 Form1Instance;
        public string cname;
        public string cport;
        public string turl;
        int d = 0;
        public Form1()
        {
            InitializeComponent();
        }

        public void Connect(string name, string port)
        {
            this.d++;
            this.cname = name;
            this.cport = port;
            TcpChannel c = new TcpChannel(Int32.Parse(port));
            ChannelServices.RegisterChannel(c, false);

            cc = new ClientGui(name);

            RemotingServices.Marshal(cc, name + this.d, typeof(ClientGui));

            this.s = (IServer)Activator.GetObject(typeof(IServer), "tcp://localhost:8086/WSDAD");
            this.turl = "tcp://" + GetLocalIPAddress() + ":" + port + "/Client" + this.d;

            if (!s.CheckName(name))
            {
                MessageBox.Show($"An user with name '{name}' already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                c.StopListening(null);
                RemotingServices.Disconnect(this);
                ChannelServices.UnregisterChannel(c);
                cc = null;
                return;
            }
            try
            {
                s.AddUser(name, turl);
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
            }

            connectButton.Enabled = false;
            cc.server = s;
            Form2 form2 = new Form2(this.cname, this.cport, this.turl, this.s, this.cc);
            form2.ShowDialog();

        }

        public void Connect2(string name, List<string> urlL , string sURL)
        {
 
            TcpChannel c = new TcpChannel(Int32.Parse(urlL[1]));
            ChannelServices.RegisterChannel(c, false);

            cc = new ClientGui(name);

            RemotingServices.Marshal(cc, urlL[2] + this.d, typeof(ClientGui));

            this.s = (IServer)Activator.GetObject(typeof(IServer), sURL);
            this.turl = $"tcp://{urlL[0]}:{urlL[1]}/{urlL[2]}";

            if (!s.CheckName(name))
            {
                MessageBox.Show($"An user with name '{name}' already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                c.StopListening(null);
                RemotingServices.Disconnect(this);
                ChannelServices.UnregisterChannel(c);
                cc = null;
                return;
            }
            try
            {
                s.AddUser(name, this.turl);
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
            }

            connectButton.Enabled = false;
            cc.server = s;
            Form2 form2 = new Form2(name, urlL[1], this.turl, this.s, this.cc);
            form2.ShowDialog();

        }
        private void connectButton_Click(object sender, EventArgs e)
        {
            Connect(nameTextBox.Text, portTextBox.Text);
        }
        protected override void OnFormClosing(FormClosingEventArgs e) // Notify Server that user has left
        {
            base.OnFormClosing(e);
            if (s != null)
                s.RemoveUser(this.cname, this.turl);
        }
        public static string GetLocalIPAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}
