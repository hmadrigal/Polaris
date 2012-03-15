using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public partial class TimePeriod : ITimePeriod
    {
        #region IDataEntity<long> Members

        public long Id
        {
            get { return this._TimePeriodId; }
        }

        #endregion

        #region IDataEntity Members

        public IDataEntity CreateNew() {
          return new TimePeriod();
        }

        #endregion
    }
}
