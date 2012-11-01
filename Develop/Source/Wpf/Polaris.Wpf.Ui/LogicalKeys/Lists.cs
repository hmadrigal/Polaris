using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using System.Windows;

namespace Polaris.Windows.Controls
{
    public class VirtualKeyCodeList : List<VirtualKeyCode>, IList<VirtualKeyCode>
    { }
    public class StringList : List<object>, IList<object>
    { }
}
