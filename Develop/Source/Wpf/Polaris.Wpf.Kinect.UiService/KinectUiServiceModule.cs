namespace Polaris.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Kinect;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Polaris.Kinect;
    using Polaris.Kinect.Extensions;

    public class KinectUiServiceModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        #region IModule Members

        public void Initialize()
        {
            Container.RegisterType<IKinectUiService, KinectUiService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IKinectUiElementController, KinectUiElementController>();
            var kinectUiService = Container.Resolve<IKinectUiService>();
            try
            {
                Container.RegisterInstance<KinectSensor>(KinectExtensions.GetDefaultKinectSensor());
            }
            catch (KinectNotFoundException ex)
            {
                //TODO: Handle exception
            }
        }

        #endregion IModule Members
    }
}