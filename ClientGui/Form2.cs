using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedInterfaces;
using ClientLibrary;
using ExceptionLibrary;

namespace ClientGui
{
    public partial class Form2 : Form
    {
        public static Form2 Form2Instance;
        IServer s = null;
        ClientGui cc = null;
        CLibrary clib = new CLibrary();
        public string cname;
        public string cport;
        public string turl;

        public Form2(string n, string p, string u, IServer se, ClientGui c)
        {
            this.cname = n;
            this.cport = p;
            this.turl = u;
            this.s = se;
            this.cc = c;
            InitializeComponent();
            Form2Instance = this;
            addSlotButton.Enabled = false;
            addInviteButton.Enabled = false;
        }

        int ni;
        int ns;
        string t;
        int m;
        List<string> ss = new List<string>();
        List<string> ii = new List<string>();
        int j = 0;
        private void createButton_Click(object sender, EventArgs e) // Faltam montes de verificações ///////////////////////////////////
        {
            if (string.IsNullOrWhiteSpace(topicTextBox.Text) || string.IsNullOrWhiteSpace(minattendTextBox.Text) || string.IsNullOrWhiteSpace(nslotsTextBox.Text) || string.IsNullOrWhiteSpace(ninvTextBox.Text))
            {
                ShowError("You left some empty parameters.");
                return;
            }

            t = topicTextBox.Text;
            m = Int32.Parse(minattendTextBox.Text);
            ns = Int32.Parse(nslotsTextBox.Text);
            ni = Int32.Parse(ninvTextBox.Text);
            addSlotButton.Enabled = true;

            slotLabel.Text = $"Slots ({ns} remaining):";
            invitesLabel.Text = $"Invites ({ni} remaining):";
            slotLabel.Refresh();
            invitesLabel.Refresh();
            createButton.Enabled = false;
            createButton.Enabled = true;
            ss.Clear();
            ii.Clear();
        }

