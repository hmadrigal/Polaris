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


        public IDevelopmentTeam DevelopmentTeam {
          get { return this.RelatedDevelopmentTeam; }
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
