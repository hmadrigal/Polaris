namespace Polaris.Kinect
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.Practices.Unity;
    using Polaris.Services;

    /// <summary>
    /// Service base for kinect movement detection service.
    /// When inherited this service should notify the registered controls with kinect input.
    /// </summary>
    public abstract class KinectUiServiceBase : IKinectUiService
    {
        #region Fields

        /// <summary>
        /// Synchronization object to guarantee thread safe operations over the cursorDictionary Dictionary.
        /// </summary>
        object cursorDictionarySync = new object();

        /// <summary>
        /// Dictionary with the registered cursors.
        /// </summary>
        Dictionary<int, IKinectCursor> cursorDictionary = new Dictionary<int, IKinectCursor>();

        /// <summary>
        /// Synchronization object to guarantee thread safe operations over the controlDictionary Dictionary.
        /// </summary>
        object controlDictionarySync = new object();

        /// <summary>
        /// Dictionary with the registered controls.
        /// </summary>
        Dictionary<int, IKinectUiControl> controlDictionary = new Dictionary<int, IKinectUiControl>();

        /// <summary>
        /// Instance of the current Active UI Element, based on the Kinect Input.
        /// </summary>
        IKinectUiControl currentActiveControl;

        ///// <summary>
        ///// Synchronization object to guarantee thread safe operations over the gestureControlDictionary Dictionary
        ///// </summary>
        //object gestureControlDictionarySync = new object();

        ///// <summary>
        ///// Dictionary with the registered GestureControls.
        ///// </summary>
        //Dictionary<int, IKinectGestureControl> gestureControlDictionary = new Dictionary<int, IKinectGestureControl>();

        /// <summary>
        /// Synchronization object to guarantee thread safe operations over the movementListenerList
        /// </summary>
        object movementListenerSync = new object();

        /// <summary>
        /// List with the movement listeners.
        /// </summary>
        List<IMovementListener> movementListenerList = new List<IMovementListener>();

        /// <summary>
        /// Synchronization object to guarantee thread safe operations over the isCursorCaptured field.
        /// </summary>
        object isCursorCapturedSync = new object();

        /// <summary>
        /// Boolean that indicates whether the kinect cursor is captured by a UIElement.
        /// </summary>
        private bool isCursorCaptured;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Unity container
        /// </summary>
        public IUnityContainer Container { get; set; }

        /// <summary>
        /// UI Dispatcher used to perform operations on the UIThread.
        /// </summary>
        public Dispatcher UiDispatcher { get; set; }

        /// <summary>
        /// DateTime when the KinectCursor entered the currentActiveControl.
        /// </summary>
        public DateTime ActiveControlEnterTime { get; set; }

        /// <summary>
        /// Boolean that indicates whether the service already call the TriggerActivation of the active control.
        /// </summary>
        public bool IsControlActivated { get; set; }

        #endregion Properties

        #region Constructor

        public KinectUiServiceBase(IUnityContainer container)
        {
            this.Container = container;
            if (container != null)
            {
                this.UiDispatcher = container.Resolve<Dispatcher>(GlobalInstanceNames.UiDispatcher);
            }
            else
            {
                this.UiDispatcher = Application.Current.MainWindow.Dispatcher;
            }
        }

        #endregion Constructor

        #region IKinectUiService Members

        public void RegisterCursor(IKinectCursor cursor)
        {
            lock (cursorDictionarySync)
            {
                cursorDictionary[cursor.GetHashCode()] = cursor;
            }
        }

        public void UnregisterCursor(IKinectCursor cursor)
        {
            lock (cursorDictionarySync)
            {
                if (cursorDictionary.ContainsKey(cursor.GetHashCode()))
                {
                    cursorDictionary.Remove(cursor.GetHashCode());
                }
            }
        }

        public void RegisterControl(IKinectUiControl control)
        {
            lock (controlDictionarySync)
            {
                controlDictionary[control.GetHashCode()] = control;
            }
        }

        public void UnregisterControl(IKinectUiControl control)
        {
            lock (controlDictionarySync)
            {
                controlDictionary.Remove(control.GetHashCode());
            }
        }

        //public void RegisterGestureControl(IKinectGestureControl control)
        //{
        //    lock (gestureControlDictionarySync)
        //    {
        //        gestureControlDictionary[control.GetHashCode()] = control;
        //    }
        //}

        //public void UnregisterGestureControl(IKinectGestureControl control)
        //{
        //    lock (gestureControlDictionarySync)
        //    {
        //        gestureControlDictionary.Remove(control.GetHashCode());
        //    }
        //}

        public abstract void Start();

        public abstract void Stop();

        ///// <summary>
        ///// Notifies all the GestureControl elements that a gesture has been detected.
        ///// </summary>
        ///// <param name="gesture">Detected gesture</param>
        //public void TriggerGestureDetected(IGesture gesture)
        //{
        //    UiDispatcher.BeginInvoke(new Action<IGesture>((target) =>
        //    {
        //        IKinectGestureControl[] gestureControlArray;
        //        lock (gestureControlDictionarySync)
        //        {
        //            gestureControlArray = this.gestureControlDictionary.Values.ToArray();
        //        }
        //        foreach (var item in gestureControlArray)
        //        {
        //            item.TriggerGestureDetected(target);
        //        }
        //    }), gesture);
        //}

        /// <summary>
        ///
        /// </summary>
        /// <param name="movementListener"></param>
        public void AddMovementListener(IMovementListener movementListener)
        {
            lock (movementListenerSync)
            {
                movementListenerList.Add(movementListener);
            }
        }

        public void RemoveMovementListener(IMovementListener movementListener)
        {
            lock (movementListenerSync)
            {
                movementListenerList.Remove(movementListener);
            }
        }

        /// <summary>
        /// Captures the cursor, causing the UiService to notify the Kinect movement only to the KinectUiControl that is currently active, regardless of the hit testing process.
        /// </summary>
        /// <param name="element">Element that will capture the cursor.</param>
        public void CaptureCursor(IKinectUiControl element = null)
        {
            lock (isCursorCapturedSync)
            {
                currentActiveControl = element;
                isCursorCaptured = true;
            }
        }

        /// <summary>
        /// Releases the cursor.
        /// </summary>
        public void ReleaseCursorCapture()
        {
            lock (isCursorCapturedSync)
            {
                isCursorCaptured = false;
            }
        }

        #endregion IKinectUiService Members

        /// <summary>
        /// Notifies the user interface registered controls based on the current state and the new position of the cursor position.
        /// </summary>
        /// <param name="normalizedX">New X position of the kinect cursor</param>
        /// <param name="normalizedY">New Y position of the kinect cursor</param>
        /// <param name="hasCursorPositionChanged">Boolean that indicates whether the normalizedX and normalizedY values are identical to the previous values</param>
        protected void UpdateControls(double normalizedX, double normalizedY, bool hasCursorPositionChanged)
        {
            UiDispatcher.BeginInvoke(new Action(() =>
            {
                var newActiveControl = GetActiveControl(normalizedX, normalizedY);

                if (newActiveControl == currentActiveControl)
                {
                    if (newActiveControl != null && hasCursorPositionChanged)
                    {
                        newActiveControl.TriggerCursorMove(new KinectUiEventArgs() { NormalizedX = normalizedX, NormalizedY = normalizedY });
                    }
                    if (newActiveControl != null && newActiveControl.IsActivationEnabled)
                    {
                        var elapsed = DateTime.Now - this.ActiveControlEnterTime;
                        var normalizedTime = Math.Min(1.0, elapsed.TotalMilliseconds / newActiveControl.ActivationTime.Value);
                        if (!IsControlActivated)
                        {
                            this.UpdateCursorsActivationProgress(normalizedTime);
                        }
                        if (normalizedTime == 1.0 && !IsControlActivated)
                        {
                            newActiveControl.TriggerActivation(new KinectUiEventArgs() { NormalizedX = normalizedX, NormalizedY = normalizedY });
                            IsControlActivated = true;
                        }
                    }
                }
                else
                {
                    if (newActiveControl != null)
                    {
                        newActiveControl.TriggerCursorEnter(new KinectUiEventArgs() { NormalizedX = normalizedX, NormalizedY = normalizedY });
                        this.ActiveControlEnterTime = DateTime.Now;
                    }

                    if (currentActiveControl != null)
                    {
                        currentActiveControl.TriggerCursorLeave(new KinectUiEventArgs() { NormalizedX = normalizedX, NormalizedY = normalizedY });
                        this.StopCursorsActivationCountdown();
                    }
                    IsControlActivated = false;
                    this.UpdateCursorsActivationProgress(0.0);
                }
                currentActiveControl = newActiveControl;
            }));
        }

        /// <summary>
        /// Notifies the movement listeners the new position of the kinect cursor.
        /// </summary>
        /// <param name="normalizedX">New X position of the kinect cursor</param>
        /// <param name="normalizedY">New Y position of the kinect cursor</param>
        protected void NotifyMovementListeners(double normalizedX, double normalizedY)
        {
            IMovementListener[] movementListenersArray = null;
            lock (this.movementListenerSync)
            {
                movementListenersArray = movementListenerList.ToArray();
            }
            foreach (var item in movementListenersArray)
            {
                item.AddCursorEntry(normalizedX, normalizedY);
            }
        }

        /// <summary>
        /// When a KinectUiControl is activating, updates the cursors with the updated countdown according to the activeControl activation time.
        /// </summary>
        /// <param name="normalizedTime">Value from 0 to 1 that represents the progress of the activation process.</param>
        private void UpdateCursorsActivationProgress(double normalizedTime)
        {
            UiDispatcher.BeginInvoke(new Action(() =>
            {
                IKinectCursor[] cursorArray;
                lock (cursorDictionarySync)
                {
                    cursorArray = cursorDictionary.Values.ToArray();
                }
                foreach (var cursor in cursorArray)
                {
                    cursor.SetActivationCountdownProgress(normalizedTime);
                }
            }));
        }

        /// <summary>
        /// Notifies the cursors that the activation process has been stopped.
        /// </summary>
        private void StopCursorsActivationCountdown()
        {
            UiDispatcher.BeginInvoke(new Action(() =>
            {
                IKinectCursor[] cursorArray;
                lock (cursorDictionarySync)
                {
                    cursorArray = cursorDictionary.Values.ToArray();
                }
                foreach (var cursor in cursorArray)
                {
                    cursor.StopActivationCountdown();
                }
            }));
        }

        /// <summary>
        /// Updates the cursors with the updated position.
        /// </summary>
        /// <param name="normalizedX">New X position of the kinect cursor</param>
        /// <param name="normalizedY">New Y position of the kinect cursor</param>
        /// <param name="hasCursorPositionChanged">Boolean that indicates whether the normalizedX and normalizedY values are identical to the previous values</param>
        protected void UpdateCursors(double normalizedX, double normalizedY, bool hasCursorPositionChanged)
        {
            if (!hasCursorPositionChanged) { return; }

            UiDispatcher.BeginInvoke(new Action(() =>
            {
                IKinectCursor[] cursorArray;
                lock (cursorDictionarySync)
                {
                    cursorArray = cursorDictionary.Values.ToArray();
                }
                foreach (var cursor in cursorArray)
                {
                    cursor.SetPosition(normalizedX, normalizedY);
                }
            }));
        }

        /// <summary>
        /// Obtains the KinectUiControl that should handle the Kinect movement according to the current position and state.
        /// </summary>
        /// <param name="normalizedX">New X position of the kinect cursor</param>
        /// <param name="normalizedY">New Y position of the kinect cursor</param>
        /// <returns>The KinectUiControl that should handle the kinect movement</returns>
        private IKinectUiControl GetActiveControl(double normalizedX, double normalizedY)
        {
            if (isCursorCaptured)
            {
                return this.currentActiveControl;
            }

            IKinectUiControl[] controlArray;
            lock (controlDictionarySync)
            {
                controlArray = controlDictionary.Values.ToArray();
            }

            var activeControl = (from control in controlArray
                                 let kinectUiElementController = control.GetKinectUiElementController()
                                 where kinectUiElementController.Visible
                                 where kinectUiElementController.IsKinectVisible
                                 where kinectUiElementController.ContainsPoint(normalizedX, normalizedY)
                                 orderby kinectUiElementController.ZIndex descending
                                 select control).FirstOrDefault();
            return activeControl;
        }
    }
}