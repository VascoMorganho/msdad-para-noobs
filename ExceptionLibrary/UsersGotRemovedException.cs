using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class UsersGotRemovedException : Exception
    {
        public UsersGotRemovedException() { }

        public UsersGotRemovedException(string message) : base(message) { }

        public UsersGotRemovedException(string message, Exception innerException) : base(message, innerException) { }

        protected UsersGotRemovedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
