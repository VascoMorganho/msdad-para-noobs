using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;
using PCS;
using System.Net;
using SharedInterfaces;
using System.Threading;
using System.Diagnostics;
using System.IO;
using ExceptionLibrary;

namespace PuppetMaster
{
    class PuppetMaster
    {
        List<Tuple<string, string, string, string>> serverList;
        List<Tuple<string, string, string, string>> clientList;
        List<Tuple<string, string, Process>> runningProcesses;
        List<string> crashedServers;

        private string curr_path = Environment.CurrentDirectory;
        private Dictionary<int, string> proc_dict = new Dictionary<int, string>();
        public PuppetMaster(string[] args) {
            serverList = new List<Tuple<string, string, string, string>>();
            clientList = new List<Tuple<string, string, string, string>>();
            runningProcesses = new List<Tuple<string, string, Process>>();
            crashedServers = new List<string>();
            this.StartPuppetMaster(args);
        }
        void CreateServerPPM(string id, string url, int max_faults, int min_delay, int max_delay) { 
            Process proc = new Process();

            string srv_path = @"..\..\..\Server\bin\Debug\Server.exe";

            proc.StartInfo.FileName = Path.GetFullPath(Path.Combine(this.curr_path, srv_path));
            proc.StartInfo.Arguments = $"{id} {url} {max_faults} {min_delay} {max_delay}";
            
            runningProcesses.Add(new Tuple<string, string, Process>(id, url, proc));
            proc.Start();
            proc_dict.Add(proc.Id, id);
        }
        void CreateClientPPM(string username, string client_url, string server_url, string script_name) {
            Process proc = new Process();
            string cli_gui_path = @"..\..\..\ClientGui\bin\Debug\ClientGui.exe";
            string cli_script_path = @"..\..\..\ClientScript\bin\Debug\ClientScript.exe";

            /* GUI Version of the client */
            if (script_name.Equals(""))
            {
                proc.StartInfo.FileName = Path.GetFullPath(Path.Combine(this.curr_path, cli_gui_path));   /* supostamente, o .exe do client vai estar na mesma directoria, basta por clientgui.exe */

                proc.StartInfo.Arguments = $"{username} {client_url} {server_url}";
                proc.Start();
                proc_dict.Add(proc.Id, username);

            }
            /* Script Version of the client */
            else
            {
                proc.StartInfo.FileName = Path.GetFullPath(Path.Combine(this.curr_path, cli_script_path));   /* supostamente, o .exe do client vai estar na mesma directoria, basta por clientscript.exe */

                proc.StartInfo.Arguments = $"{username} {client_url} {server_url} {script_name}";

                proc.Start();
                proc_dict.Add(proc.Id, username);

            }
        }


