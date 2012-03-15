using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Polaris.Bal
{
    public class FilterDefinition : DefinitionBase {

        #region Fields

        public static readonly String[] Letters = new String[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        #endregion

        #region Properties

        public Boolean IsDataDriven { get; set; }
        public Dictionary<String, FilterValueDefinition> FilterValues { get; private set; }
        public FilterValueDefinition ActiveValue { get; set; }
        public FilterRenderingType RenderingType { get; private set; }

        public static FilterDefinition TimeFilter { get; private set; }
        public static FilterDefinition CategoryFilter { get; private set; }
        public static FilterDefinition Alphabetical { get; private set; }

        #endregion

        #region Constructors

        public FilterDefinition()
        {
            FilterValues = new Dictionary<string, FilterValueDefinition>();
        }

        static FilterDefinition()
        {
            TimeFilter = new FilterDefinition()
            {
                Name = "Time",
            };

            TimeFilter.AddFilterValue(FilterValueDefinition.AllTime);
            TimeFilter.AddFilterValue(FilterValueDefinition.ThisMonth);
            TimeFilter.AddFilterValue(FilterValueDefinition.ThisWeek);
            TimeFilter.AddFilterValue(FilterValueDefinition.Today);

            CategoryFilter = new FilterDefinition()
            {
                Name = "Genre",
                IsDataDriven = true,
            };
            CategoryFilter.AddFilterValue(FilterValueDefinition.AllCategories);
            //foreach (var category in Polaris.Bal.Category.GetCategories()) {
            //    CategoryFilter.AddFilterValue(new FilterValueDefinition() {
            //        Name = category.Name,
            //        FilterValue = category.Name,
            //        IsDataDriven = true,
            //    });
            //}

            InitializeAlphabeticalFilter();
        }

        #endregion

        #region Methods

        private static void InitializeAlphabeticalFilter()
        {
            Alphabetical = new FilterDefinition()
            {
                Name = "Alpha",
                RenderingType = FilterRenderingType.LinkList,
            };
            var numbersValue = new FilterValueDefinition<String[]>()
            {
                Name = "Numbers",
                DisplayName = "#",
                FilterValue = new String[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" },
            };
            Alphabetical.AddFilterValue(numbersValue);
            foreach (var letter in Letters)
            {
                var letterFilterValue = new FilterValueDefinition<String[]>()
                {
                    Name = letter,
                    FilterValue = new String[] { letter },
                };
                Alphabetical.AddFilterValue(letterFilterValue);
            }
        }

        public void AddFilterValue(FilterValueDefinition target)
        {
            FilterValues.Add(target.UrlSafeName, target);
        }

        #endregion
    }


    public class FilterDefinition<EntityType> : FilterDefinition 
        where EntityType : IDataEntity
    {
        public PropertyInfo RelatedProperty { get; private set; }

        public FilterDefinition(String propertyName):base()
        {
            RelatedProperty = typeof(EntityType).GetProperty(propertyName);
        }
    }
}
