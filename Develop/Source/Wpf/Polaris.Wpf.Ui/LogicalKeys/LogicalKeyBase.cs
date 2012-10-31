using System;
using System.ComponentModel;
using Polaris.Windows.Services;

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

    public abstract class LogicalKeyBase : ILogicalKey
    {
        public event LogicalKeyPressedEventHandler LogicalKeyPressed;
        public event PropertyChangedEventHandler PropertyChanged;

        private string _displayName;

        public virtual string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (value != _displayName)
                {
                    _displayName = value;
                    OnPropertyChanged("DisplayName");
                }
            }
        }

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