using Microsoft.Phone.Shell;
using Polaris.PhoneLib.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Polaris.PhoneLib.Toolkit
{
    public class PolarisPhoneApp : Application
    {
        public bool WasApplicationTerminated
        {
            get { return _wasApplicationTerminated; }
        }
        private bool _wasApplicationTerminated = true;

        public PolarisPhoneApp(bool addActivationHandlers = false)
        {
            if (addActivationHandlers)
            {
                PhoneApplicationService.Current.Activated += OnApplicationActivated;
                PhoneApplicationService.Current.Deactivated += OnApplicationDeactivated;
            }
        }

        protected virtual void OnApplicationDeactivated(object sender, DeactivatedEventArgs e)
        {
            _wasApplicationTerminated = false;
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<Messages.AppLifeCycleMessage>(new Messages.AppLifeCycleMessage(Messages.LifeCycleState.Deactivating));
        }

        protected virtual void OnApplicationActivated(object sender, ActivatedEventArgs e)
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<AppLifeCycleMessage>(new AppLifeCycleMessage(_wasApplicationTerminated ? LifeCycleState.ActivatingFromTombstone : LifeCycleState.ActivatingFromDormant));
        }
    }
}
