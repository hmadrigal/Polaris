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
        //public object DisplayName { get; set; }
        #region DisplayName

        /// <summary>
        /// DisplayName Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty =
            DependencyProperty.Register("DisplayName", typeof(object), typeof(DependencyLogicalKey),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the DisplayName property.  This dependency property 
        /// indicates ....
        /// </summary>
        public object DisplayName
        {
            get { return (object)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        #endregion

        

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
            var threadSafeLogicalKeyPressed = LogicalKeyPressed;
            if (threadSafeLogicalKeyPressed != null) threadSafeLogicalKeyPressed(this, new LogicalKeyEventArgs(this));
        }
    }

}