using System;
using System.ComponentModel;
using Polaris.Windows.Services;
using System.Windows;

namespace Polaris.Windows.Controls
{
    public class LogicalKeyEventArgs : EventArgs
    {
        public ILogicalKey Key { get; private set; }

        public LogicalKeyEventArgs(ILogicalKey key)
        {
            Key = key;
        }
    }

    public delegate void LogicalKeyPressedEventHandler(object sender, LogicalKeyEventArgs e);

    public class LogicalKeyBase : DependencyObject, ILogicalKey
    {
        public event LogicalKeyPressedEventHandler LogicalKeyPressed;
        public event PropertyChangedEventHandler PropertyChanged;

        #region DisplayName

        /// <summary>
        /// DisplayName Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(object), typeof(LogicalKeyBase),
                new FrameworkPropertyMetadata((object)null,
                    new PropertyChangedCallback(OnDisplayNameChanged)));

        /// <summary>
        /// Gets or sets the DisplayName property.  This dependency property 
        /// indicates the content to display on the key.
        /// </summary>
        public object DisplayName
        {
            get { return (object)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DisplayName property.
        /// </summary>
        private static void OnDisplayNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LogicalKeyBase)d).OnDisplayNameChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DisplayName property.
        /// </summary>
        protected virtual void OnDisplayNameChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        public IKeyboardInput KeyboardService
        {
            get { return _keyboardService; }
            set { _keyboardService = value; }
        }
        private IKeyboardInput _keyboardService = VirtualKeyboardInput.Instance;

        public virtual void Press()
        {
            OnKeyPressed();
        }

        protected void OnKeyPressed()
        {
            if (LogicalKeyPressed != null) LogicalKeyPressed(this, new LogicalKeyEventArgs(this));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }
    }
}