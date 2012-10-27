namespace Polaris.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents the arguments passed when a Kinect UI event is fired.
    /// </summary>
    public class KinectUiEventArgs : EventArgs, IKinectUiEventArgs
    {
        #region IKinectUiEventArgs Members

        public double? NormalizedX { get; set; }

        public double? NormalizedY { get; set; }

        public bool IsHandled { get; set; }

        #endregion IKinectUiEventArgs Members

        public static KinectUiEventArgs FromContract(IKinectUiEventArgs target)
        {
            return new KinectUiEventArgs()
            {
                NormalizedX = target.NormalizedX,
                NormalizedY = target.NormalizedY,
                IsHandled = target.IsHandled,
            };
        }
    }
}