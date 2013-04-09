using System;
using System.IO;
using System.Diagnostics;

namespace Polaris.PhoneLib.Diagnostics
{
    public class DebugWriter : TextWriter
    {
        private const int DefaultBufferSize = 256;
        private System.Text.StringBuilder _buffer;

        public DebugWriter()
        {
            BufferSize = 256;
            _buffer = new System.Text.StringBuilder(BufferSize);
        }

        public int BufferSize
        {
            get;
            private set;
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        #region StreamWriter Overrides
        public override void Write(char value)
        {
            _buffer.Append(value);
            if (_buffer.Length >= BufferSize)
                Flush();
        }

        public override void WriteLine(string value)
        {
            Flush();

            using (var reader = new StringReader(value))
            {
                string line;
                while (null != (line = reader.ReadLine()))
                    System.Diagnostics.Debug.WriteLine(line);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Flush();
        }

        public override void Flush()
        {
            if (_buffer.Length > 0)
            {
                System.Diagnostics.Debug.WriteLine(_buffer);
                _buffer.Clear();
            }
        }
        #endregion
    }
}
