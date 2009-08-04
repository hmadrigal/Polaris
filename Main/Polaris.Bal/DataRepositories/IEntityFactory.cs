using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal
{
    public interface IEntityFactory
    {
        /// <summary>
        /// Returns a new instance of the specified entity type.
        /// </summary>
        /// <typeparam name="RepositoryType">Type of the repository to return.</typeparam>
        /// <returns>A new instance of the specified repository type.</returns>
        EntityType GetNewEntity<EntityType>() where EntityType : IDataEntity;
    }
}
