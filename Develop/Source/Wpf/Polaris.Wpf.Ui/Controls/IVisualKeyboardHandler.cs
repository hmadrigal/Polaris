using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Windows.Controls
{
    public interface IVisualKeyboardHandler
    {
        void RegisterKey(System.Windows.Controls.ContentControl sender);
        //void DeregisterKey(System.Windows.Controls.ContentControl sender);
        void HandleKeyDown(System.Windows.Controls.ContentControl sender, DependencyLogicalKey virtualKeyConfig, Dictionary<System.Windows.Controls.ContentControl, DependencyLogicalKey> virtualKeys);
    }
}
