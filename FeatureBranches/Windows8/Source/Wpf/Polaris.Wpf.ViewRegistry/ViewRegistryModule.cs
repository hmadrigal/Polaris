using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Polaris.Services
{
    public class ViewRegistryModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public ViewRegistryModule()
        {
        }

        #region IModule Members

        public void Initialize()
        {
            Container.RegisterType<IViewRegistryService, ViewRegistryService>(new ContainerControlledLifetimeManager());
        }

        #endregion IModule Members
    }
}