using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Polaris.Windows.Controls
{
    public sealed class DefaultWpfVisualKeyboardHandler : IVisualKeyboardHandler
    {
        private Dictionary<WindowsInput.VirtualKeyCode, List<ContentControl>> _shiftKeys;
        public void HandleKeyDown(System.Windows.Controls.ContentControl sender, DependencyLogicalKey virtualKeyConfig, Dictionary<ContentControl, DependencyLogicalKey> virtualKeys)
        {
            var virtualKey = QuertyKeyboard.GetVirtualKey(sender) as VirtualKey;
            var isShiftVirtualKey = GetIsShiftVirtualKey(virtualKey);
            if (isShiftVirtualKey || virtualKey == null)
            { return; }

            foreach (var kvp in _shiftKeys)
            {
                foreach (var control in _shiftKeys[kvp.Key])
                {
                    if (control is System.Windows.Controls.CheckBox)
                    {
                        var checkBox = control as System.Windows.Controls.CheckBox;
                        checkBox.IsChecked = null;
                        checkBox.IsChecked = virtualKey.KeyboardService.IsKeyDownAsync(kvp.Key);
                    }
                    else if (control is System.Windows.Controls.Primitives.ToggleButton)
                    {
                        var toggleButton = control as System.Windows.Controls.Primitives.ToggleButton;
                        toggleButton.IsChecked = null;
                        toggleButton.IsChecked = virtualKey.KeyboardService.IsKeyDownAsync(kvp.Key);
                    }
                } 
            }
        }

        public void RegisterKey(System.Windows.Controls.ContentControl sender)
        {
            var instantaneousModifierKey = QuertyKeyboard.GetVirtualKey(sender) as InstantaneousModifierKey;
            var isShiftVirtualKey = GetIsShiftVirtualKey(instantaneousModifierKey);
            if (!isShiftVirtualKey)
            { return; }

            if (!_shiftKeys.ContainsKey(instantaneousModifierKey.KeyCode))
                _shiftKeys[instantaneousModifierKey.KeyCode] = new List<ContentControl>();
            _shiftKeys[instantaneousModifierKey.KeyCode].Add(sender);
        }

        private static bool GetIsShiftVirtualKey(VirtualKey modKey)
        {
            var isShiftKey = 
                    modKey != null
                    && (   modKey.KeyCode == WindowsInput.VirtualKeyCode.SHIFT
                        || modKey.KeyCode == WindowsInput.VirtualKeyCode.LSHIFT
                        || modKey.KeyCode == WindowsInput.VirtualKeyCode.RSHIFT)
               ;
            return isShiftKey;
        }

        public void DeregisterKey(System.Windows.Controls.ContentControl sender)
        {

        }

        private void InitializeDefaultWpfVisualKeyboardHandler()
        {
            _shiftKeys = new Dictionary<WindowsInput.VirtualKeyCode, List<ContentControl>>();
        }

        #region Singleton Pattern w/ Constructor
        private DefaultWpfVisualKeyboardHandler()
            : base()
        {
            InitializeDefaultWpfVisualKeyboardHandler();
        }
        public static DefaultWpfVisualKeyboardHandler Instance
        {
            get
            {
                return SingletonDefaultWpfVisualKeyboardHandlerCreator._Instance;
            }
        }
        private class SingletonDefaultWpfVisualKeyboardHandlerCreator
        {
            private SingletonDefaultWpfVisualKeyboardHandlerCreator() { }
            public static DefaultWpfVisualKeyboardHandler _Instance = new DefaultWpfVisualKeyboardHandler();
        }
        #endregion



    }
}
