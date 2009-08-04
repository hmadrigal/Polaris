using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public class LinqToSqlEntityFactory : IEntityFactory
    {
        #region IEntityFactory Members

        public EntityType GetNewEntity<EntityType>() where EntityType : Polaris.Bal.IDataEntity
        {
            var entityType = typeof(EntityType);
            if (entityType == typeof(IUser))
            {
                return (EntityType)(new User() as IUser);
            }
            else
            {
                throw new InvalidOperationException(String.Format("This assembly does not implement {0}", entityType.Name));
            }
        }

        #endregion
    }
}
