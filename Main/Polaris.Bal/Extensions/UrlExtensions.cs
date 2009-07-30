using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Bal.Extensions
{
    public static class UrlExtensions
    {
        #region Fields

        private static readonly String SeparatorCharacter = "-";

        #endregion

        #region Public Methods

        public static String ToUrlFriendlyString(this String target)
        {

            String result = String.Empty;
            foreach (var word in target.Split(' '))
            {

                var urlFriendlyWord = String.Empty;

                // Removing URL illegal characters
                // # % & * { } \ : < > ? / + ' " = _ ( ) ^ [ ]
                urlFriendlyWord = word.ToLower();
                urlFriendlyWord = urlFriendlyWord.Replace("#", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("%", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("&", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("*", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("{", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("}", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("[", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("]", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("(", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace(")", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("\\", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("<", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace(">", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("?", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("'", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("\"", String.Empty);
                urlFriendlyWord = urlFriendlyWord.Replace("/", SeparatorCharacter);
                urlFriendlyWord = urlFriendlyWord.Replace("+", SeparatorCharacter);
                urlFriendlyWord = urlFriendlyWord.Replace(":", SeparatorCharacter);
                urlFriendlyWord = urlFriendlyWord.Replace("=", SeparatorCharacter);
                urlFriendlyWord = urlFriendlyWord.Replace(".", SeparatorCharacter);
                urlFriendlyWord = urlFriendlyWord.Replace("^", SeparatorCharacter);
                urlFriendlyWord = urlFriendlyWord.Replace("_", SeparatorCharacter);

                result += String.Format("{0}{1}",
                    (result == string.Empty ? String.Empty : SeparatorCharacter),
                    urlFriendlyWord);

            }

            return result;
        }

        /// <summary>
        /// Gets the user profile safe Url
        /// </summary>
        /// <param name="user">the user instance</param>
        /// <returns>the user profile page safe Url</returns>
        public static String GetUserSafeUrl(this IUser user)
        {
            return String.Format("/user/profile/{0}", user.Name.ToUrlFriendlyString());
        }

        /// <summary>
        /// Extension method intended to parse the value coming from an HTML checkbox into a Boolean properly.
        /// </summary>
        /// <param name="checkbox"></param>
        /// <returns></returns>
        public static Boolean GetCheckboxValue(this Boolean? checkbox)
        {
            return (checkbox.HasValue ? true : false);
        }

        /// <summary>
        /// Adds a query string parameter based on an existing Uri class.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Uri AddQuery(this Uri uri, string name, string value)
        {
            string newUrl = uri.OriginalString;
            if (!string.IsNullOrEmpty(uri.Query))
            {
                newUrl = string.Format("{0}&{1}={2}", newUrl, name, value);
            }
            else
            {
                newUrl = string.Format("{0}?{1}={2}", newUrl, name, value);
            }
            return new Uri(newUrl);
        }

        #endregion
    }
}
