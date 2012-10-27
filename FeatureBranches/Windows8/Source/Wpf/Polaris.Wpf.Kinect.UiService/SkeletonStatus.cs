using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Services
{
    using System;
    using Microsoft.Kinect;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class SkeletonStatus
    {
        float rightHandXOffset = 0.2f;
        float rightHandYOffset = -0.3f;

        float leftHandXOffset = -0.5f;
        float leftHandYOffset = -0.3f;

        private int updateCount;

        public int TrackingId { get; set; }

        public bool IsActiveAreaReady { get; set; }

        public float MaxHandDeltaX { get; private set; }

        public float ActiveAreaWidth { get; private set; }

        public float ActiveAreaHeight { get; private set; }

        public double? NormalizedCursorX { get; private set; }

        public double? NormalizedCursorY { get; private set; }

        public double? CursorX { get; private set; }

        public double? CursorY { get; private set; }

        public double? ReferenceWidth { get; set; }

        public double? ReferenceHeight { get; set; }

        public void UpdateMaxHandDeltaX(Skeleton skeletonInfo)
        {
            var referenceJoint = skeletonInfo.Joints[JointType.ShoulderCenter];

            var rightHandJoint = skeletonInfo.Joints[JointType.HandRight];
            var leftHandJoint = skeletonInfo.Joints[JointType.HandLeft];

            var rightHandYDelta = referenceJoint.Position.Y - rightHandJoint.Position.Y;
            var leftHandYDelta = referenceJoint.Position.Y - leftHandJoint.Position.Y;

            var isRightHandActive = rightHandYDelta < leftHandYDelta;

            var greetingReferenceJoint = isRightHandActive ? skeletonInfo.Joints[JointType.ElbowRight] : skeletonInfo.Joints[JointType.ElbowLeft];
            var handJoint = isRightHandActive ? rightHandJoint : leftHandJoint;

            var isGreeting = greetingReferenceJoint.Position.Y < handJoint.Position.Y;

            var handReferencePointX = referenceJoint.Position.X + (isRightHandActive ? rightHandXOffset : leftHandXOffset);
            var handReferencePointY = referenceJoint.Position.Y + (isRightHandActive ? rightHandYOffset : leftHandYOffset);

            var updatedHandXDelta = Math.Abs(handReferencePointX - handJoint.Position.X);

            var newHandMaxXDelta = Math.Max(MaxHandDeltaX, updatedHandXDelta);

            if (!IsActiveAreaReady)
            {
                if (newHandMaxXDelta != MaxHandDeltaX)
                {
                    MaxHandDeltaX = newHandMaxXDelta;
                    updateCount = 0;
                }
                else if (!isGreeting)
                {
                    updateCount = 0;
                }
                else
                {
                    updateCount++;
                }
                if (updateCount >= 3)
                {
                    ActiveAreaWidth = MaxHandDeltaX * 0.6f;
                    ActiveAreaHeight = MaxHandDeltaX * 0.6f * (9.0f / 16.0f);
                    IsActiveAreaReady = true;
                }
            }

            if (IsActiveAreaReady)
            {
                NormalizedCursorX = (((handJoint.Position.X -
                                    (handReferencePointX - (isRightHandActive ? 0 : ActiveAreaWidth)))
                                / ActiveAreaWidth));
                NormalizedCursorY = -(((((handJoint.Position.Y - handReferencePointY)) / ActiveAreaHeight)));
                CursorX = NormalizedCursorX * ReferenceWidth;
                CursorY = NormalizedCursorY * ReferenceHeight;
            }
        }
    }
}