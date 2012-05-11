using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Polaris.Kinect.Extensions;
using Polaris.Services;

namespace Polaris.Kinect
{
    public class KinectControllerLocator
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public KinectControllerLocator()
        {
        }

        public IKinectUiElementController KinectUiElementController
        {
            get
            {
                return Container.Resolve<IKinectUiElementController>();
            }
        }

        public KinectSensor KinectSensor
        {
            get
            {
                return KinectExtensions.GetDefaultKinectSensor();
            }
        }

        public IKinectUiService KinectUiService
        {
            get
            {
                return Container.Resolve<IKinectUiService>();
            }
        }

        public ISpeechRecognitionService SpeechRecognitionService
        {
            get
            {
                return Container.Resolve<ISpeechRecognitionService>();
            }
        }
    }
}