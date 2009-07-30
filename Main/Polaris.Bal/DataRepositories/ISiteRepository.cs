using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal 
{
    public interface ISiteRepository : IRepository
    {
        #region User Methods

        /// <summary>
        /// Gets all the existing site users
        /// </summary>
        /// <returns>IEnumerable<IUser></returns>
        IEnumerable<IUser> GetUsers();

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
        ISiteSection GetSiteSection(String Controller, String Action);

        #endregion

    }
}
