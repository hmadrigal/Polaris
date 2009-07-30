using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal;

namespace Polaris.Dal
{
    public abstract class Repository : IRepository
    {
        #region Fields

        protected PolarisDataContext db;

        #endregion

        #region Constructors

        public Repository()
        {
            try
            {
                db = new PolarisDataContext();
            }
            catch (Exception ex)
            {
                throw new Exception("DATA: there was a problem initializing the database context.", ex);
            }
        }

        public Repository(PolarisDataContext dataContext)
        {
            this.db = dataContext;
        }

        #endregion

        #region Persistence

        public void Save()
        {
            db.SubmitChanges();
        }

        #endregion

        #region IRepository Members

        public DataEntityType CreateNew<DataEntityType>() where DataEntityType : IDataEntity
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
