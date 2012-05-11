using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    [Serializable]
    public class ServiceInitializationException : Exception
    {
        public ServiceInitializationException() { }

        public ServiceInitializationException(string message) : base(message) { }

        public ServiceInitializationException(string message, Exception inner) : base(message, inner) { }

        protected ServiceInitializationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}