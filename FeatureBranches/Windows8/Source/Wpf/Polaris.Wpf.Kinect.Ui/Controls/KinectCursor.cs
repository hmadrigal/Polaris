namespace Polaris.Windows.Controls
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Polaris.Services;

    /// <summary>
    /// A cursor that moves on screen based on the skeletal input of the Kinect sensor.
    /// </summary>
    public class KinectCursor : Control, IKinectCursor
    {
        TranslateTransform translateTransform;

        Point absolutePosition;

        #region ReferenceContainer

        /// <summary>
        /// ReferenceContainer Dependency Property
        /// </summary>
        public static readonly DependencyProperty ReferenceContainerProperty =
            DependencyProperty.Register("ReferenceContainer", typeof(FrameworkElement), typeof(KinectCursor),
                new FrameworkPropertyMetadata((FrameworkElement)null,
                    new PropertyChangedCallback(OnReferenceContainerChanged)));

        /// <summary>
        /// Gets or sets the ReferenceContainer property.  This dependency property
        /// indicates the container to use as a reference for the cursor to move inside.
        /// </summary>
        public FrameworkElement ReferenceContainer
        {
            get { return (FrameworkElement)GetValue(ReferenceContainerProperty); }
            set { SetValue(ReferenceContainerProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ReferenceContainer property.
        /// </summary>
        private static void OnReferenceContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KinectCursor)d).OnReferenceContainerChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the ReferenceContainer property.
        /// </summary>
        protected virtual void OnReferenceContainerChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion ReferenceContainer

        #region KinectUiService

        /// <summary>
        /// KinectUiService Dependency Property
        /// </summary>
        public static readonly DependencyProperty KinectUiServiceProperty =
            DependencyProperty.Register("KinectUiService", typeof(IKinectUiService), typeof(KinectCursor),
                new FrameworkPropertyMetadata((IKinectUiService)null,
                    new PropertyChangedCallback(OnKinectUiServiceChanged)));

        /// <summary>
        /// Gets or sets the KinectUiService property.  This dependency property
        /// indicates the associated Kinect UI Service that controls this cursor.
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
            ((KinectCursor)d).HandleKinectUiServiceChanged(e);
        }

        private void HandleKinectUiServiceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((IKinectUiService)e.OldValue).UnregisterCursor(this);
            }
            if (e.NewValue != null)
            {
                ((IKinectUiService)e.NewValue).RegisterCursor(this);
            }
            OnKinectUiServiceChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KinectUiService property.
        /// </summary>
        protected virtual void OnKinectUiServiceChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion KinectUiService

        static KinectCursor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KinectCursor), new FrameworkPropertyMetadata(typeof(KinectCursor)));
        }

        public KinectCursor()
        {
            LayoutUpdated += new EventHandler(KinectCursor_LayoutUpdated);
        }

        private void KinectCursor_LayoutUpdated(object sender, EventArgs e)
        {
            var renderTransformOriginPoint = new Point(RenderTransformOrigin.X * ActualWidth, RenderTransformOrigin.Y * ActualHeight);
            absolutePosition = this.TranslatePoint(renderTransformOriginPoint, GetReferenceContainer());
            LayoutUpdated -= new EventHandler(KinectCursor_LayoutUpdated);
        }

        FrameworkElement element;

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        #region IKinectCursor Members

        public void SetPosition(double normalizedX, double normalizedY)
        {
            var referenceContainer = GetReferenceContainer();
            if (referenceContainer == null) { return; }
            var targetX = normalizedX * referenceContainer.ActualWidth;
            var targetY = normalizedY * referenceContainer.ActualHeight;

            var offsetX = targetX - absolutePosition.X;
            var offsetY = targetY - absolutePosition.Y;
            translateTransform.X = offsetX;
            translateTransform.Y = offsetY;
        }

        public void SetActivationCountdownProgress(double normalizedActivationTime)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.ActivationCountdownProgress = normalizedActivationTime;
                if (ActivationCountdownProgress == 1.0)
                {
                    VisualStateManager.GoToState(this, KinectVisualStates.ActivationCompletedState, true);
                }

                if (ActivationCountdownProgress > 0 && ActivationCountdownProgress < 1.0)
                {
                    VisualStateManager.GoToState(this, KinectVisualStates.CursorActivationState, true);
                }
            }));
        }

        public void StopActivationCountdown()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.ActivationCountdownProgress < 1.0)
                {
                    this.ActivationCountdownProgress = 0.0;
                    VisualStateManager.GoToState(this, KinectVisualStates.ActivationCanceledState, true);
                }
                else
                {
                    this.ActivationCountdownProgress = 0.0;
                    VisualStateManager.GoToState(this, KinectVisualStates.NormalState, true);
                }
            }));
        }

        #endregion IKinectCursor Members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeTranslateTransform();
        }

        private void InitializeTranslateTransform()
        {
            var transformGroup = this.RenderTransform as TransformGroup;

            if (transformGroup != null)
            {
                translateTransform = (from TranslateTransform transform in transformGroup.Children
                                      select transform).FirstOrDefault();
            }
            if (translateTransform == null)
            {
                translateTransform = this.RenderTransform as TranslateTransform;
            }
            if (translateTransform == null)
            {
                translateTransform = new TranslateTransform();
                this.RenderTransform = translateTransform;
            }
        }

        private FrameworkElement GetReferenceContainer()
        {
            if (ReferenceContainer == null)
            {
                return Application.Current.MainWindow;
            }
            else
            {
                return ReferenceContainer;
            }
        }

        #region ActivationCountdownProgress

        /// <summary>
        /// ActivationCountdownProgress Dependency Property
        /// </summary>
        public static readonly DependencyProperty ActivationCountdownProgressProperty =
            DependencyProperty.Register("ActivationCountdownProgress", typeof(Double), typeof(KinectCursor),
                new FrameworkPropertyMetadata((Double)0));

        /// <summary>
        /// Gets or sets the ActivationCountdownProgress property.  This dependency property
        /// indicates the progress of the activation process.
        /// </summary>
        public Double ActivationCountdownProgress
        {
            get { return (Double)GetValue(ActivationCountdownProgressProperty); }
            set { SetValue(ActivationCountdownProgressProperty, value); }
        }

        #endregion ActivationCountdownProgress
    }
}