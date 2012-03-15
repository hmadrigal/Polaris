//-----------------------------------------------------------------------
// <copyright file="EnumToResource.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Data;
    using System.Collections;

    public class EnumToResource : IValueConverter
    {
        public List<object> EnumMapping { get; set; }

        public EnumToResource()
        {
            EnumMapping = new List<object>();
        }

        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int adjustment = 0;
            if (parameter != null && !Int32.TryParse(parameter.ToString(), out adjustment))
            {
                adjustment = 0;
            }
            if (value == null) return this.EnumMapping.ElementAtOrDefault(0);
            else if (value is bool)
                return this.EnumMapping.ElementAtOrDefault(System.Convert.ToByte(value) + adjustment);
            else if (value is byte)
                return this.EnumMapping.ElementAtOrDefault(System.Convert.ToByte(value) + adjustment);
            else if (value is short)
                return this.EnumMapping.ElementAtOrDefault(System.Convert.ToInt16(value) + adjustment);
            else if (value is int)
                return this.EnumMapping.ElementAtOrDefault(System.Convert.ToInt32(value) + adjustment);
            else if (value is long)
                return this.EnumMapping.ElementAtOrDefault(System.Convert.ToInt32(value) + adjustment);
            else if (value is Enum)
                return this.EnumMapping.ElementAtOrDefault(System.Convert.ToInt32(value) + adjustment);

            return this.EnumMapping.ElementAtOrDefault(0);
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
