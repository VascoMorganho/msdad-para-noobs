using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLibrary
{

    [Serializable]
    public class FreezedException : Exception
    {
        public FreezedException() { }

        public FreezedException(string message) : base(message) { }

        public FreezedException(string message, Exception innerException) : base(message, innerException) { }

        protected FreezedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
