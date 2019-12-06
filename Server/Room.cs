using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    class Room
    {
        public int capacity;
        public string location;
        public string name;
        public List<string> reserves;

        public Room(string l, int c, string n)
        {
            this.location = l;
            this.name = n;
            this.capacity = c;
            this.reserves = new List<string>();
            System.Console.WriteLine($"{n} created in {l} with a capacity of {c}");
        }

        public void Reserve(string date)
        {
            this.reserves.Add(date);
        }
    }
}
