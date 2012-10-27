using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    public interface ISpeechRecognitionPayload
    {
        float Confidence { get; }

        string Text { get; }

        string Command { get; }

        string Argument { get; }
    }
}