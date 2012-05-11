//-----------------------------------------------------------------------
// <copyright file="Unity2Extensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Extensions
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;

    /// <summary>
    /// A collection of static methods that works on IUnityContainer
    /// </summary>
    public static class Unity2Extensions
    {
        /// <summary>
        /// Loads all the containers from the configuration file by creating a child-parent relationship
        /// for each given container.
        /// </summary>
        /// <param name="container">The first parent of the chain of containers</param>
        /// <param name="configurationSectionName">name of the configuration section to be loaded</param>
        /// <param name="reverseOrder">Whether load the containers from last to first or in reverse order</param>
        /// <returns>The last child of the chain of containers</returns>
        public static IUnityContainer LoadContainerHierarchyFromSection(this IUnityContainer container, string configurationSectionName = "unity", bool reverseOrder = false)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            #region Mappings based on the configuration file

            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(configurationSectionName);
            IUnityContainer currentContainer = container;
            foreach (var containerSection in section.Containers)
            {
                var nestedContainer = currentContainer.CreateChildContainer();
                Microsoft.Practices.Unity.Configuration.UnityContainerExtensions.LoadConfiguration(nestedContainer, section, containerSection.Name);
                currentContainer = nestedContainer;
            }
            if (container.IsRegistered<IUnityContainer>())
            {
                container.UnregisterType<IUnityContainer>();
            }
            container.RegisterInstance<IUnityContainer>(currentContainer);
            container = currentContainer;

            #endregion Mappings based on the configuration file

            return container;
        }

        /// <summary>
        /// Loads the specified section into a given container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configurationSectionName"></param>
        /// <returns></returns>
        public static IUnityContainer LoadContainerFromSection(this IUnityContainer container, string configurationSectionName = "unity")
        {
            if (container == null)
                throw new ArgumentNullException("container");
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection(configurationSectionName);
            Microsoft.Practices.Unity.Configuration.UnityContainerExtensions.LoadConfiguration(container, section);
            return container;
        }

        /// <summary>
        /// Remove a registered type for a given container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        public static bool UnregisterType<T>(this IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            var foundRegistration = container.Registrations.Where(r => r.RegisteredType == typeof(T)).FirstOrDefault();
            if (foundRegistration == null)
            {
                return false;
            }
            else
            {
                foundRegistration.LifetimeManager.RemoveValue();
                return true;
            }
        }

        /// <summary>
        /// Removes a set of types from a given container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="types"></param>
        public static void UnregisterTypes(this IUnityContainer container, params Type[] types)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            var foundRegistrations = container.Registrations.Where(r => types.Contains(r.RegisteredType));
            foreach (var item in foundRegistrations)
            {
                item.LifetimeManager.RemoveValue();
            }
        }
    }
}