using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public partial class Game : IGame
    {
        #region IDataEntity<long> Members

        public long Id
        {
            get { return this._GameId; }
        }

        #endregion

        #region IGame Members


        public IDevelopmentTeam DevelopmentTeam
        {
            get
            {
                return this.DevelopmentTeam;
            }
        }

        #endregion
    }
}
