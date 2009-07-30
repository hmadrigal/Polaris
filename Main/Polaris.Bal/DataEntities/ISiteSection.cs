using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal {
    public interface ISiteSection {
        String Name { get; set; }
        String Controller { get; set; }
        String Action { get; set; }
        String BaseRoute { get; set; }
    }
}
