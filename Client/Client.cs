using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;
using SharedInterfaces;
using System.IO;

namespace Client
{
    public class Client : MarshalByRefObject, IClient
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

        public Client(string name)
        {
            this.Name = name;
        }
        public Client(string name, string cUrl, string sUrl)
        {
            this.Name = name;
            this.CUrl = cUrl;
            this.SUrl = sUrl;
        }

        public void CreateMeeting(string topic, int min, int nslots, int ninvites, List<string> slots, List<string> invites)
        {
            if(nslots <= 0)
            {
                ShowError("There has to be at least 1 slot per meeting");
                return;
            }
            if (!server.CheckTopic(topic))
            {
                ShowError($"A meeting with topic '{topic}' already exists.");
                return;
            }
            if (ninvites < invites.Count())
            {
                MessageBox.Show("Too many invites.", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (nslots < slots.Count())
            {
                MessageBox.Show("Too many slots.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            server.AddMeeting(topic, min, nslots, ninvites, slots, invites, this.name);
        }

        public List<Tuple<string, List<string>>> ListAvailableMeetingsConsole() //SERA ASSIM?
        {
            List<Tuple<string, List<string>>> res = server.ListAvailableMeetingsConsole(this.name);

            return res;


        }

        public void ListMeetingsTopicsAndNumbers()
        {
            List<Tuple<string, string, int,int,int>> res = server.ListMeetingsTopicsAndNumbers(this.name);
            string[] arr = new string[6];
            foreach (Tuple<string, string, int,int,int> t in res)
            {
                arr[0] = t.Item1;
                arr[1] = t.Item2;
                arr[2] = t.Item3.ToString();
                arr[3] = t.Item4.ToString();
                arr[4] = t.Item5.ToString();
                //Form2.Form2Instance.listMeetingsTextBox.AppendText($"{t}\r\n");
                ListViewItem itm = new ListViewItem(arr);
                Form2.Form2Instance.listView1.Items.Add(itm);
            }
        }

        public void ShowMeetingSlots(string topic)
        {
            List<Tuple<string, string>> slotss = server.ShowSlotsByMeeting(topic);
            foreach (Tuple<string, string> s in slotss)
            {
                string[] arr = new string[4];
                ListViewItem itm;
                arr[0] = topic;
                arr[1] = s.Item1;
                arr[2] = s.Item2;
                itm = new ListViewItem(arr);
                Form2.Form2Instance.listView2.Items.Add(itm);
            }
        }

        public void JoinSlots(List<Tuple<string, string, string>> l)
        {
            foreach(Tuple<string, string, string> s in l)
            {
                server.JoinSlot(this.name, s.Item1, s.Item2, s.Item3);
            }
        }

        public void CloseMeetings(string topic)
        {
            int ret = server.CloseMeeting(topic, this.name);
            if (ret == -1)
            {
                ShowError("You need to choose a meeting that is still opened.");
                return;
            }
            else if (ret == -2)
            {
                ShowError("Sorry, you don't have permissions to close this meeting.");
                return;
            }
            MessageBox.Show("You closed that meeting");
        }

        public void ReceiveNotification(int v, string top, string loc, string dat)
        {
            if(v == 1)
            {
                MessageBox.Show($"Meeting {top} has been closed in slot : {loc}, {dat}.");
            }
            else { MessageBox.Show($"Meeting {top} has been canceled."); }
        }
        public void ShowError(string errormsg)
        {
            MessageBox.Show($"{errormsg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
