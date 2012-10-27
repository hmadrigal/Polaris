using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris
{
    /// <summary>
    /// Represents a control with which is
    /// possible to interact using a Kinect
    /// cursor.
    /// </summary>
    public interface IKinectUiControl
    {
        bool IsActivationEnabled { get; }

        Double? ActivationTime { get; }

        void TriggerCursorMove(IKinectUiEventArgs args);

        void TriggerCursorEnter(IKinectUiEventArgs args);

        void TriggerCursorLeave(IKinectUiEventArgs args);

        void TriggerActivation(IKinectUiEventArgs args);

        IKinectUiElementController GetKinectUiElementController();
    }
}