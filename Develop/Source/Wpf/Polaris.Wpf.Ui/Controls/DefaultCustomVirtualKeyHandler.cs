//-----------------------------------------------------------------------
// <copyright file="DefaultCustomVirtualKeyHandler.cs" company="">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
using Polaris.Windows.Services;
namespace Polaris.Windows.Controls
{
    public class DefaultCustomVirtualKeyHandler : IVirtualKeyHandler
    {
        public void HandleCustomKeyStroke(QuertyKeyboard sender, VirtualKeyConfig virtualKeyConfig, IVirtualKeyboardService keyboardService)
        {
            //var currentLayout = (KeyboardLayout)sender.KeyboardLayout;
            var keyName = virtualKeyConfig.KeyName.ToString();
            switch (keyName)
            {
                case @"KBEXPAND":
                    sender.KeyboardLayout = DefaultKeyboardLayout.SplittedKeyboard;
                    break;
                case @"KBCOMPACT":
                    sender.KeyboardLayout = DefaultKeyboardLayout.StandardKeyboard;
                    break;
                case @"KBNUMERIC":
                    sender.KeyboardLayout = DefaultKeyboardLayout.NumericKeyboard;
                    break;
                default:
                    break;
            }
        }
    }
}
