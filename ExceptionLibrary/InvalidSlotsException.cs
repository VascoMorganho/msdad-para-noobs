using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class InvalidSlotsException : Exception
    {
        public InvalidSlotsException() { }
        public InvalidSlotsException(string message) : base(message) { }
        public InvalidSlotsException(string message, Exception inner) : base(message, inner) { }
        protected InvalidSlotsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}