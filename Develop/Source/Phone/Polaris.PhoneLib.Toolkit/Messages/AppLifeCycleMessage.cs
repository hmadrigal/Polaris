using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaris.PhoneLib.Toolkit.Messages
{
    public class AppLifeCycleMessage
    {
        public LifeCycleState Status { get; private set; }

        public AppLifeCycleMessage(LifeCycleState status)
        {
            Status = status;
        }
    }
}
