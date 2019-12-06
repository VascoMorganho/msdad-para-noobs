using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using SharedInterfaces;
using ExceptionLibrary;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Threading;

namespace Server
{
    [Serializable]
    class Server : MarshalByRefObject, IServer
    {
        private string serverId;
        //public string serverUrl;
        private int maxFaults;
        private int min_delay;
        private int max_delay;
        private List<Tuple<int, bool>> electionList = new List<Tuple<int, bool>>();

        List<User> users;
        public static List<Meeting> meetings;

        public int Max_delay { get { return max_delay; } set { max_delay = value; } }

        public int Min_delay { get { return min_delay; } set { min_delay = value; } }

        public int MaxFaults { get { return maxFaults; } set { maxFaults = value; } }

        public static string ServerUrl;

        public string ServerId { get { return serverId; } set { serverId = value; } }


        bool FREEZE;
   

        List<Action> freezedActions = new List<Action>();
        public static List<Room> rooms = new List<Room>();
        List<string> serversURLs = new List<string>();


        int tag = 0;




        public Server() {
            this.users = new List<User>();
            meetings = new List<Meeting>();
        }
        
        public Server(string serverId, string serverUrl, int maxFaults, int min_delay, int max_delay) {
            this.ServerId = serverId;
            ServerUrl = serverUrl;
            Console.WriteLine(serverUrl);
            Console.WriteLine(ServerUrl);
            this.MaxFaults = maxFaults;
            this.Min_delay = min_delay;
            this.Max_delay = max_delay;
            this.users = new List<User>();
            meetings = new List<Meeting>();
            this.FREEZE = false;
            Console.WriteLine("CREATED");
        }

        

        public void write(){
            List<int> retsZero = new List<int>();
            List<int> retsOne = new List<int>();
            foreach(string url in serversURLs){
                Server server = (Server)Activator.GetObject(typeof(Server), url);
                if(server.SetMeetings(tag) ==0){
                    retsZero.Add(server.SetMeetings(tag));
                }else if (server.SetMeetings(tag) ==1){
                    retsOne.Add(server.SetMeetings(tag));
                }

                if(retsZero.Count() + retsOne.Count() > (serversURLs.Count()/2)+1){
                    if(retsOne.Count() > retsZero.Count()){
                        tag++;
                        foreach(string u in serversURLs){
                            Server ser = (Server)Activator.GetObject(typeof(Server), u);
                            ser.CommitMeetings(this.tag, meetings, rooms);
                        }
                    }
                }
            }
        }

        public void read(){
            List<Tuple<int, List<Meeting>, List<Room>>> l = new List<Tuple<int, List<Meeting>, List<Room>>>();
           Console.WriteLine("ON READ UPDATING MY MEETINGS LIST111111111");
            int maxTag = -1;
            foreach(string url in serversURLs){
                Server server = (Server)Activator.GetObject(typeof(Server), url);
                l.Add(server.GetMeetings());
                Console.WriteLine("ON READ UPDATING MY MEETINGS LIST2222222");
                if(l.Count() > (serversURLs.Count()/2)+1 || l.Count() == 1){
                    foreach(Tuple<int, List<Meeting>, List<Room>> t in l){

                        if(t.Item1 > maxTag){
                            Console.WriteLine("ON READ UPDATING MY MEETINGS LIST");
                            maxTag = t.Item1;
                            meetings = t.Item2;
                            rooms = t.Item3;

                            Console.WriteLine($"AGORA TENHO {meetings.Count()} MEETINGS");
                        }
                    }
                }           
            }
        }

        public Tuple<int, List<Meeting>, List<Room>> GetMeetings(){
            Console.WriteLine($"VOU MANDAR {meetings.Count()} MEETINGS E {rooms.Count()} ROOMS");
            return new Tuple<int, List<Meeting>, List<Room>>(this.tag, meetings, rooms);
        }
        public void CommitMeetings(int newTag,List<Meeting> m, List<Room> r){
 
            this.tag = newTag;
            meetings = m;
            rooms = r;
        }
        public int SetMeetings(int newTag){
            if(newTag >= tag){
                return 1;
            }else{
                return 0;
            }
        }

        // Method that allows publisher aka Candidate to verify if he should change his state to Leader.
      


