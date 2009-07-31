using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal
{
    public interface IPlayLog : IDataEntity<Int64>
    {
        #region Properties

        Int64 GameId { get; set; }

        IGame Game { get; }

        Int64 UserId { get; set; }

        IUser User { get; }

        DateTime Date { get; set; }

        Decimal Score { get; set; }

        #endregion
    }
}
