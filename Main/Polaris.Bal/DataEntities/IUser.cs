using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Polaris.Bal {
    public interface IUser : IDataEntity<Int64>
    {
        #region Properties

        [StringLengthValidator(5, 20, Ruleset = "UserValidator",
               MessageTemplate = "Username must be between 5 and 20 characters.")]
        String Username { get; set; }

        [StringLengthValidator(1, 30, Ruleset = "UserValidator",
               MessageTemplate = "Name must be between 1 and 30 characters.")]
        String FirstName { get; set; }

        [StringLengthValidator(1, 30, Ruleset = "UserValidator",
               MessageTemplate = "Name must be between 1 and 30 characters.")]
        String LastName { get; set; }

        [RegexValidator(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})",
            Ruleset="UserValidator",
            MessageTemplate="Email address is invalid.")]
        String Email { get; set; }

        Int64 PlayCredits { get; set; }

        Int64 RankingCredits { get; set; }

        #endregion
    }
}
