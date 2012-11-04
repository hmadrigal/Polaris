using System;
using Polaris.Windows.Services;

namespace Polaris.Windows.Controls
{
    public interface IUserDefinedKeyHandler
    {
        void HandleUserDefinedKey(QuertyKeyboard sender, DependencyLogicalKey virtualKeyConfig, IKeyboardInput keyboardService);
    }
}
