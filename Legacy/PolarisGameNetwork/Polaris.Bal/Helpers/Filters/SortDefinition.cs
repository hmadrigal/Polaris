using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Polaris.Bal
{
    /// <summary>
    /// Class representing the available sort options for the given content type
    /// </summary>
    public class SortDefinition : DefinitionBase
    {
        #region Properties

        public static SortDefinition MostPlayed { get; private set; }
        public static SortDefinition MostLiked { get; private set; }
        public static SortDefinition MostRecent { get; private set; }
        public static SortDefinition MostRelevant { get; private set; }
        public static SortDefinition Alphabetical { get; private set; }
        public static SortDefinition GameRecommendations { get; private set; }
        
        public Dictionary<String, FilterDefinition> Filters { get; private set; }

        #endregion

        #region Constructors

        public SortDefinition() {
            Filters = new Dictionary<string, FilterDefinition>();
        }

        static SortDefinition() {

            MostPlayed = new SortDefinition() {
                Name = "Most Played",
            };
            MostPlayed.AddFilter(FilterDefinition.CategoryFilter);

            MostLiked = new SortDefinition()
            {
                Name = "Most Liked",
            };
            MostPlayed.AddFilter(FilterDefinition.CategoryFilter);

            MostRecent = new SortDefinition() {
                Name = "Most Recent",
            };
            MostRecent.AddFilter(FilterDefinition.CategoryFilter);

            MostRelevant = new SortDefinition() {
                Name = "Most Relevant",
            };

            Alphabetical = new SortDefinition()
            {
                Name = "A-Z",
            };
            Alphabetical.AddFilter(FilterDefinition.Alphabetical);

            GameRecommendations = new SortDefinition() {
                Name = "Games you'll love",
                DisplayName = "Games you'll ",
                RenderingType = LinkRenderingType.HeartIconSuffix,
                IsRecommendation = true,
                IsUserSpecific = true,
            };
        }

        #endregion

        #region Methods
        
        public void AddFilter(FilterDefinition target) {
            if (Filters.ContainsKey(target.UrlSafeName)) {
                Filters[target.UrlSafeName] = target;
            } else {
                Filters.Add(target.UrlSafeName, target);
            }
        }

        #endregion

    }
}
