using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polaris.Bal
{
    public class FilterValueDefinition : DefinitionBase {
        #region Properties

        public Object FilterValue { get; set; }
        public Type FilterValueType { get; set; }
        public Boolean IsDataDriven { get; set; }

        public static FilterValueDefinition<DateTime> AllTime { get; private set; }
        public static FilterValueDefinition<DateTime> ThisMonth { get; private set; }
        public static FilterValueDefinition<DateTime> ThisWeek { get; private set; }
        public static FilterValueDefinition<DateTime> Today { get; private set; }

        public static FilterValueDefinition<Int32?> AllCategories { get; private set; }

        #endregion

        #region Constructor

        static FilterValueDefinition()
        {
            AllTime = new FilterValueDefinition<DateTime>()
            {
                Name = "All-Time",
            };
            ThisMonth = new FilterValueDefinition<DateTime>()
            {
                Name = "This Month",
            };
            ThisWeek = new FilterValueDefinition<DateTime>()
            {
                Name = "This Week",
            };
            Today = new FilterValueDefinition<DateTime>()
            {
                Name = "Today",
            };

            AllCategories = new FilterValueDefinition<Int32?>()
            {
                Name = "All Categories",
                FilterValue = null,
            };

        }

        #endregion

        #region Methods

        #endregion
    }

    public class FilterValueDefinition<T> : FilterValueDefinition
    {
        public new T FilterValue { get; set; }

        public FilterValueDefinition() {
            FilterValueType = typeof(T);
        }
    }
}
