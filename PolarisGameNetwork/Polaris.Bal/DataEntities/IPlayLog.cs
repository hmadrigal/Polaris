using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal
{
    public interface IPlayLog : IDataEntity<Int32>
    {
        #region Properties

        Int32 GameId { get; set; }

        IGame Game { get; set; }

        Int32 UserId { get; set; }

        IUser User { get; set; }

        DateTime Date { get; set; }

        Decimal Score { get; set; }

        #endregion
    }
}
