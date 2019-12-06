using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class NoRoomsAvailableException : Exception
    {
        public NoRoomsAvailableException() { }

        public NoRoomsAvailableException(string message) : base(message) { }

        public NoRoomsAvailableException(string message, Exception innerException) : base(message, innerException) { }

        protected NoRoomsAvailableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
