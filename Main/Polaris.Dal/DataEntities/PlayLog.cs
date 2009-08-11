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

        public Int32 Id
        {
            get { return this._PlayLogId; }
        }

        #endregion

        #region IPlayLog Members

        public IGame Game {
          get { return this.RelatedGame; }
          set { this.RelatedGame = value as Game; }
        }

        public IUser User {
          get { return this.RelatedUser; }
          set { this.RelatedUser = value as User; }
        }

        #endregion

        #region IDataEntity Members

        public IDataEntity CreateNew() {
          return new PlayLog();
        }

        #endregion
    }
}
