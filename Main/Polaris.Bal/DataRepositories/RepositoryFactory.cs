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
            var dalAssembly = LoadDalAssembly();
            var dalRepositoryFactoryName = AppSettings.DalRepositoryFactoryName;
            try {
                var repositoryFactory = (IRepositoryFactory)dalAssembly.CreateInstance(dalRepositoryFactoryName);
                return repositoryFactory;
            } catch (Exception e) {
                throw new InvalidOperationException(String.Format("Unable to create a new instance of the repository factory: {0}", AppSettings.DalRepositoryFactoryName), e);
            }
        }

        private static System.Reflection.Assembly LoadDalAssembly() {
            var dalAssemblyName = AppSettings.DalAssemblyName;
            try {
                var dalAssembly = System.Reflection.Assembly.Load(dalAssemblyName);
                return dalAssembly;
            } catch (Exception e) {
                throw new InvalidOperationException(String.Format("Unable to load DAL assembly: {0}", dalAssemblyName), e);
            }
        }

        #endregion

    }
}
