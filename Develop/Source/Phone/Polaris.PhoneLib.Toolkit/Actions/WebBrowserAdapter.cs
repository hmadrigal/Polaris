using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Polaris.PhoneLib.Toolkit.Actions
{
    public abstract class WebBrowserAdapter : Behavior<WebBrowser>
    {

        public Stack<Uri> WebNavigationStack
        {
            get { return _webNavigationStack; }
        }
        private readonly Stack<Uri> _webNavigationStack;

        #region ForegroundColor

        /// <summary>
        /// ForegroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Brush), typeof(WebBrowserAdapter),
                new PropertyMetadata(Application.Current.Resources["PhoneForegroundBrush"]));

        /// <summary>
        /// Gets or sets the ForegroundColor property. This dependency property 
        /// indicates ....
        /// </summary>
        [Category("WebBrowser Adapter")]
        public Brush ForegroundColor
        {
            get { return (Brush)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        #endregion

        #region BackgroundColor

        /// <summary>
        /// BackgroundColor Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(WebBrowserAdapter),
                new PropertyMetadata(Application.Current.Resources["PhoneBackgroundBrush"]));

        /// <summary>
        /// Gets or sets the BackgroundColor property. This dependency property 
        /// indicates ....
        /// </summary>
        [Category("WebBrowser Adapter")]
        public Brush BackgroundColor
        {
            get { return (Brush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        #endregion

        #region NavigationStatus

        /// <summary>
        /// NavigationStatus Dependency Property
        /// </summary>
        public static readonly DependencyProperty NavigationStatusProperty =
            DependencyProperty.Register("NavigationStatus", typeof(WebBrowserNavigationStatus), typeof(WebBrowserAdapter),
                new PropertyMetadata(WebBrowserNavigationStatus.Default));

        /// <summary>
        /// Gets or sets the NavigationStatus property. This dependency property 
        /// indicates ....
        /// </summary>
        [Category("WebBrowser Adapter")]
        public WebBrowserNavigationStatus NavigationStatus
        {
            get { return (WebBrowserNavigationStatus)GetValue(NavigationStatusProperty); }
            set { SetValue(NavigationStatusProperty, value); }
        }

        #endregion

        #region WebBrowserViewModel

        /// <summary>
        /// WebBrowserViewModel Dependency Property
        /// </summary>
        public static readonly DependencyProperty WebBrowserViewModelProperty =
            DependencyProperty.Register("WebBrowserViewModel", typeof(IActionableWebBrowser), typeof(WebBrowserAdapter),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the WebBrowserViewModel property. This dependency property 
        /// indicates ....
        /// </summary>
        [Category("WebBrowser Adapter")]
        public IActionableWebBrowser WebBrowserViewModel
        {
            get { return (IActionableWebBrowser)GetValue(WebBrowserViewModelProperty); }
            set { SetValue(WebBrowserViewModelProperty, value); }
        }

        #endregion   

        /// <summary>
        /// Gets or sets whether to suppress the scrolling of
        /// the WebBrowser control;
        /// </summary>
        [Category("WebBrowser Adapter")]
        public bool ScrollDisabled { get; set; }

        [Category("WebBrowser Adapter")]
        public bool UseCustomColors { get; set; }

        public WebBrowserAdapter()
        {
            _webNavigationStack = new Stack<Uri>();
            UseCustomColors = false;
        }

        protected override void OnAttached()
        {

            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.Background = BackgroundColor;
                AssociatedObject.Foreground = ForegroundColor;
                AssociatedObject.IsScriptEnabled = true;

                AssociatedObject.Loaded += OnLoaded;

                AssociatedObject.ScriptNotify += OnScriptNotify;
                AssociatedObject.LoadCompleted += OnNavigationCompleted;
                
                AssociatedObject.ManipulationDelta += OnBorderManipulationDelta;
                AssociatedObject.ManipulationCompleted += OnBorderManipulationCompleted;

                AssociatedObject.Navigating += OnNavigating;
                AssociatedObject.Navigated += OnNavigated;
                AssociatedObject.NavigationFailed += OnNavigationFailed;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var associatedObject = sender as WebBrowser;
            AssociatedObject.Loaded -= OnLoaded;

            WebBrowserViewModel = new ActionableWebBrowser(_webNavigationStack, GoBack);
        }

        public virtual void OnNavigating(object sender, NavigatingEventArgs e)
        {
            NavigationStatus = WebBrowserNavigationStatus.Navigating;
        }

        public virtual void OnNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {}

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.ScriptNotify -= OnScriptNotify;
                AssociatedObject.LoadCompleted -= OnNavigationCompleted;
                AssociatedObject.ManipulationDelta -= OnBorderManipulationDelta;
                AssociatedObject.ManipulationCompleted -= OnBorderManipulationCompleted;
                AssociatedObject.Navigating -= OnNavigating;
                AssociatedObject.Navigated -= OnNavigated;
                AssociatedObject.NavigationFailed -= OnNavigationFailed;

            }

            base.OnDetaching();
        }

        protected virtual void OnNavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            NavigationStatus = WebBrowserNavigationStatus.Failed;
        }

        protected virtual void OnNavigationCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            var associatedObject = sender as WebBrowser;
            if (BackgroundColor is SolidColorBrush && UseCustomColors)
            {
                var backgroundSolidColorBrush = BackgroundColor as SolidColorBrush;
                var jsBackgroundColor = string.Format(@"'#{0}{1}{2}'", backgroundSolidColorBrush.Color.R, backgroundSolidColorBrush.Color.G, backgroundSolidColorBrush.Color.B);
                EvalJavaScript(associatedObject, string.Format(@"javascript:document.getElementById('content').style.background = {0};", jsBackgroundColor));
                EvalJavaScript(associatedObject, string.Format(@"javascript:document.body.style.background = {0};", jsBackgroundColor));
                EvalJavaScript(associatedObject, string.Format(@"javascript:document.bgColor = {0};", jsBackgroundColor));
            }

            if (ForegroundColor is SolidColorBrush && UseCustomColors)
            {
                var foregroundSolidColorBrush = ForegroundColor as SolidColorBrush;
                var jsForegroundColor = string.Format(@"'#{0}{1}{2}'", foregroundSolidColorBrush.Color.R, foregroundSolidColorBrush.Color.G, foregroundSolidColorBrush.Color.B);
                EvalJavaScript(associatedObject, string.Format(@"javascript:document.getElementById('content').style.color = {0};", jsForegroundColor));
                EvalJavaScript(associatedObject, string.Format(@"javascript:document.body.style.color = {0};", jsForegroundColor));
                EvalJavaScript(associatedObject, string.Format(@"javascript:document.fgColor = {0};", jsForegroundColor));
            }

            if (_webNavigationStack.Count == 0 || e.Uri != _webNavigationStack.Peek())
            {
                _webNavigationStack.Push(e.Uri); 
            }
            NavigationStatus = WebBrowserNavigationStatus.Completed;
        }

        protected virtual void OnBorderManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            // suppress zoom
            if (e.FinalVelocities.ExpansionVelocity.X != 0.0 ||
                e.FinalVelocities.ExpansionVelocity.Y != 0.0)
                e.Handled = true;
        }

        protected virtual void OnBorderManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            // suppress zoom
            if (e.DeltaManipulation.Scale.X != 0.0 ||
                e.DeltaManipulation.Scale.Y != 0.0)
                e.Handled = true;

            // optionally suppress scrolling
            if (ScrollDisabled)
            {
                if (e.DeltaManipulation.Translation.X != 0.0 ||
                  e.DeltaManipulation.Translation.Y != 0.0)
                    e.Handled = true;
            }
        }

        protected string EvalJavaScript(WebBrowser webBrowser, string jsCommand)
        {
            var jsEvalParameters = new string[] { jsCommand };
            string scriptResult = webBrowser.InvokeScript("eval", jsEvalParameters) as String;
            return scriptResult;
        }

        protected virtual void OnScriptNotify(object sender, NotifyEventArgs e)
        { }

        public bool GoBack()
        {
            if (_webNavigationStack.Count > 0)
            {
                _webNavigationStack.Pop();
                EvalJavaScript(AssociatedObject, @"javascript:window.history.back();");
                return true;
            }
            return false;
        }

        public class ActionableWebBrowser : IActionableWebBrowser
        {
            public Stack<Uri> WebNavigationStack
            {
                get { return _webNavigationStack; }
            }
            private readonly Stack<Uri> _webNavigationStack;

            public bool GoBack()
            {
                return _goBack();
            }
            private readonly Func<bool> _goBack;

            public ActionableWebBrowser(Stack<Uri> webNavigationStack, Func<bool> goBack)
            {
                _webNavigationStack = webNavigationStack;
                _goBack = goBack;
            }

        }

        
    }
}