        // Method that sends an answer (sends the vote) from subscriber aka follower to a publisher aka Candidate 
      
        public List<string> AddUser(string name, string URL)
        { //Add a new user that just connected to the new list of users

            foreach (string url in serversURLs)
            {
                try{
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), url);
                    server.RealAddUser(name, URL);
                }catch(SocketException){
                    continue;
                }
                
            }
            RealAddUser(name, URL);

            return serversURLs;
        }

        public void RealAddUser(string name, string URL)
        {
            
            if (FREEZE)
            {
                this.freezedActions.Add(() => { AddUser(name, URL); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }

            lock(users){
                Console.WriteLine("adicionarei novo user");
                ICLibrary c = (ICLibrary)Activator.GetObject(typeof(ICLibrary), URL);  //FIXME MAYBE WRONG
                users.Add(new User(name, URL, c));
                Console.WriteLine("localmente funcionou");


                System.Console.WriteLine($"User {name} joined the server with {URL}");
            }
            
        }

        public void addServerUrl(string u)
        {
 
            this.serversURLs.Add(u);
            Console.WriteLine($"ADDING SERVERURL: {u}");
            Console.WriteLine($"MY SERVERURL: {ServerUrl}");

            IServer publisher = (IServer)Activator.GetObject(typeof(IServer), u);
            //publisher.SetMyURL(u);

            Console.WriteLine($"SUBSCRIBED TO ALL EVENTS OF {u}");
           
        }



        public void AddMeeting(string t, int m, int ns, int ni, List<string> ss, List<string> ii, string coordinator)
        {
            foreach (string url in serversURLs)
            {
                try{
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), url);
                    server.RealAddMeeting(t, m, ns, ni, ss, ii, coordinator);
                }catch(SocketException){
                    continue;                
                }
                

            }
            RealAddMeeting(t, m, ns, ni, ss, ii, coordinator);

        }

        public void RealAddMeeting(string t, int m, int ns, int ni, List<string> ss, List<string> ii, string coordinator)
        {
            if (FREEZE)
            {
                this.freezedActions.Add(() => { AddMeeting(t, m, ns, ni, ss, ii, coordinator); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            try
            {
                lock (meetings)
                {
                    //read();
                    if (CheckTopic(t))
                    {
                        meetings.Add(new Meeting(t, m, ns, ni, ss, ii, coordinator));


                        System.Console.WriteLine($"Added Meeting, topic: {t} created by {coordinator}.");
                    }
                    //write();
                }
            }
            catch (TopicAlreadyExistsException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public bool isFreezed()
        {
            if (FREEZE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Status()
        {
            Console.WriteLine("STATUS:");
            foreach(Meeting m in meetings)
            {
                Console.WriteLine($"Meeting {m.topic} is {m.status } and has slots:");
                foreach(Slot s in m.slots)
                {
                    Console.WriteLine($"    Slot {s.location}, {s.date} has users:");
                    foreach(User u in s.joinedUsers)
                    {
                        Console.WriteLine($"        User: {u.Name}.");
                    }
                }
            }
        }

        bool CheckTopic(string t)
        {
            foreach (Meeting m in meetings)
            {
                if (m.topic.Equals(t))
                {
                    throw new TopicAlreadyExistsException("CREATE: Meeting topic already exists.");
                }
            }
            return true;
        }

        public List<Tuple<string, string, int, int, int>> ListMeetingsTopicsAndNumbers (string username)
        {


            if (FREEZE)
            {
                this.freezedActions.Add(() => { ListMeetingsTopicsAndNumbers(username); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            //read();
            List<Tuple<string, string, int, int, int>> res = new List<Tuple<string, string, int, int, int>>();
            foreach(Meeting m in meetings)
            {
                if(m.ninv == 0 || m.invites.Contains(username) || m.coord.Equals(username))
                {
                    res.Add(new Tuple<string, string, int, int, int>(m.topic, m.status, m.min, m.nslots, m.ninv));
                }
            }

            return res;
        }

        public List<Tuple<string, List<string>>> ListAvailableMeetingsConsole(string username)
        {
            
            if (FREEZE)
            {
                this.freezedActions.Add(() => { ListAvailableMeetingsConsole(username); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            //read();
            List<Tuple<string, List<string>>> res = new List<Tuple<string, List<string>>>();
            foreach (Meeting m in meetings)
            {
                List<string> slotsss = new List<string>();
                foreach (Slot slot in m.slots)
                {
                    slotsss.Add(slot.location + "," + slot.date);
                }
                if(m.ninv == 0 || m.invites.Contains(username) || m.coord.Equals(username))
                {
                    res.Add(new Tuple<string, List<string>>(m.topic, slotsss));
                }
            }
            return res;
        }

        public List<Tuple<string, string>> ShowSlotsByMeeting(string topic)
        {
            if (FREEZE)
            {
                this.freezedActions.Add(() => { ShowSlotsByMeeting(topic); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            //read();
            List<Tuple<string, string>> slotss = new List<Tuple<string, string>>();
            foreach (Meeting m in meetings)
            {
                if (string.Compare(topic, m.topic) == 0)
                {
                    System.Console.WriteLine("Encontrei meeting com o topico: " + m.topic);
                    foreach (Slot slot in m.slots)
                    {
                        System.Console.WriteLine("Slot em " + slot.location + " e a " + slot.date + " foi adicionado a lista");
                        slotss.Add(new Tuple<string, string>(slot.location, slot.date));
                    }
                    break;
                }
                else if (string.Compare(topic, m.topic) != 0)
                {
                    System.Console.WriteLine("Topico " + topic + "nao é igual a " + m.topic + ".");
                }
            }
            return slotss;
        }

        List<Room> GetRoomsByLocation(string loc)
        {

            List<Room> rm = new List<Room>();
            foreach (Room r in rooms)
            {
                if (r.location.Equals(loc))
                {
                    rm.Add(r);
                }
            }

            return rm;
        }

        /*public void NotifyUsers(int v, Meeting m)
        {
            foreach (Slot slt in m.slots)
            {
                foreach (var user in slt.joinedUsers)
                {
                    if (!user.Name.Equals(m.coord))
                    {
                        user.GetIClient().ReceiveNotification(v, slt.topic, slt.location, slt.date);
                    }
                }
            }
        }*/
        
        public int JoinSlot(string name, string topic, string location, string date)
        {
            foreach (string url in serversURLs)
            {
                try{
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), url);
                    server.RealJoinSlot(name, topic, location, date);
                }catch(SocketException){
                    continue;                
                }
                

            }

            return RealJoinSlot(name, topic, location, date);

        }

        public int RealJoinSlot(string name, string topic, string location, string date)
        {
            if (FREEZE)
            {
                this.freezedActions.Add(() => { JoinSlot(name, topic, location, date); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            
            lock (meetings)
            {
                
                Console.WriteLine(meetings.Count());
                Meeting m = meetings.Single(x => x.topic.Equals(topic));
                System.Console.WriteLine($"Searching slot {location}, {date}");
                Slot s = m.slots.Single(x => x.location.Equals(location) && x.date.Equals(date));


                if (!m.status.Equals("Opened"))
                {
                    Console.WriteLine($"Meeting {topic} is already closed.");
                    throw new MeetingIsAlreadyClosedException($"JOIN: Meeting with the topic '{topic}' is already closed.");
                }

                System.Console.WriteLine("Found slot");
                User userToAdd = users.Single(x => x.Name.Equals(name));
                if (!s.joinedUsers.Contains(userToAdd))
                {
                    s.AddUser(userToAdd);
                    System.Console.WriteLine($"User {name} joined meeting {topic} in location {location} and date {date}");
                    foreach (User u in s.joinedUsers) //apenas para debug no server
                    {
                        System.Console.WriteLine($"Slot {s.location} has user {u.Name}");
                    }
                    return 1;
                }
                else
                {
                    System.Console.WriteLine($"User {userToAdd.Name} has already joined this slot.");
                    return -1;
                }
                
            }


        }

        public int CloseMeeting(string t, string username)
        {
            
            foreach (string url in serversURLs)
            {
                try{
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), url);
                    server.RealCloseMeeting(t, username);
                }catch(SocketException){
                    continue;
                }
                

            }
    
            return RealCloseMeeting(t, username);

        }

        public int RealCloseMeeting(string t, string username)
        {
            if (FREEZE)
            {
                this.freezedActions.Add(() => { CloseMeeting(t, username); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            
            lock (meetings)
            {
                //read();
                System.Console.WriteLine($"Closing begins");
                Meeting m = meetings.Single(x => x.topic.Equals(t));
                if (String.Compare(m.coord, username) != 0)
                {
                    throw new NotCoordinatorException("CLOSE: Sorry, you're not the coordinator of the meeting therefore you can't close it.");
                }
                if (m.status != "Opened")
                {
                    throw new MeetingIsAlreadyClosedException($"CLOSE: Meeting with the topic '{t}' is already closed.");
                }
                System.Console.WriteLine($"Found meeting with topic {t}");
                int ids = 0;
                int idmax = 0;
                for (int i = 0; i < m.slots.Count(); i++) //FIND SLOT WITH HIGHEST NUMBER OF PARTICIPANTS
                {
                    if (m.slots.ElementAt(i).NumberOfParticipants() > idmax)
                    {
                        ids = i;
                        idmax = m.slots.ElementAt(i).NumberOfParticipants();
                        System.Console.WriteLine($"Found new slot with most participants");
                    }
                }

                System.Console.WriteLine($"Slot {m.slots.ElementAt(ids).location},{m.slots.ElementAt(ids).date} has the highest number of inscriptions");

                if (m.slots.ElementAt(ids).NumberOfParticipants() < m.min)
                {
                    m.MeetingCancelled();
                    //NotifyUsers(0, m);
                    System.Console.WriteLine($"NOT ENOUGH PARTICIPANTS {m.slots.ElementAt(ids).NumberOfParticipants()} < {m.min}");
                    //write();
                    throw new NotEnoughParticipantsException("CLOSE: Not enough participants to close the meeting. Meeting got cancelled.");
                }

                List<Room> rms = GetRoomsByLocation(m.slots.ElementAt(ids).location);
                System.Console.WriteLine($"Found rooms in {m.slots.ElementAt(ids).location}");
                bool flag = false;
                int a = 0;

                foreach (Room r in rms)
                {
                    if (!r.reserves.Contains(m.slots.ElementAt(ids).date))
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    m.MeetingCancelled();
                    //NotifyUsers(0, m);
                    System.Console.WriteLine($"All rooms reserved for {m.slots.ElementAt(ids).date} in {m.slots.ElementAt(ids).location}");
                    //write();
                    throw new NoRoomsAvailableException("CLOSE: No rooms available. Meeting got cancelled.");
                }

                flag = false;

                string b = "";
                foreach (Room r in rms) //FIND A ROOM THAT IS NOT RESERVED FOR THAT DATE AND HAS ENOUGH CAPACITY
                {
                    if (a < r.capacity)
                    {
                        a = r.capacity;
                        b = r.name;
                    }
                    if (r.capacity >= m.slots.ElementAt(ids).NumberOfParticipants() && !r.reserves.Contains(m.slots.ElementAt(ids).date))
                    {
                        r.reserves.Add(m.slots.ElementAt(ids).date);
                        flag = true;
                        m.MeetingClosed();
                        Console.WriteLine("The meeting is closed.");
                        //NotifyUsers(1, m);
                        //write();
                        return 1;
                    }
                }

                if (flag == false)
                {
                    Room rp = rms.Single(x => x.name.Equals(b));
                    int numberOfUsersToRemove = m.slots.ElementAt(ids).NumberOfParticipants() - rp.capacity;
                    System.Console.WriteLine($"No rooms available with enough capacity, will remove {numberOfUsersToRemove} users.");
                    List<string> removedUsersList = new List<string>();
                    for (int i = a; i < a + numberOfUsersToRemove; i++)
                    {
                        removedUsersList.Add(m.slots.ElementAt(ids).joinedUsers[i].Name);
                    }
                    m.slots.ElementAt(ids).joinedUsers.RemoveRange(a, numberOfUsersToRemove);
                    string removedUsersString = string.Join(",", removedUsersList);
                    rp.reserves.Add(m.slots.ElementAt(ids).date);
                    m.MeetingClosed();
                    Console.WriteLine("The meeting is closed.");
                    // NotifyUsers(1, m);
                    //write();
                    throw new UsersGotRemovedException($"CLOSE: User(s) {removedUsersString} got removed due to capacity issues.");
                }

                //write();
                return 0;

            }

        }

        public void RemoveUser(string name, string URL) //Remove a user from the list of users
        {
            foreach (string url in serversURLs)
            {
                try{
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), url);
                    server.RealRemoveUser(name, URL);
                }catch(SocketException){
                    continue;
                }
                

            }
            RealRemoveUser(name, URL);
        }

        public void RealRemoveUser(string name, string URL) //Remove a user from the list of users
        {
            lock (users)
            {
                User userToRemove = users.Single(x => x.Name.Equals(name) && x.Url.Equals(URL));
                users.Remove(userToRemove);
                System.Console.WriteLine($"User {name} left the server.");
            }

        }

        public bool CheckName(string n)
        {
            foreach (User u in users)
            {
                if (u.Name.Equals(n))
                {
                    return false;
                }
            }

            return true;
        }

        public void AddRoom(string loc, int cap, string nam)
        {
            if (FREEZE)
            {
                freezedActions.Add(() => { AddRoom(loc, cap, nam); });
                throw new FreezedException("This is server is freezed. Will do action when unfreezed.");
            }
            Console.WriteLine(" ADDINGROOM");
            Room r = new Room(loc, cap, nam);
            Console.WriteLine($"MIDLE {r} {r.location}");
            rooms.Add(r);
            Console.WriteLine("DONE ADDINGROOM");
        }

        public void Freeze()
        {
            if (!FREEZE) { 
                this.FREEZE = true; 
            }

        }

        public void Unfreeze()
        {
            if (FREEZE)
            {
                this.FREEZE = false;
                Parallel.Invoke(this.freezedActions.ToArray());
            }
        }




        // state machine function




    
        public void ConnectServer(string serv_url) 
        {
            //ServerUrl = serv_url;
            Console.WriteLine($"MY URL IS { ServerUrl}");
            List<string> url_info = UrlBreakdown(serv_url); // [0] -> ip ; [1] -> port ; [2] -> name
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            System.Collections.IDictionary props = new Hashtable();
            props["port"] = Int32.Parse(url_info[1]);
            TcpChannel channel = new TcpChannel(props, null, provider);
            

        
            ChannelServices.RegisterChannel(channel, false);
            ChannelDataStore data = (ChannelDataStore)channel.ChannelData;

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Server), url_info[2], WellKnownObjectMode.Singleton);

 
            /*Action a = () => { StateManager(); };
            var task = Task.Run(a);*/

        }

        // url of type => tcp://localhost:xxxxx/yyyyyyy  |  tcp://123.123.123.123:xxxxx/yyyyyy
        public List<string> UrlBreakdown(string url)        
        {
            List<string> breakdown = new List<string>();

            int pFrom = url.LastIndexOf(":") + ":".Length;
            int pTo = url.LastIndexOf("/");
            string url_port = url.Substring(pFrom, pTo - pFrom);
            
            int nameFrom = url.LastIndexOf("/") + "/".Length;
            string url_name = url.Substring(nameFrom);

            int url_index = url.IndexOf("tcp://");
            int ip_To = url.LastIndexOf(":");
            string url_ip = url.Substring(6, ip_To-6);

            if(url_ip.Equals("localhost"))
            {
                url_ip = "127.0.0.1";

            }

            breakdown.Add(url_ip);
            breakdown.Add(url_port);
            breakdown.Add(url_name);

            return breakdown;

        }

        static void Main(string[] args)
        {
            try
            {

                Server srv = new Server(args[0], args[1], Int32.Parse(args[2]), Int32.Parse(args[3]), Int32.Parse(args[4]));
                Console.WriteLine($"@ line 1187 -> {args[1]}");
                srv.ConnectServer(args[1]);

                List<string> url_info = srv.UrlBreakdown(args[1]);

                Console.WriteLine($"{url_info[2]} listening on {url_info[0]} @ port number {url_info[1]}");

                System.Console.WriteLine("<enter> to leave...");
                System.Console.ReadLine();


            } catch (SocketException)
            {
                Console.WriteLine("APANHEI UMA SOCKET EXCEPTION A TENTAR CONNECTSERVER @ SERVER.cs");
            }
            

            
        }


    }
}
