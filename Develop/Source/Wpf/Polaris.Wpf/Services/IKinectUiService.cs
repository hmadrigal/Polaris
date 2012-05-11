using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Services
{
    /// <summary>
    /// Handles and fires UI element events derived from Kinect input.
    /// </summary>
    public interface IKinectUiService
    {
        void RegisterCursor(IKinectCursor cursor);

        void UnregisterCursor(IKinectCursor cursor);

        void RegisterControl(IKinectUiControl control);

        void UnregisterControl(IKinectUiControl control);

        //void RegisterGestureControl(IKinectGestureControl control);

        //void UnregisterGestureControl(IKinectGestureControl control);

        void AddMovementListener(IMovementListener gestureDetector);

        void RemoveMovementListener(IMovementListener movementListener);

        void Start();

        void Stop();

        //void TriggerGestureDetected(IGesture gesture);

        void CaptureCursor(IKinectUiControl element = null);

        void ReleaseCursorCapture();
    }
}