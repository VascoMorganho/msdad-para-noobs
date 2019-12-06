using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

using SharedInterfaces;
using ClientLibrary;
using System.Net.Sockets;
using ExceptionLibrary;

namespace ClientScript
{
    class ClientScript : MarshalByRefObject
    {
        private string name;
        private string client_url;
        private string server_url;
        private string script_filename;
        private int modo_de_processar; //1 stepByStep, 2 otherwise
        public static IServer s = null;
        List<string> serverURLS = new List<string>();
        CLibrary clib;

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

        public string ScriptFilename
        {
            get { return script_filename; }
            set { script_filename = value; }
        }

        public int Modo
        {
            get { return modo_de_processar; }
            set { modo_de_processar = value; }
        }

        public ClientScript(string name)
        {
            this.Name = name;
        }
        public ClientScript(string name, string cUrl, string sUrl, string scriptFilename, int modo)
        {
            Console.WriteLine("construtor");
            this.Name = name;
            this.CUrl = cUrl;
            this.SUrl = sUrl;
            this.ScriptFilename = scriptFilename;
            this.Modo = modo;
            clib = new CLibrary();
            this.RunClientScript();
        }

        void RunClientScript()
        {
            Console.WriteLine("running scrpt");
            this.Connect();
            if(this.Modo == 1)
            {
                this.ProcessScriptFileStepByStep();
            }
            else if (this.Modo == 2)
            {
                this.ProcessScriptFileTotal();
            }
        }

        void Connect()
        {
            Console.WriteLine("doing connect");
            List<string> lst = UrlBreakdown(this.CUrl);

            TcpChannel c = new TcpChannel(Int32.Parse(lst[1]));
            ChannelServices.RegisterChannel(c, false);

            RemotingServices.Marshal(this, lst[2], typeof(ClientScript));
            Console.WriteLine("chamarei proxy do server");
            s = (IServer)Activator.GetObject(typeof(IServer), this.SUrl);
            Console.WriteLine("yay");

            if (!s.CheckName(name))
            {
                Console.WriteLine($"An user with name '{name}' already exists.");
                c.StopListening(null);
                RemotingServices.Disconnect(this);
                ChannelServices.UnregisterChannel(c);
                //cs = null; FIXME é necessario????????????? how?
                return;
            }
            Console.WriteLine("faeri try");
            try
            {
                serverURLS = s.AddUser(name, this.CUrl);
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Connection successful");
        }

        void ProcessScriptFileStepByStep()
        {
            string script_path = @"..\..\..\scripts\";
            Console.WriteLine(Path.GetFullPath(Path.Combine(script_path, this.ScriptFilename)));

            StreamReader reader = new StreamReader(Path.GetFullPath(Path.Combine(script_path, this.ScriptFilename))); //ONDE ESTA O FILE? 
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine("***COMMAND: " + line + "***");
                ProcessCommand(line);
                Console.ReadLine();
            }
        }

        void ProcessScriptFileTotal()
        {
            string script_path = @"..\..\..\scripts\";
            Console.WriteLine(Path.GetFullPath(Path.Combine(script_path, this.ScriptFilename)));

            StreamReader reader = new StreamReader(Path.GetFullPath(Path.Combine(script_path, this.ScriptFilename))); //ONDE ESTA O FILE? 
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine("***COMMAND: " + line + "***");
                ProcessCommand(line);
            }
        }

