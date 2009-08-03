using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;
using Polaris.Bal.Extensions;

namespace Polaris.Dal
{
    /// <summary>
    /// This class implements all the Game's related functionality
    /// </summary>
    public class GameRepository : Repository, IGameRepository
    {
        #region Properties

        /// <summary>
        /// The site section this repository serves data for.
        /// </summary>
        /// <remarks>
        /// The usage of this property is optional; however,
        /// some of the methods implemented by this repository infer
        /// data from the value of this property. Check whether the
        /// methods you wish to use are dependant on this property
        /// prior to using.
        /// </remarks>
        public SiteSection RelatedSection {get; set;}

        #endregion

        #region Retrieval

        /// <summary>
        /// Gets all the existing games
        /// </summary>
        /// <returns>IEnumerable<IGame></returns>
        public IEnumerable<IGame> GetGames()
        {
            return GetGamesQuery().ToArray();
        }

        /// <summary>
        /// Gets the specified page of games.
        /// </summary>
        /// <param name="pageNumber">Number of the page to get.</param>
        /// <returns>A collection of games.</returns>
        /// <remarks>
        /// This method infers the page size from the site section related with
        /// this repository instance. If the site section is not set the method
        /// call throws an exception.
        /// </remarks>
        /// <see cref="RelatedSection"/>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the method is called but the repository is not currently
        /// related with a site section.
        /// </exception>
        public IEnumerable<IGame> GetGames(int pageNumber)
        {
            Int32 pageSize = GetPageSize<IGame>();
            return GetGames(pageNumber, pageSize);
        }

        /// <summary>
        /// Gets the specified page of games.
        /// </summary>
        /// <param name="pageNumber">Number of the page to get.</param>
        /// <param name="pageSize">Size of the page (number of elements to return)</param>
        /// <returns>A collection of games.</returns>
        public IEnumerable<IGame> GetGames(Int32 pageNumber, Int32 pageSize)
        {
            Int32 skip = (pageNumber - 1) * pageSize;
            return GetGamesQuery().Skip(skip).Take(pageSize).ToArray();
        }

        public IEnumerable<IGame> GetGames(Dictionary<FilterDefinition<IGame>, FilterValueDefinition> filters, int pageNumber, int pageSize)
        {
            Int32 skip = (pageNumber - 1) * pageSize;
            return GetGamesQuery(filters).Skip(skip).Take(pageSize).ToArray();
        }

        public IEnumerable<IGame> GetGames(Dictionary<FilterDefinition<IGame>, FilterValueDefinition> filters, int pageNumber)
        {
            Int32 pageSize = GetPageSize<IUser>();
            return GetGames(filters, pageNumber, pageSize);
        }

        public IEnumerable<IGame> GetFeaturedGames()
        {
            return GetGamesQuery().Where(g => g.IsFeatured == true).ToArray();
        }

        public long GetGamesCount()
        {
            return GetGamesQuery().Count();
        }

        public IGame GetGame(long gameId)
        {
            return db.Games.Where(g => g.GameId == gameId).FirstOrDefault();
        }

        private IQueryable<IGame> GetGamesQuery(Dictionary<FilterDefinition<IGame>, FilterValueDefinition> filters)
        {
            var query = GetGamesQuery();
            foreach (var pair in filters)
            {
                query = query.AddEqualityCondition(pair.Key, pair.Value);
            }
            return query;
        }


        private IQueryable<IGame> GetGamesQuery()
        {
            return from game in db.Games
                   select game as IGame;
        }

        private Int32 GetPageSize<EntityType>() where EntityType : IDataEntity
        {
            ValidateSiteSection();
            return RelatedSection.GetPageSize<EntityType>();
        }

        #endregion

        #region Writing

        /// <summary>
        /// Adds a game record to the db context
        /// </summary>
        /// <param name="game">game to be added</param>
        public void Add(IGame game)
        {
            db.Games.InsertOnSubmit(game as Game);
        }

        /// <summary>
        /// Deletes a game record from the db context
        /// </summary>
        /// <param name="game">game to be deleted</param>
        public void Delete(IGame game)
        {
            db.Games.DeleteOnSubmit(game as Game);
        }

        #endregion

        #region Validation

        private void ValidateSiteSection()
        {
            if (RelatedSection == SiteSection.None)
            {
                throw new InvalidOperationException("It is not possible to determine the page size because this repository is not related with a site section");
            }
        }

        #endregion
    }
}
