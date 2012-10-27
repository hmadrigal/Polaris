using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Polaris.Services
{
    /// <summary>
    /// When implemented provides an unified mechanism to register and resolve views.
    /// </summary>
    public interface IViewRegistryService
    {
        /// <summary>
        /// Registers a view
        /// </summary>
        /// <typeparam name="T">Type to view to register</typeparam>
        /// <param name="name">Identifier of the view to register</param>
        /// <param name="isSingleton">Boolean that indicates whether the view is singleton or not</param>
        void RegisterView<T>(string name, bool isSingleton = true) where T : FrameworkElement;

        /// <summary>
        /// Resolves a previously registered view
        /// </summary>
        /// <param name="name">Name of the view to resolve</param>
        /// <returns>Instance of the view associated with the given name</returns>
        FrameworkElement ResolveView(string name);
    }
}