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

        #region Site Methods

        /// <summary>
        /// Gets the site section associated with the specified controller
        /// and action names.
        /// </summary>
        /// <param name="Controller">Name of the MVC Controller.</param>
        /// <param name="Action">Name of the MVC Action.</param>
        /// <returns>
        /// Instance of the associated site section, null if none
        /// associated.
        /// </returns>
        public ISiteSection GetSiteSection(String Controller, String Action)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