        private void addSlotButton_Click(object sender, EventArgs e)
        {
            if(j < ns)
            {
                if (!slotsTextBox.Text.Contains(",")){
                    MessageBox.Show("Slot's location and date must be separared by a comma (,).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ss.Add(slotsTextBox.Text);
                j++;
                slotLabel.Text = $"Slots ({ns - j} remaining):";
                slotLabel.Refresh();
            }
            if (j == ns)
            {
                addSlotButton.Enabled = false;
                addInviteButton.Enabled = true;
                j = 0;
                slotLabel.Text = $"Slots:";
                slotLabel.Refresh();
                if (ni == 0)
                {
                    addInviteButton.Enabled = false;
                    int ret;
                    try
                    {
                        ret = clib.CreateMeeting(t, m, ns, ni, ss, ii, cc.Name, s);
                        MessageBox.Show("Meeting created successfully.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch(InvalidSlotsException ise)
                    {
                        ShowError(ise.Message);
                    }
                    catch(TopicAlreadyExistsException taee)
                    {
                        ShowError(taee.Message);
                    }
                    catch(WrongNumberOfInviteesException wnoie)
                    {
                        ShowError(wnoie.Message);
                    }
                    catch(WrongNumberOfSlotsException wnose)
                    {
                        ShowError(wnose.Message);
                    }
                    catch (FreezedException ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    createButton.Enabled = true;
                }
            }

            slotsTextBox.Clear();

        }

        private void addInviteButton_Click(object sender, EventArgs e)
        {
            if (j < ni)
            {
                ii.Add(invTextBox.Text);
                j++;
                invitesLabel.Text = $"Invites ({ni - j} remaining):";
                invitesLabel.Refresh();
            }
            if (j == ni)
            {
                addInviteButton.Enabled = false;
                j = 0;
                invitesLabel.Text = $"Invites:";
                invitesLabel.Refresh();
                int ret;
                try
                {
                    ret = clib.CreateMeeting(t, m, ns, ni, ss, ii, cc.Name, s);
                    MessageBox.Show("Meeting created successfully.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (InvalidSlotsException ise)
                {
                    ShowError(ise.Message);
                }
                catch (TopicAlreadyExistsException taee)
                {
                    ShowError(taee.Message);
                }
                catch (WrongNumberOfInviteesException wnoie)
                {
                    ShowError(wnoie.Message);
                }
                catch (WrongNumberOfSlotsException wnose)
                {
                    ShowError(wnose.Message);
                }
                catch (FreezedException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            invTextBox.Clear();
            createButton.Enabled = true;
        }

        private void listButton_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                List<Tuple<string, string, int, int, int>> res = clib.ListMeetingsTopicsAndNumbers(cc.Name, s);
                string[] arr = new string[6];
                foreach (Tuple<string, string, int, int, int> t in res)
                {
                    arr[0] = t.Item1;
                    arr[1] = t.Item2;
                    arr[2] = t.Item3.ToString();
                    arr[3] = t.Item4.ToString();
                    arr[4] = t.Item5.ToString();
                    ListViewItem itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                }
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void showSlotsButton_Click(object sender, EventArgs e)
        {
            try
            {
                listView2.Items.Clear();
                if (listView1.SelectedItems.Count > 0)
                {
                    List<Tuple<string, string>> slotss = clib.ShowMeetingSlots(listView1.SelectedItems[0].SubItems[0].Text, s);
                    foreach (Tuple<string, string> s in slotss)
                    {
                        string[] arr = new string[4];
                        ListViewItem itm;
                        arr[0] = listView1.SelectedItems[0].SubItems[0].Text;
                        arr[1] = s.Item1;
                        arr[2] = s.Item2;
                        itm = new ListViewItem(arr);
                        listView2.Items.Add(itm);
                    }
                }
                else
                {
                    MessageBox.Show("Select a meeting to show the respective slot(s).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void joinButton_Click(object sender, EventArgs e)
        {
            List<Tuple<string, string, string>> selectedSlots = new List<Tuple<string, string, string>>();
            Tuple<string, string, string> selectedSlot;
            if (listView2.SelectedItems.Count > 0)
            {
                string topic = listView2.SelectedItems[0].SubItems[0].Text;
                foreach(ListViewItem item in listView2.SelectedItems)
                {
                    selectedSlot = new Tuple<string, string, string>(topic, item.SubItems[1].Text, item.SubItems[2].Text);
                    selectedSlots.Add(selectedSlot);
                }
                try
                {
                    List<int> res = clib.JoinSlots(selectedSlots, cc.Name, s);
                    for (int i = 0; i < res.Count(); i++)
                    {
                        if (res[i] == 1) MessageBox.Show($"Joined meeting {topic} in location {selectedSlots[i].Item2} and date {selectedSlots[i].Item3}");
                        else if (res[i] == -1) ShowError($"User {cc.Name} has already joined slot with location {selectedSlots[i].Item2} and date {selectedSlots[i].Item3}.");
                    }
                }
                catch(MeetingIsAlreadyClosedException miace)
                {
                    ShowError(miace.Message);
                }
                catch (FreezedException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                listView2.Items.Clear();
                int ret;
                try
                {
                    ret = clib.CloseMeeting(listView1.SelectedItems[0].SubItems[0].Text, cc.Name, s);
                    MessageBox.Show("You closed that meeting");
                }
                catch (MeetingIsAlreadyClosedException)
                {
                    ShowError("CLOSE: You need to choose a meeting that is still opened.");
                }
                catch (NotCoordinatorException)
                {
                    ShowError("CLOSE: Sorry, you don't have permissions to close this meeting.");
                }
                catch (NotEnoughParticipantsException nepe)
                {
                    ShowError(nepe.Message);
                }
                catch(NoRoomsAvailableException nrae)
                {
                    ShowError(nrae.Message);
                }
                catch(UsersGotRemovedException ugre)
                {
                    MessageBox.Show(ugre.Message + " Meeting closed.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (FreezedException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Choose ONE meeting that you want to close.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void ShowError(string errormsg)
        {
            MessageBox.Show($"{errormsg}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
