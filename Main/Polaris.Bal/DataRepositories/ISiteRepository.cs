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

    }
}
