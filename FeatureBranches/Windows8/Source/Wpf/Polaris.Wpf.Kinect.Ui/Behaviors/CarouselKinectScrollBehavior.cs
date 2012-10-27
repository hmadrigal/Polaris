namespace Polaris.Windows.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Polaris.Services;
    using Polaris.Windows.Animation;
    using Polaris.Windows.Controls;
    using Polaris.Windows.Extensions;

    public class CarouselKinectScrollBehavior : Behavior<CarouselItemsControl>, IMovementListener
    {
        #region Constants

        internal const Int32 SwipeRightEngagementDirection = 1;
        internal const Int32 SwipeLeftEngagementDirection = -1;

        #endregion Constants

        #region Fields

        CarouselItem preEngagedItem;

        private InteractionStatus currentInteractionStatus;

        private TimeSpan InitialDelayTime = new TimeSpan(0, 0, 0, 0, 0);

        private Int32 InertiaTriggerArea = 20;

        private Point InitialPosition { get; set; }

        private DateTime InitialTime { get; set; }

        private Double InitialOffset { get; set; }

        private Point PreviousPosition { get; set; }

        private Double CurrentForce { get; set; }

        private Double CurrentVelocity { get; set; }

        private Double LastAcceleration { get; set; }

        private ForceDirection CurrentForceDirection { get; set; }

        private DateTime LastUpdate { get; set; }

        #region Snap Animation Fields

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

        private EngagementArea engagementArea;
        int minimumDegreeReturnSnap = 2;

        public int MaximumDegreeReturnSnap
        {
            get { return maximumDegreeReturnSnap; }
            set { maximumDegreeReturnSnap = value; }
        }

        int maximumDegreeReturnSnap = 30;

        bool isSnapAnimationCompleted = true;

        #endregion Snap Animation Fields

        #endregion Fields

        #region Dependency Properties

        #region Pre Engagement Properties

        #region SelectedItemScrollPosition

        /// <summary>
        /// SelectedItemScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty SelectedItemScrollPositionProperty =
            DependencyProperty.Register("SelectedItemScrollPosition", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.0));

        /// <summary>
        /// Gets or sets the SelectedItemScrollPosition property.  This dependency property
        /// indicates the scroll position of the item that will be considered the selected item of the carousel.
        /// </summary>
        [Category("Pre Engagement Properties")]
        public Double SelectedItemScrollPosition
        {
            get { return (Double)GetValue(SelectedItemScrollPositionProperty); }
            set { SetValue(SelectedItemScrollPositionProperty, value); }
        }

        #endregion SelectedItemScrollPosition

        #region PreEngagementDuration

        /// <summary>
        /// PreEngagementDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty PreEngagementDurationProperty =
            DependencyProperty.Register("PreEngagementDuration", typeof(TimeSpan), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.FromSeconds(0.4)));

        /// <summary>
        /// Gets or sets the PreEngagementDuration property.  This dependency property
        /// indicates the time that the animation of the pre engagement lasts.
        /// </summary>
        [Category("Pre Engagement Properties")]
        public TimeSpan PreEngagementDuration
        {
            get { return (TimeSpan)GetValue(PreEngagementDurationProperty); }
            set { SetValue(PreEngagementDurationProperty, value); }
        }

        #endregion PreEngagementDuration

        #region PreEngagementEasingFunction

        /// <summary>
        /// PreEngagementEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty PreEngagementEasingFunctionProperty =
            DependencyProperty.Register("PreEngagementEasingFunction", typeof(IEasingFunction), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((IEasingFunction)null));

        /// <summary>
        /// Gets or sets the PreEngagementEasingFunction property.  This dependency property
        /// indicates the easing function used to animate the item.
        /// </summary>
        [Category("Pre Engagement Properties")]
        public IEasingFunction PreEngagementEasingFunction
        {
            get { return (IEasingFunction)GetValue(PreEngagementEasingFunctionProperty); }
            set { SetValue(PreEngagementEasingFunctionProperty, value); }
        }

        #endregion PreEngagementEasingFunction

        #region PreEngagementScrollPositionDelta

        /// <summary>
        /// PreEngagementScrollPositionDelta Dependency Property
        /// </summary>
        public static readonly DependencyProperty PreEngagementScrollPositionDeltaProperty =
            DependencyProperty.Register("PreEngagementScrollPositionDelta", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)5));

        /// <summary>
        /// Gets or sets the PreEngagementScrollPositionDelta property.  This dependency property
        /// indicates the scroll position change caused by being engaged.
        /// </summary>
        [Category("Pre Engagement Properties")]
        public Double PreEngagementScrollPositionDelta
        {
            get { return (Double)GetValue(PreEngagementScrollPositionDeltaProperty); }
            set { SetValue(PreEngagementScrollPositionDeltaProperty, value); }
        }

        #endregion PreEngagementScrollPositionDelta

        #endregion Pre Engagement Properties

        #region Kinect Properties

        #region KinectUiService

        /// <summary>
        /// KinectUiService Dependency Property
        /// </summary>
        public static readonly DependencyProperty KinectUiServiceProperty =
            DependencyProperty.Register("KinectUiService", typeof(IKinectUiService), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata(default(IKinectUiService),
                    new PropertyChangedCallback(OnKinectUiServiceChanged)));

        /// <summary>
        /// Gets or sets the KinectUiService property.  This dependency property
        /// indicates the instance of KinectUiService that will provide the behavior with the movement data.
        /// </summary>
        public IKinectUiService KinectUiService
        {
            get { return (IKinectUiService)GetValue(KinectUiServiceProperty); }
            set { SetValue(KinectUiServiceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the KinectUiService property.
        /// </summary>
        private static void OnKinectUiServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselKinectScrollBehavior)d).OnKinectUiServiceChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KinectUiService property.
        /// </summary>
        protected virtual void OnKinectUiServiceChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as IKinectUiService;
            if (oldValue != null)
            {
                oldValue.RemoveMovementListener(this);
            }

            var newValue = e.NewValue as IKinectUiService;
            if (newValue != null)
            {
                newValue.AddMovementListener(this);
            }
        }

        #endregion KinectUiService

        #region LeftEngagementBoundary

        /// <summary>
        /// LeftEngagementBoundary Dependency Property
        /// </summary>
        public static readonly DependencyProperty LeftEngagementBoundaryProperty =
            DependencyProperty.Register("LeftEngagementBoundary", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)(-0.15)));

        /// <summary>
        /// Gets or sets the LeftEngagementBoundary property.  This dependency property
        /// indicates the left boundary of the screen that will be still consider part of the valid area to keep the engagement.
        /// </summary>
        public Double LeftEngagementBoundary
        {
            get { return (Double)GetValue(LeftEngagementBoundaryProperty); }
            set { SetValue(LeftEngagementBoundaryProperty, value); }
        }

        #endregion LeftEngagementBoundary

        #region LeftEngagementThreshold

        /// <summary>
        /// LeftEngagementThreshold Dependency Property
        /// </summary>
        public static readonly DependencyProperty LeftEngagementThresholdProperty =
            DependencyProperty.Register("LeftEngagementThreshold", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.15));

        /// <summary>
        /// Gets or sets the LeftEngagementThreshold property.  This dependency property
        /// indicates the value from 0 to 1 that represents the offset in the screen that is considered as left engagement area.
        /// </summary>
        public Double LeftEngagementThreshold
        {
            get { return (Double)GetValue(LeftEngagementThresholdProperty); }
            set { SetValue(LeftEngagementThresholdProperty, value); }
        }

        #endregion LeftEngagementThreshold

        #region RightEngagementBoundary

        /// <summary>
        /// RightEngagementBoundary Dependency Property
        /// </summary>
        public static readonly DependencyProperty RightEngagementBoundaryProperty =
            DependencyProperty.Register("RightEngagementBoundary", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)1.15));

        /// <summary>
        /// Gets or sets the RightEngagementBoundary property.  This dependency property
        /// indicates the right engagement boundary of the screen that will still be considered valid for the engagement.
        /// </summary>
        public Double RightEngagementBoundary
        {
            get { return (Double)GetValue(RightEngagementBoundaryProperty); }
            set { SetValue(RightEngagementBoundaryProperty, value); }
        }

        #endregion RightEngagementBoundary

        #region RightEngagementThreshold

        /// <summary>
        /// RightEngagementThreshold Dependency Property
        /// </summary>
        public static readonly DependencyProperty RightEngagementThresholdProperty =
            DependencyProperty.Register("RightEngagementThreshold", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.85));

        /// <summary>
        /// Gets or sets the RightEngagementThreshold property.  This dependency property
        /// indicates the value from 0 to 1 that represents the offset in the screen that is considered as right engagement area.
        /// </summary>
        public Double RightEngagementThreshold
        {
            get { return (Double)GetValue(RightEngagementThresholdProperty); }
            set { SetValue(RightEngagementThresholdProperty, value); }
        }

        #endregion RightEngagementThreshold

        #endregion Kinect Properties

        #region ScrollResolution

        /// <summary>
        /// ScrollResolution Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollResolutionProperty =
            DependencyProperty.Register("ScrollResolution", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)1.0));

        /// <summary>
        /// Gets or sets the ScrollResolution property.  This dependency property
        /// indicates the scrolling magnitude associated with each kinect cursor movement.
        /// </summary>
        public Double ScrollResolution
        {
            get { return (Double)GetValue(ScrollResolutionProperty); }
            set { SetValue(ScrollResolutionProperty, value); }
        }

        #endregion ScrollResolution

        #region BodyMass

        /// <summary>
        /// BodyMass Dependency Property
        /// </summary>
        public static readonly DependencyProperty BodyMassProperty =
            DependencyProperty.Register("BodyMass", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.15));

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
            DependencyProperty.Register("Friction", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.002));

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

        #region Snap Animation Properties

        #region SnapDuration

        /// <summary>
        /// SnapDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty SnapDurationProperty =
            DependencyProperty.Register("SnapDuration", typeof(TimeSpan), typeof(CarouselKinectScrollBehavior),
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
            ((CarouselKinectScrollBehavior)d).OnSnapDurationChanged(e);
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
            DependencyProperty.Register("SnapEasingFunction", typeof(IEasingFunction), typeof(CarouselKinectScrollBehavior),
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
            ((CarouselKinectScrollBehavior)d).OnSnapEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the SnapEasingFunction property.
        /// </summary>
        protected virtual void OnSnapEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
            SnapAnimationHelper.EasingFunction = SnapEasingFunction;
        }

        #endregion SnapEasingFunction

        #region LeftFixedSnapPoint

        /// <summary>
        /// LeftFixedSnapPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty LeftFixedSnapPointProperty =
            DependencyProperty.Register("LeftFixedSnapPoint", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.3));

        /// <summary>
        /// Gets or sets the LeftFixedSnapPoint property.  This dependency property
        /// indicates the normalized value used to finished the engagement and trigger a completion of the scroll action.
        /// </summary>
        public Double LeftFixedSnapPoint
        {
            get { return (Double)GetValue(LeftFixedSnapPointProperty); }
            set { SetValue(LeftFixedSnapPointProperty, value); }
        }

        #endregion LeftFixedSnapPoint

        #region RightFixedSnapPoint

        /// <summary>
        /// RightFixedSnapPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty RightFixedSnapPointProperty =
            DependencyProperty.Register("RightFixedSnapPoint", typeof(Double), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata((Double)0.7));

        /// <summary>
        /// Gets or sets the RightFixedSnapPoint property.  This dependency property
        /// indicates the normalized value used to finished the engagement and trigger a completion of the scroll action.
        /// </summary>
        public Double RightFixedSnapPoint
        {
            get { return (Double)GetValue(RightFixedSnapPointProperty); }
            set { SetValue(RightFixedSnapPointProperty, value); }
        }

        #endregion RightFixedSnapPoint

        Polaris.Windows.Animation.DoubleAnimationHelper SnapAnimationHelper;
        Polaris.Windows.Animation.DoubleAnimationHelper PreEngagementAnimationHelper;

        #endregion Snap Animation Properties

        #region ScrollStarted

        /// <summary>
        /// ScrollStarted Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollStartedProperty =
            DependencyProperty.Register("ScrollStarted", typeof(ICommand), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the ScrollStarted property.  This dependency property
        /// indicates the command to be executed when the scroll action starts.
        /// </summary>
        public ICommand ScrollStarted
        {
            get { return (ICommand)GetValue(ScrollStartedProperty); }
            set { SetValue(ScrollStartedProperty, value); }
        }

        #endregion ScrollStarted

        #region ScrollCompleted

        /// <summary>
        /// ScrollCompleted Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollCompletedProperty =
            DependencyProperty.Register("ScrollCompleted", typeof(ICommand), typeof(CarouselKinectScrollBehavior),
                new FrameworkPropertyMetadata(default(ICommand)));

        /// <summary>
        /// Gets or sets the ScrollCompleted property.  This dependency property
        /// indicates the command to be executed when the scroll action finishes.
        /// </summary>
        public ICommand ScrollCompleted
        {
            get { return (ICommand)GetValue(ScrollCompletedProperty); }
            set { SetValue(ScrollCompletedProperty, value); }
        }

        #endregion ScrollCompleted

        #endregion Dependency Properties

        #region Methods

        #region SnapAnimation Methods

        private void InitializeSnapAnimation()
        {
            SnapAnimationHelper =
                new DoubleAnimationHelper
                    (
                    OnSnapAnimationCompleted,
                    OnSnapAnimationProgress
                    );
            SnapAnimationHelper.Duration = SnapDuration;
            SnapAnimationHelper.EasingFunction = SnapEasingFunction;
        }

        private void InitializePreEngagementAnimation()
        {
            PreEngagementAnimationHelper =
                new DoubleAnimationHelper
                    (
                    OnPreEngagementAnimationCompleted,
                    OnPreEngagementAnimationProgress
                    );
            SnapAnimationHelper.Duration = PreEngagementDuration;
            SnapAnimationHelper.EasingFunction = PreEngagementEasingFunction;
        }

        private void OnPreEngagementAnimationCompleted()
        {
            preEngagedItem.ScrollPosition = PreEngagementAnimationHelper.FinalValue;
            currentInteractionStatus = InteractionStatus.Engaged;
        }

        private void OnPreEngagementAnimationProgress(Double currentStepValue)
        {
            preEngagedItem.ScrollPosition = currentStepValue;
        }

        private void StartSnapAnimation(double initialValue = 1.0, double finalValue = 0.0)
        {
            isSnapAnimationCompleted = false;
            SnapAnimationHelper.StartAnimation(initialValue, finalValue);
        }

        private void StopSnapAnimation()
        {
            SnapAnimationHelper.StopAnimation();
            //lock (inertiaSync)
            //{
            isSnapAnimationCompleted = true;
            //}
        }

        private void UpdateSnapAnimation()
        {
            SnapAnimationHelper.Update();
        }

        private void OnSnapAnimationCompleted()
        {
            currentInteractionStatus = InteractionStatus.None;
            isSnapAnimationCompleted = true;
            AssociatedObject.ScrollPosition = SnapAnimationHelper.FinalValue;
            this.ScrollCompleted.ExecuteCommand();
        }

        private void OnSnapAnimationProgress(double currentStepValue)
        {
            //lock (inertiaSync)
            //{
            AssociatedObject.ScrollPosition = currentStepValue;
            //}
        }

        #endregion SnapAnimation Methods

        #endregion Methods

        public CarouselKinectScrollBehavior()
        {
            var renderingEventListener = new WeakEventListener<CarouselKinectScrollBehavior, object, EventArgs>(this);
            renderingEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.CompositionTarget_Rendering(source, eventArgs);
            renderingEventListener.OnDetachAction = (weakEventListenerParameter) =>
                CompositionTarget.Rendering -= weakEventListenerParameter.OnEvent;
            CompositionTarget.Rendering += renderingEventListener.OnEvent;

            InitializeSnapAnimation();
            InitializePreEngagementAnimation();
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (AssociatedObject == null || !AssociatedObject.IsVisible) { return; }

            if (currentInteractionStatus == InteractionStatus.PreEngaged)
            {
                PreEngagementAnimationHelper.Update();
            }
            if (currentInteractionStatus != InteractionStatus.Scrolling) { return; }
            UpdateSnapAnimation();
            if (CurrentForce <= 0 && (Convert.ToInt32(AssociatedObject.ScrollPosition % MaximumDegreeReturnSnap) == 0) && isSnapAnimationCompleted)
            {
                currentInteractionStatus = InteractionStatus.None;
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

            var newOffset = AssociatedObject.ScrollPosition + dueDisplacement;

            if (Double.IsNaN(newOffset))
            {
                return;
            }
            AssociatedObject.ScrollPosition = newOffset;

            CurrentForce = Math.Max(0, CurrentForce - ((Double)elapsedTime.TotalMilliseconds * CurrentForce * Friction));

            if (CurrentForce < 0.0001)
            {
                CurrentForce = 0;
            }
            if (!isSnapAnimationCompleted)
            {
                return;
            }

            if (CurrentVelocity <= SnapVelocity)
            {
                var roundedOffset = Math.Round(newOffset, 0);
                var goalOffset = GetSnapOffset(roundedOffset);
                CurrentForce = 0;
                StartSnapAnimation(AssociatedObject.ScrollPosition, goalOffset);
            }

            LastUpdate = DateTime.Now;
        }

        private Double GetSnapOffset(Double roundedOffset)
        {
            int direction = this.engagementArea == EngagementArea.Left ? 1 : -1;
            int snapDegree = -1 * direction * MinimumDegreeReturnSnap;
            Double? goalOffset = null;
            while (goalOffset == null)
            {
                //if (CurrentForceDirection == ForceDirection.Down)
                //{
                //    snapDegree *= -1;
                //}

                var potentialGoalOffset = roundedOffset + snapDegree;
                if ((potentialGoalOffset % AssociatedObject.ItemSeparation) == 0)
                {
                    goalOffset = potentialGoalOffset;
                }
                snapDegree += direction;
            }
            return goalOffset.Value;
        }

        public void AddCursorEntry(double normalizedX, double normalizedY)
        {
            this.Dispatcher.BeginInvoke(new Action<double, double>((x, y) =>
            {
                var point = new Point(x, y);
                switch (currentInteractionStatus)
                {
                    case InteractionStatus.None:
                        {
                            if (x >= LeftEngagementBoundary && x <= LeftEngagementThreshold && y > 0.0 & y < 1.0)
                            {
                                Engage(point, EngagementArea.Left);
                            }
                            if (x <= RightEngagementBoundary && x >= RightEngagementThreshold && y > 0.0 && y < 1.0)
                            {
                                Engage(point, EngagementArea.Right);
                            }
                        }
                        break;
                    case InteractionStatus.PreEngaged:
                        {
                            //Animating... Waiting for the carousel item to be engaged.
                        }
                        break;
                    case InteractionStatus.Engaged:
                        {
                            //Evaluate if should start considering kinect cursor movement.
                            if (this.engagementArea == EngagementArea.Left && point.X > this.InitialPosition.X)
                            {
                                BeginInteraction(point);
                            }

                            if (this.engagementArea == EngagementArea.Right && point.X < this.InitialPosition.X)
                            {
                                BeginInteraction(point);
                            }
                        }
                        break;
                    case InteractionStatus.Interacting:
                        {
                            bool shouldCancelEngagement = this.ShouldCancelEngagement(x, y);
                            if (shouldCancelEngagement)
                            {
                                this.FinishInteraction(point);
                            }
                            else
                            {
                                this.UpdateInteraction(point);
                            }
                        }
                        break;
                    case InteractionStatus.Scrolling:
                        {
                            //Nothing to do as the carousel is being controlled by the snap animation.
                        }
                        break;
                    default:
                        break;
                }
            }), normalizedX, normalizedY);
        }

        private CarouselItem GetPreEngagementCarouselItem(Point point, EngagementArea engagementArea)
        {
            var direction = engagementArea == EngagementArea.Left ? -1 : 1;
            var items = this.AssociatedObject.FindVisualChild<Canvas>().Children;
            //System.Diagnostics.Debug.WriteLine("PRE ENGAGEMENT Scroll Position = " + AssociatedObject.ScrollPosition + " Engagement " + engagementArea);
            //foreach (CarouselItem item in items)
            //{
            //    System.Diagnostics.Debug.WriteLine(String.Format("{0} - {1}", item.DataContext, item.ScrollPosition));
            //}
            var nextItem = (from CarouselItem item in items
                            where Math.Abs((SelectedItemScrollPosition + AssociatedObject.ItemSeparation * direction) - item.ScrollPosition) < 0.001
                            select item).ToArray().FirstOrDefault();
            if (nextItem == null)
            {
                switch (engagementArea)
                {
                    case EngagementArea.Left:
                        {
                            nextItem = (from CarouselItem item in items
                                        orderby item.ScrollPosition descending
                                        select item).ToArray().FirstOrDefault();
                        }
                        break;
                    case EngagementArea.Right:
                        {
                            nextItem = (from CarouselItem item in items
                                        orderby item.ScrollPosition ascending
                                        select item).ToArray().FirstOrDefault();
                        }
                        break;
                    default:
                        break;
                }
            }
            return nextItem;
        }

        private void UpdateInteraction(Point currentPosition)
        {
            if (currentInteractionStatus != InteractionStatus.Interacting) { return; }
            if (AssociatedObject == null) { return; }
            var elapsedTime = DateTime.Now - InitialTime;
            if (elapsedTime <= InitialDelayTime) { return; }
            var delta = currentPosition - PreviousPosition;
            var magnitude = delta.X * ScrollResolution;
            var newOffset = AssociatedObject.ScrollPosition + magnitude;
            AssociatedObject.ScrollPosition = newOffset;
            PreviousPosition = currentPosition;
        }

        private void FinishInteraction(Point releasePoint)
        {
            if (currentInteractionStatus != InteractionStatus.Interacting) { return; }

            var delta = releasePoint - PreviousPosition;

            if (delta.Length <= InertiaTriggerArea)
            {
                this.ScrollCompleted.ExecuteCommand();
                currentInteractionStatus = InteractionStatus.Scrolling;
                return;
            }
            CalculateForce();
            currentInteractionStatus = InteractionStatus.Scrolling;
        }

        private void CalculateForce()
        {
            //lock (inertiaSync)
            //{
            if (AssociatedObject == null) { return; }
            var finalTime = DateTime.Now;
            var elapsedTime = finalTime - InitialTime;
            var finalOffset = AssociatedObject.ScrollPosition;
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
            //}
        }

        private bool ShouldCancelEngagement(double normalizedX, double normalizedY)
        {
            //TODO: Include the fixed points.
            var outBounds = normalizedY > 1.0 || normalizedY < 0.0 || normalizedX < LeftEngagementBoundary || normalizedX > RightEngagementBoundary;
            var outOfLeftArea = this.engagementArea == EngagementArea.Left && normalizedX > LeftFixedSnapPoint;
            var outOfRightArea = this.engagementArea == EngagementArea.Right && normalizedX < RightFixedSnapPoint;
            return outBounds || outOfLeftArea || outOfRightArea;
        }

        private void BeginInteraction(Point initialPoint)
        {
            CurrentForce = 0.0;
            currentInteractionStatus = InteractionStatus.Interacting;
            if (AssociatedObject == null) { return; }
            InitialTime = DateTime.Now;
            InitialPosition = initialPoint;
            PreviousPosition = InitialPosition;
            InitialOffset = AssociatedObject.ScrollPosition;
            this.ScrollStarted.ExecuteCommand();
        }

        private void Engage(Point point, EngagementArea engagementArea)
        {
            CurrentForce = 0.0;
            currentInteractionStatus = InteractionStatus.Engaged;
            if (AssociatedObject == null) { return; }
            InitialTime = DateTime.Now;
            InitialPosition = point;
            PreviousPosition = InitialPosition;
            InitialOffset = AssociatedObject.ScrollPosition;
            this.engagementArea = engagementArea;

            //var preEngagedItem = GetPreEngagementCarouselItem(point, engagementArea);
            //this.preEngagedItem = preEngagedItem;
            //var direction = engagementArea == EngagementArea.Left ? 1 : -1;
            //var finalValue = preEngagedItem.ScrollPosition + PreEngagementScrollPositionDelta * direction;
            //PreEngagementAnimationHelper.StartAnimation(preEngagedItem.ScrollPosition, finalValue);
        }
    }

    internal enum ForceDirection
    {
        Up,
        Down,
    }

    internal enum EngagementArea
    {
        Left,
        Right,
    }

    internal enum InteractionStatus
    {
        /// <summary>
        /// User is not interacting with the carousel, nor the carousel is doing any automatic snap
        /// </summary>
        None,

        /// <summary>
        /// The user has reached the engagement area and the pre engagement animation is happening.
        /// </summary>
        PreEngaged,

        /// <summary>
        /// The pre engagement animation is completed, and the user is engaged with the carousel,
        /// however it has not start the movement towards the expected direction, and therefore movement is being ignored.
        /// </summary>
        Engaged,

        /// <summary>
        /// The user is engaged and already moved the carousel to the expected direction after the engagement.
        /// From now till the moment the movement exceeds the boundaries the carousel will be under user control.
        /// </summary>
        Interacting,

        /// <summary>
        /// The user interaction has been finished, and the behavior is snapping to the next item.
        /// </summary>
        Scrolling,
    }
}