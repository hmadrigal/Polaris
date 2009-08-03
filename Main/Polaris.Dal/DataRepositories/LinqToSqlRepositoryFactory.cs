using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public class LinqToSqlRepositoryFactory : IRepositoryFactory
    {
        #region IRepositoryFactory Members

        public RepositoryType GetNewRepository<RepositoryType>() where RepositoryType : IRepository
        {
            var repositoryType = typeof(RepositoryType); 
            if(repositoryType == typeof(ISiteRepository))
            {
                return (RepositoryType)(new SiteRepository() as IRepository);
            }
            else if (repositoryType == typeof(IGameRepository))
            {
                return (RepositoryType)(new GameRepository() as IGameRepository);
            }
            else
            {
                throw new InvalidOperationException(String.Format("This assembly does not implement {0}", repositoryType.Name));
            }
        }

        #endregion
    }
}
