//-----------------------------------------------------------------------
// <copyright file="VirtualKeyboardService.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------

namespace Polaris.Windows.Services
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using WindowsInput;
    using System.Diagnostics;

    public sealed class DebugKeyboardInput : IKeyboardInput
    {
        private void InitializeDebugKeyboardInput()
        {
            _keyPress = new Dictionary<VirtualKeyCode, bool>();
        }

        #region Singleton Pattern w/ Constructor
        private DebugKeyboardInput()
            : base()
        {
            InitializeDebugKeyboardInput();
        }
        public static DebugKeyboardInput Instance
        {
            get
            {
                return SingletonDebugKeyboardInputCreator._Instance;
            }
        }
        private class SingletonDebugKeyboardInputCreator
        {
            private SingletonDebugKeyboardInputCreator() { }
            public static DebugKeyboardInput _Instance = new DebugKeyboardInput();
        }
        #endregion

        private Dictionary<VirtualKeyCode, bool> _keyPress;

        #region IKeyboardInput
        public bool IsKeyDownAsync(VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[IsKeyDownAsync] keyCode:{0}", keyCode);
            return _keyPress.ContainsKey(keyCode) ? _keyPress[keyCode] : false;
        }

        public bool IsKeyDown(VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[IsKeyDown] keyCode:{0}", keyCode);
            return _keyPress.ContainsKey(keyCode) ? _keyPress[keyCode] : false;
        }

        public bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[IsTogglingKeyInEffect] keyCode:{0}", keyCode);
            return _keyPress.ContainsKey(keyCode) ? _keyPress[keyCode] : false;
        }

        public void SimulateKeyDown(VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[SimulateKeyDown] keyCode:{0}", keyCode);
            _keyPress[keyCode] = true;
        }

        public void SimulateKeyUp(VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[SimulateKeyUp] keyCode:{0}", keyCode);
            _keyPress[keyCode] = false;
        }

        public void SimulateKeyPress(VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[SimulateKeyPress] keyCode:{0}", keyCode);
        }

        public void SimulateTextEntry(string text)
        {
            Debug.WriteLine(string.Format("[SimulateKeyPress] text:{0}", text));
        }

        public void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            Debug.WriteLine("[SimulateModifiedKeyStroke] modifierKeyCode:{0} keyCode:{1}", modifierKeyCode, keyCode);
        }

        public void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            foreach (var modifierKeyCode in modifierKeyCodes)
                Debug.WriteLine("[SimulateModifiedKeyStroke] modifierKeyCode:{0} keyCode:{1}", modifierKeyCode, keyCode);
        }

        public void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, IEnumerable<VirtualKeyCode> keyCodes)
        {
            foreach (var keyCode in keyCodes)
            {
                Debug.WriteLine("[SimulateModifiedKeyStroke] modifierKeyCode:{0} keyCode:{1}", modifierKeyCode, keyCode);
            }

        }

        public void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            foreach (var modifierKeyCode in modifierKeyCodes)
                foreach (var keyCode in keyCodes)
                    Debug.WriteLine("[SimulateModifiedKeyStroke] modifierKeyCode:{0} keyCode:{1}", modifierKeyCode, keyCode);

        }
        #endregion
    }
}
