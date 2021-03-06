﻿using WindowsInput;

namespace Polaris.Windows.Controls
{
    public class TogglingModifierKey : ModifierKeyBase
    {
        public override void Press()
        {
            // This is a bit tricky because we can only get the state of a toggling key after the input has been
            // read off the MessagePump.  Ergo if we make that assumption that in the time it takes to run this method
            // we will be toggling the state of the key, set IsInEffect to the new state and then press the key.
            IsInEffect = !KeyboardService.IsTogglingKeyInEffect(KeyCode);
            base.Press();
        }

        public override void SynchroniseKeyState()
        {
            IsInEffect = KeyboardService.IsTogglingKeyInEffect(KeyCode);
        }
    }
}