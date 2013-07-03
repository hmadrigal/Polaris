using System;
using System.ComponentModel;
using System.Globalization;

namespace Polaris.EnterpriseEx.TypeConverters
{
    [TypeConverter(typeof(TimeSpanConverter))]
    public class TimeSpanConverter : TypeConverter
    {
        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var stringValue = (string)value;
                var newTimeSpan = TimeSpan.Parse(stringValue);
                return newTimeSpan;
            }
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is TimeSpan)
            {
                var currentTimeSpan = (TimeSpan)value;
                var stringTimeSpan = currentTimeSpan.ToString();
                return stringTimeSpan;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