        void CrashPPM(string id) {
            int flag = 0;
            foreach (KeyValuePair<int, string> item in proc_dict)
            {
                try
                {
                    if (item.Value.Equals(id))
                    {
                        Process.GetProcessById(item.Key).Kill();
                        Tuple<string, string, Process> t = runningProcesses.Single(pr => pr.Item1 == id);
                        runningProcesses.Remove(t);
                        flag = item.Key;
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Process is not running"); //TODO throw exception to PuppetMaster ???
                }
                //Console.WriteLine($"{item.Value} process is running and its process_id is {item.Key}.");
            }
            if (flag != 0){
                proc_dict.Remove(flag);
            }
        }

        public void CreateServer(string id, string url, int max_faults, int min_delay, int max_delay)
        {
            if(serverList.Any(cp => cp.Item1 == id))
            {
                System.Console.WriteLine("A server with that id already exists.");
                return;
            }

            int pFrom = url.IndexOf("//") + "//".Length;
            int pTo = url.LastIndexOf(":");

            String server_ip = url.Substring(pFrom, pTo - pFrom);

            if (string.Compare(server_ip, GetLocalIPAddress()) !=0 && string.Compare(server_ip, "localhost") != 0)
            {
                if (serverList.Any(pc => pc.Item2 == url))
                {
                    System.Console.WriteLine("A server with that URL already exists.");
                    return;
                }
                else
                {
                    System.Console.WriteLine("Telling PCS to create server.");
                    PCS.PCS p = (PCS.PCS)Activator.GetObject(typeof(PCS.PCS), $"tcp://{server_ip}:10000/PCS");
                    p.CreateServer(id, url, max_faults, min_delay, max_delay);

                    IServer server = (IServer)Activator.GetObject(typeof(IServer), url);

                    foreach (Tuple<string, string, string, string> t in serverList)
                    {
                        server.addServerUrl(t.Item2);
                        IServer s = (IServer)Activator.GetObject(typeof(IServer), t.Item2);
                        s.addServerUrl(url);
                    }

                    serverList.Add(new Tuple<string, string, string, string>(id, url, server_ip, "PCS"));

                }               
            }
            else
            {
                System.Console.WriteLine("Creating server locally.");
                this.CreateServerPPM(id, url, max_faults, min_delay, max_delay);

                IServer server = (IServer)Activator.GetObject(typeof(IServer), url);
                foreach (Tuple<string, string, string, string> t in serverList)
                {
                    server.addServerUrl(t.Item2);
                    IServer s = (IServer)Activator.GetObject(typeof(IServer), t.Item2);
                    s.addServerUrl(url);
                }

                serverList.Add(new Tuple<string, string, string, string>(id, url, server_ip,"PPM"));
            }
        }

        public void CreateClient(string username, string client_url, string server_url, string script_name)
        {
            if (clientList.Any(cp => cp.Item1 == username))
            {
                System.Console.WriteLine("An user with that name already exists.");
                return;
            }

            int pFrom = client_url.IndexOf("//") + "//".Length;
            int pTo = client_url.LastIndexOf(":");

            String client_ip = client_url.Substring(pFrom, pTo - pFrom);

            if (string.Compare(client_url, GetLocalIPAddress()) != 0 && string.Compare(client_ip, "localhost") != 0)
            {
                if (clientList.Any(pc => pc.Item2 == client_url))
                {
                    System.Console.WriteLine("A client with that URL already exists.");
                    return;
                }
                else if(!serverList.Any(s => s.Item2 == server_url))
                {
                    System.Console.WriteLine("There is no server running on that URL.");
                    return;
                }
                else
                {
                    System.Console.WriteLine("Telling PCS to create client.");
                    PCS.PCS p = (PCS.PCS)Activator.GetObject(typeof(PCS.PCS), $"tcp://{client_ip}:10000/PCS");
                    p.CreateClient(username, client_url, server_url, script_name);
                    clientList.Add(new Tuple<string, string, string, string>(username, client_url, client_ip, "PCS"));
                }
            }
            else
            {
                System.Console.WriteLine("Creating client locally.");
                this.CreateClientPPM(username, client_url, server_url, script_name);
                clientList.Add(new Tuple<string, string, string, string>(username, client_url, client_ip, "PPM"));
            }
        }

        void AddRoom(string location, int capacity, string room_name) { //ADICIONAR A TODOS OS SERVERS, OU A SO UM E DPS PROPAGAR ??
            try
            {
                foreach (Tuple<string, string, string, string> t in serverList)
                {
                    Console.WriteLine($"{t.Item2}\n");
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), t.Item2);
                    server.AddRoom(location, capacity, room_name);
                }
            }
            catch (FreezedException fe) {
                Console.WriteLine($"{fe.Message}");
            }
        }

        void Status()
        {
            foreach (Tuple<string, string, string, string> t in serverList)
            {
                try{
                    string sts = "";
                    IServer server = (IServer)Activator.GetObject(typeof(IServer), t.Item2);
                    if (server.isFreezed())
                    {
                        sts = "freezed";
                    }
                    else
                    {
                        sts = "unfreezed";
                    }
                    Console.WriteLine($"Server {t.Item1} is {sts}, and has status:");
                    server.Status();
                }catch(SocketException){
                    continue;
                }
                
            }
            foreach(string s in crashedServers)
            {
                Console.WriteLine($"Server {s} is crashed.");
            }
        }

