using System;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows;

namespace Polaris.PhoneLib.Toolkit.Converters
{
    public class EqualityToVisibilityConverter : IValueConverter
    {
        public bool InvertResult { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var areEquals = value == parameter;
            if (InvertResult)
                areEquals = !areEquals;
            return areEquals ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Application cannot convert back EqualityToVisibilityConverter value ");
        }
    }
}
