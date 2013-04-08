using System;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;

namespace Polaris.PhoneLib.Toolkit.Actions
{
    /// <summary>
    /// Allows to Bind HtmlContent to a WebView using Mvvm approach. Moreover it provides some level of 
    /// customization to the Html View based on CSS and JS interaction. (see more details at http://blogs.msdn.com/b/mikeormond/archive/2010/12/16/displaying-html-content-in-windows-phone-7.aspx )
    /// </summary>
    public class FormatHtmlContent : WebBrowserAdapter
    {

        #region HtmlContent

        /// <summary>
        /// HtmlContent Dependency Property
        /// </summary>
        public static readonly DependencyProperty HtmlContentProperty =
            DependencyProperty.Register("HtmlContent", typeof(string), typeof(FormatHtmlContent),
                new PropertyMetadata(string.Empty,
                    new PropertyChangedCallback(OnHtmlContentChanged)));

        /// <summary>
        /// Gets or sets the HtmlContent property. This dependency property 
        /// indicates ....
        /// </summary>
        [Category("WebBrowser Adapter")]
        public string HtmlContent
        {
            get { return (string)GetValue(HtmlContentProperty); }
            set { SetValue(HtmlContentProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HtmlContent property.
        /// </summary>
        private static void OnHtmlContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FormatHtmlContent target = (FormatHtmlContent)d;
            string oldHtmlContent = (string)e.OldValue;
            string newHtmlContent = target.HtmlContent;
            target.OnHtmlContentChanged(oldHtmlContent, newHtmlContent);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the HtmlContent property.
        /// </summary>
        protected virtual void OnHtmlContentChanged(string oldHtmlContent, string newHtmlContent)
        {
            NavigateToHtmlContent(newHtmlContent);

        }


        #endregion

        private bool _isDocumentLoaded;

        private bool _hasRequestedHmlContent;

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                var htmlConcat = GetFullHtmlDocument(string.Empty);
                _isDocumentLoaded = false;
                _hasRequestedHmlContent = false;
                AssociatedObject.NavigateToString(htmlConcat);
            }
        }

        protected override void OnScriptNotify(object sender, NotifyEventArgs e)
        {
            _isDocumentLoaded = true;
            if (_hasRequestedHmlContent)
            {
                NavigateToHtmlContent(HtmlContent);
            }
        }

        protected virtual void NavigateToHtmlContent(string newHtmlContent)
        {
            if (AssociatedObject == null)
                return;
            if (_isDocumentLoaded)
            {
                NavigationStatus = WebBrowserNavigationStatus.Started;
                AssociatedObject.InvokeScript("setContent", newHtmlContent);
            }
            else
            {
                _hasRequestedHmlContent = true;
            }
        }

        protected virtual string GetFullHtmlDocument(string newHtmlContent)
        {
            SolidColorBrush foregroundSolidColorBrush = ForegroundColor as SolidColorBrush;
            var jsForegroundRgb = string.Empty;
            if (foregroundSolidColorBrush != null)
            {
                jsForegroundRgb = string.Format(@"rgb({0},{1},{2})", foregroundSolidColorBrush.Color.R, foregroundSolidColorBrush.Color.G, foregroundSolidColorBrush.Color.B);
            }

            SolidColorBrush backgroundSolidColorBrush = BackgroundColor as SolidColorBrush;
            var jsBackgroundRgb = string.Empty;
            if (backgroundSolidColorBrush != null)
            {
                jsBackgroundRgb = string.Format(@"rgb({0},{1},{2})", backgroundSolidColorBrush.Color.R, backgroundSolidColorBrush.Color.G, backgroundSolidColorBrush.Color.B);
            }
            var htmlScript =
                @"<script>
                    function setContent(s) { 
                        //document.body.innerHTML = s; 
                        document.getElementById('pageWrapper').innerHTML = s; 
                    }
                    function getDocHeight() { 
                        return document.getElementById('pageWrapper').offsetHeight;
                    }
                    function SendDataToPhoneApp() {
                        window.external.Notify('' + getDocHeight());
                    }
                </script>";
            var htmlConcat = string.Format(
                @"<html>
                    <head>
                        <meta name='viewport' content='width=device-width,height=device-height' />
                        {0}
                    </head>
                    <body 
                        style=""margin:0px;padding:0px;background-color:{3};"" 
                        onLoad=""SendDataToPhoneApp()"">
                        <div id=""pageWrapper"" style=""width:100%; color:{2}; 
                             background-color:{3}"">{1}
                        </div>
                    </body>
                </html>",
                htmlScript,
                newHtmlContent,
                jsForegroundRgb,
                jsBackgroundRgb);
            return htmlConcat;
        }

        protected override void OnNavigationCompleted(object sender, NavigationEventArgs e)
        {
            NavigationStatus = WebBrowserNavigationStatus.Completed;
        }
    }
}