        void Crash(string id)
        {
            Tuple<string, string, string, string> process;

            if (serverList.Any(pc => pc.Item1 == id))
            {
                process = serverList.Single(s => s.Item1 == id);
                serverList.Remove(process);
                crashedServers.Add(id);

            }
            else
            {
                System.Console.WriteLine($"There is no process running with id: {id}");
                return;
            }


            if (string.Compare(process.Item4, "PPM") == 0)
            {
                this.CrashPPM(id);
            }
            else
            {
                PCS.PCS p = (PCS.PCS)Activator.GetObject(typeof(PCS.PCS), $"tcp://{process.Item3}:10000/PCS");
                p.Crash(id);
            }
        }
        void Freeze(string id)
        {
            Tuple<string, string, string, string> t;

            if (serverList.Any(pc => pc.Item1 == id))
            {
                t = serverList.Single(s => s.Item1 == id);
                IServer server = (IServer)Activator.GetObject(typeof(IServer), t.Item2);
                server.Freeze();
            }
            else
            {
                System.Console.WriteLine($"There is no process running with id: {id}");
                return;
            }

        }

        void Unfreeze(string id)
        {
            Tuple<string, string, string, string> t;

            if (serverList.Any(pc => pc.Item1 == id))
            {
                t = serverList.Single(s => s.Item1 == id);
                IServer server = (IServer)Activator.GetObject(typeof(IServer), t.Item2);
                server.Unfreeze();
            }
            else
            {
                System.Console.WriteLine($"There is no process running with id: {id}");
                return;
            }
        }

        void Wait(int xms)
        {
            System.Console.WriteLine($"This thread will sleep for {xms} milliseconds.");
            Thread.Sleep(xms);
        }

        public static string GetLocalIPAddress() // RETURNS IP OF PUPPETMASTER
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        
        //---------------------------PUPPETMASTER READS CONSOLE BELOW----------------------------------
        public void ReceiveCommands(List<string> commandArgs)
        {
            switch (commandArgs[0].ToLower())
            {
                case "server":
                    this.CreateServer(commandArgs[1], commandArgs[2], Int32.Parse(commandArgs[3]), Int32.Parse(commandArgs[4]), Int32.Parse(commandArgs[5]));
                    break;

                case "client":
                    if (commandArgs.Count == 5)
                    {
                        this.CreateClient(commandArgs[1], commandArgs[2], commandArgs[3], commandArgs[4]);
                    }
                    else
                    {
                        this.CreateClient(commandArgs[1], commandArgs[2], commandArgs[3], "");
                    }
                    break;

                case "addroom":
                    this.AddRoom(commandArgs[1], Int32.Parse(commandArgs[2]), commandArgs[3]);
                    break;

                case "status":
                    this.Status();
                    break;

                case "crash":
                    this.Crash(commandArgs[1]);
                    break;

                case "freeze":
                    this.Freeze(commandArgs[1]);
                    break;

                case "unfreeze":
                    this.Unfreeze(commandArgs[1]);
                    break;

                case "wait":
                    this.Wait(Int32.Parse(commandArgs[1]));
                    break;
            }
        }

        void ReadScript(string script_name)
        {
            try
            {
                string[] scriptCommands = System.IO.File.ReadAllLines(script_name);

                foreach (string c in scriptCommands)
                {
                    this.ReceiveCommands(c.Split(' ').ToList());
                }
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("File wasn't found, please try again.");
            }
        }
        void StartPuppetMaster(string[] args)
        {
            TcpChannel channel = new TcpChannel(10001);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(PuppetMaster), "PuppetMaster", WellKnownObjectMode.Singleton);
            Console.WriteLine("Welcome to the PuppetMaster! Write the command(s) you want or specify the script filename to read from:");

            while (true)
            {
                string input = Console.ReadLine();
                if (input.Contains(" ") || string.Compare(input, "Status")==0 || string.Compare(input, "status") == 0)
                {
                    this.ReceiveCommands(input.Split(' ').ToList());
                }
                else if (input.Equals(""))
                {
                    continue;
                }
                else
                {
                    System.Console.WriteLine($"Trying to execute script with the name: {input}");
                    this.ReadScript(input);
                }

            }
        }

        static void Main(string[] args)
        {
            new PuppetMaster(args);

        }
    }
}
