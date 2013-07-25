using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Polaris.PhoneLib.Toolkit.Converters
{
    public class FactorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doubleValue = System.Convert.ToDouble(value);
            var doubleParameter = System.Convert.ToDouble(parameter);
            return doubleValue * doubleParameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doubleValue = System.Convert.ToDouble(value);
            var doubleParameter = System.Convert.ToDouble(parameter);
            return doubleValue / doubleParameter;
        }
    }
}
