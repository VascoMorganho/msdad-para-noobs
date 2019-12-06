using System;

namespace ExceptionLibrary
{
    [Serializable]
    public class TopicAlreadyExistsException : Exception
    {
        public TopicAlreadyExistsException() { }
        public TopicAlreadyExistsException(string message) : base(message) { }
        public TopicAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected TopicAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}