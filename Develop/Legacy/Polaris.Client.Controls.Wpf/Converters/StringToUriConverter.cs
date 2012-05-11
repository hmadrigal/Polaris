//-----------------------------------------------------------------------
// <copyright file="StringToUriConverter.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Converters
{
    using System;
    using System.Windows.Data;

    public class StringToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;
            var uriString = value.ToString();
            Uri resultUri;
            return Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out resultUri) ? resultUri : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }
    }
}