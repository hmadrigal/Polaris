using System.ComponentModel;
using Polaris.Windows.Services;

namespace Polaris.Windows.Controls
{
    public interface ILogicalKey : INotifyPropertyChanged
    {
        IKeyboardInput KeyboardService { get; set; }
        string DisplayName { get; }
        void Press();
        event LogicalKeyPressedEventHandler LogicalKeyPressed;
    }
}