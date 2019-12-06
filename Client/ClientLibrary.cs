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
using ExceptionLibrary;

namespace ClientLibrary
{
    public class CLibrary : ICLibrary
    {
        public int CreateMeeting(string topic, int min, int nslots, int ninvites, List<string> slots, List<string> invites, string username, IServer server)
        {
            if(nslots <= 0)
            {
                throw new InvalidSlotsException("CREATE: Invalid number of slots.");
            }
            if (ninvites < invites.Count())
            {
                throw new WrongNumberOfInviteesException("CREATE: Wrong number of invites or invitees.");
            }
            if (nslots < slots.Count())
            {
                throw new WrongNumberOfSlotsException("CREATE: Wrong number of slots.");
            }
            try
            {
                server.AddMeeting(topic, min, nslots, ninvites, slots, invites, username);
            }
            catch(SocketException e ){
                throw e;
            }
            catch(TopicAlreadyExistsException)
            {
                throw;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return 1;
        }

        public List<Tuple<string, List<string>>> ListAvailableMeetingsConsole(string username, IServer server) //SERA ASSIM? quiçá
        {
            try
            {
                List<Tuple<string, List<string>>> res = server.ListAvailableMeetingsConsole(username);
                return res;
            }
            catch(SocketException e ){
                throw e;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Tuple<string, string, int, int, int>> ListMeetingsTopicsAndNumbers(string username, IServer server)
        {
            try
            {
                List<Tuple<string, string, int, int, int>> res = server.ListMeetingsTopicsAndNumbers(username);
                return res;
            }
            catch(SocketException e ){
                throw e;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Tuple<string, string>> ShowMeetingSlots(string topic, IServer server)
        {
            try
            {
                List<Tuple<string, string>> slotss = server.ShowSlotsByMeeting(topic);
                return slotss;
            }
            catch(SocketException e ){
                throw e;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<int> JoinSlots(List<Tuple<string, string, string>> l, string username, IServer server)
        {
            try
            {
                List<int> res = new List<int>();
                foreach (Tuple<string, string, string> s in l)
                {
                    res.Add(server.JoinSlot(username, s.Item1, s.Item2, s.Item3));
                }
                return res;
            }
            catch(SocketException e ){
                throw e;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public int CloseMeeting(string topic, string username, IServer server)
        {
            int res;
            try
            {
               res = server.CloseMeeting(topic, username);
            }
            catch(SocketException e ){
                throw e;
            }
            catch(NotCoordinatorException)
            {
                throw;
            }
            catch(MeetingIsAlreadyClosedException)
            {
                throw;
            }
            catch (NotEnoughParticipantsException)
            {
                throw;
            }
            catch (NoRoomsAvailableException)
            {
                throw;
            }
            catch (UsersGotRemovedException)
            {
                throw;
            }
            catch (FreezedException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return res;
        }

        public void ReceiveNotification(int v, string top, string loc, string dat)
        {
            if(v == 1)
            {
                MessageBox.Show($"Meeting {top} has been closed in slot : {loc}, {dat}.");
            }
            else { MessageBox.Show($"Meeting {top} has been canceled."); }
        }

    }
}
