//-----------------------------------------------------------------------
// <copyright file="AnimatedExpander.cs" company="">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Controls.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Polaris.Client.Controls.Wpf.Extensions;

    //NOTE: this class should not exist.  It is very trivial to make the stock expander act like this class (a simple animation)
    public class AnimatedExpander : ContentControl
    {

        #region CollapsedHeight

        /// <summary>
        /// CollapsedHeight Dependency Property
        /// </summary>
        public static readonly DependencyProperty CollapsedHeightProperty =
            DependencyProperty.Register("CollapsedHeight", typeof(double), typeof(AnimatedExpander),
                new FrameworkPropertyMetadata((double)50.0,
                    new PropertyChangedCallback(OnCollapsedHeightChanged)));

        /// <summary>
        /// Gets or sets the CollapsedHeight property.  This dependency property 
        /// indicates the fixed height of the control when collapsed.
        /// </summary>
        public double CollapsedHeight
        {
            get { return (double)GetValue(CollapsedHeightProperty); }
            set { SetValue(CollapsedHeightProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CollapsedHeight property.
        /// </summary>
        private static void OnCollapsedHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedExpander)d).OnCollapsedHeightChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CollapsedHeight property.
        /// </summary>
        protected virtual void OnCollapsedHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion

        #region IsExpanded

        /// <summary>
        /// IsExpanded Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(AnimatedExpander),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnIsExpandedChanged)));

        /// <summary>
        /// Gets or sets the IsExpanded property.  This dependency property 
        /// indicates whether the control is expanded or collapsed.
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsExpanded property.
        /// </summary>
        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedExpander)d).OnIsExpandedChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsExpanded property.
        /// </summary>
        protected virtual void OnIsExpandedChanged(DependencyPropertyChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

        #endregion

        #region CurrentSize

        /// <summary>
        /// CurrentSize Dependency Property
        /// </summary>
        private static readonly DependencyProperty CurrentSizeProperty =
            DependencyProperty.Register("CurrentSize", typeof(Size), typeof(AnimatedExpander),
                new FrameworkPropertyMetadata((Size)new Size(100, 50),
                    new PropertyChangedCallback(OnCurrentSizeChanged)));

        /// <summary>
        /// Gets or sets the CurrentSize property.  This dependency property 
        /// indicates the current size of the control.
        /// </summary>
        private Size CurrentSize
        {
            get { return (Size)GetValue(CurrentSizeProperty); }
            set { SetValue(CurrentSizeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CurrentSize property.
        /// </summary>
        private static void OnCurrentSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedExpander)d).OnCurrentSizeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CurrentSize property.
        /// </summary>
        protected virtual void OnCurrentSizeChanged(DependencyPropertyChangedEventArgs e)
        {
            InvalidateMeasure();
        }

        #endregion



        #region Animation Properties
        #region Duration

        /// <summary>
        /// Duration Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AnimatedExpander),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.Zero,
                    new PropertyChangedCallback(OnDurationChanged)));

        /// <summary>
        /// Gets or sets the Duration property.  This dependency property 
        /// indicates the duration of the expand/collapse animation.
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Duration property.
        /// </summary>
        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedExpander)d).OnDurationChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Duration property.
        /// </summary>
        protected virtual void OnDurationChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion
        #region EasingFunction

        /// <summary>
        /// EasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(AnimatedExpander),
                new FrameworkPropertyMetadata((IEasingFunction)null,
                    new PropertyChangedCallback(OnEasingFunctionChanged)));

        /// <summary>
        /// Gets or sets the EasingFunction property.  This dependency property 
        /// indicates the easing function to use on the expand animation.
        /// </summary>
        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the EasingFunction property.
        /// </summary>
        private static void OnEasingFunctionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedExpander)d).OnEasingFunctionChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the EasingFunction property.
        /// </summary>
        protected virtual void OnEasingFunctionChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion


        DateTime? StartTime;
        Size? InitialSize;
        Size? FinalSize;

        double? InitialVerticalOffset;
        double? FinalVerticalOffset;

        void ProcessExpandAnimation()
        {
            if (StartTime == null) { return; }
            var startTime = StartTime.Value;

            var elapsedTime = DateTime.Now.Subtract(startTime);

            if (elapsedTime < TimeSpan.Zero) { return; }

            var totalDuration = Duration;

            if (elapsedTime < totalDuration && this.IsVisible)
            {
                var normalizedTime = (Double)elapsedTime.Ticks / (Double)totalDuration.Ticks;
                var easing =
                    (EasingFunction == null ?
                        normalizedTime // if there is no easing function, use linear animation.
                        :
                        EasingFunction.Ease(normalizedTime));

                if (InitialSize != null && FinalSize != null)
                {
                    var currentWidth = InitialSize.Value.Width + ((FinalSize.Value.Width - InitialSize.Value.Width) * easing);
                    var currentHeight = InitialSize.Value.Height + ((FinalSize.Value.Height - InitialSize.Value.Height) * easing);

                    CurrentSize = new Size(currentWidth, currentHeight);
                }

                if (InitialVerticalOffset != null && FinalVerticalOffset != null)
                {
                    var currentVerticalOffset = InitialVerticalOffset.Value + ((FinalVerticalOffset.Value - InitialVerticalOffset.Value) * easing);
                    ParentScrollViewer.ScrollToVerticalOffset(currentVerticalOffset);
                }

            }
            else
            {
                CurrentSize = FinalSize.Value;
                if (ParentScrollViewer != null && FinalVerticalOffset != null)
                {
                    ParentScrollViewer.ScrollToVerticalOffset(FinalVerticalOffset.Value);
                    InitialVerticalOffset = null;
                    FinalVerticalOffset = null;
                }
                StartTime = null;
                InitialSize = null;
                FinalSize = null;
            }

        }

        #endregion

        ItemsControl ParentItemsControl;
        ScrollViewer ParentScrollViewer;
        ListBoxItem ParentListBoxItem;

        static AnimatedExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedExpander), new FrameworkPropertyMetadata(typeof(AnimatedExpander)));
        }

        public AnimatedExpander()
        {
            //CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            var renderingEventListener = new WeakEventListener<AnimatedExpander, object, EventArgs>(this);
            renderingEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.CompositionTarget_Rendering(source, eventArgs);
            renderingEventListener.OnDetachAction = (weakEventListenerParameter) =>
                CompositionTarget.Rendering -= weakEventListenerParameter.OnEvent;
            CompositionTarget.Rendering += renderingEventListener.OnEvent;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ParentItemsControl = this.FindVisualParent<ItemsControl>();
            ParentScrollViewer = this.FindVisualParent<ScrollViewer>();
            ParentListBoxItem = this.FindVisualParent<ListBoxItem>();
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            ProcessExpandAnimation();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var currentMeasure = base.MeasureOverride(constraint);
            if (IsExpanded)
            {
                FinalSize = currentMeasure;
                if (ParentScrollViewer != null)
                {
                    var scrollViewerPosition = ParentScrollViewer.GetAbsolutePosition(referenceVisual: ParentItemsControl);
                    var currentPosition = this.GetAbsolutePosition(referenceVisual: ParentItemsControl);
                    var lastVisibleY = ParentScrollViewer.ViewportHeight + scrollViewerPosition.Y;
                    var firstVisibleY = scrollViewerPosition.Y;
                    var currentBorderPosition = FinalSize.Value.Height + currentPosition.Y;
                    if (currentBorderPosition > lastVisibleY)
                    {
                        InitialVerticalOffset = ParentScrollViewer.VerticalOffset;
                        FinalVerticalOffset = InitialVerticalOffset.Value + (currentBorderPosition - lastVisibleY);
                    }
                    else if (firstVisibleY > currentPosition.Y)
                    {
                        InitialVerticalOffset = ParentScrollViewer.VerticalOffset;
                        FinalVerticalOffset = InitialVerticalOffset.Value - (firstVisibleY - currentPosition.Y);
                    }

                }
            }
            else
            {
                FinalSize = new Size(currentMeasure.Width, CollapsedHeight);
            }
            if (StartTime == null)
            {
                if (FinalSize != null && CurrentSize != FinalSize.Value)
                {
                    StartTime = DateTime.Now;
                    InitialSize = CurrentSize;
                }
                else if (InitialVerticalOffset != null && FinalVerticalOffset != null)
                {
                    StartTime = DateTime.Now;
                }
            }
            return CurrentSize;
        }


        #region Tap Gesture Processing
        DateTime? TapStartTime;
        Point? TapStartPosition;
        TimeSpan TapThreshold = new TimeSpan(0, 0, 0, 0, 400);
        Point TapPositionThreshold = new Point(3, 3);



        private void ProcessTapStart(Point position)
        {
            if (ParentListBoxItem == null) { return; }
            if (!ParentListBoxItem.IsSelected) { return; }
            if (TapStartTime != null || TapStartPosition != null) { return; }
            TapStartTime = DateTime.Now;
            TapStartPosition = position;
        }

        private void ProcessTapFinished(Point finalPosition)
        {
            if (ParentListBoxItem == null) { return; }
            if (!ParentListBoxItem.IsSelected) { return; }
            if (TapStartTime == null) { return; }
            if (TapStartPosition == null) { return; }
            var tapDuration = DateTime.Now.Subtract(TapStartTime.Value);
            var tapPositionDelta = new Point(Math.Abs(finalPosition.X - TapStartPosition.Value.X), Math.Abs(finalPosition.Y - TapStartPosition.Value.Y));
            if (tapDuration <= TapThreshold
                && tapPositionDelta.X < TapPositionThreshold.X
                && tapPositionDelta.Y < TapPositionThreshold.Y)
            {
                ParentListBoxItem.IsSelected = false;
            }
            TapStartTime = null;
            TapStartPosition = null;
        }

        protected override void OnTouchDown(System.Windows.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);
            var position = e.GetTouchPoint(this).Position;
            ProcessTapStart(position);
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            var position = e.GetPosition(this);
            ProcessTapStart(position);
        }

        protected override void OnTouchUp(System.Windows.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);
            var finalPosition = e.GetTouchPoint(this).Position;
            ProcessTapFinished(finalPosition);
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            var finalPosition = e.GetPosition(this);
            ProcessTapFinished(finalPosition);
        }

        #endregion Tap Gesture Processing

    }
}
