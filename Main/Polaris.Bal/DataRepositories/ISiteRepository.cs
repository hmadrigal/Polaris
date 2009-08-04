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

        #region Retrieval

        /// <summary>
        /// Gets all the existing site users
        /// </summary>
        /// <returns>IEnumerable<IUser></returns>
        IEnumerable<IUser> GetUsers();

        /// <summary>
        /// Gets the specified page of users.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalUsers">After the method call, it will contains the total of found users </param>
        /// <returns></returns>
        IEnumerable<IUser> GetUsers(Int32 pageNumber, Int32 pageSize, out Int32 totalUsers);

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

        /// <summary>
        /// Gets the specified page of users.
        /// </summary>
        /// <param name="pageNumber">Number of the page to get.</param>
        /// <param name="pageSize">Size of the page (number of elements to return)</param>
        /// <returns>A collection of users.</returns>
        IEnumerable<IUser> GetUsers(Int32 pageNumber, Int32 pageSize);

        IEnumerable<IUser> GetUsers(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters, Int32 pageNumber, Int32 pageSize);

        IEnumerable<IUser> GetUsers(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters, Int32 pageNumber);

        Int64 GetUserCount();
        
        #endregion

        #region Writing

        /// <summary>
        /// Adds a user record to the db context
        /// </summary>
        /// <param name="user"></param>
        void Add(IUser user);

        /// <summary>
        /// Deletes a user record from the db context
        /// </summary>
        /// <param name="user"></param>
        void Delete(IUser user);
       
        
        /// <summary>
        /// Gets a particular user by its username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        IUser GetUserByUsername(String username);

        /// <summary>
        /// Gets a particular user by its email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IUser GetUserByEmail(String email);

        /// <summary>
        /// Gets a particular ser by its Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IUser GetUserById(Int64 userId);

        /// <summary>
        /// Checks if the provided password and username were previously stored. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Boolean ValidateUser(String username, String password);

        /// <summary>
        /// Returns a list of the users that match the provided username
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IEnumerable<IUser> FindUsersByUsername(string usernameToMatch, int pageNumber, int pageSize, out int totalUsers);
        /// <summary>
        /// Returns a list of the users that match the provided email
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IEnumerable<IUser> FindUsersByEmail(string emailToMatch, int pageNumber, int pageSize, out int totalUsers);

        #endregion
    }
}
