using System;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Collections.Generic;

namespace FooWPhoneLibrary.Toolkit
{
    public interface INavigatableViewModel : INavigate
    {
        IDictionary<string, object> State { get; set; }
        void OnFragmentNavigation(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, FragmentNavigationEventArgs e);
        void OnNavigatedFrom(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.Windows.Navigation.NavigationEventArgs e);
        void OnNavigatedTo(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.Windows.Navigation.NavigationEventArgs e);
        void OnNavigatingFrom(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.Windows.Navigation.NavigatingCancelEventArgs e);
        void OnBackKeyPress(NavigationCacheMode navigationCacheMode, NavigationContext navigationContext, NavigationService navigationService, System.ComponentModel.CancelEventArgs e);
    }
}
