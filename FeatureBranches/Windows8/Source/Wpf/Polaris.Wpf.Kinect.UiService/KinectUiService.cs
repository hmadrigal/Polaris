using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Kinect;
using Microsoft.Practices.Unity;
using Polaris.Kinect;

namespace Polaris.Services
{
    /// <summary>
    /// A service that translates skeletal information
    /// coming from the Kinect sensor into user interface
    /// interactions.
    /// </summary>
    internal class KinectUiService : KinectUiServiceBase
    {
        #region Fields

        /// <summary>
        /// The kinect sensor.
        /// </summary>
        KinectSensor kinectSensor;

        /// <summary>
        /// Dictionary that contains the skeleton status instances indexed by SkeletonId
        /// </summary>
        Dictionary<int, SkeletonStatus> skeletonStatusDictionary = new Dictionary<int, SkeletonStatus>();

        #endregion Fields

        public KinectUiService(IUnityContainer container)
            : base(container)
        {
        }

        #region IKinectUiService Members

        public override void Start()
        {
            if (kinectSensor == null)
            {
                FindKinectSensorAsync();
            }
            else
            {
                kinectSensor.Start();
            }
        }

        public override void Stop()
        {
            if (kinectSensor == null) { return; }
            kinectSensor.Stop();
        }

        #endregion IKinectUiService Members

        private void FindKinectSensorAsync()
        {
            ThreadPool.QueueUserWorkItem(FindKinectSensor);
        }

        private void FindKinectSensor(object payload)
        {
            var kinectCount = KinectSensor.KinectSensors.Count;
            if (kinectCount > 0)
            {
                kinectSensor = KinectSensor.KinectSensors[0];
                kinectSensor.Start();
                kinectSensor.SkeletonFrameReady += new System.EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);
                kinectSensor.SkeletonStream.Enable();
            }
        }

        private void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null) { return; }
                var skeletonInfoArray = new Skeleton[skeletonFrame.SkeletonArrayLength];
                skeletonFrame.CopySkeletonDataTo(skeletonInfoArray);
                foreach (var skeletonInfo in skeletonInfoArray)
                {
                    SkeletonStatus skeletonStatus = GetSkeletonStatus(skeletonInfo.TrackingId);

                    switch (skeletonInfo.TrackingState)
                    {
                        case SkeletonTrackingState.NotTracked:
                            break;
                        case SkeletonTrackingState.PositionOnly:
                            break;
                        case SkeletonTrackingState.Tracked:
                            var previousCursorX = skeletonStatus.NormalizedCursorX;
                            var previousCursorY = skeletonStatus.NormalizedCursorY;
                            skeletonStatus.UpdateMaxHandDeltaX(skeletonInfo);
                            if (skeletonStatus.IsActiveAreaReady)
                            {
                                var normalizedX = skeletonStatus.NormalizedCursorX.Value;
                                var normalizedY = skeletonStatus.NormalizedCursorY.Value;

                                var hasCursorPositionChanged = !previousCursorX.HasValue
                                                                || previousCursorX.Value != normalizedX
                                                                || !previousCursorY.HasValue
                                                                || previousCursorY != normalizedY;
                                UpdateCursors(normalizedX, normalizedY, hasCursorPositionChanged);
                                UpdateControls(normalizedX, normalizedY, hasCursorPositionChanged);
                                NotifyMovementListeners(normalizedX, normalizedY);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private SkeletonStatus GetSkeletonStatus(int trackingId)
        {
            if (skeletonStatusDictionary.ContainsKey(trackingId))
            {
                return skeletonStatusDictionary[trackingId];
            }
            else
            {
                var skeletonStatus = new SkeletonStatus()
                {
                    TrackingId = trackingId,
                };
                skeletonStatusDictionary[trackingId] = skeletonStatus;
                return skeletonStatus;
            }
        }
    }
}