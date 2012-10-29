namespace Carousel.Rt.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.ApplicationModel;

    class MainViewModel : ViewModelBase
    {
        #region CarouselItems Property
        public ObservableCollection<CarouselItemViewModel> CarouselItems
        {
            get
            {
                return _carouselItems;
            }
        }
        ObservableCollection<CarouselItemViewModel> _carouselItems = new ObservableCollection<CarouselItemViewModel>();
        #endregion

        public MainViewModel()
        {
            var installedLocation = Package.Current.InstalledLocation.Path;

            for (int i = 1; i <= 12; i++)
            {
                var imageName = string.Format("Assets/BirdsCarousel/Bird{0:0000}.jpg", i);
                var imagePath = Path.Combine(installedLocation, imageName);
                var imageUri = new Uri(imagePath);

                var carouselItemViewModel = new CarouselItemViewModel() 
                {
                    Name = string.Format("BIRD {0:0000}", i),
                    Description = string.Format("BIRD {0:0000} DESCRIPTION", i),
                    ImageUriString = imageUri.ToString(),
                };

                _carouselItems.Add(carouselItemViewModel);
            }
        }
    }
}
