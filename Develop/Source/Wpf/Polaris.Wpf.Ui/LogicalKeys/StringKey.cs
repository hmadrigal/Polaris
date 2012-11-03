using System;
using System.Linq;
using WindowsInput;

namespace Polaris.Windows.Controls
{
    public class StringKey : LogicalKeyBase
    {
        public virtual string StringToSimulate { get; set; }

        public override void Press()
        {
            KeyboardService.SimulateTextEntry(StringToSimulate);
            base.Press();
        }
    }
}
