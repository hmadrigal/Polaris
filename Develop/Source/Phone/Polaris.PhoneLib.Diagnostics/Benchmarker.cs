using System;
using System.Diagnostics;
using System.IO;

namespace Polaris.PhoneLib.Diagnostics
{
#if DEBUG
    public class Benchmarker : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _endingFormatMessage;
        private readonly string _tag;
        private readonly TextWriter _log;
        private readonly string _initialMessage;
        private static int _passCount = 0;

        public Benchmarker(string initialMessage = "Begin {2} [{1}] {0}", string endingFormatMessage = "End {2} [{1}] {0}", string tag = "")
            : this(new DebugWriter(), initialMessage, endingFormatMessage, tag)
        { }

        public Benchmarker(TextWriter log, string initialMessage = "Begin {2} [{1}] {0}", string endingFormatMessage = "End {2} [{1}] {0}", string tag = "")
        {
            _log = log;
            _initialMessage = initialMessage;
            _endingFormatMessage = endingFormatMessage;
            _tag = tag;
            _stopwatch = new Stopwatch();
            if (_log != null)
                _log.WriteLine(_initialMessage, _stopwatch.Elapsed, _passCount, _tag);
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            if (_log != null)
                _log.WriteLine(_endingFormatMessage, _stopwatch.Elapsed, _passCount, _tag);
            _passCount++;
        }
    }
#endif
}
