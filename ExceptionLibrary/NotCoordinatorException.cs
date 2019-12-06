using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class NotCoordinatorException : Exception
    {
        public NotCoordinatorException() { }

        public NotCoordinatorException(string message) : base(message) { }

        public NotCoordinatorException(string message, Exception innerException) : base(message, innerException) { }

        protected NotCoordinatorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}