using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public partial class User : IUser
    {
        #region IDataEntity<long> Members

        public long Id
        {
            get { return this._UserId; }
        }

        #endregion

        #region IDataEntity Members

        public IDataEntity CreateNew() {
          return new User();
        }

        #endregion
    }
}
