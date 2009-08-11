using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal {

  public abstract class Repository : IRepository {

    #region Fields

    protected PolarisDataContext db;
    protected Dictionary<Type, IDataEntity> RegisteredEntities { get; set; }

    #endregion

    #region Constructors

    public Repository():this(new PolarisDataContext()) {
    }

    public Repository(PolarisDataContext dataContext) {
      RegisteredEntities = new Dictionary<Type, IDataEntity>();
      this.db = dataContext;
    }

    #endregion

    #region Persistence

    public void Save() {
      db.SubmitChanges();
    }

    #endregion

    #region IRepository Members

    public DataEntityType CreateNew<DataEntityType>() where DataEntityType : IDataEntity {
      var entityType = typeof(DataEntityType);
      if(RegisteredEntities.ContainsKey(entityType)) {
        return (DataEntityType)RegisteredEntities[entityType].CreateNew();
      } else {
        throw new InvalidOperationException(String.Format("This repository is unable to create an instance of type {0}", entityType.FullName));
      }
    }

    #endregion
  }
}
