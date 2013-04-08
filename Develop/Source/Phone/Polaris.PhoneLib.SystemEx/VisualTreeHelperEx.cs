using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace FooWPhoneLibrary.Toolkit.Extensions
{
    /// <summary>
    /// Provides addition visual tree helper methods.
    /// </summary>
    public static class VisualTreeHelperEx
    {
        /// <summary>
        /// Gets specified visual state group.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public static VisualStateGroup TryGetVisualStateGroup(this DependencyObject dependencyObject, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(dependencyObject);
            if (root == null) {
                return null;
            }
            return (from @group in VisualStateManager.GetVisualStateGroups(root).OfType<VisualStateGroup>()
                    where string.CompareOrdinal(groupName, @group.Name) == 0
                    select @group).FirstOrDefault<VisualStateGroup>();
        }

        /// <summary>
        /// Gets the implementation root.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns></returns>
        public static FrameworkElement GetImplementationRoot(this DependencyObject dependencyObject)
        {
            if (1 != VisualTreeHelper.GetChildrenCount(dependencyObject)) {
                return null;
            }
            return (VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement);
        }

    }
}
