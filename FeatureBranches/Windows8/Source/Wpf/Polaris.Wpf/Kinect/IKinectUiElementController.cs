using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    public interface IKinectUiElementController
    {
        IKinectUiElementController Initialize(IKinectUiControl element);

        int ZIndex { get; }

        bool IsKinectVisible { get; }

        bool Visible { get; }

        bool ContainsPoint(double normalizedX, double normalizedY);

        void Detach();
    }
}