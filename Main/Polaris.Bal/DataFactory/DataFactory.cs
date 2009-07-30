using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Polaris.Bal.Helpers.Settings;
using System.Reflection;

namespace Polaris.Bal
{
    /// <summary>
    /// This class serves to create instances of the Data Abstraction Layer data provider classes
    /// </summary>
    public class DataFactory
    {
        #region Fields

        // Look up the DAL implementation we should be using
        private static readonly string path = AppSettings.DalAssembly;

        #endregion

        #region Constructor

        // Default constructor
        private DataFactory() { }

        #endregion

        #region Provider Creator Methods

        /// <summary>
        /// Creates an instance of the Data Abstraction Layer class
        /// </summary>
        /// <param name="dalName">the name of the dal class</param>
        /// <returns>an instance of the dal class</returns>
        public static IRepository CreateDAL(string dalName)
        {
            string className = path + "." + dalName;
            return (IRepository)Assembly.Load(path).CreateInstance(className);
        }

        /// <summary>
        /// Creates an instance of the Data Abstraction Layer Sponsor class
        /// </summary>
        /// <returns>an instance of the dal Sponsor class</returns>
        public static ISiteRepository CreateSiteRepository()
        {
            string className = path + ".SiteRepository";
            return (ISiteRepository)Assembly.Load(path).CreateInstance(className);
        }


        #endregion
    }
}
