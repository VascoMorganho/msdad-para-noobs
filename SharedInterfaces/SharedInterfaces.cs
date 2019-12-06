using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedInterfaces
{


    public interface ICLibrary
    {
        int CreateMeeting(string topic, int min, int nslots, int ninvites, List<string> slots, List<string> invites, string username, IServer s);
        
        List<Tuple<string, string, int, int, int>> ListMeetingsTopicsAndNumbers(string username, IServer s);

        List<Tuple<string, List<string>>> ListAvailableMeetingsConsole(string username, IServer s);

        List<Tuple<string, string>> ShowMeetingSlots(string topic, IServer s);

        List<int> JoinSlots(List<Tuple<string, string, string>> l, string username, IServer s);

        int CloseMeeting(string topic, string username, IServer s);

        void ReceiveNotification(int v, string top, string loc, string dat);

    }



    public interface IServer
    {



        List<string> AddUser(string name, string URL);

        void RealAddUser(string name, string URL);
        void AddMeeting(string t, int m, int ns, int ni, List<string> ss, List<string> ii, string coordinator);
        void RealAddMeeting(string t, int m, int ns, int ni, List<string> ss, List<string> ii, string coordinator);
        void RemoveUser(string name, string URL);
        void RealRemoveUser(string name, string URL);

        int JoinSlot(string name, string topic, string location, string date);
        int RealJoinSlot(string name, string topic, string location, string date);

        List<Tuple<string, string, int, int, int>> ListMeetingsTopicsAndNumbers(string username);

        List<Tuple<string, string>> ShowSlotsByMeeting(string topic);

        List<Tuple<string, List<string>>> ListAvailableMeetingsConsole(string username);

        bool CheckName(string n);

        int CloseMeeting(string meeting, string username);
        int RealCloseMeeting(string meeting, string username);

        void AddRoom(string location, int capacity, string room_name);

        void Status();

        bool isFreezed();

        void Freeze();

        void Unfreeze();

        void addServerUrl(string url);


       
    }
}
