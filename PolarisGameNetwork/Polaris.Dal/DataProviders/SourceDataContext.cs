using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Dal
{
    public partial class SourceDataContext
    {
        partial void OnCreated()
        {
            Log = new DebugTextWriter();
        }
    }
}
