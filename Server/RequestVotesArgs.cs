using System;

namespace Server
{
    class RequestVotesArgs : EventArgs
    {
        public int Term { get; set; }

        public string CandidateURL { get; set; }

        public int LastLogIndex { get; set; }

        public int LastLogTerm { get; set; }

    }
}