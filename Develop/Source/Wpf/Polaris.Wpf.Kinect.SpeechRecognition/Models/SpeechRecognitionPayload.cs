using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Services
{
    internal class SpeechRecognitionPayload : ISpeechRecognitionPayload
    {
        public float Confidence { get; set; }

        public String Text { get; set; }

        public string Command { get; set; }

        public string Argument { get; set; }
    }
}