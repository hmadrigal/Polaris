namespace Polaris
{
    using System;

    /// <summary>
    /// Represents a visual cursor controlled by a Kinect joint.
    /// </summary>
    public interface IKinectCursor
    {
        /// <summary>
        /// Sets the position of the cursor on the screen
        /// according to normalized X and Y coordinates.
        /// </summary>
        /// <param name="normalizedX">
        /// A number from 0.0 to 1.0 that represents the
        /// normalized position of the cursor on the X-axis
        /// of the screen.
        /// </param>
        /// <param name="normalizedY">
        /// A number from 0.0 to 1.0 that represents the
        /// normalized position of the cursor on the Y-axis
        /// of the screen.
        /// </param>
        void SetPosition(double normalizedX, double normalizedY);

        /// <summary>
        /// Sets the current progress of the activation countdown.
        /// </summary>
        /// <param name="normalizedActivationTime">
        /// A number between 0.0 and 1.0 which represents the progress in
        /// the activation countdown.
        /// </param>
        /// <remarks>
        /// The activation progress is not guaranteed to start with value 0.0,
        /// however, it is guaranteed to end with value 1.0 so that the cursor
        /// has the chance to signal the user that activation has occurred.
        /// If the activation is interrupted in the middle of the countdown,
        /// the StopActivationCountdown method will be invoked.
        /// </remarks>
        void SetActivationCountdownProgress(double normalizedActivationTime);

        /// <summary>
        /// Stops the countdown to activation before it happens.
        /// </summary>
        /// <remarks>
        /// On this method the cursor should quickly revert
        /// the countdown animation, to signal the user that
        /// activation won't happen anymore.
        /// </remarks>
        void StopActivationCountdown();
    }
}