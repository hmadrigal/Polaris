using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaris.PhoneLib.Mvvm
{
    public interface IActionableWebBrowser
    {
        Stack<Uri> WebNavigationStack { get; }
        bool GoBack();
    }
}
