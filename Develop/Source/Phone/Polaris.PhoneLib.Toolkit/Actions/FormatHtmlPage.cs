using System;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FooWPhoneLibrary.Toolkit.Actions
{
    public class FormatHtmlPage : WebBrowserAdapter
    {

        #region Source

        /// <summary>
        /// Source Dependency Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(Uri), typeof(FormatHtmlPage),
                new PropertyMetadata(default(Uri),
                    new PropertyChangedCallback(OnSourceChanged)));

        /// <summary>
        /// Gets or sets the Source property. This dependency property 
        /// indicates ....
        /// </summary>
        [Category("WebBrowser Adapter")]
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Source property.
        /// </summary>
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FormatHtmlPage target = (FormatHtmlPage)d;
            Uri oldSource = (Uri)e.OldValue;
            Uri newSource = target.Source;
            target.OnSourceChanged(oldSource, newSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Source property.
        /// </summary>
        protected virtual void OnSourceChanged(Uri oldSource, Uri newSource)
        {
            NavigateTo(newSource);
        }
        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                if (Source != default(Uri))
                    AssociatedObject.Navigate(Source);
            }
        }

        protected virtual void NavigateTo(Uri targetUri)
        {
            if (AssociatedObject == null || targetUri == default(Uri))
                return;
            AssociatedObject.Navigate(targetUri);
            NavigationStatus = WebBrowserNavigationStatus.Started;
        }
    }
}
