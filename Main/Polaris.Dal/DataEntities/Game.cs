using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public partial class Game : IGame
    {
        #region IDataEntity<long> Members

        public long Id
        {
            get { return this._GameId; }
        }

        #endregion

        #region IGame Members

        /// <summary>
        /// Exposes the related entity.
        /// </summary>
        /// <remarks>
        /// By convention the parent properties for the associations 
        /// in the DBML file should use the prefix 'Related' (i.e.: 'RelatedUser') 
        /// so that it is possible to expose properties without the prefixes to the BAL.
        /// </remarks>
        public IDevelopmentTeam DevelopmentTeam {
          get { return this.RelatedDevelopmentTeam; }
          set { this.RelatedDevelopmentTeam = value as DevelopmentTeam; }
        }

        public Boolean IsFeatured
        {
            get
            {
                if ((this.FeaturedEndDate.HasValue) && (this.FeaturedEndDate > DateTime.Now))
                    return true;
                else
                    return false;
            }
        }

        public Boolean IsComingSoon
        {
            get
            {
                if ((this.FeaturedStartDate.HasValue) && (this.FeaturedStartDate > DateTime.Now))
                    return true;
                else
                    return false;
            }
        }

        #endregion

        #region IDataEntity Members

        public IDataEntity CreateNew() {
          return new Game();
        }

        #endregion
    }
}
