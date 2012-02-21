//-----------------------------------------------------------------------
// <copyright file="ViewModelLocatorBase.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    public class ViewModelLocatorBase : IViewModelLocator
    {
        [Dependency]
        public Microsoft.Practices.Unity.IUnityContainer Container { get; set; }

        /// <summary>
        /// Indicates whether the property values should be copied from the mock view model upon
        /// instantiation. This is useful during development when the view model property values
        /// aren't set yet.
        /// </summary>
        public bool CopyMockValues { get; set; }

        /// <summary>
        /// Indicates the mock view model locator from which to optionally copy
        /// mock property values upon instantiation of view models.
        /// </summary>
        public IViewModelLocator MockViewModelLocator { get; set; }

        #region Methods

        public ViewModelType Resolve<ViewModelType>()
        {
            if (Container == null)
            {
                if (typeof(ViewModelType).IsInterface)
                {
                    return default(ViewModelType);
                }
                else if (typeof(ViewModelType).IsClass)
                {
                    // Note: Resolve should not return a null ViewModelType, so create an instance if necessary
                    // This is necessary for VS Design mode viewing and for Blend
                    return Activator.CreateInstance<ViewModelType>();
                }
            }
            ViewModelType viewModel = Container.Resolve<ViewModelType>();
            viewModel = SetMockValues<ViewModelType>(viewModel);
            return viewModel;
        }

        private ViewModelType SetMockValues<ViewModelType>(ViewModelType viewModel)
        {
            if (viewModel == null) { return viewModel; }
            if (!CopyMockValues) { return viewModel; }

            // Mock values should only be copied for view models marked as main content pages.
            var mainContentViewModel = viewModel as IMainContentViewModel;

            if (mainContentViewModel == null) { return viewModel; }

            // Mock values should only be copied if this view model isn't retrieving real data yet.
            if (mainContentViewModel != null && mainContentViewModel.IsRetrievingRealData) { return viewModel; }

            // Mock values cannot be copied if a mock view model locator hasn't been specified.
            if (MockViewModelLocator == null) { return viewModel; }

            var mockViewModel = MockViewModelLocator.Resolve<ViewModelType>();
            if (mockViewModel == null) { return viewModel; }

            ((IMainContentViewModel)mockViewModel).Container = mainContentViewModel.Container;
            ((IMainContentViewModel)mockViewModel).Initialize();

            var viewModelConcreteType = viewModel.GetType();
            IEnumerable<PropertyInfo> properties = (from Type typeInterface in typeof(ViewModelType).GetInterfaces()
                                                    from prop in typeInterface.GetProperties()
                                                    select prop);
            properties = properties.Union(typeof(ViewModelType).GetProperties());

            foreach (var property in properties)
            {
                TrySetValue<ViewModelType>(viewModel, mockViewModel, viewModelConcreteType, property);
            }
            return viewModel;
        }

        private static void TrySetValue<ViewModelType>(ViewModelType viewModel, ViewModelType mockViewModel, Type viewModelConcreteType, System.Reflection.PropertyInfo property)
        {
            object value = property.GetValue(mockViewModel, null);
            if (value == null) { return; }
            var concreteProperty = viewModelConcreteType.GetProperty(property.Name);
            try
            {
                concreteProperty.SetValue(viewModel, value, null);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Property {0} couldn't be set", concreteProperty.ToString());
            }
            return;
        }

        #endregion Methods
    }
}