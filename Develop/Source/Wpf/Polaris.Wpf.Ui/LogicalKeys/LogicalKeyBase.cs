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

    public class LogicalKeyBase :  ILogicalKey
    {
        public event LogicalKeyPressedEventHandler LogicalKeyPressed;

        public IKeyboardInput KeyboardService { get; set; }
        public object DisplayName { get; set; }

        public LogicalKeyBase()
        {
            KeyboardService = VirtualKeyboardInput.Instance;
        }


        public virtual void Press()
        {
            OnKeyPressed();
        }

        protected void OnKeyPressed()
        {
            if (LogicalKeyPressed != null) LogicalKeyPressed(this, new LogicalKeyEventArgs(this));
        }


    }
}