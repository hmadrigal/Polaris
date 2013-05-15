using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaris.PhoneLib.Messages
{
    public enum LifeCycleState
    {
        ActivatingFromDormant,
        ActivatingFromTombstone,
        Deactivating,
    }
}
