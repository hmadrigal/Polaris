//-----------------------------------------------------------------------
// <copyright file="ScrollOnTouchAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using Polaris.Windows.Extensions;

    [TypeConstraint(typeof(ScrollViewer))]
    public class ScrollOnTouchAction : TargetedTriggerAction<UIElement>
    {
        bool isScrolling = false;

        TimeSpan InitialDelayTime = new TimeSpan(0, 0, 0, 0, 0);

        Int32 InertiaTriggerArea = 20;

        double InitialScrollingTriggerThreshold = 0.0;

        private Point InitialPosition { get; set; }

        private DateTime InitialTime { get; set; }

        private Double InitialOffset { get; set; }

        private Point PreviousPosition { get; set; }

        private Double CurrentForce { get; set; }

        private Double CurrentVelocity { get; set; }

        private Double LastAcceleration { get; set; }

        private ForceDirection CurrentForceDirection { get; set; }

        private DateTime LastUpdate { get; set; }

        private ScrollViewer AssociatedScrollViewer
        {
            get
            {
                if (associatedScrollViewer == null)
                {
                    associatedScrollViewer = AssociatedObject as ScrollViewer;
                }
                return associatedScrollViewer;
            }
        }

        private ScrollViewer associatedScrollViewer;

        private object inertiaSync = new object();

        #region ScrollResolution

        /// <summary>
        /// ScrollResolution Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollResolutionProperty =
            DependencyProperty.Register("ScrollResolution", typeof(Double), typeof(ScrollOnTouchAction),
                new FrameworkPropertyMetadata(1.0));

        /// <summary>
        /// Gets or sets the ScrollResolution property.  This dependency property
        /// indicates the scrolling magnitude associated with each mouse movement.
        /// </summary>
        public Double ScrollResolution
        {
            get { return (Double)GetValue(ScrollResolutionProperty); }
            set { SetValue(ScrollResolutionProperty, value); }
        }

        #endregion ScrollResolution

        #region ScrollOrientation

        /// <summary>
        /// ScrollOrientation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollOrientationProperty =
            DependencyProperty.Register("ScrollOrientation", typeof(Orientation), typeof(ScrollOnTouchAction),
                new FrameworkPropertyMetadata((Orientation.Vertical)));

        /// <summary>
        /// Gets or sets the ScrollOrientation property.  This dependency property
        /// indicates the orientation that will be affected by the mouse movement.
        /// </summary>
        public Orientation ScrollOrientation
        {
            get { return (Orientation)GetValue(ScrollOrientationProperty); }
            set { SetValue(ScrollOrientationProperty, value); }
        }

        #endregion ScrollOrientation

        #region BodyMass

        /// <summary>
        /// BodyMass Dependency Property
        /// </summary>
        public static readonly DependencyProperty BodyMassProperty =
            DependencyProperty.Register("BodyMass", typeof(Double), typeof(ScrollOnTouchAction),
                new FrameworkPropertyMetadata(0.15));

        /// <summary>
        /// Gets or sets the BodyMass property.  This dependency property
        /// indicates the theoretical mass of the scrolling object.
        /// </summary>
        public Double BodyMass
        {
            get { return (Double)GetValue(BodyMassProperty); }
            set { SetValue(BodyMassProperty, value); }
        }

        #endregion BodyMass

        #region Friction

        /// <summary>
        /// Friction Dependency Property
        /// </summary>
        public static readonly DependencyProperty FrictionProperty =
            DependencyProperty.Register("Friction", typeof(Double), typeof(ScrollOnTouchAction),
                new FrameworkPropertyMetadata(0.001));

        /// <summary>
        /// Gets or sets the Friction property.  This dependency property
        /// indicates the friction that slows down inertia over time.
        /// </summary>
        public Double Friction
        {
            get { return (Double)GetValue(FrictionProperty); }
            set { SetValue(FrictionProperty, value); }
        }

        #endregion Friction

        #region InputType

        /// <summary>
        /// InputType Dependency Property
        /// </summary>
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType", typeof(InputType), typeof(ScrollOnTouchAction),
                new FrameworkPropertyMetadata((InputType)InputType.Mouse));

        /// <summary>
        /// Gets or sets the InputType property.  This dependency property 
        /// indicates whether to use mouse or touch events (or both) as input for this behavior.
        /// </summary>
        public InputType InputType
        {
            get { return (InputType)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        #endregion


        public ScrollOnTouchAction()
        {
            //CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            var renderingEventListener = new WeakEventListener<ScrollOnTouchAction, object, EventArgs>(this);
            renderingEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.CompositionTarget_Rendering(source, eventArgs);
            renderingEventListener.OnDetachAction = (weakEventListenerParameter) =>
                CompositionTarget.Rendering -= weakEventListenerParameter.OnEvent;
            CompositionTarget.Rendering += renderingEventListener.OnEvent;
        }

        protected override void Invoke(object parameter)
        {
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            lock (inertiaSync)
            {
                if (CurrentForce <= 0)
                {
                    return;
                }
                var currentTime = DateTime.Now;
                var elapsedTime = currentTime - LastUpdate;

                CurrentVelocity = CurrentForce / BodyMass;

                var dueDisplacement = CurrentVelocity * (Double)elapsedTime.TotalMilliseconds;

                if (CurrentForceDirection == ForceDirection.Down)
                {
                    dueDisplacement *= -1;
                }

                var newOffset = (ScrollOrientation == Orientation.Vertical ?
                    AssociatedScrollViewer.VerticalOffset :
                    AssociatedScrollViewer.HorizontalOffset) + dueDisplacement;

                if (Double.IsNaN(newOffset))
                {
                    return;
                }

                if (ScrollOrientation == Orientation.Vertical)
                {
                    AssociatedScrollViewer.ScrollToVerticalOffset(newOffset);
                }
                else
                {
                    AssociatedScrollViewer.ScrollToHorizontalOffset(newOffset);
                }

                CurrentForce = Math.Max(0, CurrentForce - ((Double)elapsedTime.TotalMilliseconds * CurrentForce * Friction));

                if (CurrentForce < 0.0001)
                {
                    CurrentForce = 0;
                }
                LastUpdate = DateTime.Now;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.Target != null)
            {
                this.Target.TouchDown -= new EventHandler<TouchEventArgs>(Target_TouchDown);
                this.Target.TouchMove -= new EventHandler<TouchEventArgs>(Target_TouchMoved);
                this.Target.TouchUp -= new EventHandler<TouchEventArgs>(Target_TouchUp);
                this.Target.TouchEnter -= new EventHandler<TouchEventArgs>(Target_TouchEnter);
                this.Target.TouchLeave -= new EventHandler<TouchEventArgs>(Target_TouchLeave);
            }
        }

        protected override void OnTargetChanged(UIElement oldTarget, UIElement newTarget)
        {
            if (oldTarget != null)
            {
                if (InputType == Actions.InputType.Touch || InputType == Actions.InputType.Both)
                {
                    oldTarget.TouchDown -= new EventHandler<TouchEventArgs>(Target_TouchDown);
                    oldTarget.TouchMove -= new EventHandler<TouchEventArgs>(Target_TouchMoved);
                    oldTarget.TouchUp -= new EventHandler<TouchEventArgs>(Target_TouchUp);
                    oldTarget.TouchEnter -= new EventHandler<TouchEventArgs>(Target_TouchEnter);
                    oldTarget.TouchLeave -= new EventHandler<TouchEventArgs>(Target_TouchLeave);
                }
                if (InputType == Actions.InputType.Mouse || InputType == Actions.InputType.Both)
                {
                    oldTarget.MouseDown -= new MouseButtonEventHandler(Target_MouseDown);
                    oldTarget.MouseMove -= new MouseEventHandler(Target_MouseMove);
                    oldTarget.MouseUp -= new MouseButtonEventHandler(Target_MouseUp);
                }
            }
            if (newTarget != null)
            {
                if (InputType == Actions.InputType.Touch || InputType == Actions.InputType.Both)
                {
                    newTarget.TouchDown += new EventHandler<TouchEventArgs>(Target_TouchDown);
                    newTarget.TouchMove += new EventHandler<TouchEventArgs>(Target_TouchMoved);
                    newTarget.TouchUp += new EventHandler<TouchEventArgs>(Target_TouchUp);
                    newTarget.TouchEnter += new EventHandler<TouchEventArgs>(Target_TouchEnter);
                    newTarget.TouchLeave += new EventHandler<TouchEventArgs>(Target_TouchLeave);
                }
                if (InputType == Actions.InputType.Mouse || InputType == Actions.InputType.Both)
                {
                    newTarget.MouseDown += new MouseButtonEventHandler(Target_MouseDown);
                    newTarget.MouseMove += new MouseEventHandler(Target_MouseMove);
                    newTarget.MouseUp += new MouseButtonEventHandler(Target_MouseUp);
                }
            }
            base.OnTargetChanged(oldTarget, newTarget);
        }

        private void Target_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isScrolling) { return; }
            isScrolling = true;
            Target.CaptureMouse();
            var position = e.GetPosition((IInputElement)sender);
            BeginInteraction(position);
        }

        private void Target_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isScrolling) { return; }
            if (AssociatedScrollViewer == null) { return; }

            var currentPosition = e.GetPosition((IInputElement)sender);

            OnTouchMoved(currentPosition);
        }

        private void Target_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!isScrolling) { return; }
            isScrolling = false;
            Target.ReleaseMouseCapture();
            var releasePosition = e.GetPosition((IInputElement)sender);
            if (releasePosition == null) { return; }

            var currentPosition = releasePosition;

            var delta = currentPosition - InitialPosition;

            if (delta.Length <= InertiaTriggerArea) { return; }

            CalculateForce();
        }

        private void Target_TouchLeave(object sender, TouchEventArgs e)
        {
            if (!isScrolling) { return; }
            isScrolling = false;
            var currentPosition = e.GetTouchPoint((IInputElement)sender).Position;

            var delta = currentPosition - PreviousPosition;

            if (delta.Length <= InertiaTriggerArea) { return; }

            CalculateForce();
        }

        private void Target_TouchEnter(object sender, TouchEventArgs e)
        {
            if (isScrolling) { return; }
            isScrolling = true;
            var position = e.GetTouchPoint((IInputElement)sender).Position;
            BeginInteraction(position);
        }

        private void Target_TouchUp(object sender, TouchEventArgs e)
        {
            if (!isScrolling) { return; }
            isScrolling = false;
            if (e == null) { return; }
            var releasePosition = e.GetTouchPoint((IInputElement)sender).Position;
            if (releasePosition == null) { return; }

            var currentPosition = releasePosition;

            var delta = currentPosition - PreviousPosition;

            if (delta.Length <= InertiaTriggerArea) { return; }

            CalculateForce();
        }

        private void Target_TouchMoved(object sender, TouchEventArgs e)
        {
            if (!isScrolling) { return; }
            if (AssociatedScrollViewer == null) { return; }

            var currentPosition = e.GetTouchPoint((IInputElement)sender).Position;

            OnTouchMoved(currentPosition);
        }

        private void OnTouchMoved(Point currentPosition)
        {
            var elapsedTime = DateTime.Now - InitialTime;

            if (elapsedTime <= InitialDelayTime) { return; }

            //var deltaFromInitialPosition = currentPosition - InitialPosition;

            //if (deltaFromInitialPosition.Length < InitialScrollingTriggerThreshold)
            //{
            //    return;
            //}

            var delta = currentPosition - PreviousPosition;

            var magnitude = delta.Length * ScrollResolution;

            if (ScrollOrientation == Orientation.Vertical)
            {
                if (currentPosition.Y < PreviousPosition.Y)
                {
                    magnitude *= -1;
                }
            }
            else
            {
                if (currentPosition.X < PreviousPosition.X)
                {
                    magnitude *= -1;
                }
            }

            var newOffset = (ScrollOrientation == Orientation.Vertical ?
                AssociatedScrollViewer.VerticalOffset :
                AssociatedScrollViewer.HorizontalOffset) + magnitude;

            if (ScrollOrientation == Orientation.Vertical)
            {
                AssociatedScrollViewer.ScrollToVerticalOffset(newOffset);
            }
            else
            {
                AssociatedScrollViewer.ScrollToHorizontalOffset(newOffset);
            }

            PreviousPosition = currentPosition;
        }

        private void Target_TouchDown(object sender, TouchEventArgs e)
        {
            if (isScrolling) { return; }
            isScrolling = true;
            var position = e.GetTouchPoint((IInputElement)sender).Position;
            BeginInteraction(position);
        }

        private void BeginInteraction(Point position)
        {
            lock (inertiaSync)
            {
                CurrentForce = 0.0;
            }
            if (AssociatedScrollViewer == null) { return; }
            InitialTime = DateTime.Now;
            InitialPosition = position;
            PreviousPosition = InitialPosition;
            InitialOffset = (ScrollOrientation == Orientation.Vertical ?
                AssociatedScrollViewer.VerticalOffset :
                AssociatedScrollViewer.HorizontalOffset);
        }

        private void CalculateForce()
        {
            lock (inertiaSync)
            {
                if (AssociatedScrollViewer == null) { return; }
                var finalTime = DateTime.Now;
                var elapsedTime = finalTime - InitialTime;
                var finalOffset = (ScrollOrientation == Orientation.Vertical ?
                        AssociatedScrollViewer.VerticalOffset :
                        AssociatedScrollViewer.HorizontalOffset);
                var totalDisplacement = Math.Abs(finalOffset - InitialOffset);

                var velocity = totalDisplacement / (Double)elapsedTime.TotalMilliseconds;

                LastAcceleration = velocity / (Double)elapsedTime.TotalMilliseconds;

                CurrentForce = (BodyMass * velocity);

                //System.Diagnostics.Debug.WriteLine(velocity);

                if (finalOffset < InitialOffset)
                {
                    CurrentForceDirection = ForceDirection.Down;
                }
                else
                {
                    CurrentForceDirection = ForceDirection.Up;
                }

                LastUpdate = DateTime.Now;
            }
        }
    }

    public enum InputType
    {
        Mouse,
        Touch,
        Both,
    }
}