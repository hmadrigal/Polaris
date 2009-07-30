using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Polaris.Bal.Helpers.Settings
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
                return ConfigurationManager.ConnectionStrings["PolarisConnection"].ConnectionString;
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
        public static String DalAssembly
        {
            get
            {
                return ConfigurationManager.AppSettings["PolarisDal"];
            }
        }

        #endregion
    }
}
