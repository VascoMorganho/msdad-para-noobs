using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class WrongNumberOfSlotsException : Exception
    {
        public WrongNumberOfSlotsException() { }

        public WrongNumberOfSlotsException(string message) : base(message) { }

        public WrongNumberOfSlotsException(string message, Exception innerException) : base(message, innerException) { }

        protected WrongNumberOfSlotsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
