//-----------------------------------------------------------------------
// <copyright file="DefaultCustomVirtualKeyHandler.cs" company="">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
using Polaris.Windows.Services;
namespace Polaris.Windows.Controls
{
    public class DefaultUserDefinedKeyHandler : IUserDefinedKeyHandler
    {
        public void HandleUserDefinedKey(QuertyKeyboard sender, ILogicalKey virtualKeyConfig, IKeyboardInput keyboardService)
        {
            //var currentLayout = (QuertyKeyboard)sender.KeyboardLayout;

            var keyName = (virtualKeyConfig as UserDefineKey).Id.ToString();
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
