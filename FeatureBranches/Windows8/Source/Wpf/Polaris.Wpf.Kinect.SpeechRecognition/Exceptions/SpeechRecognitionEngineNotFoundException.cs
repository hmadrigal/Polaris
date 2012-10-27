using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Services
{
    [Serializable]
    internal class SpeechRecognitionEngineNotFoundException : ServiceInitializationException
    {
        public SpeechRecognitionEngineNotFoundException() { }

        public SpeechRecognitionEngineNotFoundException(string message) : base(message) { }

        public SpeechRecognitionEngineNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected SpeechRecognitionEngineNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}