using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal 
{
    /// <summary>
    /// This class implements all the Game's related functionality
    /// </summary>
    public interface IGameRepository : IRepository
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
        SiteSection RelatedSection { get; set; }

        #endregion

        #region Retrieval

        /// <summary>
        /// Gets all the existing games
        /// </summary>
        /// <returns>IEnumerable<IGame></returns>
        IEnumerable<IGame> GetGames();

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
        IEnumerable<IGame> GetGames(Int32 pageNumber);

        /// <summary>
        /// Gets the specified page of games.
        /// </summary>
        /// <param name="pageNumber">Number of the page to get.</param>
        /// <param name="pageSize">Size of the page (number of elements to return)</param>
        /// <returns>A collection of games.</returns>
        IEnumerable<IGame> GetGames(Int32 pageNumber, Int32 pageSize);

        IEnumerable<IGame> GetGames(Dictionary<FilterDefinition<IGame>, FilterValueDefinition> filters, Int32 pageNumber, Int32 pageSize);

        IEnumerable<IGame> GetGames(Dictionary<FilterDefinition<IGame>, FilterValueDefinition> filters, Int32 pageNumber);

        IEnumerable<IGame> GetFeaturedGames();

        Int64 GetGamesCount();

        IGame GetGame(Int64 gameId);

        #endregion

        #region Writing

        /// <summary>
        /// Adds a game record to the db context
        /// </summary>
        /// <param name="game">game to be added</param>
        void Add(IGame game);

        /// <summary>
        /// Deletes a game record from the db context
        /// </summary>
        /// <param name="game">game to be deleted</param>
        void Delete(IGame game);

        #endregion

        #region Validation
        #endregion

    }
}
