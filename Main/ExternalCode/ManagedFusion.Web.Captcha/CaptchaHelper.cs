/** 
 * Copyright (C) 2007-2008 Nicholas Berardi, Managed Fusion, LLC (nick@managedfusion.com)
 * 
 * <author>Nicholas Berardi</author>
 * <author_email>nick@managedfusion.com</author_email>
 * <company>Managed Fusion, LLC</company>
 * <product>ASP.NET MVC CAPTCHA</product>
 * <license>Microsoft Public License (Ms-PL)</license>
 * <agreement>
 * This software, as defined above in <product />, is copyrighted by the <author /> and the <company /> 
 * and is licensed for use under <license />, all defined above.
 * 
 * This copyright notice may not be removed and if this <product /> or any parts of it are used any other
 * packaged software, attribution needs to be given to the author, <author />.  This can be in the form of a textual
 * message at program startup or in documentation (online or textual) provided with the packaged software.
 * </agreement>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Routing;

using ManagedFusion.Web.Controls;

namespace System.Web.Mvc
{
    public static class CaptchaHelper
    {
        /// <summary>
        /// Captchas the text box.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string CaptchaTextBox(this HtmlHelper helper, string name)
        {
            return String.Format(@"<input type=""text"" id=""{0}"" name=""{0}"" value="""" maxlength=""{1}"" autocomplete=""off"" />",
                name,
                ManagedFusion.Web.Controls.CaptchaImage.TextLength
                );

        }

        /// <summary>
        /// Generates the captcha image.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <returns>
        /// Returns the <see cref="Uri"/> for the generated <see cref="CaptchaImage"/>.
        /// </returns>
        public static string CaptchaImage(this HtmlHelper helper, int height, int width)
        {
            CaptchaImage image = new CaptchaImage
            {
                Height = height,
                Width = width,
            };

            HttpRuntime.Cache.Add(
                image.UniqueId,
                image,
                null,
                DateTime.Now.AddSeconds(ManagedFusion.Web.Controls.CaptchaImage.CacheTimeOut),
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null);

            var imageUrl = VirtualPathUtility.ToAbsolute("~/captcha.ashx", HttpContext.Current.Request.ApplicationPath); ;
            StringBuilder stringBuilder = new StringBuilder(256);
            stringBuilder.Append("<input type=\"hidden\" name=\"captcha-guid\" value=\"");
            stringBuilder.Append(image.UniqueId);
            stringBuilder.Append("\" />");
            stringBuilder.AppendLine();
            stringBuilder.Append("<img src=\"");
            stringBuilder.Append(String.Format("{0}?guid={1}", imageUrl,image.UniqueId));
            stringBuilder.Append("\" alt=\"CAPTCHA\" width=\"");
            stringBuilder.Append(width);
            stringBuilder.Append("\" height=\"");
            stringBuilder.Append(height);
            stringBuilder.Append("\" />");

            return stringBuilder.ToString();
        }
    }
}
