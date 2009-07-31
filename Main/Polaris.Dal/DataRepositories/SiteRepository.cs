using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public class SiteRepository : Repository, ISiteRepository
    {
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
            return db.Users.Cast<IUser>();
        }

        #endregion

    }
}
