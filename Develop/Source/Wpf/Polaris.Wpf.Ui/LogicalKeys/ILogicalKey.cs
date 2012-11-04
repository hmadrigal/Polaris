namespace Polaris.Windows.Controls
{
    using System.ComponentModel;
    using Polaris.Windows.Services;
    using System.Windows;

    public interface ILogicalKey 
    {
        IKeyboardInput KeyboardService { get; set; }
        object DisplayName { get; }
        void Press();
        event LogicalKeyPressedEventHandler LogicalKeyPressed;
    }

    public class DependencyLogicalKey : DependencyObject, ILogicalKey
    {
        public event LogicalKeyPressedEventHandler LogicalKeyPressed;

        public IKeyboardInput KeyboardService { get; set; }
        public object DisplayName { get; set; }

        public DependencyLogicalKey()
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