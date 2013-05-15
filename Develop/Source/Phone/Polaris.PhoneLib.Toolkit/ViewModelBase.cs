using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows;

namespace Polaris.PhoneLib.Toolkit
{
    public abstract class ViewModelBase<TResource> : ViewModelBase
#if WINDOWS_PHONE
, INavigate
#endif
 where TResource : class
    {

#if WINDOWS_PHONE
        public Dispatcher UiDispatcher { get { return Deployment.Current.Dispatcher; } }
        public Boolean IsInDesignTool { get { return System.ComponentModel.DesignerProperties.IsInDesignTool; } }
        public bool IsDarkThemeSet { get { return Visibility.Visible == (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]; } }

        private static Frame _mainFrame;
        private static Frame MainFrame
        {
            get
            {
                if (_mainFrame == null)
                {
                    _mainFrame = Application.Current.RootVisual as Frame;
                }
                return _mainFrame;
            }
        }
#endif
        #region LoadCommand (INotifyPropertyChanged Property)
        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get { return _loadCommand; }
            set
            {
                if (_loadCommand != value)
                {
                    _loadCommand = value;
                    RaisePropertyChanged(() => LoadCommand);
                }
            }
        }
        #endregion

        #region LocalizedResources (INotifyPropertyChanged Property)
        private TResource _localizedResources;

        public TResource LocalizedResources
        {
            get { return _localizedResources; }
            set
            {
                if (_localizedResources != value)
                {
                    _localizedResources = value;
                    RaisePropertyChanged(() => LocalizedResources);
                }
            }
        }
        #endregion


        public bool Navigate(Uri source)
        {
            return MainFrame.Navigate(source);
        }

        public ViewModelBase(TResource localizedResources = default(TResource), bool addLoadCommandHandler = false)
        {
            _localizedResources = localizedResources;
            if (addLoadCommandHandler)
            {
                LoadCommand = new RelayCommand(OnLoadCommandInvoked);
            }
        }

        public virtual void OnLoadCommandInvoked()
        { }

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

#if DEBUG
        ~ViewModelModelBase()
        {
            System.Diagnostics.Debug.WriteLine("[{0}] Finalizing {1} #{2}", DateTime.Now.ToString("o"), GetType().FullName, GetHashCode());
        }
#endif
    }
}
