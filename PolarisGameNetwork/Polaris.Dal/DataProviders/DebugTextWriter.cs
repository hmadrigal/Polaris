using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Polaris.Dal
{
    public class DebugTextWriter : TextWriter
    {
        public override void Write(char[] buffer, int index, int count)
        {
            System.Diagnostics.Debug.Write(new String(buffer, index, count));
        }
        public override void Write(string value)
        {
            System.Diagnostics.Debug.Write(value);
        }
        public override Encoding Encoding
        {
            get
            {
                return System.Text.Encoding.Default;
            }
        }
    }
}
