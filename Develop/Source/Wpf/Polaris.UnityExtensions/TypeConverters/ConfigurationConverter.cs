using System;
using System.ComponentModel;
using System.Configuration;

namespace Polaris.EnterpriseEx.TypeConverters
{
    /// <summary>
    /// <para>
    /// Allows to interpret specific strings to return values from the configuration file. 
    ///     AppSettings[stringKey] : It's equivalent to ConfigurationManager.AppSettings[stringKey]
    ///     ConnectionStrings[stringKey] : It's equivalent to ConfigurationManager.ConnectionStrings[stringKey].ConnectionStrin
    /// </para>
    /// </summary>
    public class ConfigurationConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (typeof(string) == sourceType) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }

            var stringValue = value.ToString();
            string stringKey;
            object objectValue = null;
            if (stringValue.StartsWith("AppSettings["))
            {
                stringKey = stringValue.Substring(12, stringValue.Length - 13);
                objectValue = ConfigurationManager.AppSettings[stringKey];
            }
            else if (stringValue.StartsWith("ConnectionStrings["))
            {
                stringKey = stringValue.Substring(18, stringValue.Length - 19);
                objectValue = ConfigurationManager.ConnectionStrings[stringKey].ConnectionString;
            }

            return objectValue ?? base.ConvertFrom(context, culture, value);

        }
    }
}

