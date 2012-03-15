//-----------------------------------------------------------------------
// <copyright file="ScrollCarouselOnTouchAction.cs" company="Polaris Community">
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
    using System.Windows.Media.Animation;
    using Polaris.Windows.Controls;
    using Polaris.Windows.Extensions;
    using System.ComponentModel;

    [TypeConstraint(typeof(CarouselItemsControl))]
    public class ScrollCarouselOnTouchAction : TargetedTriggerAction<UIElement>
    {
        bool isScrolling = false;
        bool isInteracting = false;

        TimeSpan InitialDelayTime = new TimeSpan(0, 0, 0, 0, 0);

        Int32 InertiaTriggerArea = 20;

        private Point InitialPosition { get; set; }

        private DateTime InitialTime { get; set; }

        private Double InitialOffset { get; set; }

        private Point PreviousPosition { get; set; }

        private Double CurrentForce { get; set; }

        private Double CurrentVelocity { get; set; }

        private Double LastAcceleration { get; set; }

        private ForceDirection CurrentForceDirection { get; set; }

        private DateTime LastUpdate { get; set; }

        private CarouselItemsControl AssociatedCarousel
        {
            get
            {
                if (associatedCarousel == null)
                {
                    associatedCarousel = AssociatedObject as CarouselItemsControl;
                }
                return associatedCarousel;
            }
        }

        private CarouselItemsControl associatedCarousel;

        private object inertiaSync = new object();

        #region ScrollResolution

        /// <summary>
        /// ScrollResolution Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollResolutionProperty =
            DependencyProperty.Register("ScrollResolution", typeof(Double), typeof(ScrollCarouselOnTouchAction),
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
            DependencyProperty.Register("ScrollOrientation", typeof(Orientation), typeof(ScrollCarouselOnTouchAction),
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
            DependencyProperty.Register("BodyMass", typeof(Double), typeof(ScrollCarouselOnTouchAction),
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
            DependencyProperty.Register("Friction", typeof(Double), typeof(ScrollCarouselOnTouchAction),
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
            DependencyProperty.Register("InputType", typeof(InputType), typeof(ScrollCarouselOnTouchAction),
                new FrameworkPropertyMetadata((InputType)InputType.Mouse));

        /// <summary>
        /// Gets or sets the InputType property.  This dependency property
        /// indicates whether to use the mouse or touch events (or both) as input.
        /// </summary>
        public InputType InputType
        {
            get { return (InputType)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }

        #endregion InputType

        #region Snap Animation Properties

        #region SnapDuration

        /// <summary>
        /// SnapDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty SnapDurationProperty =
            DependencyProperty.Register("SnapDuration", typeof(TimeSpan), typeof(ScrollCarouselOnTouchAction),
                new FrameworkPropertyMetadata((TimeSpan)new TimeSpan(0, 0, 0, 0, 800),
                    new PropertyChangedCallback(OnSnapDurationChanged)));

        /// <summary>
        /// Gets or sets the SnapDuration property.  This dependency property
        /// indicates the duration of the Snap animation.
        /// </summary>
        public TimeSpan SnapDuration
        {
            get { return (TimeSpan)GetValue(SnapDurationProperty); }
            set { SetValue(SnapDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SnapDuration property.
        /// </summary>
        private static void OnSnapDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ScrollCarouselOnTouchAction)d).OnSnapDurationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SnapDuration property.
        /// </summary>
        protected virtual void OnSnapDurationChanged(DependencyPropertyChangedEventArgs e)
        {
            SnapAnimationHelper.Duration = SnapDuration;
        }

        #endregion SnapDuration

        #region SnapEasingFunction

        /// <summary>
        /// SnapEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty SnapEasingFunctionProperty =
            DependencyProperty.Register("SnapEasingFunction", typeof(IEasingFunction), typeof(ScrollCarouselOnTouchAction),
                new FrameworkPropertyMetadata((IEasingFunction)new PowerEase(),
                    new PropertyChangedCallback(OnSnapEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the SnapEasingFunction property.  This dependency property
        /// indicates the funtion used to apply easing to the Snap animation.
        /// </summary>
        public IEasingFunction SnapEasingFunction
        {
            get { return (IEasingFunction)GetValue(SnapEasingFunctionProperty); }
            set { SetValue(SnapEasingFunctionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SnapEasingFunction property.
        /// </summary>
        private static void OnSnapEasingFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ScrollCarouselOnTouchAction)d).OnSnapEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SnapEasingFunction property.
        /// </summary>
        protected virtual void OnSnapEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
            SnapAnimationHelper.EasingFunction = SnapEasingFunction;
        }

        #endregion SnapEasingFunction

        Polaris.Windows.Animation.DoubleAnimationHelper SnapAnimationHelper;

        private void InitializeSnapAnimation()
        {
            SnapAnimationHelper =
                new Polaris.Windows.Animation.DoubleAnimationHelper
                    (
                    OnSnapAnimationCompleted,
                    OnSnapAnimationProgress
                    );
            SnapAnimationHelper.Duration = SnapDuration;
            SnapAnimationHelper.EasingFunction = SnapEasingFunction;
        }

        private void StartSnapAnimation(double initialValue = 1.0, double finalValue = 0.0)
        {
            isSnapAnimationCompleted = false;
            SnapAnimationHelper.StartAnimation(initialValue, finalValue);
        }

        private void StopSnapAnimation()
        {
            SnapAnimationHelper.StopAnimation();
            isSnapAnimationCompleted = true;
        }

        private void UpdateSnapAnimation()
        {
            SnapAnimationHelper.Update();
        }

        private void OnSnapAnimationCompleted()
        {
            isInteracting = false;
            isSnapAnimationCompleted = true;
            AssociatedCarousel.ScrollPosition = SnapAnimationHelper.FinalValue;
            ExecuteCommand(this.ScrollCompleted);
        }


        private void OnSnapAnimationProgress(double currentStepValue)
        {
            AssociatedCarousel.ScrollPosition = currentStepValue;
        }

        #endregion Snap Animation Properties

        #region ScrollStarted

        /// <summary>
        /// ScrollStarted Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollStartedProperty =
            DependencyProperty.Register("ScrollStarted", typeof(ICommand), typeof(ScrollCarouselOnTouchAction),
                new FrameworkPropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the ScrollStarted property.  This dependency property 
        /// indicates ....
        /// </summary>
        public ICommand ScrollStarted
        {
            get { return (ICommand)GetValue(ScrollStartedProperty); }
            set { SetValue(ScrollStartedProperty, value); }
        }

        #endregion

        #region ScrollCompleted

        /// <summary>
        /// ScrollCompleted Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollCompletedProperty =
            DependencyProperty.Register("ScrollCompleted", typeof(ICommand), typeof(ScrollCarouselOnTouchAction),
                new FrameworkPropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the ScrollCompleted property.  This dependency property 
        /// indicates ....
        /// </summary>
        public ICommand ScrollCompleted
        {
            get { return (ICommand)GetValue(ScrollCompletedProperty); }
            set { SetValue(ScrollCompletedProperty, value); }
        }

        #endregion




        protected override void Invoke(object parameter)
        {
            //CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        public ScrollCarouselOnTouchAction()
        {
            var renderingEventListener = new WeakEventListener<ScrollCarouselOnTouchAction, object, EventArgs>(this);
            renderingEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.CompositionTarget_Rendering(source, eventArgs);
            renderingEventListener.OnDetachAction = (weakEventListenerParameter) =>
                CompositionTarget.Rendering -= weakEventListenerParameter.OnEvent;
            CompositionTarget.Rendering += renderingEventListener.OnEvent;

            InitializeSnapAnimation();

            // NOTE: Adds an event handler when the dependency property 'System.Windows.Interactivity.TriggerAction.IsEnabledProperty' changes
            DependencyPropertyDescriptor IsEnabledPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(System.Windows.Interactivity.TriggerAction.IsEnabledProperty, typeof(ScrollCarouselOnTouchAction));
            if (IsEnabledPropertyDescriptor != null)
            {
                IsEnabledPropertyDescriptor.AddValueChanged(this, OnIsEnabledPropertyChanged);
            }


        }
        private void OnIsEnabledPropertyChanged(object sender, EventArgs e)
        {
            if (!IsEnabled)
            {
                Target.ReleaseMouseCapture();
                isScrolling = false;
            }
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (!IsEnabled) { return; }
            //! TODO: Remove conditions: AssociatedCarousel == null
            if (AssociatedCarousel == null || !AssociatedCarousel.IsVisible) { return; }
            if (isScrolling) { return; }
            if (!isInteracting) { return; }
            if (CurrentForce <= 0 && (Convert.ToInt32(AssociatedCarousel.ScrollPosition % MaximumDegreeReturnSnap) == 0) && isSnapAnimationCompleted)
            {
                return;
            }
            UpdateSnapAnimation();
            lock (inertiaSync)
            {
                var currentTime = DateTime.Now;
                var elapsedTime = currentTime - LastUpdate;

                CurrentVelocity = CurrentForce / BodyMass;

                var dueDisplacement = CurrentVelocity * (Double)elapsedTime.TotalMilliseconds;

                if (CurrentForceDirection == ForceDirection.Down)
                {
                    dueDisplacement *= -1;
                }

                var newOffset = AssociatedCarousel.ScrollPosition + dueDisplacement;

                if (Double.IsNaN(newOffset))
                {
                    return;
                }
                AssociatedCarousel.ScrollPosition = newOffset;

                CurrentForce = Math.Max(0, CurrentForce - ((Double)elapsedTime.TotalMilliseconds * CurrentForce * Friction));

                if (CurrentForce < 0.0001)
                {
                    CurrentForce = 0;
                }

                if (CurrentVelocity <= SnapVelocity)
                {
                    var roundedOffset = Math.Round(newOffset, 0);
                    var offsetSign = (roundedOffset < 0 ? -1 : 1);

                    Func<int, bool> fireSnapAnimation = (i) =>
                    {
                        var offsetAdjustment = i;
                        if (CurrentForceDirection == ForceDirection.Down)
                        {
                            offsetAdjustment *= -1;
                        }

                        var goalOffset = roundedOffset + offsetAdjustment;
                        if ((goalOffset % AssociatedCarousel.ItemSeparation) == 0)
                        {
                            //AssociatedCarousel.ScrollPosition = roundedOffset;
                            CurrentForce = 0;
                            StartSnapAnimation(AssociatedCarousel.ScrollPosition, goalOffset);
                            return true;
                        }
                        return false;
                    };
                    if (snapFired)
                    {
                        return;
                    }

                    //snapFired = false;
                    //MinimumDegreeReturnSnap: A number which represents the minimum degrees to move backward when snapping.
                    for (int i = -MinimumDegreeReturnSnap; i <= MinimumDegreeReturnSnap; i++)
                    {
                        snapFired = fireSnapAnimation(i);
                        if (snapFired) { break; }
                    }
                    if (!snapFired)
                    {
                        //MaximumDegreeReturnSnap:
                        for (int i = MinimumDegreeReturnSnap; i <= MaximumDegreeReturnSnap; i++)
                        {
                            snapFired = fireSnapAnimation(i);
                            if (snapFired) { break; }
                        }
                    }
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
                if (InputType == InputType.Touch || InputType == InputType.Both)
                {
                    oldTarget.TouchDown -= new EventHandler<TouchEventArgs>(Target_TouchDown);
                    oldTarget.TouchMove -= new EventHandler<TouchEventArgs>(Target_TouchMoved);
                    oldTarget.TouchUp -= new EventHandler<TouchEventArgs>(Target_TouchUp);
                    oldTarget.TouchEnter -= new EventHandler<TouchEventArgs>(Target_TouchEnter);
                    oldTarget.TouchLeave -= new EventHandler<TouchEventArgs>(Target_TouchLeave);
                }
                if (InputType == Polaris.Windows.Actions.InputType.Mouse || InputType == Polaris.Windows.Actions.InputType.Both)
                {
                    oldTarget.MouseDown -= new MouseButtonEventHandler(Target_MouseDown);
                    oldTarget.MouseMove -= new MouseEventHandler(Target_MouseMove);
                    oldTarget.MouseUp -= new MouseButtonEventHandler(Target_MouseUp);
                }
            }
            if (newTarget != null)
            {
                if (InputType == Polaris.Windows.Actions.InputType.Touch || InputType == Polaris.Windows.Actions.InputType.Both)
                {
                    newTarget.TouchDown += new EventHandler<TouchEventArgs>(Target_TouchDown);
                    newTarget.TouchMove += new EventHandler<TouchEventArgs>(Target_TouchMoved);
                    newTarget.TouchUp += new EventHandler<TouchEventArgs>(Target_TouchUp);
                    newTarget.TouchEnter += new EventHandler<TouchEventArgs>(Target_TouchEnter);
                    newTarget.TouchLeave += new EventHandler<TouchEventArgs>(Target_TouchLeave);
                }
                if (InputType == Polaris.Windows.Actions.InputType.Mouse || InputType == Polaris.Windows.Actions.InputType.Both)
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
            if (!IsEnabled) { return; }
            //if (CurrentForce > 0) { return; }
            if (isScrolling) { return; }
            isScrolling = true;
            snapFired = false;
            Target.CaptureMouse();
            var position = e.GetPosition((IInputElement)sender);
            BeginInteraction(position);
        }

        private void Target_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (!isScrolling) { return; }
            if (AssociatedCarousel == null) { return; }

            var currentPosition = e.GetPosition((IInputElement)sender);

            OnTouchMoved(currentPosition);
        }

        private void Target_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (!isScrolling) { return; }
            isScrolling = false;
            Target.ReleaseMouseCapture();
            var releasePosition = e.GetPosition((IInputElement)sender);
            if (releasePosition == null) { return; }

            var currentPosition = releasePosition;

            var delta = currentPosition - InitialPosition;

            if (delta.Length <= InertiaTriggerArea)
            {
                ExecuteCommand(this.ScrollCompleted);
                return;
            }

            CalculateForce();
        }

        private void Target_TouchLeave(object sender, TouchEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (!isScrolling) { return; }
            isScrolling = false;
            var currentPosition = e.GetTouchPoint((IInputElement)sender).Position;

            var delta = currentPosition - PreviousPosition;

            if (delta.Length <= InertiaTriggerArea)
            {
                ExecuteCommand(this.ScrollCompleted);
                return;
            }

            CalculateForce();
        }

        private void Target_TouchEnter(object sender, TouchEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (isScrolling) { return; }
            isScrolling = true;
            var position = e.GetTouchPoint((IInputElement)sender).Position;
            BeginInteraction(position);
        }

        private void Target_TouchUp(object sender, TouchEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (!isScrolling) { return; }
            isScrolling = false;
            if (e == null) { return; }
            var releasePosition = e.GetTouchPoint((IInputElement)sender).Position;
            if (releasePosition == null) { return; }

            var currentPosition = releasePosition;

            var delta = currentPosition - PreviousPosition;

            if (delta.Length <= InertiaTriggerArea)
            {
                ExecuteCommand(this.ScrollCompleted);
                return;
            }

            CalculateForce();
        }

        private void Target_TouchMoved(object sender, TouchEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (!isScrolling) { return; }
            if (AssociatedCarousel == null) { return; }

            var currentPosition = e.GetTouchPoint((IInputElement)sender).Position;

            OnTouchMoved(currentPosition);
        }

        private void OnTouchMoved(Point currentPosition)
        {
            if (!IsEnabled) { return; }
            var elapsedTime = DateTime.Now - InitialTime;

            if (elapsedTime <= InitialDelayTime) { return; }

            var delta = currentPosition - PreviousPosition;

            var magnitude = 0.0;//delta.Length * ScrollResolution;

            if (ScrollOrientation == Orientation.Vertical)
            {
                magnitude = currentPosition.Y - PreviousPosition.Y;
                //if (currentPosition.Y < PreviousPosition.Y)
                //{
                //    magnitude *= -1;
                //}
            }
            else
            {
                magnitude = currentPosition.X - PreviousPosition.X;
                //if (currentPosition.X < PreviousPosition.X)
                //{
                //    magnitude *= -1;
                //}
            }

            magnitude *= ScrollResolution;

            var newOffset = AssociatedCarousel.ScrollPosition + magnitude;

            //if (ScrollOrientation == Orientation.Vertical)
            //{
            //    AssociatedCarousel.ScrollToVerticalOffset(newOffset);
            //}
            //else
            //{
            //    AssociatedCarousel.ScrollToHorizontalOffset(newOffset);
            //}
            AssociatedCarousel.ScrollPosition = newOffset;

            PreviousPosition = currentPosition;
        }

        private void Target_TouchDown(object sender, TouchEventArgs e)
        {
            if (!IsEnabled) { return; }
            if (isScrolling) { return; }
            isScrolling = true;
            var position = e.GetTouchPoint((IInputElement)sender).Position;
            BeginInteraction(position);
        }

        private void BeginInteraction(Point position)
        {
            StopSnapAnimation();
            lock (inertiaSync)
            {
                CurrentForce = 0.0;
            }
            isInteracting = true;
            if (AssociatedCarousel == null) { return; }
            InitialTime = DateTime.Now;
            InitialPosition = position;
            PreviousPosition = InitialPosition;
            InitialOffset = AssociatedCarousel.ScrollPosition;
            ExecuteCommand(this.ScrollStarted);
        }

        private void CalculateForce()
        {
            lock (inertiaSync)
            {
                if (AssociatedCarousel == null) { return; }
                var finalTime = DateTime.Now;
                var elapsedTime = finalTime - InitialTime;
                var finalOffset = AssociatedCarousel.ScrollPosition;
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

        private void ExecuteCommand<T>(T command, object arg = null) where T : ICommand
        {
            if (command != null && command.CanExecute(arg))
            {
                command.Execute(arg);
            }
        }

        double snapVelocity = 0.01775;

        public double SnapVelocity
        {
            get { return snapVelocity; }
            set { snapVelocity = value; }
        }

        public int MinimumDegreeReturnSnap
        {
            get { return minimumDegreeReturnSnap; }
            set { minimumDegreeReturnSnap = value; }
        }

        int minimumDegreeReturnSnap = 2;

        public int MaximumDegreeReturnSnap
        {
            get { return maximumDegreeReturnSnap; }
            set { maximumDegreeReturnSnap = value; }
        }

        int maximumDegreeReturnSnap = 30;

        bool snapFired = false;

        bool isSnapAnimationCompleted = false;
    }

    internal enum ForceDirection
    {
        Up,
        Down,
    }
}
