using System;
using System.ComponentModel;
using System.Globalization;
using TimeOffSetEntry = System.Collections.Generic.KeyValuePair<int, System.TimeSpan>;

namespace Polaris.UnityExtensions
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(TimeOffSetEntry))]
    public class TimeOffSetEntryConverter : TypeConverter
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
                var stringParts = stringValue.Split(',');
                var stringKeyPart = stringParts[0].Substring(1, stringParts[0].Length - 1);
                var stringValuePart = stringParts[1].Substring(0, stringParts[1].Length - 1);
                var newTimeOffSetEntry = new TimeOffSetEntry(int.Parse(stringKeyPart), TimeSpan.Parse(stringValuePart));
                return newTimeOffSetEntry;
            }
            return base.ConvertFrom(context, culture, value);
        }

        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is TimeOffSetEntry)
            {
                var currentTimeOffSetEntry = (TimeOffSetEntry)value;
                var stringTimeOffSetEntry = string.Format("{{{0},{1}}}", currentTimeOffSetEntry.Key, currentTimeOffSetEntry.Value);
                return stringTimeOffSetEntry;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}