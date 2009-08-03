using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Polaris.Bal 
{
    public interface IGame : IDataEntity<Int64>
    {
        #region Properties

        [StringLengthValidator(5, 20, Ruleset = "GameValidator",
            MessageTemplate = "Game Name must be between 5 and 20 characters.")]
        String Name { get; set; }

        Guid Key { get; set; }

        Boolean Active { get; set; }

        Int64 DevelopmentTeamId { get; set; }

        IDevelopmentTeam DevelopmentTeam { get; }

        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }

        DateTime? FeaturedStartDate { get; set; }

        DateTime? FeaturedEndDate { get; set; }

        Boolean IsFeatured { get; }

        Boolean IsComingSoon { get; }

        #endregion
    }
}
