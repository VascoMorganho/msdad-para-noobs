using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class MeetingIsAlreadyClosedException : Exception
    {
        public MeetingIsAlreadyClosedException() { }

        public MeetingIsAlreadyClosedException(string message) : base(message) { }

        public MeetingIsAlreadyClosedException(string message, Exception innerException) : base(message, innerException) { }

        protected MeetingIsAlreadyClosedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
