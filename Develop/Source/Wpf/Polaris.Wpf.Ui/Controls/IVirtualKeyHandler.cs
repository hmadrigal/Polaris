using System;
using Polaris.Windows.Services;

namespace Polaris.Windows.Controls
{
    public interface IVirtualKeyHandler
    {
        void HandleCustomKeyStroke(QuertyKeyboard sender, VirtualKeyConfig virtualKeyConfig, IVirtualKeyboardService keyboardService);
    }
}