        void ProcessCommand(string command)
        {
            Console.WriteLine("WTF");
            string first = GetFirstWord(command);
            string[] words = command.Split(' ');
            switch (first)
            {
                case "create":
                    Console.WriteLine("CREATING MEETING");
                    CaseCreateMeeting(words);
                    break;
                case "list":
                    CaseListMeetings();
                    break;
                case "join":
                    CaseJoinSlots(words);
                    break;
                case "close":
                    CaseCloseMeeting(words);
                    break;
                case "wait":
                    Task.Delay(Int32.Parse(words[1])).Wait();
                    break;
            }
        }
        int iccm =0;
        void CaseCreateMeeting(string[] words)
        {
            
            Console.WriteLine("***INSIDE CREATE***");
            if (words.Count() < 6)
            {
                Console.WriteLine("Insuficient number of Parameters on 'create' command.");
            }
            else if (words.Count() != (5 + Int32.Parse(words[3]) + Int32.Parse(words[4])))
            {
                Console.WriteLine("Wrong number of Parameters on 'create' command.");
            }
            List<string> slots = new List<string>();
            List<string> invitees = new List<string>();
            int i = 5;
            while (i < 5 + Int32.Parse(words[3]))
            {
                slots.Add(words[i]);
                i++;
            }
            while (i < 5 + Int32.Parse(words[3]) + Int32.Parse(words[4]))
            {
                invitees.Add(words[i]);
                i++;
            }
            int resCreate;
            try
            {
                resCreate = clib.CreateMeeting(words[1], Int32.Parse(words[2]), Int32.Parse(words[3]), Int32.Parse(words[4]), slots, invitees, this.Name, s);
                if (resCreate == 1)
                {
                    Console.WriteLine($"Meeting '{words[1]}' created successfully.");
                }
            }
            catch(SocketException){
                Console.WriteLine("Changing server.");
                s = (IServer)Activator.GetObject(typeof(IServer), serverURLS[iccm]);
                CaseCreateMeeting(words);
                iccm++;
            }
            catch (InvalidSlotsException ise)
            {
                Console.WriteLine("Error: " + ise.Message);
            }
            catch(TopicAlreadyExistsException taee)
            {
                Console.WriteLine("Error: " + taee.Message);
            }
            catch(WrongNumberOfInviteesException wnoie)
            {
                Console.WriteLine("Error: " + wnoie.Message);
            }
            catch(WrongNumberOfSlotsException wnose)
            {
                Console.WriteLine("Error: " + wnose.Message);
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        int iclm = 0;
        void CaseListMeetings()
        {
            try
            {
                List<Tuple<string, List<string>>> resList = clib.ListAvailableMeetingsConsole(this.Name, s);
                string stringToWrite;
                foreach (Tuple<string, List<string>> tuplo in resList)
                {
                    stringToWrite = "Topico: " + tuplo.Item1 + " Slots:";
                    foreach (string s in tuplo.Item2)
                    {
                        stringToWrite += $" {s}";
                    }
                    Console.WriteLine(stringToWrite);
                }
            }
            catch(SocketException){
                Console.WriteLine("Changing server.");
                s = (IServer)Activator.GetObject(typeof(IServer), serverURLS[iclm]);
                CaseListMeetings();
                iclm++;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        int icjs = 0;
        void CaseJoinSlots(string[] words)
        {
            List<Tuple<string, string, string>> slotsToJoin = new List<Tuple<string, string, string>>();
            for (int j = 3; j < words.Count(); j++)
            {
                Tuple<string, string, string> aux = new Tuple<string, string, string>(words[1], words[j].Split(',').First(), words[j].Split(',').Last());
                slotsToJoin.Add(aux);
            }
            try
            {
                List<int> res = clib.JoinSlots(slotsToJoin, this.Name, s);
                for (int k = 0; k < res.Count(); k++)
                {
                    if (res[k] == 1) Console.WriteLine($"Joined meeting {words[1]} in slot {words[3 + k]}");
                    else if (res[k] == -1) Console.WriteLine($"Cannon join slot {words[3 + k]} because User {this.Name} has already joined this slot.");
                }
            }
            catch(SocketException){
                Console.WriteLine("Changing server.");
                s = (IServer)Activator.GetObject(typeof(IServer), serverURLS[icjs]);
                CaseJoinSlots(words);
                icjs++;
            }
            catch (MeetingIsAlreadyClosedException miace)
            {
                Console.WriteLine(miace.Message);
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        int iccm2=0;
        void CaseCloseMeeting(string[] words)
        {
            try
            {
                int resClose = clib.CloseMeeting(words[1], this.Name, s);
                if (resClose == 1)
                {
                    Console.WriteLine("You closed that meeting.");
                }
            }
            catch(SocketException){
                Console.WriteLine("Changing server.");
                s = (IServer)Activator.GetObject(typeof(IServer), serverURLS[iccm2]);
                CaseCloseMeeting(words);
                iccm2++;
            }
            catch (MeetingIsAlreadyClosedException)
            {
                Console.WriteLine("Error: CLOSE: You need to choose a meeting that is still opened.");
            }
            catch (NotCoordinatorException)
            {
                Console.WriteLine("Error: CLOSE: Sorry, you don't have permissions to close this meeting.");
            }
            catch (NotEnoughParticipantsException nepe)
            {
                Console.WriteLine("Error: " + nepe.Message);
            }
            catch (NoRoomsAvailableException nrae)
            {
                Console.WriteLine("Error: " + nrae.Message);
            }
            catch (UsersGotRemovedException ugre)
            {
                Console.WriteLine("Warning: " + ugre.Message + " Meeting closed.");
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        static string GetFirstWord(string text)
        {
            string firstWord = String.Empty;

            if (String.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            firstWord = text.Split(' ').FirstOrDefault();
            if (String.IsNullOrEmpty(firstWord))
            {
                return string.Empty;
            }

            return firstWord;
        }

        /*public static string GetLocalIPAddress()
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
        }*/

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

        static void Main(string[] args)
        {
            //FIXME: TALVEZ NAO SEJA BOA IDEIA SER ASSIM QUE E FEITO
            //FIXME (cont.): PODERIA SER DITO O MODO DE COMO CORRER A SCRIPT AO CRIAR O CLIENT NO PUPPETMASTER
            Console.WriteLine($"Do you want to run the script step by step or everything at once? file: {args[3]}");
            Console.WriteLine("Press 1 and Enter for the 1st option, Press 2 and Enter to do everything at once, Press 0 and Enter to exit.");
            string input_modo = Console.ReadLine();
            while(!input_modo.Equals("1") && !input_modo.Equals("2") && !input_modo.Equals("0"))
            {
                Console.WriteLine($"Do you want to run the script step by step or everything at once?: {args[3]}");
                Console.WriteLine("Press 1 and Enter for the 1st option, Press 2 and Enter to do everything at once, Press 0 and Enter to exit.");
                input_modo = Console.ReadLine();
            }
            if (input_modo.Equals("1"))
            {
                new ClientScript(args[0], args[1], args[2], args[3], 1); //stepByStep
                Console.WriteLine("PRESS ANY BUTTON TO EXIT");
                Console.ReadLine();
                return;
            }
            else if (input_modo.Equals("2"))
            {
                new ClientScript(args[0], args[1], args[2], args[3], 2); //allAtOnce
                Console.WriteLine("PRESS ANY BUTTON TO EXIT");
                Console.ReadLine();
                return;
            }
            else if (input_modo.Equals("0"))
            {
                return;
            }
            
        }
    }
}
