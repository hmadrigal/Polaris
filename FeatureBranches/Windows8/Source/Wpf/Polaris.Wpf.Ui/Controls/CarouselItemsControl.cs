//-----------------------------------------------------------------------
// <copyright file="CarouselItemsControl.cs" company="">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
#if NETFX_CORE
namespace Polaris.Controls
#else
namespace Polaris.Windows.Controls
#endif
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
#if NETFX_CORE
    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media.Animation;
    using System.Threading.Tasks;
#else
    using System.Timers;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Collections.Specialized;
#endif

    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CarouselItem))]
#if NETFX_CORE
#else
    [DisplayName("Carousel Items Control")]
    [Description("Displays a collection of items by indicating four custom points and areas.")]
#endif
    public class CarouselItemsControl : ItemsControl
    {
        internal const string CATEGORY_CAROUSE_QUADRANT_POSITIONS = @"Carousel Quadrant Positions";
        internal const string CATEGORY_CAROUSE_QUADRANT_SIZES = @"Carousel Quadrant Sizes";
        internal const string CATEGORY_CAROUSE_QUADRANT_OPACITIES = @"Carousel Quadrant Opacities";
        internal const string CATEGORY_CAROUSE_QUADRANT_BLUR_SETTINGS = @"Carousel Quadrant Blur Values";
        internal const string CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS = @"Carousel Item Insertion Properties";
        internal const string CATEGORY_CAROUSE_ITEM_REMOVAL_SETTINGS = @"Carousel Item Removal Properties";
        internal const string CATEGORY_CAROUSE_IDLE_SETTINGS = @"Carousel Idle Properties";
        internal const string CATEGORY_CAROUSE_SETTINGS = @"Carousel Items Properties";
        internal const string QUADRANT_C_LABEL = @"Quadrant C";
        internal const string QUADRANT_A_LABEL = @"Quadrant A";
        internal const string QUADRANT_D_LABEL = @"Quadrant D";
        internal const string QUADRANT_B_LABEL = @"Quadrant B";

        #region Animation Properties

        #region IsAnimating

        /// <summary>
        /// IsAnimating Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register("IsAnimating", typeof(bool), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(bool)false,
                    new PropertyChangedCallback(OnIsAnimatingChanged)));

        /// <summary>
        /// Gets or sets the IsAnimating property.  This dependency property
        /// indicates whether the control is currently playing an animation.
        /// </summary>
#if NETFX_CORE

