using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Polaris.Bal
{
    public interface ITimePeriod : IDataEntity<Int64>
    {

        #region Properties

        [StringLengthValidator(5, 20, Ruleset = "TimePeriodValidator",
            MessageTemplate = "Name must be between 5 and 20 characters.")]
        String Name { get; set; }

        Int32 Hours { get; set; }

        #endregion
    }
}
