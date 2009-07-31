using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public partial class PlayLog : IPlayLog
    {

        #region IDataEntity<long> Members

        public long Id
        {
            get { return this._PlayLogId; }
        }

        #endregion

        #region IPlayLog Members
        
        public IGame Game
        {
            get { return this.Game; }
        }

        public IUser User
        {
            get { return this.User; }
        }

        #endregion
    }
}