#else
        [Bindable(true)]
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(BooleanConverter))]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            set { SetValue(IsAnimatingProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsAnimating property.
        /// </summary>
        private static void OnIsAnimatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIsAnimatingChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsAnimating property.
        /// </summary>
        protected virtual void OnIsAnimatingChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion IsAnimating

        protected DateTime StartTime { get; set; }

        protected Boolean IsAnimationCompleted { get; set; }

        protected Double? InitialScrollPosition { get; set; }

        protected Double? FinalScrollPosition { get; set; }

        #region AnimatedScrollDuration

        /// <summary>
        /// AnimatedScrollDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimatedScrollDurationProperty =
            DependencyProperty.Register("AnimatedScrollDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnAnimatedScrollDurationChanged)));

        /// <summary>
        /// Gets or sets the AnimatedScrollDuration property.  This dependency property
        /// indicates the duration that the animated scrolling should take.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan AnimatedScrollDuration
        {
            get { return (TimeSpan)GetValue(AnimatedScrollDurationProperty); }
            set { SetValue(AnimatedScrollDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the AnimatedScrollDuration property.
        /// </summary>
        private static void OnAnimatedScrollDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnAnimatedScrollDurationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the AnimatedScrollDuration property.
        /// </summary>
        protected virtual void OnAnimatedScrollDurationChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion AnimatedScrollDuration

        #region AnimatedScrollPosition

        /// <summary>
        /// AnimatedScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimatedScrollPositionProperty =
            DependencyProperty.Register("AnimatedScrollPosition", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double)0.0,
                    new PropertyChangedCallback(OnAnimatedScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the AnimatedScrollPosition property.  This dependency property
        /// indicates the position to which the scroll position should animate to.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public double AnimatedScrollPosition
        {
            get { return (double)GetValue(AnimatedScrollPositionProperty); }
            set { SetValue(AnimatedScrollPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the AnimatedScrollPosition property.
        /// </summary>
        private static void OnAnimatedScrollPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnAnimatedScrollPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the AnimatedScrollPosition property.
        /// </summary>
        protected virtual void OnAnimatedScrollPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            //Trigger animation only if the animated scroll position is different than the current scroll position.
            if (AnimatedScrollPosition != ScrollPosition)
            {
                InitialScrollPosition = ScrollPosition;
                FinalScrollPosition = AnimatedScrollPosition;
                StartTime = DateTime.Now;
                IsAnimationCompleted = false;
            }
        }

        #endregion AnimatedScrollPosition

        #region AnimatedScrollEasingFunction
#if NETFX_CORE

        /// <summary>
        /// AnimatedScrollEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimatedScrollEasingFunctionProperty =
            DependencyProperty.Register("AnimatedScrollEasingFunction", typeof(EasingFunctionBase), typeof(CarouselItemsControl),
                new PropertyMetadata((EasingFunctionBase)null,
                    new PropertyChangedCallback(OnAnimatedScrollEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the AnimatedScrollEasingFunction property.  This dependency property
        /// indicates the easing function the animated scrolling should use.
        /// </summary>

        public EasingFunctionBase AnimatedScrollEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(AnimatedScrollEasingFunctionProperty); }
            set { SetValue(AnimatedScrollEasingFunctionProperty, value); }
        }
#else

        /// <summary>
        /// AnimatedScrollEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimatedScrollEasingFunctionProperty =
            DependencyProperty.Register("AnimatedScrollEasingFunction", typeof(IEasingFunction), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((IEasingFunction)null,
                    new PropertyChangedCallback(OnAnimatedScrollEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the AnimatedScrollEasingFunction property.  This dependency property
        /// indicates the easing function the animated scrolling should use.
        /// </summary>
        [Localizability(LocalizationCategory.None)]
        //[TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
        public IEasingFunction AnimatedScrollEasingFunction
        {
            get { return (IEasingFunction)GetValue(AnimatedScrollEasingFunctionProperty); }
            set { SetValue(AnimatedScrollEasingFunctionProperty, value); }
        }
#endif

        /// <summary>
        /// Handles changes to the AnimatedScrollEasingFunction property.
        /// </summary>
        private static void OnAnimatedScrollEasingFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnAnimatedScrollEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the AnimatedScrollEasingFunction property.
        /// </summary>
        protected virtual void OnAnimatedScrollEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion AnimatedScrollEasingFunction

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            UpdateIdleScrollingAnimation();

            var requestsToProcess = pendingAnimationRequests.ToArray();
            var hasAnimationRequest = false;
            foreach (var animationRequest in requestsToProcess)
            {
                hasAnimationRequest = true;
                ProcessAnimationRequest(animationRequest);
            }
            var opacityRequestsToProcess = pendingOpacityAnimationRequests.ToArray();
            foreach (var animationRequest in opacityRequestsToProcess)
            {
                hasAnimationRequest = true;
                ProcessNewItemAnimationRequest(animationRequest);
            }
            IsAnimating = hasAnimationRequest;
            if (hasAnimationRequest)
            {
                UpdateVanishingPointItem();
            }

            if (IsAnimationCompleted) { return; }
            if (FinalScrollPosition == null || InitialScrollPosition == null) { return; }
            var elapsedTime = DateTime.Now.Subtract(StartTime);

            if (elapsedTime < TimeSpan.Zero) { return; }
            var currentDuration = AnimatedScrollDuration;

            if (elapsedTime < currentDuration)
            {
                IsAnimating = true;
                var normalizedTime = (Double)elapsedTime.Ticks / (Double)currentDuration.Ticks;

                var easing =
                    (AnimatedScrollEasingFunction == null ?
                        normalizedTime // if there is no easing function, use linear animation.
                        :
                        AnimatedScrollEasingFunction.Ease(normalizedTime))//Ease(normalizedTime);
                        ;

                var initialScrollPosition = InitialScrollPosition.Value;
                var finalScrollPosition = FinalScrollPosition.Value;
                var currentScrollPositionDisplacement = initialScrollPosition + ((finalScrollPosition - initialScrollPosition) * easing);

                ScrollPosition = currentScrollPositionDisplacement;
            }
            else
            {
                ScrollPosition = FinalScrollPosition.Value;
                FinalScrollPosition = null;
                InitialScrollPosition = null;
                IsAnimationCompleted = true;
                IsAnimating = IsAnimating || !IsAnimationCompleted;
                //OnAnimationCompleted();
            }
        }

        private void ProcessAnimationRequest(CarouselItemAnimationRequest animationRequest)
        {
            var elapsedTime = DateTime.Now.Subtract(animationRequest.StartTime);
            if (elapsedTime < TimeSpan.Zero)
            {
                //pendingAnimationRequests.Remove(animationRequest);
                return;
            }
            var currentDuration = ItemRemovalDuration;
            if (elapsedTime < currentDuration)
            {
                var normalizedTime = (Double)elapsedTime.Ticks / (Double)currentDuration.Ticks;

                var easing =
                    (ItemRemovalEasingFunction == null ?
                        normalizedTime // if there is no easing function, use linear animation.
                        :
                        ItemRemovalEasingFunction.Ease(normalizedTime))//Ease(normalizedTime);
                        ;

                var initialScrollPosition = animationRequest.InitialScrollPosition;
                var finalScrollPosition = animationRequest.FinalScrollPosition;
                var currentScrollPositionDisplacement = initialScrollPosition + ((finalScrollPosition - initialScrollPosition) * easing);

                animationRequest.TargetItem.ScrollPosition = currentScrollPositionDisplacement;
            }
            else
            {
                animationRequest.TargetItem.ScrollPosition = animationRequest.FinalScrollPosition;
                pendingAnimationRequests.Remove(animationRequest);
            }
        }

        private void ProcessNewItemAnimationRequest(CarouselNewItemAnimationRequest animationRequest)
        {
            var elapsedTime = DateTime.Now.Subtract(animationRequest.StartTime);

            var initialOpacity = animationRequest.InitialOpacity;
            var finalOpacity = animationRequest.FinalOpacity;
            var initialX = animationRequest.InitialPosition.X;
            var initialY = animationRequest.InitialPosition.Y;
            var finalX = animationRequest.FinalPosition.X;
            var finalY = animationRequest.FinalPosition.Y;
            var initialWidth = animationRequest.InitialSize.Width;
            var initialHeight = animationRequest.InitialSize.Height;
            var finalWidth = animationRequest.FinalSize.Width;
            var finalHeight = animationRequest.FinalSize.Height;

            if (elapsedTime < TimeSpan.Zero)
            {
                if (!animationRequest.IsBeforeAddedStateRequested)
                {
                    animationRequest.TargetItem.Opacity = 0;
                    Canvas.SetLeft(animationRequest.TargetItem, initialX);
                    Canvas.SetTop(animationRequest.TargetItem, initialY);
                    animationRequest.TargetItem.Width = initialWidth;
                    animationRequest.TargetItem.Height = initialHeight;

                    animationRequest.IsBeforeAddedStateRequested = true;
                    VisualStateManager.GoToState(animationRequest.TargetItem, CarouselItem.BeforeAddedStateName, useTransitions: false);
                }
                else if (!animationRequest.IsWhileAddingStateRequested)
                {
                    animationRequest.TargetItem.Opacity = 1;
                    animationRequest.IsWhileAddingStateRequested = true;
                    VisualStateManager.GoToState(animationRequest.TargetItem, CarouselItem.WhileAddingStateName, useTransitions: true);
                }
                return;
            }
            var currentDuration = NewItemDuration;
            if (elapsedTime < currentDuration)
            {
                if (!animationRequest.IsAddedStateRequested)
                {
                    animationRequest.IsAddedStateRequested = true;
                    VisualStateManager.GoToState(animationRequest.TargetItem, CarouselItem.AddedStateName, useTransitions: true);
                }

                var normalizedTime = (Double)elapsedTime.Ticks / (Double)currentDuration.Ticks;

                var easing =
                    (NewItemEasingFunction == null ?
                        normalizedTime // if there is no easing function, use linear animation.
                        :
                        NewItemEasingFunction.Ease(normalizedTime))//Ease(normalizedTime);
                        ;

                var currentOpacityDisplacement = initialOpacity + ((finalOpacity - initialOpacity) * easing);
                var currentXDisplacement = initialX + ((finalX - initialX) * easing);
                var currentYDisplacement = initialY + ((finalY - initialY) * easing);
                var currentWidthDisplacement = initialWidth + ((finalWidth - initialWidth) * easing);
                var currentHeightDisplacement = initialHeight + ((finalHeight - initialHeight) * easing);

                animationRequest.TargetItem.Opacity = currentOpacityDisplacement;
                Canvas.SetLeft(animationRequest.TargetItem, currentXDisplacement);
                Canvas.SetTop(animationRequest.TargetItem, currentYDisplacement);
                animationRequest.TargetItem.Width = currentWidthDisplacement;
                animationRequest.TargetItem.Height = currentHeightDisplacement;
            }
            else
            {
                animationRequest.TargetItem.Opacity = animationRequest.FinalOpacity;
                Canvas.SetLeft(animationRequest.TargetItem, animationRequest.FinalPosition.X);
                Canvas.SetTop(animationRequest.TargetItem, animationRequest.FinalPosition.Y);
                animationRequest.TargetItem.Width = animationRequest.FinalSize.Width;
                animationRequest.TargetItem.Height = animationRequest.FinalSize.Height;
                pendingOpacityAnimationRequests.Remove(animationRequest);
            }
        }

        #endregion Animation Properties

        #region NewItemInitialPosition

        /// <summary>
        /// NewItemInitialPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemInitialPositionProperty =
            DependencyProperty.Register("NewItemInitialPosition", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
new Point(0, 0)));

        /// <summary>
        /// Gets or sets the NewItemInitialPosition property.  This dependency property
        /// indicates ....
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public Point NewItemInitialPosition
        {
            get { return (Point)GetValue(NewItemInitialPositionProperty); }
            set { SetValue(NewItemInitialPositionProperty, value); }
        }

        #endregion NewItemInitialPosition

        #region NewItemInitialSize

        /// <summary>
        /// NewItemInitialSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemInitialSizeProperty =
            DependencyProperty.Register("NewItemInitialSize", typeof(Size), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
new Size(0, 0)));

        /// <summary>
        /// Gets or sets the NewItemInitialSize property.  This dependency property
        /// indicates ....
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(SizeConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public Size NewItemInitialSize
        {
            get { return (Size)GetValue(NewItemInitialSizeProperty); }
            set { SetValue(NewItemInitialSizeProperty, value); }
        }

        #endregion NewItemInitialSize

        #region NewItemExtraStartTime

        /// <summary>
        /// NewItemExtraStartTime Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemExtraStartTimeProperty =
            DependencyProperty.Register("NewItemExtraStartTime", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
TimeSpan.Zero));

        /// <summary>
        /// Gets or sets the NewItemExtraStartTime property.  This dependency property
        /// indicates ....
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan NewItemExtraStartTime
        {
            get { return (TimeSpan)GetValue(NewItemExtraStartTimeProperty); }
            set { SetValue(NewItemExtraStartTimeProperty, value); }
        }

        #endregion NewItemExtraStartTime

        #region ItemSeparation

        /// <summary>
        /// ItemSeparation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemSeparationProperty =
            DependencyProperty.Register("ItemSeparation", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double)30.0,
                    new PropertyChangedCallback(OnItemSeparationChanged)));

        /// <summary>
        /// Gets or sets the ItemSeparation property.  This dependency property
        /// indicates the separation between items in terms of scroll position.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("Represents the degrees of separation between inserted items")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        [DisplayName("Item separation degrees")]
#endif
        public double ItemSeparation
        {
            get { return (double)GetValue(ItemSeparationProperty); }
            set { SetValue(ItemSeparationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemSeparation property.
        /// </summary>
        private static void OnItemSeparationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemSeparationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemSeparation property.
        /// </summary>
        protected virtual void OnItemSeparationChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion ItemSeparation

        #region TotalArc

        /// <summary>
        /// TotalArc Dependency Property
        /// </summary>
        public static readonly DependencyProperty TotalArcProperty =
            DependencyProperty.Register("TotalArc", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double)360,
                    new PropertyChangedCallback(OnTotalArcChanged)));

        /// <summary>
        /// Gets or sets the TotalArc property.  This dependency property
        /// indicates the total arc of the carousel.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public double TotalArc
        {
            get { return (double)GetValue(TotalArcProperty); }
            set { SetValue(TotalArcProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TotalArc property.
        /// </summary>
        private static void OnTotalArcChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnTotalArcChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TotalArc property.
        /// </summary>
        protected virtual void OnTotalArcChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion TotalArc

        #region ScrollStep

        /// <summary>
        /// ScrollStep Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollStepProperty =
            DependencyProperty.Register("ScrollStep", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double)0.1,
                    new PropertyChangedCallback(OnScrollStepChanged)));

        /// <summary>
        /// Gets or sets the ScrollStep property.  This dependency property
        /// indicates the magnitude of the scroll step.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public double ScrollStep
        {
            get { return (double)GetValue(ScrollStepProperty); }
            set { SetValue(ScrollStepProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ScrollStep property.
        /// </summary>
        private static void OnScrollStepChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnScrollStepChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ScrollStep property.
        /// </summary>
        protected virtual void OnScrollStepChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion ScrollStep

        #region ScrollPosition

        /// <summary>
        /// ScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollPositionProperty =
            DependencyProperty.Register("ScrollPosition", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)0.0,
                    new PropertyChangedCallback(OnScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the ScrollPosition property.  This dependency property
        /// indicates the current position of the item in the carousel scrolling cycle.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public Double ScrollPosition
        {
            get { return (Double)GetValue(ScrollPositionProperty); }
            set { SetValue(ScrollPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ScrollPosition property.
        /// </summary>
        private static void OnScrollPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnScrollPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ScrollPosition property.
        /// </summary>
        protected virtual void OnScrollPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (Double)e.OldValue;
            var newValue = (Double)e.NewValue;
            if (ItemDelay == default(TimeSpan))
            {
                UpdateItems(oldValue, newValue);
            }
            else
            {
                UpdateItemsWithDelay(oldValue, newValue);
            }

            //Update animated scroll position to current position.
            this.AnimatedScrollPosition = newValue;
        }

        #endregion ScrollPosition

        #region QuadrantAPosition

        /// <summary>
        /// QuadrantAPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantAPositionProperty =
            DependencyProperty.Register("QuadrantAPosition", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantAPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantAPosition property.  This dependency property
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the positions of the first quadrant. It usually points to the south.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_POSITIONS)]
        [DisplayName(QUADRANT_A_LABEL)]
#endif
        public Point QuadrantAPosition
        {
            get { return (Point)GetValue(QuadrantAPositionProperty); }
            set { SetValue(QuadrantAPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantAPosition property.
        /// </summary>
        private static void OnQuadrantAPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantAPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantAPosition property.
        /// </summary>
        protected virtual void OnQuadrantAPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantAPosition

        #region QuadrantBPosition

        /// <summary>
        /// QuadrantBPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBPositionProperty =
            DependencyProperty.Register("QuadrantBPosition", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantBPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBPosition property.  This dependency property
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the positions of the first quadrant. It usually points to the East.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_POSITIONS)]
        [DisplayName(QUADRANT_B_LABEL)]
#endif
        public Point QuadrantBPosition
        {
            get { return (Point)GetValue(QuadrantBPositionProperty); }
            set { SetValue(QuadrantBPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBPosition property.
        /// </summary>
        private static void OnQuadrantBPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantBPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBPosition property.
        /// </summary>
        protected virtual void OnQuadrantBPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantBPosition

        #region QuadrantCPosition

        /// <summary>
        /// QuadrantCPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCPositionProperty =
            DependencyProperty.Register("QuadrantCPosition", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantCPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCPosition property.  This dependency property
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the positions of the first quadrant. It usually points to the north.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_POSITIONS)]
        [DisplayName(QUADRANT_C_LABEL)]
#endif
        public Point QuadrantCPosition
        {
            get { return (Point)GetValue(QuadrantCPositionProperty); }
            set { SetValue(QuadrantCPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCPosition property.
        /// </summary>
        private static void OnQuadrantCPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantCPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCPosition property.
        /// </summary>
        protected virtual void OnQuadrantCPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantCPosition

        #region QuadrantDPosition

        /// <summary>
        /// QuadrantDPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDPositionProperty =
            DependencyProperty.Register("QuadrantDPosition", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantDPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDPosition property.  This dependency property
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the positions of the first quadrant. It usually points to the west.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_POSITIONS)]
        [DisplayName(QUADRANT_D_LABEL)]
#endif
        public Point QuadrantDPosition
        {
            get { return (Point)GetValue(QuadrantDPositionProperty); }
            set { SetValue(QuadrantDPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDPosition property.
        /// </summary>
        private static void OnQuadrantDPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantDPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDPosition property.
        /// </summary>
        protected virtual void OnQuadrantDPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantDPosition

        #region QuadrantASize

        /// <summary>
        /// QuadrantASize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantASizeProperty =
            DependencyProperty.Register("QuadrantASize", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantASizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantASize property.  This dependency property
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the size that a item should have when it reaches the south point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_SIZES)]
        [DisplayName(QUADRANT_A_LABEL)]
#endif
        public Point QuadrantASize
        {
            get { return (Point)GetValue(QuadrantASizeProperty); }
            set { SetValue(QuadrantASizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantASize property.
        /// </summary>
        private static void OnQuadrantASizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantASizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantASize property.
        /// </summary>
        protected virtual void OnQuadrantASizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantASize

        #region QuadrantBSize

        /// <summary>
        /// QuadrantBSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBSizeProperty =
            DependencyProperty.Register("QuadrantBSize", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantBSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBSize property.  This dependency property
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the size that a item should have when it reaches the east point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_SIZES)]
        [DisplayName(QUADRANT_B_LABEL)]
#endif
        public Point QuadrantBSize
        {
            get { return (Point)GetValue(QuadrantBSizeProperty); }
            set { SetValue(QuadrantBSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBSize property.
        /// </summary>
        private static void OnQuadrantBSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantBSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBSize property.
        /// </summary>
        protected virtual void OnQuadrantBSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantBSize

        #region QuadrantCSize

        /// <summary>
        /// QuadrantCSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCSizeProperty =
            DependencyProperty.Register("QuadrantCSize", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantCSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCSize property.  This dependency property
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the size that a item should have when it reaches the north point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_SIZES)]
        [DisplayName(QUADRANT_C_LABEL)]
#endif
        public Point QuadrantCSize
        {
            get { return (Point)GetValue(QuadrantCSizeProperty); }
            set { SetValue(QuadrantCSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCSize property.
        /// </summary>
        private static void OnQuadrantCSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantCSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCSize property.
        /// </summary>
        protected virtual void OnQuadrantCSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantCSize

        #region QuadrantDSize

        /// <summary>
        /// QuadrantDSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDSizeProperty =
            DependencyProperty.Register("QuadrantDSize", typeof(Point), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantDSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDSize property.  This dependency property
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(PointConverter))]
        [Bindable(true)]
        [Description("Defines the size that a item should have when it reaches the west point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_SIZES)]
        [DisplayName(QUADRANT_D_LABEL)]
#endif
        public Point QuadrantDSize
        {
            get { return (Point)GetValue(QuadrantDSizeProperty); }
            set { SetValue(QuadrantDSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDSize property.
        /// </summary>
        private static void OnQuadrantDSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantDSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDSize property.
        /// </summary>
        protected virtual void OnQuadrantDSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantDSize

        #region QuadrantAOpacity

        /// <summary>
        /// QuadrantAOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantAOpacityProperty =
            DependencyProperty.Register("QuadrantAOpacity", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)1.0,
                    new PropertyChangedCallback(OnQuadrantAOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantAOpacity property.  This dependency property
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("Defines the opacity that a item should have when it reaches the south point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_OPACITIES)]
        [DisplayName(QUADRANT_A_LABEL)]
#endif
        public Double QuadrantAOpacity
        {
            get { return (Double)GetValue(QuadrantAOpacityProperty); }
            set { SetValue(QuadrantAOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantAOpacity property.
        /// </summary>
        private static void OnQuadrantAOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantAOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantAOpacity property.
        /// </summary>
        protected virtual void OnQuadrantAOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantAOpacity

        #region QuadrantBOpacity

        /// <summary>
        /// QuadrantBOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBOpacityProperty =
            DependencyProperty.Register("QuadrantBOpacity", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)1.0,
                    new PropertyChangedCallback(OnQuadrantBOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBOpacity property.  This dependency property
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("Defines the opacity that a item should have when it reaches the east point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_OPACITIES)]
        [DisplayName(QUADRANT_B_LABEL)]
#endif
        public Double QuadrantBOpacity
        {
            get { return (Double)GetValue(QuadrantBOpacityProperty); }
            set { SetValue(QuadrantBOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBOpacity property.
        /// </summary>
        private static void OnQuadrantBOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantBOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBOpacity property.
        /// </summary>
        protected virtual void OnQuadrantBOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantBOpacity

        #region QuadrantCOpacity

        /// <summary>
        /// QuadrantCOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCOpacityProperty =
            DependencyProperty.Register("QuadrantCOpacity", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)1.0,
                    new PropertyChangedCallback(OnQuadrantCOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCOpacity property.  This dependency property
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("Defines the opacity that a item should have when it reaches the north point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_OPACITIES)]
        [DisplayName(QUADRANT_C_LABEL)]
#endif
        public Double QuadrantCOpacity
        {
            get { return (Double)GetValue(QuadrantCOpacityProperty); }
            set { SetValue(QuadrantCOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCOpacity property.
        /// </summary>
        private static void OnQuadrantCOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantCOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCOpacity property.
        /// </summary>
        protected virtual void OnQuadrantCOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantCOpacity

        #region QuadrantDOpacity

        /// <summary>
        /// QuadrantDOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDOpacityProperty =
            DependencyProperty.Register("QuadrantDOpacity", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)1.0,
                    new PropertyChangedCallback(OnQuadrantDOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDOpacity property.  This dependency property
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("Defines the opacity that a item should have when it reaches the west point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_OPACITIES)]
        [DisplayName(QUADRANT_D_LABEL)]
#endif
        public Double QuadrantDOpacity
        {
            get { return (Double)GetValue(QuadrantDOpacityProperty); }
            set { SetValue(QuadrantDOpacityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDOpacity property.
        /// </summary>
        private static void OnQuadrantDOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantDOpacityChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDOpacity property.
        /// </summary>
        protected virtual void OnQuadrantDOpacityChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantDOpacity

        #region QuadrantABlurRadius

        /// <summary>
        /// QuadrantABlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantABlurRadiusProperty =
            DependencyProperty.Register("QuadrantABlurRadius", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)0.0,
                    new PropertyChangedCallback(OnQuadrantABlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantABlurRadius property.  This dependency property
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("When blur is enabled. It defines the blur radius that a item should have when it reaches the south point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_BLUR_SETTINGS)]
        [DisplayName(QUADRANT_A_LABEL)]
#endif
        public Double QuadrantABlurRadius
        {
            get { return (Double)GetValue(QuadrantABlurRadiusProperty); }
            set { SetValue(QuadrantABlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantABlurRadius property.
        /// </summary>
        private static void OnQuadrantABlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantABlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantABlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantABlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantABlurRadius

        #region QuadrantBBlurRadius

        /// <summary>
        /// QuadrantBBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBBlurRadiusProperty =
            DependencyProperty.Register("QuadrantBBlurRadius", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)0.0,
                    new PropertyChangedCallback(OnQuadrantBBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBBlurRadius property.  This dependency property
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("When blur is enabled. It defines the blur radius that a item should have when it reaches the east point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_BLUR_SETTINGS)]
        [DisplayName(QUADRANT_B_LABEL)]
#endif
        public Double QuadrantBBlurRadius
        {
            get { return (Double)GetValue(QuadrantBBlurRadiusProperty); }
            set { SetValue(QuadrantBBlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantBBlurRadius property.
        /// </summary>
        private static void OnQuadrantBBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantBBlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantBBlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantBBlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantBBlurRadius

        #region QuadrantCBlurRadius

        /// <summary>
        /// QuadrantCBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCBlurRadiusProperty =
            DependencyProperty.Register("QuadrantCBlurRadius", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)0.0,
                    new PropertyChangedCallback(OnQuadrantCBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCBlurRadius property.  This dependency property
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("When blur is enabled. It defines the blur radius that a item should have when it reaches the north point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_BLUR_SETTINGS)]
        [DisplayName(QUADRANT_C_LABEL)]
#endif
        public Double QuadrantCBlurRadius
        {
            get { return (Double)GetValue(QuadrantCBlurRadiusProperty); }
            set { SetValue(QuadrantCBlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantCBlurRadius property.
        /// </summary>
        private static void OnQuadrantCBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantCBlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantCBlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantCBlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantCBlurRadius

        #region QuadrantDBlurRadius

        /// <summary>
        /// QuadrantDBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDBlurRadiusProperty =
            DependencyProperty.Register("QuadrantDBlurRadius", typeof(Double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Double)0.0,
                    new PropertyChangedCallback(OnQuadrantDBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDBlurRadius property.  This dependency property
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("When blur is enabled. It defines the blur radius that a item should have when it reaches the west point.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_BLUR_SETTINGS)]
        [DisplayName(QUADRANT_D_LABEL)]
#endif
        public Double QuadrantDBlurRadius
        {
            get { return (Double)GetValue(QuadrantDBlurRadiusProperty); }
            set { SetValue(QuadrantDBlurRadiusProperty, value); }
        }

        /// <summary>
        /// Handles changes to the QuadrantDBlurRadius property.
        /// </summary>
        private static void OnQuadrantDBlurRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnQuadrantDBlurRadiusChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the QuadrantDBlurRadius property.
        /// </summary>
        protected virtual void OnQuadrantDBlurRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion QuadrantDBlurRadius

        #region IsBlurEnabled

        /// <summary>
        /// IsBlurEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsBlurEnabledProperty =
            DependencyProperty.Register("IsBlurEnabled", typeof(bool), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(bool)true,
                    new PropertyChangedCallback(OnIsBlurEnabledChanged)));

        /// <summary>
        /// Gets or sets the IsBlurEnabled property.  This dependency property
        /// indicates ....
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(BooleanConverter))]
        [Bindable(true)]
        [Description("Indicates whether or not the blur is enabled.")]
        [Category(CATEGORY_CAROUSE_QUADRANT_BLUR_SETTINGS)]
        [DisplayName("Is blur enabled")]
#endif
        public bool IsBlurEnabled
        {
            get { return (bool)GetValue(IsBlurEnabledProperty); }
            set { SetValue(IsBlurEnabledProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsBlurEnabled property.
        /// </summary>
        private static void OnIsBlurEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIsBlurEnabledChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsBlurEnabled property.
        /// </summary>
        protected virtual void OnIsBlurEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateItems();
        }

        #endregion IsBlurEnabled

        #region VanishingPoint

        /// <summary>
        /// VanishingPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty VanishingPointProperty =
            DependencyProperty.Register("VanishingPoint", typeof(Quadrant), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(Quadrant)Quadrant.Unknown,
                    new PropertyChangedCallback(OnVanishingPointChanged)));

        /// <summary>
        /// Gets or sets the VanishingPoint property.  This dependency property
        /// indicates the quadrant at which items "vanish" in the distance.
        /// This is very useful when it is necessary to remove items from the visual
        /// space smoothly. Items that have reached the vanishing point should be
        /// removable from the collection without been noticed by the user.
        /// It is necessary that the vanishing point is either off-screen or
        /// visually unrecognizable for this mechanism to work smoothly.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(EnumConverter))]
        [Bindable(true)]
        [Description("Indicates which quadrant can be used to replace items.")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        [DisplayName("Vanishing Quadrant")]
#endif
        public Quadrant VanishingPoint
        {
            get { return (Quadrant)GetValue(VanishingPointProperty); }
            set { SetValue(VanishingPointProperty, value); }
        }

        /// <summary>
        /// Handles changes to the VanishingPoint property.
        /// </summary>
        private static void OnVanishingPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnVanishingPointChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the VanishingPoint property.
        /// </summary>
        protected virtual void OnVanishingPointChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion VanishingPoint

        #region ItemDelay

        /// <summary>
        /// ItemDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemDelayProperty =
            DependencyProperty.Register("ItemDelay", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnItemDelayChanged)));

        /// <summary>
        /// Gets or sets the ItemDelay property.  This dependency property
        /// indicates the delay between item changes.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan ItemDelay
        {
            get { return (TimeSpan)GetValue(ItemDelayProperty); }
            set { SetValue(ItemDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemDelay property.
        /// </summary>
        private static void OnItemDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemDelayChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemDelay property.
        /// </summary>
        protected virtual void OnItemDelayChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ItemDelay

        #region NewItemAngle

        /// <summary>
        /// NewItemAngle Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemAngleProperty =
            DependencyProperty.Register("NewItemAngle", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double)0.0,
                    new PropertyChangedCallback(OnNewItemAngleChanged)));

        /// <summary>
        /// Gets or sets the NewItemAngle property.  This dependency property
        /// indicates the angle where new items are inserted.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Angle where new items are going to be inserted.")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Insertion item angle")]
#endif
        public double NewItemAngle
        {
            get { return (double)GetValue(NewItemAngleProperty); }
            set { SetValue(NewItemAngleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NewItemAngle property.
        /// </summary>
        private static void OnNewItemAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnNewItemAngleChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the NewItemAngle property.
        /// </summary>
        protected virtual void OnNewItemAngleChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion NewItemAngle

        #region NewItemScrollPosition

        /// <summary>
        /// NewItemScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemScrollPositionProperty =
            DependencyProperty.Register("NewItemScrollPosition", typeof(double?), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double?)null,
                    new PropertyChangedCallback(OnNewItemScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the NewItemScrollPosition property.  This dependency property
        /// indicates the scroll position where the new item will animate from.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        //[TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public double? NewItemScrollPosition
        {
            get { return (double?)GetValue(NewItemScrollPositionProperty); }
            set { SetValue(NewItemScrollPositionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NewItemScrollPosition property.
        /// </summary>
        private static void OnNewItemScrollPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnNewItemScrollPositionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the NewItemScrollPosition property.
        /// </summary>
        protected virtual void OnNewItemScrollPositionChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion NewItemScrollPosition

        #region NewItemDelay

        /// <summary>
        /// NewItemDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemDelayProperty =
            DependencyProperty.Register("NewItemDelay", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnNewItemDelayChanged)));

        /// <summary>
        /// Gets or sets the NewItemDelay property.  This dependency property
        /// indicates time to wait before animating in items added to the carousel.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan NewItemDelay
        {
            get { return (TimeSpan)GetValue(NewItemDelayProperty); }
            set { SetValue(NewItemDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NewItemDelay property.
        /// </summary>
        private static void OnNewItemDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnNewItemDelayChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the NewItemDelay property.
        /// </summary>
        protected virtual void OnNewItemDelayChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion NewItemDelay

        #region NewItemDuration

        /// <summary>
        /// NewItemDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemDurationProperty =
            DependencyProperty.Register("NewItemDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnNewItemDurationChanged)));

        /// <summary>
        /// Gets or sets the NewItemDuration property.  This dependency property
        /// indicates the duration of the animation to play when new items are added to the carousel.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan NewItemDuration
        {
            get { return (TimeSpan)GetValue(NewItemDurationProperty); }
            set { SetValue(NewItemDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the NewItemDuration property.
        /// </summary>
        private static void OnNewItemDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnNewItemDurationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the NewItemDuration property.
        /// </summary>
        protected virtual void OnNewItemDurationChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion NewItemDuration

        #region NewItemEasingFunction

#if NETFX_CORE
        /// <summary>
        /// NewItemEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemEasingFunctionProperty =
            DependencyProperty.Register("NewItemEasingFunction", typeof(EasingFunctionBase), typeof(CarouselItemsControl),
                new PropertyMetadata((EasingFunctionBase)null,
                    new PropertyChangedCallback(OnNewItemEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the NewItemEasingFunction property.  This dependency property
        /// indicates the easing function to use when animating a new item into the carousel.
        /// </summary>

        public EasingFunctionBase NewItemEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(NewItemEasingFunctionProperty); }
            set { SetValue(NewItemEasingFunctionProperty, value); }
        }
#else
        /// <summary>
        /// NewItemEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemEasingFunctionProperty =
            DependencyProperty.Register("NewItemEasingFunction", typeof(IEasingFunction), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((IEasingFunction)null,
                    new PropertyChangedCallback(OnNewItemEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the NewItemEasingFunction property.  This dependency property
        /// indicates the easing function to use when animating a new item into the carousel.
        /// </summary>
        [Localizability(LocalizationCategory.None)]
        //[TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_INSERTION_SETTINGS)]
        //[DisplayName("Item insertion time out")]
        public IEasingFunction NewItemEasingFunction
        {
            get { return (IEasingFunction)GetValue(NewItemEasingFunctionProperty); }
            set { SetValue(NewItemEasingFunctionProperty, value); }
        }
#endif

        /// <summary>
        /// Handles changes to the NewItemEasingFunction property.
        /// </summary>
        private static void OnNewItemEasingFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnNewItemEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the NewItemEasingFunction property.
        /// </summary>
        protected virtual void OnNewItemEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion NewItemEasingFunction

        #region ItemRemovalIsAnimated

        /// <summary>
        /// ItemRemovalIsAnimated Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalIsAnimatedProperty =
            DependencyProperty.Register("ItemRemovalIsAnimated", typeof(bool), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(bool)false,
                    new PropertyChangedCallback(OnItemRemovalIsAnimatedChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalIsAnimated property.  This dependency property
        /// indicates whether the control should perform an animation when an item is removed.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(BooleanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_REMOVAL_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public bool ItemRemovalIsAnimated
        {
            get { return (bool)GetValue(ItemRemovalIsAnimatedProperty); }
            set { SetValue(ItemRemovalIsAnimatedProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemRemovalIsAnimated property.
        /// </summary>
        private static void OnItemRemovalIsAnimatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemRemovalIsAnimatedChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemRemovalIsAnimated property.
        /// </summary>
        protected virtual void OnItemRemovalIsAnimatedChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ItemRemovalIsAnimated

        #region ItemRemovalEasingFunction
#if NETFX_CORE

        /// <summary>
        /// ItemRemovalEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalEasingFunctionProperty =
            DependencyProperty.Register("ItemRemovalEasingFunction", typeof(EasingFunctionBase), typeof(CarouselItemsControl),
                new PropertyMetadata((EasingFunctionBase)null,
                    new PropertyChangedCallback(OnItemRemovalEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalEasingFunction property.  This dependency property
        /// indicates the easing function to use when animating item removal.
        /// </summary>

        public EasingFunctionBase ItemRemovalEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(ItemRemovalEasingFunctionProperty); }
            set { SetValue(ItemRemovalEasingFunctionProperty, value); }
        }
#else

        /// <summary>
        /// ItemRemovalEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalEasingFunctionProperty =
            DependencyProperty.Register("ItemRemovalEasingFunction", typeof(IEasingFunction), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((IEasingFunction)null,
                    new PropertyChangedCallback(OnItemRemovalEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalEasingFunction property.  This dependency property
        /// indicates the easing function to use when animating item removal.
        /// </summary>
        [Localizability(LocalizationCategory.None)]
        //[TypeConverter(typeof(BooleanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_REMOVAL_SETTINGS)]
        //[DisplayName("Item insertion time out")]
        public IEasingFunction ItemRemovalEasingFunction
        {
            get { return (IEasingFunction)GetValue(ItemRemovalEasingFunctionProperty); }
            set { SetValue(ItemRemovalEasingFunctionProperty, value); }
        }
#endif

        /// <summary>
        /// Handles changes to the ItemRemovalEasingFunction property.
        /// </summary>
        private static void OnItemRemovalEasingFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemRemovalEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemRemovalEasingFunction property.
        /// </summary>
        protected virtual void OnItemRemovalEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ItemRemovalEasingFunction

        #region ItemRemovalDuration

        /// <summary>
        /// ItemRemovalDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalDurationProperty =
            DependencyProperty.Register("ItemRemovalDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnItemRemovalDurationChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalDuration property.  This dependency property
        /// indicates the duration of the item removal animation.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_REMOVAL_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan ItemRemovalDuration
        {
            get { return (TimeSpan)GetValue(ItemRemovalDurationProperty); }
            set { SetValue(ItemRemovalDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemRemovalDuration property.
        /// </summary>
        private static void OnItemRemovalDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemRemovalDurationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemRemovalDuration property.
        /// </summary>
        protected virtual void OnItemRemovalDurationChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ItemRemovalDuration

        #region ItemRemovalDelay

        /// <summary>
        /// ItemRemovalDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalDelayProperty =
            DependencyProperty.Register("ItemRemovalDelay", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnItemRemovalDelayChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalDelay property.  This dependency property
        /// indicates the delay to introduce between items when performing the item removal animation.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_REMOVAL_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan ItemRemovalDelay
        {
            get { return (TimeSpan)GetValue(ItemRemovalDelayProperty); }
            set { SetValue(ItemRemovalDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemRemovalDelay property.
        /// </summary>
        private static void OnItemRemovalDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemRemovalDelayChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemRemovalDelay property.
        /// </summary>
        protected virtual void OnItemRemovalDelayChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ItemRemovalDelay

        #region ItemRemovalDelayMode

        /// <summary>
        /// ItemRemovalDelayMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalDelayModeProperty =
            DependencyProperty.Register("ItemRemovalDelayMode", typeof(DelayMode), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(DelayMode)DelayMode.None,
                    new PropertyChangedCallback(OnItemRemovalDelayModeChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalDelayMode property.  This dependency property
        /// indicates the delay mode to use when animating an item removal.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(EnumConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_ITEM_REMOVAL_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public DelayMode ItemRemovalDelayMode
        {
            get { return (DelayMode)GetValue(ItemRemovalDelayModeProperty); }
            set { SetValue(ItemRemovalDelayModeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ItemRemovalDelayMode property.
        /// </summary>
        private static void OnItemRemovalDelayModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnItemRemovalDelayModeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ItemRemovalDelayMode property.
        /// </summary>
        protected virtual void OnItemRemovalDelayModeChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ItemRemovalDelayMode

        #region Idle Properties

        #region IsIdle

        /// <summary>
        /// IsIdle Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsIdleProperty =
            DependencyProperty.Register("IsIdle", typeof(bool), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(bool)false,
                    new PropertyChangedCallback(OnIsIdleChanged)));

        /// <summary>
        /// Gets or sets the IsIdle property.  This dependency property
        /// indicates whether the control is currently idle. It is possible to set this property to true so that the control starts scrolling itself regularly.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(BooleanConverter))]
        [Bindable(true)]
        [Description("Whether or not the carousel is in Idle mode")]
        [Category(CATEGORY_CAROUSE_IDLE_SETTINGS)]
        [DisplayName("Is Idle")]
#endif
        public bool IsIdle
        {
            get { return (bool)GetValue(IsIdleProperty); }
            set { SetValue(IsIdleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsIdle property.
        /// </summary>
        private static void OnIsIdleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIsIdleChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsIdle property.
        /// </summary>
        protected virtual void OnIsIdleChanged(DependencyPropertyChangedEventArgs e)
        {
            InitializeIdleScrollingTimer();
#if NETFX_CORE
#else
            if (IsIdle)
            {
                IdleScrollingTimer.Start();
            }
            else
            {
                IdleScrollingTimer.Stop();
                StopIdleScrollingAnimation();
            }
#endif
        }

        private void InitializeIdleScrollingTimer()
        {
#if NETFX_CORE
#else
            if (IdleScrollingTimer == null)
            {
                IdleScrollingTimer = new Timer(IdleScrollingTimeInterval);
            }
            else
            {
                IdleScrollingTimer.Interval = IdleScrollingTimeInterval;
            }
#endif
        }


#if NETFX_CORE
        private async void IdleScrollingProcess()
        {
            Task updateIdleScrolling = new Task(UpdateIdleScrolling);
            while(true)
            {
                if (IsIdle)
                {
                    updateIdleScrolling.Start();
                }
                await Task.Delay(1000);
            }
        }
#endif


        #endregion IsIdle

        #region IdleScrollingInterval

        /// <summary>
        /// IdleScrollingInterval Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingIntervalProperty =
            DependencyProperty.Register("IdleScrollingInterval", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)new TimeSpan(0, 0, 0, 15, 0),
                    new PropertyChangedCallback(OnIdleScrollingIntervalChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingInterval property.  This dependency property
        /// indicates the interval between automatic idle animations.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        [Description("Time between each increment of  the IdleScrollingVariationProperty")]
        [Category(CATEGORY_CAROUSE_IDLE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan IdleScrollingInterval
        {
            get { return (TimeSpan)GetValue(IdleScrollingIntervalProperty); }
            set { SetValue(IdleScrollingIntervalProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IdleScrollingInterval property.
        /// </summary>
        private static void OnIdleScrollingIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIdleScrollingIntervalChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IdleScrollingInterval property.
        /// </summary>
        protected virtual void OnIdleScrollingIntervalChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion IdleScrollingInterval

        #region IdleScrollingVariation

        /// <summary>
        /// IdleScrollingVariation Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingVariationProperty =
            DependencyProperty.Register("IdleScrollingVariation", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(double)15.0,
                    new PropertyChangedCallback(OnIdleScrollingVariationChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingVariation property.  This dependency property
        /// indicates the variation of the automatic idle scrolling. Each idle scrolling will take up to +/- the value of this property on each iteration.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(DoubleConverter))]
        [Bindable(true)]
        [Description("Increment applied to the scroll position on when carouse is idle")]
        [Category(CATEGORY_CAROUSE_IDLE_SETTINGS)]
        [DisplayName("Idle Scrolling Variation")]
#endif
        public double IdleScrollingVariation
        {
            get { return (double)GetValue(IdleScrollingVariationProperty); }
            set { SetValue(IdleScrollingVariationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IdleScrollingVariation property.
        /// </summary>
        private static void OnIdleScrollingVariationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIdleScrollingVariationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IdleScrollingVariation property.
        /// </summary>
        protected virtual void OnIdleScrollingVariationChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion IdleScrollingVariation

        private DateTime latestIdleAnimation;

        static Random randomGenerator = new Random();

        #region IdleScrollingTimeInterval

        /// <summary>
        /// IdleScrollingTimeInterval Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingTimeIntervalProperty =
            DependencyProperty.Register("IdleScrollingTimeInterval", typeof(double), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
1000.0));

        /// <summary>
        /// Gets or sets the IdleScrollingTimeInterval property.  This dependency property
        /// indicates the time between each auto increment of the scroll position when idle.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(BooleanConverter))]
        [Bindable(true)]
        [Description("Indicates the frequency time -in milliseconds- between every verification of the IdleScrollingInterval.")]
        [Category(CATEGORY_CAROUSE_IDLE_SETTINGS)]
        [DisplayName("Idle Scrolling Time Interval")]
#endif
        public double IdleScrollingTimeInterval
        {
            get { return (double)GetValue(IdleScrollingTimeIntervalProperty); }
            set { SetValue(IdleScrollingTimeIntervalProperty, value); }
        }

        #endregion IdleScrollingTimeInterval

#if NETFX_CORE
#else
        Timer IdleScrollingTimer;//= new Timer(1000);

        private void IdleScrollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                UpdateIdleScrolling();
            }));
        }
#endif

        private void UpdateIdleScrolling()
        {
            var elapsedTime = DateTime.Now.Subtract(latestIdleAnimation);
            if (elapsedTime > IdleScrollingInterval)
            {
                latestIdleAnimation = DateTime.Now;
                var randomModifier = randomGenerator.NextDouble();
                //var randomSign = (randomGenerator.Next(-1, 1) == -1 ? -1 : 1);
                //var randomVariation = IdleScrollingVariation * randomModifier * randomSign;
                var initialScrollPosition = ScrollPosition;
                var finalScrollPosition = ScrollPosition + IdleScrollingVariation;
                StartIdleScrollingAnimation(initialScrollPosition, finalScrollPosition);
            }
        }


        #region IdleScrolling Animation Properties

        #region IdleScrollingDuration

        /// <summary>
        /// IdleScrollingDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingDurationProperty =
            DependencyProperty.Register("IdleScrollingDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
#if NETFX_CORE
 new PropertyMetadata(
#else
                new FrameworkPropertyMetadata(
#endif
(TimeSpan)new TimeSpan(0, 0, 0, 3, 800),
                    new PropertyChangedCallback(OnIdleScrollingDurationChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingDuration property.  This dependency property
        /// indicates the duration of the IdleScrolling animation.
        /// </summary>
#if NETFX_CORE

#else
        [Localizability(LocalizationCategory.None)]
        [TypeConverter(typeof(TimeSpanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
#endif
        public TimeSpan IdleScrollingDuration
        {
            get { return (TimeSpan)GetValue(IdleScrollingDurationProperty); }
            set { SetValue(IdleScrollingDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IdleScrollingDuration property.
        /// </summary>
        private static void OnIdleScrollingDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIdleScrollingDurationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IdleScrollingDuration property.
        /// </summary>
        protected virtual void OnIdleScrollingDurationChanged(DependencyPropertyChangedEventArgs e)
        {
            IdleScrollingAnimationHelper.Duration = IdleScrollingDuration;
        }

        #endregion IdleScrollingDuration

        #region IdleScrollingEasingFunction
#if NETFX_CORE

        /// <summary>
        /// IdleScrollingEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingEasingFunctionProperty =
            DependencyProperty.Register("IdleScrollingEasingFunction", typeof(EasingFunctionBase), typeof(CarouselItemsControl),
                new PropertyMetadata((EasingFunctionBase)new QuinticEase() { EasingMode = EasingMode.EaseInOut, },
                    new PropertyChangedCallback(OnIdleScrollingEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingEasingFunction property.  This dependency property
        /// indicates the funtion used to apply easing to the IdleScrolling animation.
        /// </summary>

        public EasingFunctionBase IdleScrollingEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(IdleScrollingEasingFunctionProperty); }
            set { SetValue(IdleScrollingEasingFunctionProperty, value); }
        }
#else

        /// <summary>
        /// IdleScrollingEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingEasingFunctionProperty =
            DependencyProperty.Register("IdleScrollingEasingFunction", typeof(IEasingFunction), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((IEasingFunction)new QuinticEase() { EasingMode = EasingMode.EaseInOut, },
                    new PropertyChangedCallback(OnIdleScrollingEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingEasingFunction property.  This dependency property
        /// indicates the funtion used to apply easing to the IdleScrolling animation.
        /// </summary>
        [Localizability(LocalizationCategory.None)]
        //[TypeConverter(typeof(BooleanConverter))]
        [Bindable(true)]
        //[Description("Timeout of a given element before it's inserted into the carousel")]
        [Category(CATEGORY_CAROUSE_SETTINGS)]
        //[DisplayName("Item insertion time out")]
        public IEasingFunction IdleScrollingEasingFunction
        {
            get { return (IEasingFunction)GetValue(IdleScrollingEasingFunctionProperty); }
            set { SetValue(IdleScrollingEasingFunctionProperty, value); }
        }
#endif

        /// <summary>
        /// Handles changes to the IdleScrollingEasingFunction property.
        /// </summary>
        private static void OnIdleScrollingEasingFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CarouselItemsControl)d).OnIdleScrollingEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IdleScrollingEasingFunction property.
        /// </summary>
        protected virtual void OnIdleScrollingEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
            IdleScrollingAnimationHelper.EasingFunction = IdleScrollingEasingFunction;
        }

        #endregion IdleScrollingEasingFunction

#if NETFX_CORE
        Polaris.Animation.
#else
        Polaris.Windows.Animation.
#endif
DoubleAnimationHelper IdleScrollingAnimationHelper;

        private void InitializeIdleScrollingAnimation()
        {
            IdleScrollingAnimationHelper =
#if NETFX_CORE
 new Polaris.Animation.
#else
                new Polaris.Windows.Animation.
#endif
DoubleAnimationHelper
                    (
                    OnIdleScrollingAnimationCompleted,
                    OnIdleScrollingAnimationProgress
                    );
            IdleScrollingAnimationHelper.Duration = IdleScrollingDuration;
            IdleScrollingAnimationHelper.EasingFunction = IdleScrollingEasingFunction;
#if NETFX_CORE
            IdleScrollingProcess();
#else
            InitializeIdleScrollingTimer();
            IdleScrollingTimer.Elapsed += new ElapsedEventHandler(IdleScrollingTimer_Elapsed);
#endif
        }

        private void StartIdleScrollingAnimation(double initialValue = 1.0, double finalValue = 0.0)
        {
            IdleScrollingAnimationHelper.StartAnimation(initialValue, finalValue);
        }

        private void UpdateIdleScrollingAnimation()
        {
            IdleScrollingAnimationHelper.Update();
        }

        private void OnIdleScrollingAnimationCompleted()
        {
            ScrollPosition = IdleScrollingAnimationHelper.FinalValue;
        }

        private void OnIdleScrollingAnimationProgress(double currentStepValue)
        {
            ScrollPosition = currentStepValue;
        }

        private void StopIdleScrollingAnimation()
        {
            IdleScrollingAnimationHelper.StopAnimation();
        }

        #endregion IdleScrolling Animation Properties

        #endregion Idle Properties

        List<CarouselItemAnimationRequest> pendingAnimationRequests = new List<CarouselItemAnimationRequest>();
        List<CarouselNewItemAnimationRequest> pendingOpacityAnimationRequests = new List<CarouselNewItemAnimationRequest>();

        CarouselItem currentItemOnVanishingPoint = null;

        /// <summary>
        /// Number of items that have already been generated.
        /// </summary>
        private Int32 generatedItemCount = 0;

        public CarouselItemsControl()
        {
#if NETFX_CORE
            ProcessChangesAsync();
#else
            System.Threading.ThreadPool.QueueUserWorkItem(ProcessChanges);
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
#endif
            InitializeIdleScrollingAnimation();
        }



#if NETFX_CORE
        public async void ProcessChangesAsync()
#else
        public void ProcessChanges(Object state)
#endif
        {
            while (true)
            {
#if NETFX_CORE
                await Task.Delay(60);
                CompositionTarget_Rendering(this, EventArgs.Empty);
#else
                System.Threading.Thread.Sleep(60);
#endif


                var currentDate = DateTime.Now;
                ItemChangeSet[] pendingChangesArray = new ItemChangeSet[] { };
                lock (pendingChangesSync)
                {
                    pendingChangesArray = pendingChanges.ToArray();
                }
                var dueChanges = (from changeSet in pendingChangesArray
                                  where changeSet.DueTime <= currentDate
                                  orderby changeSet.DueTime ascending
                                  select changeSet).ToList();
                foreach (var changeSet in dueChanges)
                {
                    lock (pendingChangesSync)
                    {
                        pendingChanges.Remove(changeSet);
                    }
#if NETFX_CORE
                    ApplyChangeSet(changeSet);
#else
                    this.Dispatcher.BeginInvoke(new Action<ItemChangeSet>(itemChangeSet =>
                    {
                        ApplyChangeSet(itemChangeSet);
                    }), changeSet);
#endif
                }
            }
        }

        private void ApplyChangeSet(ItemChangeSet changeSet)
        {
            changeSet.TargetItem.IsSynchronized = false;
            changeSet.TargetItem.ScrollStep = changeSet.ScrollStep;

            changeSet.TargetItem.VanishingPoint = changeSet.VanishingPoint;

            changeSet.TargetItem.QuadrantABlurRadius = changeSet.QuadrantABlurRadius;
            changeSet.TargetItem.QuadrantAOpacity = changeSet.QuadrantAOpacity;
            changeSet.TargetItem.QuadrantAPosition = changeSet.QuadrantAPosition;
            changeSet.TargetItem.QuadrantASize = changeSet.QuadrantASize;

            changeSet.TargetItem.QuadrantBBlurRadius = changeSet.QuadrantBBlurRadius;
            changeSet.TargetItem.QuadrantBOpacity = changeSet.QuadrantBOpacity;
            changeSet.TargetItem.QuadrantBPosition = changeSet.QuadrantBPosition;
            changeSet.TargetItem.QuadrantBSize = changeSet.QuadrantBSize;

            changeSet.TargetItem.QuadrantCBlurRadius = changeSet.QuadrantCBlurRadius;
            changeSet.TargetItem.QuadrantCOpacity = changeSet.QuadrantCOpacity;
            changeSet.TargetItem.QuadrantCPosition = changeSet.QuadrantCPosition;
            changeSet.TargetItem.QuadrantCSize = changeSet.QuadrantCSize;

            changeSet.TargetItem.QuadrantDBlurRadius = changeSet.QuadrantDBlurRadius;
            changeSet.TargetItem.QuadrantDOpacity = changeSet.QuadrantDOpacity;
            changeSet.TargetItem.QuadrantDPosition = changeSet.QuadrantDPosition;
            changeSet.TargetItem.QuadrantDSize = changeSet.QuadrantDSize;

            changeSet.TargetItem.ScrollPosition = changeSet.ScrollPosition;
            changeSet.TargetItem.IsSynchronized = true;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var itemCount = generatedItemCount;
            generatedItemCount++;
            var newItemScrollPosition = (NewItemScrollPosition.HasValue ? NewItemScrollPosition.Value : itemCount * ItemSeparation);
            var newItemContainer = new CarouselItem()
            {
                IsSynchronized = false,
                ScrollStep = this.ScrollStep,
                TotalArc = this.TotalArc,
                VanishingPoint = this.VanishingPoint,
                QuadrantABlurRadius = this.QuadrantABlurRadius,
                QuadrantAPosition = this.QuadrantAPosition,
                QuadrantAOpacity = this.QuadrantAOpacity,
                QuadrantASize = this.QuadrantASize,
                QuadrantBBlurRadius = this.QuadrantBBlurRadius,
                QuadrantBOpacity = this.QuadrantBOpacity,
                QuadrantBPosition = this.QuadrantBPosition,
                QuadrantBSize = this.QuadrantBSize,
                QuadrantCBlurRadius = this.QuadrantCBlurRadius,
                QuadrantCOpacity = this.QuadrantCOpacity,
                QuadrantCPosition = this.QuadrantCPosition,
                QuadrantCSize = this.QuadrantCSize,
                QuadrantDBlurRadius = this.QuadrantDBlurRadius,
                QuadrantDOpacity = this.QuadrantDOpacity,
                QuadrantDPosition = this.QuadrantDPosition,
                QuadrantDSize = this.QuadrantDSize,
                ScrollPosition = newItemScrollPosition,
                IsBlurEnabled = this.IsBlurEnabled,
                Opacity = (NewItemScrollPosition.HasValue ? 0 : 1),
            };

            newItemContainer.IsSynchronized = true;

            //newItemContainer.Style = this.ItemContainerStyle;

            if (NewItemScrollPosition.HasValue)
            {
                var scrollPositionDescriptor = newItemContainer.GetScrollPositionDescriptor(NewItemScrollPosition.Value);

                var animationRequest = new CarouselNewItemAnimationRequest(NewItemInitialPosition, NewItemInitialSize)
                {
                    StartTime = DateTime.Now.Add(NewItemDelay),
                    InitialOpacity = 1,
                    FinalOpacity = 1,
                    FinalPosition = new Point(scrollPositionDescriptor.Left, scrollPositionDescriptor.Top),
                    FinalSize = new Size(scrollPositionDescriptor.Width, scrollPositionDescriptor.Height),
                    TargetItem = newItemContainer,
                };

                pendingOpacityAnimationRequests.Add(animationRequest);
            }

            return newItemContainer;
        }

        internal class ItemChangeSet
        {
            public CarouselItem TargetItem { get; set; }

            public DateTime DueTime { get; set; }

            public Quadrant VanishingPoint { get; set; }

            public Double QuadrantABlurRadius { get; set; }

            public Double QuadrantBBlurRadius { get; set; }

            public Double QuadrantCBlurRadius { get; set; }

            public Double QuadrantDBlurRadius { get; set; }

            public Point QuadrantAPosition { get; set; }

            public Point QuadrantBPosition { get; set; }

            public Point QuadrantCPosition { get; set; }

            public Point QuadrantDPosition { get; set; }

            public Double QuadrantAOpacity { get; set; }

            public Double QuadrantBOpacity { get; set; }

            public Double QuadrantCOpacity { get; set; }

            public Double QuadrantDOpacity { get; set; }

            public Point QuadrantASize { get; set; }

            public Point QuadrantBSize { get; set; }

            public Point QuadrantCSize { get; set; }

            public Point QuadrantDSize { get; set; }

            public Double ScrollStep { get; set; }

            public Double TotalArc { get; set; }

            public Double ScrollPosition { get; set; }
        }

        Object pendingChangesSync = new object();
        List<ItemChangeSet> pendingChanges = new List<ItemChangeSet>();

        private void UpdateItems()
        {
            UpdateItems(this.ScrollPosition, this.ScrollPosition);
        }

        private void UpdateItems(double oldValue, double newValue)
        {
            var delta = newValue - oldValue;
#if NETFX_CORE
            //ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
#else
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
                return;
            }
#endif
            //double? itemScrollPosition = null;

            var orderedItems = (from item in GetCarouselItems()
                                orderby item.ScrollPosition % (this.Items.Count * ItemSeparation)
                                select item).ToArray();

            double? itemScrollPosition = null;

            foreach (var itemContainer in orderedItems)
            {
                //var itemContainer = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;
                itemContainer.IsSynchronized = false;
                itemContainer.ScrollStep = this.ScrollStep;
                itemContainer.TotalArc = this.TotalArc;
                itemContainer.VanishingPoint = this.VanishingPoint;
                itemContainer.QuadrantABlurRadius = this.QuadrantABlurRadius;
                itemContainer.QuadrantAPosition = this.QuadrantAPosition;
                itemContainer.QuadrantAOpacity = this.QuadrantAOpacity;
                itemContainer.QuadrantASize = this.QuadrantASize;
                itemContainer.QuadrantBBlurRadius = this.QuadrantBBlurRadius;
                itemContainer.QuadrantBOpacity = this.QuadrantBOpacity;
                itemContainer.QuadrantBPosition = this.QuadrantBPosition;
                itemContainer.QuadrantBSize = this.QuadrantBSize;
                itemContainer.QuadrantCBlurRadius = this.QuadrantCBlurRadius;
                itemContainer.QuadrantCOpacity = this.QuadrantCOpacity;
                itemContainer.QuadrantCPosition = this.QuadrantCPosition;
                itemContainer.QuadrantCSize = this.QuadrantCSize;
                itemContainer.QuadrantDBlurRadius = this.QuadrantDBlurRadius;
                itemContainer.QuadrantDOpacity = this.QuadrantDOpacity;
                itemContainer.QuadrantDPosition = this.QuadrantDPosition;
                itemContainer.QuadrantDSize = this.QuadrantDSize;

                //itemContainer.ScrollPosition = itemContainer.ScrollPosition + delta;

                if (itemScrollPosition == null)
                {
                    itemScrollPosition = itemContainer.ScrollPosition + delta;
                }
                else
                {
                    itemScrollPosition += ItemSeparation;
                }
                itemContainer.ScrollPosition = itemScrollPosition.Value;

                itemContainer.IsBlurEnabled = this.IsBlurEnabled;
                itemContainer.IsSynchronized = true;
            }

            UpdateVanishingPointItem();
        }

        private IEnumerable<CarouselItem> GetCarouselItems()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                var carouselItem = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;
                if (carouselItem != null)
                {
                    yield return carouselItem;
                }
            }
        }

        private double GetVanishingPointAngle()
        {
            var normalizedArc = TotalArc % CarouselItem.CircleArc;
            normalizedArc = (normalizedArc == 0 ? CarouselItem.CircleArc : normalizedArc);
            var quadrantSize = normalizedArc / 4;
            var aQuadrantAngle = 0 * quadrantSize;
            var bQuadrantAngle = 1 * quadrantSize;
            var cQuadrantAngle = 2 * quadrantSize;
            var dQuadrantAngle = 3 * quadrantSize;
            var quadrantUnit = 1 / quadrantSize;
            var startTime = DateTime.Now;
            var animationRequests = new List<CarouselItemAnimationRequest>();
            double vanishingPointAngle;
            switch (VanishingPoint)
            {
                case Quadrant.QuadrantA:
                    vanishingPointAngle = aQuadrantAngle;
                    break;
                case Quadrant.QuadrantB:
                    vanishingPointAngle = bQuadrantAngle;
                    break;
                case Quadrant.QuadrantC:
                    vanishingPointAngle = cQuadrantAngle;
                    break;
                default:
                case Quadrant.Unknown:
                case Quadrant.QuadrantD:
                    vanishingPointAngle = dQuadrantAngle;
                    break;
            }

            return vanishingPointAngle;
        }

        private void UpdateVanishingPointItem()
        {
            double vanishingPointAngle = GetVanishingPointAngle();
            CarouselItem itemOnVanishingPoint = null;
            double? angleCloserToVanishingPoint = null;
            for (int i = 0; i < this.Items.Count; i++)
            {
                var itemContainer = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;
                var itemScrollPosition = i * ItemSeparation;

                if (itemContainer == null) { continue; }

                var angle = itemContainer.GetCurrentAngle();
                var complementaryAngle = TotalArc - angle;
                var angleDistanceToVanishingPoint = Math.Abs(angle - vanishingPointAngle);

                if (angleCloserToVanishingPoint == null || angleDistanceToVanishingPoint < angleCloserToVanishingPoint.Value)
                {
                    angleCloserToVanishingPoint = angleDistanceToVanishingPoint;
                    itemOnVanishingPoint = itemContainer;
                }
            }

            if (itemOnVanishingPoint != null && itemOnVanishingPoint != currentItemOnVanishingPoint)
            {
                currentItemOnVanishingPoint = itemOnVanishingPoint;
                itemOnVanishingPoint.OnVanishingPointReached();
#if DEBUG
                PrintCurrentAngles();
#endif
            }
        }


#if NETFX_CORE

        void ItemContainerGenerator_ItemsChanged(object sender, Windows.UI.Xaml.Controls.Primitives.ItemsChangedEventArgs e)
        {
            UpdateItems();
        }
#else


        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            ItemContainerGenerator.StatusChanged -= new EventHandler(ItemContainerGenerator_StatusChanged);
            UpdateItems();
        }
#endif

        private void UpdateItemsWithDelay(Double oldValue, Double newValue)
        {
#if NETFX_CORE
#else
            if (ItemContainerGenerator.Status == GeneratorStatus.NotStarted) { return; }
#endif
            var isAscending = (newValue > oldValue);
            for (int i = 0; i < this.Items.Count; i++)
            {
                var itemContainer = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;
                var itemScrollPosition = i * ItemSeparation;
                //System.Diagnostics.Debug.WriteLine(isAscending);
                var itemDelayTicks = ItemDelay.Ticks * (isAscending ? i : Items.Count - i);
                var currentItemDelay = new TimeSpan(itemDelayTicks);
                var itemDueTime = DateTime.Now.Add(currentItemDelay);

                var itemChangeSet = new ItemChangeSet()
                {
                    DueTime = itemDueTime,
                    TargetItem = itemContainer,
                    ScrollStep = this.ScrollStep,
                    ScrollPosition = newValue + itemScrollPosition,

                    VanishingPoint = this.VanishingPoint,

                    QuadrantABlurRadius = this.QuadrantABlurRadius,
                    QuadrantAOpacity = this.QuadrantAOpacity,
                    QuadrantAPosition = this.QuadrantAPosition,
                    QuadrantASize = this.QuadrantASize,

                    QuadrantBBlurRadius = this.QuadrantBBlurRadius,
                    QuadrantBOpacity = this.QuadrantBOpacity,
                    QuadrantBPosition = this.QuadrantBPosition,
                    QuadrantBSize = this.QuadrantBSize,

                    QuadrantCBlurRadius = this.QuadrantCBlurRadius,
                    QuadrantCOpacity = this.QuadrantCOpacity,
                    QuadrantCPosition = this.QuadrantCPosition,
                    QuadrantCSize = this.QuadrantCSize,

                    QuadrantDBlurRadius = this.QuadrantDBlurRadius,
                    QuadrantDOpacity = this.QuadrantDOpacity,
                    QuadrantDPosition = this.QuadrantDPosition,
                    QuadrantDSize = this.QuadrantDSize,
                };

                lock (pendingChangesSync)
                {
                    pendingChanges.Add(itemChangeSet);
                }
            }
        }

#if NETFX_CORE
        protected override void OnItemsChanged(object args)
        {
            var e = args as NotifyCollectionChangedEventArgs;
            if (e == null) { return; }
#else
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
#endif
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
#if DEBUG
                //System.Diagnostics.Debug.WriteLine("ITEM(s) REMOVED:");
                //foreach (var item in e.OldItems)
                //{
                //    System.Diagnostics.Debug.WriteLine("Item hash code: {0}", item.GetHashCode());
                //}
#endif
                ProcessSingleItemRemoval();
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
#if DEBUG
                //System.Diagnostics.Debug.WriteLine("ITEM(s) ADDED:");
                //foreach (var item in e.NewItems)
                //{
                //    System.Diagnostics.Debug.WriteLine("Item hash code: {0}", item.GetHashCode());
                //}
#endif
            }
            UpdateVanishingPointItem();
            base.OnItemsChanged(e);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return base.IsItemItsOwnContainerOverride(item);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

#if NETFX_CORE
#else
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
#endif

        private void ProcessSingleItemRemoval()
        {
            generatedItemCount--;
            if (ItemRemovalIsAnimated)
            {
                var normalizedArc = TotalArc % CarouselItem.CircleArc;
                normalizedArc = (normalizedArc == 0 ? CarouselItem.CircleArc : normalizedArc);
                var quadrantSize = normalizedArc / 4;
                var aQuadrantAngle = 0 * quadrantSize;
                var bQuadrantAngle = 1 * quadrantSize;
                var cQuadrantAngle = 2 * quadrantSize;
                var dQuadrantAngle = 3 * quadrantSize;
                var quadrantUnit = 1 / quadrantSize;
                var startTime = DateTime.Now.Add(NewItemDelay).Add(NewItemExtraStartTime);
                var animationRequests = new List<CarouselItemAnimationRequest>();
                double vanishingPointAngle;
                switch (VanishingPoint)
                {
                    case Quadrant.QuadrantA:
                        vanishingPointAngle = aQuadrantAngle;
                        break;
                    case Quadrant.QuadrantB:
                        vanishingPointAngle = bQuadrantAngle;
                        break;
                    case Quadrant.QuadrantC:
                        vanishingPointAngle = cQuadrantAngle;
                        break;
                    default:
                    case Quadrant.Unknown:
                    case Quadrant.QuadrantD:
                        vanishingPointAngle = dQuadrantAngle;
                        break;
                }

                for (int i = 0; i < this.Items.Count; i++)
                {
                    var itemContainer = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;

                    var degrees = itemContainer.GetCurrentAngle();

                    animationRequests.Add(new CarouselItemAnimationRequest()
                    {
                        StartTime = startTime,
                        TargetItem = itemContainer,
                        InitialScrollPosition = itemContainer.ScrollPosition,
                        FinalScrollPosition = itemContainer.ScrollPosition + ItemSeparation,
                        Degrees = degrees,
                    });

                    //System.Diagnostics.Debug.WriteLine(degrees);
                }

                var sortedAnimationRequests = (from request in animationRequests
                                               orderby request.Degrees
                                               select request).ToArray();

                animationRequests.Clear();

                quadrantUnit = 0.01;
                var currentAngle = NewItemAngle - quadrantUnit;

                CarouselItemAnimationRequest firstRequest = null;
                while (firstRequest == null)
                {
                    currentAngle += quadrantUnit;
                    currentAngle = Math.Round(currentAngle % normalizedArc, 2);
                    firstRequest = (from request in sortedAnimationRequests
                                    where Math.Round(request.Degrees, 2) == currentAngle
                                    select request).FirstOrDefault();
                }

                NewItemScrollPosition = firstRequest.InitialScrollPosition;

                do
                {
                    var nextRequest = (from request in sortedAnimationRequests
                                       where Math.Round(request.Degrees, 2) == currentAngle
                                       select request).FirstOrDefault();
                    if (nextRequest != null)
                    {
                        animationRequests.Add(nextRequest);
                    }
                    currentAngle += quadrantUnit;
                    currentAngle = Math.Round(currentAngle % normalizedArc, 2);
                } while (currentAngle != vanishingPointAngle);
                var lastRequest = (from request in sortedAnimationRequests
                                   where Math.Round(request.Degrees, 2) == currentAngle
                                   select request).FirstOrDefault();
                if (lastRequest != null)
                {
                    animationRequests.Add(lastRequest);
                }

                var delayIndex = 0;
                foreach (var request in animationRequests)
                {
                    var addIndex = (ItemRemovalDelayMode == DelayMode.Sequential ? delayIndex : animationRequests.Count - delayIndex);
                    request.StartTime = request.StartTime.AddTicks(ItemRemovalDelay.Ticks * addIndex);
                    delayIndex++;
                }
#if DEBUG
                //System.Diagnostics.Debug.WriteLine("Animating {0} items", animationRequests.Count);
#endif
                pendingAnimationRequests.AddRange(animationRequests);
            }
        }

        internal class CarouselNewItemAnimationRequest
        {
            public CarouselItem TargetItem { get; set; }

            public double InitialOpacity { get; set; }

            public double FinalOpacity { get; set; }

            public Point InitialPosition { get; set; }

            public Point FinalPosition { get; set; }

            public Size InitialSize { get; set; }

            public Size FinalSize { get; set; }

            public DateTime StartTime { get; set; }

            /// <summary>
            /// Whether the carousel item BeforeAdded state has already been requested or not.
            /// </summary>
            public bool IsBeforeAddedStateRequested { get; set; }

            /// <summary>
            /// Whether the carousel item WhileAdding state has already been requested or not.
            /// </summary>
            public bool IsWhileAddingStateRequested { get; set; }

            /// <summary>
            /// Whether the carousel item Added state has already been requested or not.
            /// </summary>
            public bool IsAddedStateRequested { get; set; }

            public CarouselNewItemAnimationRequest(Point initialPoint, Size initialSize)
            {
                InitialPosition = initialPoint;
                InitialSize = initialSize;
            }
        }

        internal class CarouselItemAnimationRequest
        {
            public CarouselItem TargetItem { get; set; }

            public double InitialScrollPosition { get; set; }

            public double FinalScrollPosition { get; set; }

            public DateTime StartTime { get; set; }

            public double Degrees { get; set; }
        }

#if DEBUG

        private void PrintCurrentAngles()
        {
            //var carouselItems = new List<CarouselItem>();
            //for (int i = 0; i < this.Items.Count; i++)
            //{
            //    var itemContainer = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;
            //    carouselItems.Add(itemContainer);
            //}
            //var sortedCarouselItems = from carouselItem in carouselItems
            //                          orderby carouselItem.GetCurrentAngle()
            //                          select carouselItem;
            //double? previousAngle = null;
            //foreach (var item in sortedCarouselItems)
            //{
            //    double? distanceFromPreviousAngle = null;
            //    if (previousAngle != null)
            //    {
            //        distanceFromPreviousAngle = item.GetCurrentAngle() - previousAngle.Value;
            //    }
            //    previousAngle = item.GetCurrentAngle();
            //    System.Diagnostics.Debug.WriteLine("item {0} angle: {1}|distance from previous angle: {2}", item.GetHashCode(), item.GetCurrentAngle(), distanceFromPreviousAngle);
            //}
        }

#endif
    }
}