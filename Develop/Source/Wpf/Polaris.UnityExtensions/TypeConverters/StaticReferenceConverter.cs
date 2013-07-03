using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

namespace Polaris.EnterpriseEx.TypeConverters
{

    /// <summary>
    /// Converts an (formatted) string to a reference of a given static member.
    /// The string uses this format: {member}@{assemblyQualifiedName}
    /// For example:
    ///     Version@System.Environment, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
    /// </summary>
    public class StaticReferenceConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (typeof(string) == sourceType) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string stringValue;
            if (value == null
                || (stringValue = value as string) == null
                || string.IsNullOrEmpty(stringValue)
                || !stringValue.Contains("@")
            )
            {
                return null;
            }

            var stringParts = stringValue.Split('@');
            if (stringParts.Length != 2)
            {
                return null;
            }
            var propertyPath = stringParts[0];
            var propertyPathParts = propertyPath.Split('.');
            var assemblyQualifiedName = stringParts[1];
            // Gets the static member
            var currentClassType = Type.GetType(assemblyQualifiedName, true);
            var currentPropertyInfo = currentClassType.GetProperty(propertyPathParts[0],
                                                                 BindingFlags.Public | BindingFlags.Static |
                                                                 BindingFlags.FlattenHierarchy);
            var currentPropertyValue = currentPropertyInfo.GetValue(null, null);
            // Loops into the path to get a value
            for (var i = 1; i < propertyPathParts.Length; i++)
            {
                currentPropertyInfo = currentClassType.GetProperty(propertyPathParts[i]);
                currentPropertyValue = currentPropertyInfo.GetValue(currentPropertyValue, null);
            }
            return currentPropertyValue ?? base.ConvertFrom(context, culture, value);
        }
    }
}


