using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Polaris.Services
{
    public class SpeechRecognitionModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        #region IModule Members

        public void Initialize()
        {
            Container.RegisterType<ISpeechRecognitionService, SpeechRecognitionService>(new ContainerControlledLifetimeManager());
        }

        #endregion IModule Members
    }
}