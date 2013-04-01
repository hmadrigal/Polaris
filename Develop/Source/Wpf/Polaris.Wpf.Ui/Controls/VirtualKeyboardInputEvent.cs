using System;
namespace Polaris.Windows.Controls
{
    [Flags]
    public enum VirtualKeyboardInputEvent : int
    {
        MouseBasedEvent = 1,
        TouchBasedEvent = 2
    }
}
