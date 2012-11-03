using WindowsInput;

namespace Polaris.Windows.Controls
{
    public abstract class ModifierKeyBase : VirtualKey
    {
        public bool IsInEffect { get; set; }

        public abstract void SynchroniseKeyState();
    }
}