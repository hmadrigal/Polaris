using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public class PolarisDataContext : SourceDataContext
    {

        public PolarisDataContext() : base(AppSettings.ConnectionString) { }
    }
}
