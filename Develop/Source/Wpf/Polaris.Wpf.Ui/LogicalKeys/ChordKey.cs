using System.Collections.Generic;
using WindowsInput;

namespace Polaris.Windows.Controls
{
    public class ChordKey : LogicalKeyBase
    {
        public VirtualKeyCodeList ModifierKeys { get; set; }
        public VirtualKeyCodeList Keys { get; set; }

        //public ChordKey(string displayName, VirtualKeyCode modifierKey, VirtualKeyCode key)
        //    : this(displayName, new List<VirtualKeyCode> { modifierKey }, new List<VirtualKeyCode> { key })
        //{
        //}

        //public ChordKey(string displayName, IList<VirtualKeyCode> modifierKeys, VirtualKeyCode key)
        //    : this(displayName, modifierKeys, new List<VirtualKeyCode> { key })
        //{
        //}

        //public ChordKey(string displayName, VirtualKeyCode modifierKey, IList<VirtualKeyCode> keys)
        //    : this(displayName, new List<VirtualKeyCode> { modifierKey }, keys)
        //{
        //}

        //public ChordKey(string displayName, IList<VirtualKeyCode> modifierKeys, IList<VirtualKeyCode> keys)
        //{
        //    DisplayName = displayName;
        //    ModifierKeys = modifierKeys;
        //    Keys = keys;
        //}

        public override void Press()
        {
            KeyboardService.SimulateModifiedKeyStroke(ModifierKeys, Keys);
            base.Press();
        }
    }
}