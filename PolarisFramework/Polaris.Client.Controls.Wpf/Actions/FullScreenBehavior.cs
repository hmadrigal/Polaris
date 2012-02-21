//-----------------------------------------------------------------------
// <copyright file="FullScreenBehavior.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public sealed class FullScreenBehavior : Behavior<Window>
    {
        private static WindowStyle _windowStyle;
        private static WindowState _windowState;
        private static ResizeMode _resizeMode;

        public static readonly DependencyProperty FullScreenOnMaximizeProperty = DependencyProperty.Register("FullScreenOnMaximize", typeof(bool), typeof(FullScreenBehavior), new PropertyMetadata(false));

        public bool FullScreenOnMaximize
        {
            get { return (bool)GetValue(FullScreenOnMaximizeProperty); }
            set { SetValue(FullScreenOnMaximizeProperty, value); }
        }

        public static readonly DependencyProperty FullScreenOnDoubleClickProperty = DependencyProperty.Register("FullScreenOnDoubleClick", typeof(bool), typeof(FullScreenBehavior), new PropertyMetadata(false));

        public bool FullScreenOnDoubleClick
        {
            get { return (bool)GetValue(FullScreenOnDoubleClickProperty); }
            set { SetValue(FullScreenOnDoubleClickProperty, value); }
        }

        private static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.RegisterAttached("IsFullScreen", typeof(bool), typeof(FullScreenBehavior), new PropertyMetadata(default(bool), OnIsFullScreenChanged));

        public static bool GetIsFullScreen(Window window)
        {
            return (bool)window.GetValue(IsFullScreenProperty);
        }

        public static void SetIsFullScreen(Window window, bool value)
        {
            window.SetValue(IsFullScreenProperty, value);
        }

        private static void OnIsFullScreenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var window = (Window)sender;
            var oldFullScreen = (bool)e.OldValue;
            var fullScreen = (bool)e.NewValue;

            if (fullScreen != oldFullScreen && window != null)
            {
                if (fullScreen)
                {
                    _windowStyle = window.WindowStyle;
                    _windowState = window.WindowState;
                    _resizeMode = window.ResizeMode;

                    window.WindowState = WindowState.Normal;
                    window.WindowStyle = WindowStyle.None;
                    window.WindowState = WindowState.Maximized;
                    window.ResizeMode = ResizeMode.NoResize;
                    window.Topmost = true;
                }
                else
                {
                    window.Topmost = false;
                    window.WindowStyle = _windowStyle;
                    window.WindowState = _windowState;
                    window.ResizeMode = _resizeMode;
                }
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseDoubleClick += OnMouseDoubleClick;
            AssociatedObject.KeyDown += OnKeyDown;
            AssociatedObject.StateChanged += OnStateChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDoubleClick -= OnMouseDoubleClick;
            AssociatedObject.KeyDown -= OnKeyDown;
            AssociatedObject.StateChanged -= OnStateChanged;
            base.OnDetaching();
        }

        private void OnStateChanged(object sender, EventArgs eventArgs)
        {
            if (FullScreenOnMaximize && (sender is Window) && (sender as Window).WindowState == WindowState.Maximized)
                SetIsFullScreen(AssociatedObject, true);
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FullScreenOnDoubleClick && !e.Handled)
            {
                bool isFullScreen = GetIsFullScreen(AssociatedObject);
                SetIsFullScreen(AssociatedObject, !isFullScreen);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11 && !e.Handled)
            {
                bool isFullScreen = GetIsFullScreen(AssociatedObject);
                SetIsFullScreen(AssociatedObject, !isFullScreen);
            }
            else
                if (e.Key == Key.Escape && !e.Handled)
                {
                    SetIsFullScreen(AssociatedObject, false);
                }
        }
    }
}