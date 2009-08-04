using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal.Helpers.Settings
{
    internal static class Plugin
    {
        /// <summary>
        /// Creates an instance of an concreteType by specifying the resultant type and the source assembly
        /// </summary>
        /// <typeparam name="T">resultant type</typeparam>
        /// <param name="assemblyName">assembly name</param>
        /// <param name="concreteClass">class to be loaded dynamically</param>
        /// <returns></returns>
        internal static T CreateNewInstanceOf<T>(String assemblyName, String concreteClass)
        {
            var assembly = LoadAssembly(assemblyName);
            try
            {
                var repositoryFactory = (T)assembly.CreateInstance(concreteClass);
                return repositoryFactory;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(String.Format("Unable to create a new instance of : {0}", concreteClass), e);
            }
        }

        /// <summary>
        /// Loads an specific assembly dynamically
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        internal static System.Reflection.Assembly LoadAssembly(String assemblyName)
        {
            try
            {
                var assembly = System.Reflection.Assembly.Load(assemblyName);
                return assembly;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(String.Format("Unable to load assembly: {0}", assemblyName), e);
            }
        }


    }
}
