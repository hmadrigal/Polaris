using System;
using System.Collections.Generic;

#if NETFX_CORE
using Windows.UI.Xaml.Navigation;
#endif
#if WINDOWS_PHONE
using System.Windows.Navigation;
using System.Windows.Controls; 
#endif

namespace Polaris.PhoneLib.Mvvm
{
    public interface INavigatableViewModel
    {
        IDictionary<string, object> State { get; set; }

#if WINDOWS_PHONE
        void OnFragmentNavigation(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, FragmentNavigationEventArgs e);
        void OnNavigatedFrom(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.Windows.Navigation.NavigationEventArgs e);
        void OnNavigatedTo(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.Windows.Navigation.NavigationEventArgs e);
        void OnNavigatingFrom(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.Windows.Navigation.NavigatingCancelEventArgs e);
        void OnBackKeyPress(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.ComponentModel.CancelEventArgs e); 
#endif

    }
}
