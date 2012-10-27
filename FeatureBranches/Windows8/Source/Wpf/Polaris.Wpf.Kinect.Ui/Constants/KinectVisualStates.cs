using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Windows
{
    internal static class KinectVisualStates
    {
        public const String HandOverState = "HandOver";
        public const String HandLeaveState = "HandLeave";
        public const String ActivatedState = "Activated";

        #region Cursor States

        public const String NormalState = "Normal";
        public const String CursorActivationState = "Activation";
        public const String ActivationCanceledState = "ActivationCanceled";
        public const String ActivationCompletedState = "ActivationCompleted";

        #endregion Cursor States
    }
}