using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal 
{
    public static class RepositoryFactory
    {
        #region Properties

        private static IRepositoryFactory Factory { get; set; }

        #endregion

        #region Constructor

        static RepositoryFactory() {
            Factory = CreateNewRepositoryFactory();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a new instance of the specified repository type.
        /// </summary>
        /// <typeparam name="RepositoryType">Type of the repository to return.</typeparam>
        /// <returns>A new instance of the specified repository type.</returns>
        public static RepositoryType GetNewRepository<RepositoryType>() where RepositoryType : IRepository {
            return Factory.GetNewRepository<RepositoryType>();
        }

        #endregion

        #region Private Methods

        private static IRepositoryFactory CreateNewRepositoryFactory() {
            return Polaris.Bal.Helpers.Settings.Plugin.CreateNewInstanceOf <IRepositoryFactory> (AppSettings.DalAssemblyName,AppSettings.DalRepositoryFactoryName);
        }

        #endregion

    }
}
