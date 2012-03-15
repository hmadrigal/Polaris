using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal {
  public partial class DevelopmentTeam : IDevelopmentTeam {
    #region IDataEntity<long> Members

    public Int32 Id {
      get { return DevelopmentTeamId; }
    }

    #endregion

    #region IDataEntity Members

    public IDataEntity CreateNew() {
      return new DevelopmentTeam();
    }

    #endregion
  }
}
