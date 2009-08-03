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

        #region Constructor

        //public SiteRepository()
        //{

        //}

        #endregion

        #region User Methods

        public void Add(IUser user)
        {
            db.Users.InsertOnSubmit(user as User);
        }

        public void Delete(IUser user)
        {
            db.Users.DeleteOnSubmit(user as User);
        }

        public IEnumerable<IUser> GetUsers()
        {
            return GetUsersQuery().ToArray();
        }

        public IEnumerable<IUser> GetUsers(Int32 pageNumber, Int32 pageSize)
        {
            Int32 skip = (pageNumber - 1) * pageSize;
            return GetUsersQuery().Skip(skip).Take(pageSize).ToArray();
        }

        public IEnumerable<IUser> GetUsers(Int32 pageNumber)
        {
            Int32 pageSize = GetPageSize<IUser>();
            return GetUsers(pageNumber, pageSize);
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

        public IUser GetUser(String username)
        {
            return db.Users.Where(u => u.Username == username).FirstOrDefault();
        }
        public Boolean ValidateUser(String username, String password)
        {
            return (db.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault() != null);
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
