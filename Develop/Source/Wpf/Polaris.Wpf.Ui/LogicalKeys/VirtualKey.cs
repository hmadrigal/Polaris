using WindowsInput;

namespace Polaris.Windows.Controls
{
    public class VirtualKey : LogicalKeyBase
    {
        public virtual VirtualKeyCode KeyCode { get; set; }

        public override void Press()
        {
            KeyboardService.SimulateKeyPress(KeyCode);
            base.Press();
        }
    }
}