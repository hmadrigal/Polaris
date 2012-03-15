using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Polaris.Bal.Extensions;

namespace Polaris.Bal
{
    public class UserContent
    {
        #region Properties

        public static ContentDefinition Games { get; private set; }
        public static ContentDefinition UserGames { get; private set; }
        public static Dictionary<String, ContentDefinition> ContentTypes { get; private set; }

        #endregion

        #region Constructor

        static UserContent() {

            ContentTypes = new Dictionary<string, ContentDefinition>();

            Games = new ContentDefinition()
            {
                Name = "Games",
                RenderingType = LinkRenderingType.HeartIconPrefix,
                IsFavorites = true,
            };
            Games.AddSortOption(SortDefinition.MostLiked);
            Games.AddSortOption(SortDefinition.MostPlayed);
            Games.AddSortOption(SortDefinition.MostRecent);
            Games.AddSortOption(SortDefinition.Alphabetical);
            Games.DefaultSort = SortDefinition.MostLiked;

            UserGames = new ContentDefinition()
            {
                Name = "UserGames",
                DisplayName = "[UserName]'s Games",
            };
            UserGames.AddSortOption(SortDefinition.GameRecommendations);
            UserGames.DefaultSort = SortDefinition.GameRecommendations;

            ContentTypes.Add(Games.UrlSafeName, Games);
            ContentTypes.Add(UserGames.UrlSafeName, UserGames);
        }

        #endregion

        #region Public Methods

        public static ContentDefinition GetActiveContentType(string contentTypeName) {
            return ContentTypes.GetActiveContentType(contentTypeName, Games);
        }

        #endregion
    }
}
