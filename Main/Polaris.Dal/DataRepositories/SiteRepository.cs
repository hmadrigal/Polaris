using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;
using Polaris.Bal.Extensions;

namespace Polaris.Dal
{
    public class SiteRepository : Repository, ISiteRepository
    {
        #region Properties

        public SiteSection RelatedSection { get; set; }

        #endregion

        #region Retrieval

        public IEnumerable<IUser> GetUsers()
        {
            return GetUsersQuery().ToArray();
        }

        public IEnumerable<IUser> GetUsers(Int32 pageNumber, Int32 pageSize, out Int32 totalUsers)
        {
            var users = GetUsersQuery();
            totalUsers = GetUsersQuery().Count();
            var skip = (pageNumber - 1) * pageSize;
            return users.Skip(skip).Take(pageSize).ToArray();
        }

        public IEnumerable<IUser> GetUsers(Int32 pageNumber)
        {
            Int32 pageSize = GetPageSize<IUser>();
            return GetUsers(pageNumber, pageSize);
        }

        public IEnumerable<IUser> GetUsers(Int32 pageNumber, Int32 pageSize)
        {
            Int32 skip = (pageNumber - 1) * pageSize;
            return GetUsersQuery().Skip(skip).Take(pageSize).ToArray();
        }

        public IEnumerable<IUser> GetUsers(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters, Int32 pageNumber, Int32 pageSize)
        {
            Int32 skip = (pageNumber - 1) * pageSize;
            return GetUsersQuery(filters).Skip(skip).Take(pageSize).ToArray();
        }

        public IEnumerable<IUser> GetUsers(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters, Int32 pageNumber)
        {
            Int32 pageSize = GetPageSize<IUser>();
            return GetUsers(filters, pageNumber, pageSize);
        }

        public Int64 GetUserCount()
        {
            return GetUsersQuery().Count();
        }

        public IUser GetUserByUsername(String username)
        {
            return db.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public IUser GetUserByEmail(String email)
        {
            return db.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public IUser GetUserById(Int64 userId)
        {
            return db.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        

        /// <summary>
        /// Returns a list of the users that match the provided username
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<IUser> FindUsersByUsername(string usernameToMatch, int pageNumber, int pageSize, out int totalUsers)
        {
            var users = GetUsersQuery();
            var skip = (pageNumber - 1) * pageSize;
            var foundUsers = users.Where(u => u.Username.Contains(usernameToMatch)).Skip(skip).Take(pageSize);
            totalUsers = foundUsers.Count();
            return foundUsers.ToArray();
        }
        /// <summary>
        /// Returns a list of the users that match the provided email
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<IUser> FindUsersByEmail(string emailToMatch, int pageNumber, int pageSize, out int totalUsers)
        {
            var users = GetUsersQuery();
            var skip = (pageNumber - 1) * pageSize;
            var foundUsers = users.Where(u => u.Email.Contains(emailToMatch)).Skip(skip).Take(pageSize);
            totalUsers = foundUsers.Count();
            return foundUsers.ToArray();
        }


        private IQueryable<IUser> GetUsersQuery(Dictionary<FilterDefinition<IUser>, FilterValueDefinition> filters)
        {
            var query = GetUsersQuery();
            foreach (var pair in filters)
            {
                query = query.AddEqualityCondition(pair.Key, pair.Value);
            }
            return query;
        }


        private IQueryable<IUser> GetUsersQuery()
        {
            return from user in db.Users
                   select user as IUser;
        }

        private Int32 GetPageSize<EntityType>() where EntityType : IDataEntity
        {
            ValidateSiteSection();
            return RelatedSection.GetPageSize<EntityType>();
        }

        #endregion

        #region Writing
        
        public void Add(IUser user)
        {
            db.Users.InsertOnSubmit(user as User);
        }

        public void Delete(IUser user)
        {
            db.Users.DeleteOnSubmit(user as User);
        }

        #endregion

        #region Validation

        public Boolean ValidateUser(String username, String password)
        {
            return (db.Users.Where(u => u.Username == username && u.Password == password).Count() > 0);
        }

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
