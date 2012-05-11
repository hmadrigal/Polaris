using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    [Serializable]
    public class KinectNotFoundException : Exception
    {
        public KinectNotFoundException() { }

        public KinectNotFoundException(string message) : base(message) { }

        public KinectNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected KinectNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}