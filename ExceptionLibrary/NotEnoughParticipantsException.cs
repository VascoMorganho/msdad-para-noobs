using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class NotEnoughParticipantsException : Exception
    {
        public NotEnoughParticipantsException() { }

        public NotEnoughParticipantsException(string message) : base(message) { }

        public NotEnoughParticipantsException(string message, Exception innerException) : base(message, innerException) { }

        protected NotEnoughParticipantsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
