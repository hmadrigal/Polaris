using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polaris.PhoneLib.Toolkit.Actions
{
    public interface IActionableWebBrowser
    {
        Stack<Uri> WebNavigationStack { get; }
        bool GoBack();
    }

    public enum WebBrowserNavigationStatus
    {
        Default,
        Started,
        Navigating,
        Completed,
        Failed
    }
}
