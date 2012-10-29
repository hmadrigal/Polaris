using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carousel.Rt.ViewModels
{
    class CarouselItemViewModel : ViewModelBase
    {

        #region Name (INotifyPropertyChanged Property)
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        #endregion

        #region Description (INotifyPropertyChanged Property)
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        #endregion

        #region ImageUriString (INotifyPropertyChanged Property)
        private string _imageUriString;

        public string ImageUriString
        {
            get { return _imageUriString; }
            set
            {
                if (_imageUriString != value)
                {
                    _imageUriString = value;
                    OnPropertyChanged("ImageUriString");
                }
            }
        }
        #endregion


    }
}
