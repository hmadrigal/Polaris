using Polaris.PhoneLib.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
namespace Polaris.Quickstart.CalPhoneApp.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {
        public ICommand CountDownCommand { get; set; }
        
        #region SelectedItem (INotifyPropertyChanged Property)
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }
        }
        private ItemViewModel _selectedItem;
        #endregion
        
        public DetailsViewModel()
        {
            CountDownCommand = new RelayCommand<object>(OnCountDownCommandInvoked);
        }

        public void OnCountDownCommandInvoked(object payload)
        {

        }
    }
}
