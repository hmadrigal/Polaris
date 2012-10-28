using System;
namespace Polaris.Windows.Services
{
    public interface IVirtualKeyboardService
    {
        bool Alt { get; set; }
        bool CapsLock { get; set; }
        bool Ctrl { get; set; }
        bool LeftAlt { get; set; }
        bool LeftShift { get; set; }
        void PressAndHold(KeysEx keyCode);
        void PressAndRelease(KeysEx keyCode);
        void PressKey(KeysEx keyCode);
        void ReleaseKey(KeysEx keyCode);
        void ReleaseStickyKeys();
        bool RightAlt { get; set; }
        bool RightShift { get; set; }
        void SendKey(KeysEx keyCode);
        bool Shift { get; set; }
        bool Win { get; set; }
    }
}
