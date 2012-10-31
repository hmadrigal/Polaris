using WindowsInput;

namespace Polaris.Windows.Controls
{
    public class InstantaneousModifierKey : ModifierKeyBase
    {
        //public InstantaneousModifierKey(string displayName, VirtualKeyCode keyCode) :
        //    base(keyCode)
        //{
        //    DisplayName = displayName;
        //}

        public override void Press()
        {
            if (IsInEffect) KeyboardService.SimulateKeyUp(KeyCode);
            else KeyboardService.SimulateKeyDown(KeyCode);

            // We need to use IsKeyDownAsync here so we will know exactly what state the key will be in
            // once the active windows read the input from the MessagePump.  IsKeyDown will only report
            // the correct value after the input has been read from the MessagePump and will not be correct
            // by the time we set IsInEffect.
            IsInEffect = KeyboardService.IsKeyDownAsync(KeyCode);
            OnKeyPressed();
        }

        public override void SynchroniseKeyState()
        {
            IsInEffect = KeyboardService.IsKeyDownAsync(KeyCode);
        }
    }
}