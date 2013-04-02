using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Polaris.Quickstart.QuertyKeyboard
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region IsShiftEnabled (INotifyPropertyChanged Property)
        public bool IsShiftEnabled
        {
            get { return _isShiftEnabled; }
            set
            {
                if (_isShiftEnabled != value)
                {
                    _isShiftEnabled = value;
                    RaisePropertyChanged("IsShiftEnabled");
                }
            }
        }
        private bool _isShiftEnabled;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var threadSafePropertyChanged = PropertyChanged;
            if (threadSafePropertyChanged != null)
            {
                threadSafePropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
