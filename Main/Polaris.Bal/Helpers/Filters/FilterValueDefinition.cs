using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polaris.Bal.Helpers.Filters
{
    public class FilterValueDefinition : DefinitionBase
    {
        #region Properties

        public Object FilterValue { get; set; }
        public Boolean IsDataDriven { get; set; }

        public static FilterValueDefinition AllTime { get; private set; }
        public static FilterValueDefinition ThisMonth { get; private set; }
        public static FilterValueDefinition ThisWeek { get; private set; }
        public static FilterValueDefinition Today { get; private set; }

        public static FilterValueDefinition AllCategories { get; private set; }

        #endregion

        #region Constructor 

        static FilterValueDefinition() {
            AllTime = new FilterValueDefinition() {
                Name = "All-Time",
            };
            ThisMonth = new FilterValueDefinition() {
                Name = "This Month",
            };
            ThisWeek = new FilterValueDefinition() {
                Name = "This Week",
            };
            Today = new FilterValueDefinition() {
                Name = "Today",
            };

            AllCategories = new FilterValueDefinition() {
                Name = "All Categories",
                FilterValue = null,
            };

        }

        #endregion

        #region Methods

        #endregion
    }
}
