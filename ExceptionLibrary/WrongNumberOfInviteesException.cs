using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class WrongNumberOfInviteesException : Exception
    {
        public WrongNumberOfInviteesException() { }

        public WrongNumberOfInviteesException(string message) : base(message) { }

        public WrongNumberOfInviteesException(string message, Exception innerException) : base(message, innerException) { }

        protected WrongNumberOfInviteesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}