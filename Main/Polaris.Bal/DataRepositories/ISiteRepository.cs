using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal 
{
    public interface ISiteRepository : IRepository
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

        #region User Methods

        /// <summary>
        /// Gets all the existing site users
        /// </summary>
        /// <returns>IEnumerable<IUser></returns>
        IEnumerable<IUser> GetUsers();

        /// <summary>
        /// Gets the specified page of users.
        /// </summary>
        /// <param name="pageNumber">Number of the page to get.</param>
        /// <param name="pageSize">Size of the page (number of elements to return)</param>
        /// <returns>A collection of users.</returns>
        IEnumerable<IUser> GetUsers(Int32 pageNumber, Int32 pageSize);

        /// <summary>
        /// Gets the specified page of users.
        /// </summary>
        /// <param name="pageNumber">Number of the page to get.</param>
        /// <returns>A collection of users.</returns>
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
        IEnumerable<IUser> GetUsers(Int32 pageNumber);

        IEnumerable<IUser> GetUsers(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters, Int32 pageNumber, Int32 pageSize);

        IEnumerable<IUser> GetUsers(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters, Int32 pageNumber);

        Int64 GetUserCount();

        #endregion

    }
}
