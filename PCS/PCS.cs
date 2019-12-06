using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Net.Sockets;


namespace PCS
{
    public class PCS : MarshalByRefObject
    {

        private string curr_path = Environment.CurrentDirectory;
        private Dictionary<int, string> proc_dict = new Dictionary<int, string>();
        List<Tuple<string, string, Process>> runningProcesses = new List<Tuple<string, string, Process>>();

        public PCS(){}

        public void Hello()
        {
            Console.WriteLine("HELLO!");
        }



        /* This command creates a server process identified by server id, available at URL that delays any
         * incoming message for a random amount of time(specified in milliseconds) between min delay and max delay.
         * If the value of both min delay and max delay is set to 0, the server should not add any delay to incoming messages.
         * Note that the delay should affect all communications with the server.
         * The parameter max faults determines how many simultaneous faults may happen, i.e., how many servers may fail before an operation is guaranteed to be replicated.
         * Important note: delays should not cause to be re-ordered.
         * For instance, if you are using TCP, and TCP messages in a given order, this order should be preserved. */

        public void CreateServer(string server_id, string url, int max_faults, int min_delay, int max_delay)
        {
            Process proc = new Process();

            string srv_path = @"..\..\..\Server\bin\Debug\Server.exe";

            proc.StartInfo.FileName = Path.GetFullPath(Path.Combine(this.curr_path, srv_path));
            proc.StartInfo.Arguments = $"{server_id} {url} {max_faults} {min_delay} {max_delay}";
            proc.Start();
            runningProcesses.Add(new Tuple<string, string, Process>(server_id, url, proc));
            proc_dict.Add(proc.Id, server_id);
        }



        /* This command creates a client process identified by the string username, available at client URL, that will connect
            to a preferred server at server URL, and that will execute the commands in the script file.
            It can be assumed that the script file is located in the disk folder as the executable. */

        /* The variable string script_file= "" means that this argument is optional. The default value is an empty string */

        public void CreateClient(string username, string client_url, string server_url, string script_file =  "")   
        {
            Process proc = new Process();
            string cli_gui_path = @"..\..\..\ClientGui\bin\Debug\ClientGui.exe";
            string cli_script_path = @"..\..\..\ClientScript\bin\Debug\ClientScript.exe";
            
            /* GUI Version of the client */
            if (script_file.Equals(""))         
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

                proc.StartInfo.Arguments = $"{username} {client_url} {server_url} {script_file}";

                proc.Start();
                proc_dict.Add(proc.Id, username);

            }

        }

        /* This command makes all nodes in the system print their current status.
         * The status command should present brief information about the state of the system
         * (who is present, which nodes are presumed failed, etc...).
         * Status information can be printed on each nodes’ console and does not need to be centralised at the PuppetMaster. */

        public void Status()
        {
            foreach (KeyValuePair<int, string> item in proc_dict)
            {
                try
                {
                    Process.GetProcessById(item.Key);
                } catch (ArgumentException)
                {
                    Console.WriteLine("Process is not running"); //TODO throw exception to PuppetMaster ???
                }
                Console.WriteLine($"{item.Value} process is running and its process_id is {item.Key}.");
            }

        }

        public void Crash(string id)
        {
            foreach (KeyValuePair<int, string> item in proc_dict)
            {
                try
                {
                    if (item.Value.Equals(id))
                    {
                        Process.GetProcessById(item.Key).Kill();
                        Tuple<string, string, Process> t = runningProcesses.Single(pr => pr.Item1 == id);
                        runningProcesses.Remove(t);
                        proc_dict.Remove(item.Key);
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Process is not running"); //TODO throw exception to PuppetMaster ???
                }
                //Console.WriteLine($"{item.Value} process is running and its process_id is {item.Key}.");
            }
        }


        static void Main(string[] args)
        {
            TcpChannel c = new TcpChannel(10000);
            ChannelServices.RegisterChannel(c, false);

            PCS cc = new PCS();

            RemotingServices.Marshal(cc, "PCS", typeof(PCS));
            //string v = @"..\..\..\Client\bin\Debug\";
            //string t = Environment.CurrentDirectory;
            //string x = Path.GetFullPath(Path.Combine(t, v ));
            
            //System.Console.WriteLine(x);
            System.Console.WriteLine(" < enter> to leave...");
            //Console.WriteLine("ATTEMPTING CONNECTION TO MINION");

            //cc.CreateServer("s1", "tcp://localhost:3000/server1", 0, 100, 200);

            //cc.CreateClient("DEEZNUTZ", "tcp://194.210.223.254:4000/client3", "tcp://231.210.221.230:8086/WSDAD", "script_my_nuts");
            //System.Console.WriteLine("CONNECTION SUCCESSFUL");

            //cc.CreateClient("DEEXNUR", "tcp://194.123.213.123:4000/clit", "tcp://localhost:3000/server1");

            System.Console.ReadLine();
        }
    }
}
