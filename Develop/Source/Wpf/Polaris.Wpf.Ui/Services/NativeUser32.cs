//-----------------------------------------------------------------------
// <copyright file="NativeUser32.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Services
{
    using System.Runtime.InteropServices;

    internal struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public long time;
        public uint dwExtraInfo;
    };

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    internal struct INPUT
    {
        [FieldOffset(0)]
        public uint type;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct InputKeys
    {
        public uint type;
        public uint wVk;
        public uint wScan;
        public uint dwFlags;
        public uint time;
        public uint dwExtra;
    }

    /// <summary>
    /// Class wrapper for imported DLL functions
    /// </summary>
    internal static class NativeWin32
    {
        public const uint INPUT_KEYBOARD = 1;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("User32.dll")]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("User32.dll", EntryPoint = "SendInput")]
        public static extern uint SendInput(uint nInputs, InputKeys[] inputs, int cbSize);

        // Imported from http://stackoverflow.com/questions/577411/how-can-i-find-the-state-of-numlock-capslock-and-scrolllock-in-net
        [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
    }
}
