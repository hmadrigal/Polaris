using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows;

namespace Polaris.PhoneLib.Toolkit
{
    public abstract class ViewModelModelBase<TResource> : ViewModelBase where TResource : class
    {

        public bool Navigate(Uri source)
        {
            return MainFrame.Navigate(source);
        }

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

        public Dispatcher UiDispatcher { get { return Deployment.Current.Dispatcher; } } 

        public ViewModelModelBase(TResource localizedResources = default(TResource), bool addLoadCommandHandler = false)
        {
            _localizedResources = localizedResources;
            if (addLoadCommandHandler)
            {
                LoadCommand = new RelayCommand(OnLoadCommandInvoked);
            }
        }

        public virtual void OnLoadCommandInvoked()
        { }

#if DEBUG
        ~ViewModelModelBase()
        {
            System.Diagnostics.Debug.WriteLine("[{0}] Finalizing {1} #{2}", DateTime.Now, GetType().FullName, GetHashCode());
        }
#endif
    }
}
