using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    [Serializable]
    public class ServiceNotInitializedException : Exception
    {
        public ServiceNotInitializedException() { }

        public ServiceNotInitializedException(string message) : base(message) { }

        public ServiceNotInitializedException(string message, Exception inner) : base(message, inner) { }

        protected ServiceNotInitializedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}