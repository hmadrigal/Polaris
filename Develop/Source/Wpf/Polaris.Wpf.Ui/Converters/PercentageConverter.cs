using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Polaris.Windows.Converters
{
    /// <summary>
    /// A converter that receives a Double from 0 to 1 and returns the interpolated value based on the given Initial and Final values.
    /// </summary>
    public class PercentageConverter : IValueConverter
    {
        public Double InitialValue { get; set; }

        public Double FinalValue { get; set; }

        public PercentageConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Double)
            {
                var normalizedElapsed = (Double)value;
                var totalRange = FinalValue - InitialValue;
                var absoluteElapsed = InitialValue + normalizedElapsed * totalRange;
                return absoluteElapsed;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}