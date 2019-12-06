using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Slot
    {
        public string location;
        public string date;
        public string topic;
        public List<User> joinedUsers;

        public Slot(string t, string l, string d)
        {
            this.topic = t;
            this.location = l;
            this.date = d;
            this.joinedUsers = new List<User>();
        }

        public void AddUser(User u)
        {
            this.joinedUsers.Add(u);
        }

        public int NumberOfParticipants()
        {
            return joinedUsers.Count();
        }
    }


}
