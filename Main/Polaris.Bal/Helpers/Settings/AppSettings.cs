using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Polaris.Bal
{
    /// <summary>
    /// Serves as a helper to retrieve configuration values and reduce the number of hits to the config
    /// file to retrieve the same application value. This class implements a singleton pattern to ensure
    /// that only one instance of the class will be running at any given time.
    /// </summary>
    public class AppSettings
    {
        #region Database Settings

        /// <summary>
        /// Gets the connection string parameter for the application
        /// </summary>
        public static String ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PolarisConnectionString"].ConnectionString;
            }
        }

        #endregion

        #region Image Settings

        /// <summary>
        /// Gets the image root path
        /// </summary>
        public static String ImagesPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ImagesPath"];
            }
        }

        /// <summary>
        /// Gets the genre image url root path
        /// </summary>
        public static String UserImagePath
        {
            get
            {
                return ConfigurationManager.AppSettings["UserImagePath"];
            }
        }


        #endregion

        #region Pal Settings
        #endregion

        #region Cms Settings
        #endregion

        #region Cal Settings
        #endregion

        #region Dal Settings

        /// <summary>
        /// Gets the dal assembly
        /// </summary>
        public static String DalAssemblyName
        {
            get
            {
                return ConfigurationManager.AppSettings["DalAssemblyName"];
            }
        }

        /// <summary>
        /// Gets the class name of the repository factory class implementation.
        /// </summary>
        public static String DalRepositoryFactoryName {
            get {
                return String.Format("{0}.{1}", DalAssemblyName,
                    ConfigurationManager.AppSettings["DalRepositoryFactoryName"]);
            }
        }

        #endregion
    }
}
