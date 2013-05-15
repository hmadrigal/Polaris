using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Polaris.PhoneLib.Mvvm
{
    public class PageViewModelBase<TResource> : ViewModelBase<TResource>, INavigatableViewModel where TResource : class
    {
        public IDictionary<string, object> State { get; set; }

        public PageViewModelBase(TResource localizedResources = default(TResource), bool addLoadCommandHandler = false)
            : base(localizedResources, addLoadCommandHandler)
        {

        }

#if WINDOWS_PHONE
        public virtual void OnFragmentNavigation(System.Windows.Navigation.NavigationCacheMode navigationCacheMode, System.Windows.Navigation.NavigationContext navigationContext, System.Windows.Navigation.NavigationService navigationService, System.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedFrom(System.Windows.Navigation.NavigationCacheMode navigationCacheMode, System.Windows.Navigation.NavigationContext navigationContext, System.Windows.Navigation.NavigationService navigationService, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedTo(System.Windows.Navigation.NavigationCacheMode navigationCacheMode, System.Windows.Navigation.NavigationContext navigationContext, System.Windows.Navigation.NavigationService navigationService, System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatingFrom(System.Windows.Navigation.NavigationCacheMode navigationCacheMode, System.Windows.Navigation.NavigationContext navigationContext, System.Windows.Navigation.NavigationService navigationService, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        public virtual void OnBackKeyPress(System.Windows.Navigation.NavigationCacheMode navigationCacheMode, System.Windows.Navigation.NavigationContext navigationContext, System.Windows.Navigation.NavigationService navigationService, System.ComponentModel.CancelEventArgs e)
        {
        } 
#endif


    }
}
