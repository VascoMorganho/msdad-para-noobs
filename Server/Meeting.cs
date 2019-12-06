using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    class Meeting
    {
        public string topic;
        public int nslots;
        public int min;
        public int ninv;
        public List<Slot> slots;
        public List<string> invites;
        public string coord;
        public string status;

        public Meeting(string t, int m, int ns, int ni, List<string> ss, List<string> ii, string coordinator)
        {
            this.topic = t;
            this.min = m;
            this.nslots = ns;
            this.ninv = ni;
            this.invites = ii;
            this.coord = coordinator;
            this.slots = new List<Slot>();
            this.status = "Opened";

            foreach(string s in ss)
            {
                string[] values = s.Split(',');
                Slot slt = new Slot(t, values.First(), values.Last());
                slots.Add(slt);
                System.Console.WriteLine($"Slot in {values.First()} for {values.Last()} created in meeting {t}");
            }
        }

        public bool CheckInvite(string n)
        {
            if (invites.Contains(n)){
                return true;
            }
            else { return false; }
        }

        public void MeetingClosed()
        {
            this.status = "Closed";
        }

        public void MeetingCancelled()
        {
            this.status = "Cancelled";
        }
    }
}
