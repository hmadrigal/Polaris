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
}
