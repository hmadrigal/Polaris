//-----------------------------------------------------------------------
// <copyright file="CarouselItemsControl.cs" company="">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Controls.Wpf.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Timers;

    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(CarouselItem))]
    public class CarouselItemsControl : ItemsControl
    {
        #region Animation Properties

        #region IsAnimating

        /// <summary>
        /// IsAnimating Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.Register("IsAnimating", typeof(bool), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnIsAnimatingChanged)));

        /// <summary>
        /// Gets or sets the IsAnimating property.  This dependency property 
        /// indicates whether the control is currently playing an animation.
        /// </summary>
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

        #endregion




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
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnAnimatedScrollDurationChanged)));

        /// <summary>
        /// Gets or sets the AnimatedScrollDuration property.  This dependency property 
        /// indicates the duration that the animated scrolling should take.
        /// </summary>
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

        #endregion

        #region AnimatedScrollPosition

        /// <summary>
        /// AnimatedScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty AnimatedScrollPositionProperty =
            DependencyProperty.Register("AnimatedScrollPosition", typeof(double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double)0.0,
                    new PropertyChangedCallback(OnAnimatedScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the AnimatedScrollPosition property.  This dependency property 
        /// indicates the position to which the scroll position should animate to.
        /// </summary>
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
            InitialScrollPosition = ScrollPosition;
            FinalScrollPosition = AnimatedScrollPosition;
            StartTime = DateTime.Now;
            IsAnimationCompleted = false;
        }

        #endregion

        #region AnimatedScrollEasingFunction

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
        public IEasingFunction AnimatedScrollEasingFunction
        {
            get { return (IEasingFunction)GetValue(AnimatedScrollEasingFunctionProperty); }
            set { SetValue(AnimatedScrollEasingFunctionProperty, value); }
        }

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

        #endregion

        void CompositionTarget_Rendering(object sender, EventArgs e)
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


        #endregion

        #region NewItemInitialPosition

        /// <summary>
        /// NewItemInitialPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemInitialPositionProperty =
            DependencyProperty.Register("NewItemInitialPosition", typeof(Point), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata(new Point(0, 0)));

        /// <summary>
        /// Gets or sets the NewItemInitialPosition property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Point NewItemInitialPosition
        {
            get { return (Point)GetValue(NewItemInitialPositionProperty); }
            set { SetValue(NewItemInitialPositionProperty, value); }
        }

        #endregion

        #region NewItemInitialSize

        /// <summary>
        /// NewItemInitialSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemInitialSizeProperty =
            DependencyProperty.Register("NewItemInitialSize", typeof(Size), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata(new Size(0, 0)));

        /// <summary>
        /// Gets or sets the NewItemInitialSize property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Size NewItemInitialSize
        {
            get { return (Size)GetValue(NewItemInitialSizeProperty); }
            set { SetValue(NewItemInitialSizeProperty, value); }
        }

        #endregion

        #region NewItemExtraStartTime

        /// <summary>
        /// NewItemExtraStartTime Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemExtraStartTimeProperty =
            DependencyProperty.Register("NewItemExtraStartTime", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata(TimeSpan.Zero));

        /// <summary>
        /// Gets or sets the NewItemExtraStartTime property.  This dependency property 
        /// indicates ....
        /// </summary>
        public TimeSpan NewItemExtraStartTime
        {
            get { return (TimeSpan)GetValue(NewItemExtraStartTimeProperty); }
            set { SetValue(NewItemExtraStartTimeProperty, value); }
        }

        #endregion

        #region ItemSeparation

        /// <summary>
        /// ItemSeparation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemSeparationProperty =
            DependencyProperty.Register("ItemSeparation", typeof(double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double)30.0,
                    new PropertyChangedCallback(OnItemSeparationChanged)));

        /// <summary>
        /// Gets or sets the ItemSeparation property.  This dependency property 
        /// indicates the separation between items in terms of scroll position.
        /// </summary>
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

        #endregion

        #region TotalArc

        /// <summary>
        /// TotalArc Dependency Property
        /// </summary>
        public static readonly DependencyProperty TotalArcProperty =
            DependencyProperty.Register("TotalArc", typeof(double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double)360,
                    new PropertyChangedCallback(OnTotalArcChanged)));

        /// <summary>
        /// Gets or sets the TotalArc property.  This dependency property 
        /// indicates the total arc of the carousel.
        /// </summary>
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

        #endregion

        #region ScrollStep

        /// <summary>
        /// ScrollStep Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollStepProperty =
            DependencyProperty.Register("ScrollStep", typeof(double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double)0.1,
                    new PropertyChangedCallback(OnScrollStepChanged)));

        /// <summary>
        /// Gets or sets the ScrollStep property.  This dependency property 
        /// indicates the magnitude of the scroll step.
        /// </summary>
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

        #endregion

        #region ScrollPosition

        /// <summary>
        /// ScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollPositionProperty =
            DependencyProperty.Register("ScrollPosition", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the ScrollPosition property.  This dependency property 
        /// indicates the current position of the item in the carousel scrolling cycle.
        /// </summary>
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
        }

        #endregion

        #region QuadrantAPosition

        /// <summary>
        /// QuadrantAPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantAPositionProperty =
            DependencyProperty.Register("QuadrantAPosition", typeof(Point), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantAPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantAPosition property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantBPosition

        /// <summary>
        /// QuadrantBPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBPositionProperty =
            DependencyProperty.Register("QuadrantBPosition", typeof(Point), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantBPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBPosition property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantCPosition

        /// <summary>
        /// QuadrantCPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCPositionProperty =
            DependencyProperty.Register("QuadrantCPosition", typeof(Point), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantCPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCPosition property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantDPosition

        /// <summary>
        /// QuadrantDPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDPositionProperty =
            DependencyProperty.Register("QuadrantDPosition", typeof(Point), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Point)new Point(),
                    new PropertyChangedCallback(OnQuadrantDPositionChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDPosition property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantASize

        /// <summary>
        /// QuadrantASize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantASizeProperty =
            DependencyProperty.Register("QuadrantASize", typeof(Size), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantASizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantASize property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
        public Size QuadrantASize
        {
            get { return (Size)GetValue(QuadrantASizeProperty); }
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

        #endregion

        #region QuadrantBSize

        /// <summary>
        /// QuadrantBSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBSizeProperty =
            DependencyProperty.Register("QuadrantBSize", typeof(Size), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantBSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBSize property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
        public Size QuadrantBSize
        {
            get { return (Size)GetValue(QuadrantBSizeProperty); }
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

        #endregion

        #region QuadrantCSize

        /// <summary>
        /// QuadrantCSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCSizeProperty =
            DependencyProperty.Register("QuadrantCSize", typeof(Size), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantCSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCSize property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
        public Size QuadrantCSize
        {
            get { return (Size)GetValue(QuadrantCSizeProperty); }
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

        #endregion

        #region QuadrantDSize

        /// <summary>
        /// QuadrantDSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDSizeProperty =
            DependencyProperty.Register("QuadrantDSize", typeof(Size), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Size)new Size(),
                    new PropertyChangedCallback(OnQuadrantDSizeChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDSize property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
        public Size QuadrantDSize
        {
            get { return (Size)GetValue(QuadrantDSizeProperty); }
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

        #endregion

        #region QuadrantAOpacity

        /// <summary>
        /// QuadrantAOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantAOpacityProperty =
            DependencyProperty.Register("QuadrantAOpacity", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantAOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantAOpacity property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantBOpacity

        /// <summary>
        /// QuadrantBOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBOpacityProperty =
            DependencyProperty.Register("QuadrantBOpacity", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantBOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBOpacity property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantCOpacity

        /// <summary>
        /// QuadrantCOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCOpacityProperty =
            DependencyProperty.Register("QuadrantCOpacity", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantCOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCOpacity property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantDOpacity

        /// <summary>
        /// QuadrantDOpacity Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDOpacityProperty =
            DependencyProperty.Register("QuadrantDOpacity", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)1.0,
                    new PropertyChangedCallback(OnQuadrantDOpacityChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDOpacity property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantABlurRadius

        /// <summary>
        /// QuadrantABlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantABlurRadiusProperty =
            DependencyProperty.Register("QuadrantABlurRadius", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantABlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantABlurRadius property.  This dependency property 
        /// indicates the goal position of the item in quadrant A, at 0 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantBBlurRadius

        /// <summary>
        /// QuadrantBBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantBBlurRadiusProperty =
            DependencyProperty.Register("QuadrantBBlurRadius", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantBBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantBBlurRadius property.  This dependency property 
        /// indicates the initial position of the item in quadrant B, at 90 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantCBlurRadius

        /// <summary>
        /// QuadrantCBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantCBlurRadiusProperty =
            DependencyProperty.Register("QuadrantCBlurRadius", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantCBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantCBlurRadius property.  This dependency property 
        /// indicates the initial position of the item in quadrant C, at 180 degrees.
        /// </summary>
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

        #endregion

        #region QuadrantDBlurRadius

        /// <summary>
        /// QuadrantDBlurRadius Dependency Property
        /// </summary>
        public static readonly DependencyProperty QuadrantDBlurRadiusProperty =
            DependencyProperty.Register("QuadrantDBlurRadius", typeof(Double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Double)0.0,
                    new PropertyChangedCallback(OnQuadrantDBlurRadiusChanged)));

        /// <summary>
        /// Gets or sets the QuadrantDBlurRadius property.  This dependency property 
        /// indicates the initial position of the item in quadrant D, at 270 degrees.
        /// </summary>
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

        #endregion

        #region IsBlurEnabled

        /// <summary>
        /// IsBlurEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsBlurEnabledProperty =
            DependencyProperty.Register("IsBlurEnabled", typeof(bool), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((bool)true,
                    new PropertyChangedCallback(OnIsBlurEnabledChanged)));

        /// <summary>
        /// Gets or sets the IsBlurEnabled property.  This dependency property 
        /// indicates ....
        /// </summary>
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

        #endregion

        #region VanishingPoint

        /// <summary>
        /// VanishingPoint Dependency Property
        /// </summary>
        public static readonly DependencyProperty VanishingPointProperty =
            DependencyProperty.Register("VanishingPoint", typeof(Quadrant), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((Quadrant)Quadrant.Unknown,
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

        #endregion

        #region ItemDelay

        /// <summary>
        /// ItemDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemDelayProperty =
            DependencyProperty.Register("ItemDelay", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnItemDelayChanged)));

        /// <summary>
        /// Gets or sets the ItemDelay property.  This dependency property 
        /// indicates the delay between item changes.
        /// </summary>
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

        #endregion

        #region NewItemAngle

        /// <summary>
        /// NewItemAngle Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemAngleProperty =
            DependencyProperty.Register("NewItemAngle", typeof(double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double)0.0,
                    new PropertyChangedCallback(OnNewItemAngleChanged)));

        /// <summary>
        /// Gets or sets the NewItemAngle property.  This dependency property 
        /// indicates the angle where new items are inserted.
        /// </summary>
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

        #endregion

        #region NewItemScrollPosition

        /// <summary>
        /// NewItemScrollPosition Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemScrollPositionProperty =
            DependencyProperty.Register("NewItemScrollPosition", typeof(double?), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double?)null,
                    new PropertyChangedCallback(OnNewItemScrollPositionChanged)));

        /// <summary>
        /// Gets or sets the NewItemScrollPosition property.  This dependency property 
        /// indicates the scroll position where the new item will animate from.
        /// </summary>
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

        #endregion

        #region NewItemDelay

        /// <summary>
        /// NewItemDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemDelayProperty =
            DependencyProperty.Register("NewItemDelay", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnNewItemDelayChanged)));

        /// <summary>
        /// Gets or sets the NewItemDelay property.  This dependency property 
        /// indicates time to wait before animating in items added to the carousel.
        /// </summary>
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

        #endregion

        #region NewItemDuration

        /// <summary>
        /// NewItemDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty NewItemDurationProperty =
            DependencyProperty.Register("NewItemDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnNewItemDurationChanged)));

        /// <summary>
        /// Gets or sets the NewItemDuration property.  This dependency property 
        /// indicates the duration of the animation to play when new items are added to the carousel.
        /// </summary>
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

        #endregion

        #region NewItemEasingFunction

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
        public IEasingFunction NewItemEasingFunction
        {
            get { return (IEasingFunction)GetValue(NewItemEasingFunctionProperty); }
            set { SetValue(NewItemEasingFunctionProperty, value); }
        }

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

        #endregion

        #region ItemRemovalIsAnimated

        /// <summary>
        /// ItemRemovalIsAnimated Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalIsAnimatedProperty =
            DependencyProperty.Register("ItemRemovalIsAnimated", typeof(bool), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnItemRemovalIsAnimatedChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalIsAnimated property.  This dependency property 
        /// indicates whether the control should perform an animation when an item is removed.
        /// </summary>
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

        #endregion

        #region ItemRemovalEasingFunction

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
        public IEasingFunction ItemRemovalEasingFunction
        {
            get { return (IEasingFunction)GetValue(ItemRemovalEasingFunctionProperty); }
            set { SetValue(ItemRemovalEasingFunctionProperty, value); }
        }

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

        #endregion

        #region ItemRemovalDuration

        /// <summary>
        /// ItemRemovalDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalDurationProperty =
            DependencyProperty.Register("ItemRemovalDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnItemRemovalDurationChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalDuration property.  This dependency property 
        /// indicates the duration of the item removal animation.
        /// </summary>
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

        #endregion

        #region ItemRemovalDelay

        /// <summary>
        /// ItemRemovalDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalDelayProperty =
            DependencyProperty.Register("ItemRemovalDelay", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnItemRemovalDelayChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalDelay property.  This dependency property 
        /// indicates the delay to introduce between items when performing the item removal animation.
        /// </summary>
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

        #endregion

        #region ItemRemovalDelayMode

        /// <summary>
        /// ItemRemovalDelayMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemRemovalDelayModeProperty =
            DependencyProperty.Register("ItemRemovalDelayMode", typeof(DelayMode), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((DelayMode)DelayMode.None,
                    new PropertyChangedCallback(OnItemRemovalDelayModeChanged)));

        /// <summary>
        /// Gets or sets the ItemRemovalDelayMode property.  This dependency property 
        /// indicates the delay mode to use when animating an item removal.
        /// </summary>
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

        #endregion

        #region Idle Properties

        #region IsIdle

        /// <summary>
        /// IsIdle Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsIdleProperty =
            DependencyProperty.Register("IsIdle", typeof(bool), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnIsIdleChanged)));

        /// <summary>
        /// Gets or sets the IsIdle property.  This dependency property 
        /// indicates whether the control is currently idle. It is possible to set this property to true so that the control starts scrolling itself regularly.
        /// </summary>
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
            if (IsIdle)
            {
                IdleScrollingTimer.Start();
            }
            else
            {
                IdleScrollingTimer.Stop();
                StopIdleScrollingAnimation();
            }
        }

        #endregion

        #region IdleScrollingInterval

        /// <summary>
        /// IdleScrollingInterval Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingIntervalProperty =
            DependencyProperty.Register("IdleScrollingInterval", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)new TimeSpan(0, 0, 0, 15, 0),
                    new PropertyChangedCallback(OnIdleScrollingIntervalChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingInterval property.  This dependency property 
        /// indicates the interval between automatic idle animations.
        /// </summary>
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

        #endregion

        #region IdleScrollingVariation

        /// <summary>
        /// IdleScrollingVariation Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingVariationProperty =
            DependencyProperty.Register("IdleScrollingVariation", typeof(double), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((double)15.0,
                    new PropertyChangedCallback(OnIdleScrollingVariationChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingVariation property.  This dependency property 
        /// indicates the variation of the automatic idle scrolling. Each idle scrolling will take up to +/- the value of this property on each iteration.
        /// </summary>
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

        #endregion



        private DateTime latestIdleAnimation;

        static Random randomGenerator = new Random();

        Timer IdleScrollingTimer = new Timer(1000);

        void IdleScrollingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
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
            }));
        }

        #region IdleScrolling Animation Properties

        #region IdleScrollingDuration

        /// <summary>
        /// IdleScrollingDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty IdleScrollingDurationProperty =
            DependencyProperty.Register("IdleScrollingDuration", typeof(TimeSpan), typeof(CarouselItemsControl),
                new FrameworkPropertyMetadata((TimeSpan)new TimeSpan(0, 0, 0, 3, 800),
                    new PropertyChangedCallback(OnIdleScrollingDurationChanged)));

        /// <summary>
        /// Gets or sets the IdleScrollingDuration property.  This dependency property 
        /// indicates the duration of the IdleScrolling animation.
        /// </summary>
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

        #endregion
        #region IdleScrollingEasingFunction

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
        public IEasingFunction IdleScrollingEasingFunction
        {
            get { return (IEasingFunction)GetValue(IdleScrollingEasingFunctionProperty); }
            set { SetValue(IdleScrollingEasingFunctionProperty, value); }
        }

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

        #endregion

        Polaris.Client.Controls.Wpf.Animation.DoubleAnimationHelper IdleScrollingAnimationHelper;

        private void InitializeIdleScrollingAnimation()
        {
            IdleScrollingAnimationHelper =
                new Polaris.Client.Controls.Wpf.Animation.DoubleAnimationHelper
                    (
                    OnIdleScrollingAnimationCompleted,
                    OnIdleScrollingAnimationProgress
                    );
            IdleScrollingAnimationHelper.Duration = IdleScrollingDuration;
            IdleScrollingAnimationHelper.EasingFunction = IdleScrollingEasingFunction;
            IdleScrollingTimer.Elapsed += new ElapsedEventHandler(IdleScrollingTimer_Elapsed);
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

        #endregion


        #endregion

        List<CarouselItemAnimationRequest> pendingAnimationRequests = new List<CarouselItemAnimationRequest>();
        List<CarouselNewItemAnimationRequest> pendingOpacityAnimationRequests = new List<CarouselNewItemAnimationRequest>();

        CarouselItem currentItemOnVanishingPoint = null;

        /// <summary>
        /// Number of items that have already been generated.
        /// </summary>
        private Int32 generatedItemCount = 0;

        static CarouselItemsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CarouselItemsControl), new FrameworkPropertyMetadata(typeof(CarouselItemsControl)));
        }

        public CarouselItemsControl()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(ProcessChanges);
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            InitializeIdleScrollingAnimation();
        }



        public void ProcessChanges(Object state)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(20);
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
                    this.Dispatcher.BeginInvoke(new Action<ItemChangeSet>(itemChangeSet =>
                    {
                        ApplyChangeSet(itemChangeSet);
                    }), changeSet);
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

            public Size QuadrantASize { get; set; }
            public Size QuadrantBSize { get; set; }
            public Size QuadrantCSize { get; set; }
            public Size QuadrantDSize { get; set; }

            public Double ScrollStep { get; set; }
            public Double TotalArc { get; set; }
            public Double ScrollPosition { get; set; }
        }

        Object pendingChangesSync = new object();
        List<ItemChangeSet> pendingChanges = new List<ItemChangeSet>();

        void UpdateItems()
        {
            UpdateItems(this.ScrollPosition, this.ScrollPosition);
        }

        void UpdateItems(double oldValue, double newValue)
        {
            var delta = newValue - oldValue;
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
                return;
            }
            for (int i = 0; i < this.Items.Count; i++)
            {
                var itemContainer = ItemContainerGenerator.ContainerFromIndex(i) as CarouselItem;
                var itemScrollPosition = i * ItemSeparation;
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
                itemContainer.ScrollPosition = itemContainer.ScrollPosition + delta;
                itemContainer.IsBlurEnabled = this.IsBlurEnabled;
                itemContainer.IsSynchronized = true;
            }


            UpdateVanishingPointItem();


        }


        double GetVanishingPointAngle()
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

        void UpdateVanishingPointItem()
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

        void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            ItemContainerGenerator.StatusChanged -= new EventHandler(ItemContainerGenerator_StatusChanged);
            UpdateItems();
        }

        void UpdateItemsWithDelay(Double oldValue, Double newValue)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.NotStarted) { return; }
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

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ITEM(s) REMOVED:");
                foreach (var item in e.OldItems)
                {
                    System.Diagnostics.Debug.WriteLine("Item hash code: {0}", item.GetHashCode());
                }
#endif
                ProcessSingleItemRemoval();
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("ITEM(s) ADDED:");
                foreach (var item in e.NewItems)
                {
                    System.Diagnostics.Debug.WriteLine("Item hash code: {0}", item.GetHashCode());
                }
#endif

            }
            UpdateVanishingPointItem();
            base.OnItemsChanged(e);
        }

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
                System.Diagnostics.Debug.WriteLine("Animating {0} items", animationRequests.Count);
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
