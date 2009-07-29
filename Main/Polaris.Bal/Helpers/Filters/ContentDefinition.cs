using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polaris.Bal.Helpers.Filters
{
    public class ContentDefinition : DefinitionBase
    {
        #region Properties

        public static Dictionary<String, ContentDefinition> ContentTypes { get; private set; }
        public Dictionary<String, SortDefinition> SortOptions { get; private set; }
        public SortDefinition ActiveSort { get; set; }
        public Dictionary<FilterDefinition, FilterValueDefinition> ActiveFilters { get; private set; }

        #endregion

        #region Constructors

        public ContentDefinition() {
            SortOptions = new Dictionary<string, SortDefinition>();
            ActiveFilters = new Dictionary<FilterDefinition, FilterValueDefinition>();
        }

        #endregion

        #region Methods

        public void AddSortOption(SortDefinition target) {
            SortOptions.Add(target.UrlSafeName, target);
        }

        public ContentDefinition Clone() {
            ContentDefinition clone = new ContentDefinition() {
                Name = this.Name,
                DefaultSort = this.DefaultSort,
                RenderingType = this.RenderingType,
                IsRecommendation = this.IsRecommendation,
                IsUserSpecific = this.IsUserSpecific,
                IsFavorites = this.IsFavorites,
            };
            if (HasDisplayName) {
                clone.DisplayName = DisplayName;
            }
            foreach (var sortOption in SortOptions.Values) {
                clone.AddSortOption(sortOption);
            }
            return clone;
        }

        #endregion
    }
}
