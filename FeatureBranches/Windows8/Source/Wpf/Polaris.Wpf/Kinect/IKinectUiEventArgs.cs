using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    public interface IKinectUiEventArgs
    {
        double? NormalizedX { get; set; }

        double? NormalizedY { get; set; }

        bool IsHandled { get; set; }
    }
}