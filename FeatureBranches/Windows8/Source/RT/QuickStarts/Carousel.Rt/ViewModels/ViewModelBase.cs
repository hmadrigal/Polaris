namespace Carousel.Rt.ViewModels
{
    using System.ComponentModel;

    public class ViewModelBase : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var threadSafePropertyChanged = PropertyChanged;
            if (threadSafePropertyChanged != null)
            {
                threadSafePropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion PropertyChanged
    }
}