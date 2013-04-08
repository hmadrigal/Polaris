using System;
using System.Collections.Generic;

#if NETFX_CORE
using Windows.UI.Xaml.Navigation;
#endif

#if WINDOWS_PHONE
using System.Windows.Navigation;
#endif
namespace Polaris.PhoneLib.Mvvm
{
    public class PageViewModelBase<TResource> : ViewModelBase<TResource>, INavigatableViewModel where TResource : class
    {
        public IDictionary<string, object> State { get; set; }

        public PageViewModelBase(TResource localizedResources = default(TResource), bool addLoadCommandHandler = false)
            : base(localizedResources)
        {

        }

#if WINDOWS_PHONE
        public virtual void OnFragmentNavigation(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, FragmentNavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedFrom(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedTo(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatingFrom(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, NavigatingCancelEventArgs e)
        {
        }

        public virtual void OnBackKeyPress(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.ComponentModel.CancelEventArgs e)
        {
        } 
#endif
    }
}
