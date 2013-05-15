using System;
using Microsoft.Phone.Controls;
using System.Xaml.Linq;
using System.Linq;
using Microsoft.Phone.Shell;
using Polaris.PhoneLib.Mvvm;

namespace Polaris.PhoneLib.Toolkit.Controls
{
    public class PhoneApplicationNavigablePage : PhoneApplicationPage
    {
        internal INavigatableViewModel NavigatablePage
        {
            get
            {
                return DataContext as INavigatableViewModel;
            }
        }

        public PhoneApplicationNavigablePage()
        {
            Loaded += OnPageLoaded;
            Unloaded += OnPageUnloaded;
        }

        protected virtual void OnPageUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as PhoneApplicationNavigablePage).Unloaded += OnPageUnloaded;
        }

        protected virtual void OnPageLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as PhoneApplicationNavigablePage).Loaded -= OnPageLoaded;
            if (ApplicationBar == null || ApplicationBar.Buttons.Count == 0)
            {
                return;
            }
            foreach (var applicationBarIconButton in ApplicationBar.Buttons.OfType<IApplicationBarIconButton>())
            {
                applicationBarIconButton.Text = GetLocalizationFor(applicationBarIconButton.Text);
            }
            
        }

        protected virtual string GetLocalizationFor(string localizationKey)
        {
            return localizationKey;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (NavigatablePage == null)
                return;
            NavigatablePage.State = State;
            NavigatablePage.OnNavigatedFrom(NavigationCacheMode, NavigationContext, NavigationService, e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            OnRestoreState(e);
            if (NavigatablePage == null)
                return;
            NavigatablePage.State = State;
            NavigatablePage.OnNavigatedTo(NavigationCacheMode, NavigationContext, NavigationService, e);
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            OnSaveState(e);
            if (NavigatablePage == null || e.Cancel)
                return;
            NavigatablePage.State = State;
            NavigatablePage.OnNavigatingFrom(NavigationCacheMode, NavigationContext, NavigationService, e);
        }

        protected virtual void OnSaveState(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        protected virtual void OnRestoreState(System.Windows.Navigation.NavigationEventArgs e)
        {
        }

        protected override void OnFragmentNavigation(System.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            base.OnFragmentNavigation(e);
            if (NavigatablePage == null)
                return;
            NavigatablePage.State = State;
            NavigatablePage.OnFragmentNavigation(NavigationCacheMode, NavigationContext, NavigationService, e);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            if (NavigatablePage == null || e.Cancel)
                return;
            NavigatablePage.State = State;
            NavigatablePage.OnBackKeyPress(NavigationCacheMode, NavigationContext, NavigationService, e);
        }

        protected override void OnRemovedFromJournal(System.Windows.Navigation.JournalEntryRemovedEventArgs e)
        {
            base.OnRemovedFromJournal(e);
            DataContext = null;
        }

#if DEBUG
        ~PhoneApplicationNavigablePage()
        {
            System.Diagnostics.Debug.WriteLine("[{0}] Finalizing {1} #{2}", DateTime.Now, GetType().FullName, GetHashCode());
        }
#endif
    }
}
