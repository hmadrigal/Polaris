using System.Collections.Generic;
using WindowsInput;

namespace Polaris.Windows.Controls
{
    public class ChordKey : LogicalKeyBase
    {
        public VirtualKeyCodeList ModifierKeys { get; set; }
        public VirtualKeyCodeList Keys { get; set; }

        public override void Press()
        {
            KeyboardService.SimulateModifiedKeyStroke(ModifierKeys, Keys);
            base.Press();
        }
    }
}