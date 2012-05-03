using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Practices.Unity;

namespace Polaris.Windows.Services
{
    public class ViewRegistryService : IViewRegistryService
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void RegisterView<T>(string name, bool isSingleton = true) where T : FrameworkElement
        {
            if (isSingleton)
            {
                Container.RegisterType(typeof(T), name, new ContainerControlledLifetimeManager());
            }
            else
            {
                Container.RegisterType(typeof(T), name);
            }
        }

        public FrameworkElement ResolveView(string name)
        {
            var registration = Container.Registrations.Where(t => !String.IsNullOrEmpty(t.Name) && t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            if (registration == null)
            {
                throw new InvalidOperationException(string.Format(ExceptionStrings.ViewIsNotRegistered, name));
            }
            var view = Container.Resolve(registration.RegisteredType) as FrameworkElement;
            return view;
        }
    }
}