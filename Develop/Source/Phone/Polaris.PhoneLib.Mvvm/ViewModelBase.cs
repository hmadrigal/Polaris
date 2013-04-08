using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Linq.Expressions;
using GalaSoft.MvvmLight.Command;
using System.Runtime.CompilerServices;

#if WINDOWS_PHONE
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes; 
#endif

#if NETFX_CORE

#endif

namespace Polaris.PhoneLib.Mvvm
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
#if WINDOWS_PHONE
    , INavigate
#endif
    {

#if WINDOWS_PHONE
        public Dispatcher UiDispatcher { get { return Deployment.Current.Dispatcher; } }
        public Boolean IsInDesignTool { get { return DesignerProperties.IsInDesignTool; } }
        public bool IsDarkThemeSet { get { return Visibility.Visible == (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"]; } } 
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
                    OnPropertyChanged("LoadCommand");
                }
            }
        }
        #endregion

        public ViewModelBase(bool useLoadCommand = false)
        {
            if (useLoadCommand)
            {
                LoadCommand = new RelayCommand(LoadCommandInvoked);
            }
        }

        public virtual void LoadCommandInvoked()
        {
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(propertyExpression);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            RaisePropertyChanged(propertyName: propertyName);
        }

#if WINDOWS_PHONE
        protected virtual Microsoft.Phone.Controls.PhoneApplicationFrame ApplicationFrame
        {
            get { return _phoneApplicationFrame ?? (_phoneApplicationFrame = Application.Current.RootVisual as Microsoft.Phone.Controls.PhoneApplicationFrame); }
        }
        private Microsoft.Phone.Controls.PhoneApplicationFrame _phoneApplicationFrame;

        public virtual bool Navigate(Uri source)
        {
            return ApplicationFrame.Navigate(source);
        } 
#endif

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
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

    }

    public abstract class ViewModelBase<TResource> : GalaSoft.MvvmLight.ViewModelBase where TResource : class
    {
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

        public ViewModelBase(TResource localizedResources = default(TResource))
        {
            _localizedResources = localizedResources;
        }

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
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

    }
}
