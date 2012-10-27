using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Polaris.Services;

namespace Polaris.Kinect
{
    public interface IKinectCursorViewModel
    {
        IKinectUiService KinectUiService { get; set; }
    }

    public class KinectCursorViewModel : ViewModelBase, IKinectCursorViewModel
    {
        #region KinectUiService (INotifyPropertyChanged Property)

        private IKinectUiService kinectUiService;

        public IKinectUiService KinectUiService
        {
            get { return kinectUiService; }
            set
            {
                if (kinectUiService != value)
                {
                    kinectUiService = value;
                    OnPropertyChanged("KinectUiService");
                }
            }
        }

        #endregion KinectUiService (INotifyPropertyChanged Property)

        public KinectCursorViewModel(IUnityContainer container)
            : base(container)
        {
        }

        protected override void OnResolveDependencies()
        {
            base.OnResolveDependencies();
            KinectUiService = Container.Resolve<IKinectUiService>();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            KinectUiService.Start();
        }
    }
}