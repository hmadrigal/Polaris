using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// String constant that stored the pattern for identifying an email.
        /// </summary>
        public const String EmailPattern = @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})";

        /// <summary>
        /// Tries to convert an string to the a boolean type. If it's not posssible then the default value is returned.
        /// </summary>
        /// <param name="booleanStr">String to be converted. It represents a boolean value</param>
        /// <param name="defaultValue">Default value if the convertion fails</param>
        /// <returns></returns>
        public static Boolean ToBoolean(this String booleanStr, Boolean defaultValue)
        {
            if (String.IsNullOrEmpty(booleanStr)) return defaultValue;
            var result = defaultValue;
            if (!Boolean.TryParse(booleanStr, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Tries to convert an string to Int32. If it's not possible then the default value is returned. 
        /// </summary>
        /// <param name="intStr">String that represents an integer value</param>
        /// <param name="defaultValue">Default value if the convertion fails</param>
        /// <returns></returns>
        public static Int32 ToInt32(this String intStr, Int32 defaultValue)
        {
            if (String.IsNullOrEmpty(intStr)) return defaultValue;
            var result = defaultValue;
            if (!Int32.TryParse(intStr, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// Tries to convert an String to an specific Enumeration value. If it's not possible then the default value is returned.
        /// </summary>
        /// <typeparam name="T">Target enum type.</typeparam>
        /// <param name="enumValue">String that represents the enum value</param>
        /// <param name="defaultValue">Default value to return if the convertion fails</param>
        /// <param name="ignoreCase">Indicates if the convertion should be case sensitive </param>
        /// <returns></returns>
        public static T ToEnum<T>(this String enumValue, T defaultValue, Boolean ignoreCase)
        {
            // If the string is empty
            if (String.IsNullOrEmpty(enumValue)) return defaultValue;
            var sourceType = defaultValue.GetType();
            // if It's not an Enumeration
            if (!sourceType.IsEnum) return defaultValue;

            // If the value does not belongs to the enumeration
            if (!Enum.GetNames(sourceType).Contains(enumValue, ignoreCase ? StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture)) return defaultValue;

            // Just in case, it tries to convert the value.
            var result = defaultValue;
            try
            {
                result = (T)Enum.Parse(sourceType, enumValue, ignoreCase);
            }
            catch { result = defaultValue; }
            return result;
        }

        /// <summary>
        /// Tries to convert an String to an specific Enumeration value. If it's not possible then the default value is returned.
        /// This method is case insensitive
        /// </summary>
        /// <typeparam name="T">Target enum type.</typeparam>
        /// <param name="enumValue">String that represents the enum value</param>
        /// <param name="defaultValue">Default value to return if the convertion fails</param>
        /// <returns></returns>
        public static T ToEnum<T>(this String enumValue, T defaultValue)
        {
            return ToEnum<T>(enumValue, defaultValue, true);
        }
    }
}
