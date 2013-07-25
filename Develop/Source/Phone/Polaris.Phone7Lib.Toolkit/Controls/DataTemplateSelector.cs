// Inspired on the original from Prism Library and this article http://www.windowsphonegeek.com/articles/Implementing-Windows-Phone-7-DataTemplateSelector-and-CustomDataTemplateSelector
// Type: Microsoft.Practices.Prism.ViewModel.DataTemplateSelector
// Assembly: Microsoft.Practices.Prism, Version=4.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35

using System.Windows;
using System.Windows.Controls;

namespace Polaris.PhoneLib.Toolkit.Controls
{
    /// <summary>
    /// This custom ContentControl changes its ContentTemplate based on the content it is presenting.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// In order to determine the template it must use for the new content, this control retrieves it from its
    ///             resources using the name for the type of the new content as the key.
    /// 
    /// </remarks>
    public class DataTemplateSelector : ContentControl
    {
        public string ResourceKeyFormat
        {
            get { return _resourceKeyFormat; }
            set { _resourceKeyFormat = value; }
        }
        private string _resourceKeyFormat = @"{0}";


        public string FallbackResourceKey
        {
            get { return _fallbackResourceKey; }
            set { _fallbackResourceKey = value; }
        }
        private string _fallbackResourceKey = string.Empty;

        /// <summary>
        /// Called when the value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property changes.
        /// 
        /// </summary>
        /// <param name="oldContent">The old value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param><param name="newContent">The new value of the <see cref="P:System.Windows.Controls.ContentControl.Content"/> property.</param>
        /// <remarks>
        /// Will attempt to discover the <see cref="T:System.Windows.DataTemplate"/> from the <see cref="T:System.Windows.ResourceDictionary"/> by matching the type name of <paramref name="newContent"/>.
        /// 
        /// </remarks>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (newContent == null)
                return;

            var dataTemplate = GetDataTemplate(newContent, this);

            if (dataTemplate == null)
                dataTemplate = Resources[string.Format(ResourceKeyFormat, newContent.GetType().Name)] as DataTemplate;

            if (dataTemplate == null && !string.IsNullOrEmpty(FallbackResourceKey))
                dataTemplate = Resources[FallbackResourceKey] as DataTemplate;
            ContentTemplate = dataTemplate;
        }

        /// <summary>
        /// Returns the default content template to use if not other content template can be located.
        /// 
        /// </summary>
        /// 
        /// <returns/>
        protected virtual DataTemplate GetDataTemplate(object newContent, DependencyObject container)
        {
            return null;
        }
    }
}
