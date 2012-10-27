namespace Polaris.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Polaris.Kinect;

    /// <summary>
    /// A button which is activated using the Kinect Sensor.
    /// </summary>
    public class KinectHoverButton : Button, IKinectUiControl
    {
        public const double DefaultActivationTime = 2000;

        static KinectHoverButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KinectHoverButton), new FrameworkPropertyMetadata(typeof(KinectHoverButton)));
        }

        public KinectHoverButton()
        {
            this.ActivationTime = DefaultActivationTime;
        }

        #region KinectUiElementController

        /// <summary>
        /// KinectUiElementController Dependency Property
        /// </summary>
        public static readonly DependencyProperty KinectUiElementControllerProperty =
            DependencyProperty.Register("KinectUiElementController", typeof(IKinectUiElementController), typeof(KinectHoverButton),
                new FrameworkPropertyMetadata((IKinectUiElementController)default(IKinectUiElementController),
                    new PropertyChangedCallback(OnKinectUiElementControllerChanged)));

        /// <summary>
        /// Gets or sets the KinectUiElementController property.  This dependency property
        /// indicates ....
        /// </summary>
        [Category(KinectCategories.Kinect)]
        public IKinectUiElementController KinectUiElementController
        {
            get { return (IKinectUiElementController)GetValue(KinectUiElementControllerProperty); }
            set { SetValue(KinectUiElementControllerProperty, value); }
        }

        /// <summary>
        /// Handles changes to the KinectUiElementController property.
        /// </summary>
        private static void OnKinectUiElementControllerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KinectHoverButton)d).OnKinectUiElementControllerChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KinectUiElementController property.
        /// </summary>
        protected virtual void OnKinectUiElementControllerChanged(DependencyPropertyChangedEventArgs e)
        {
            var newController = e.NewValue as IKinectUiElementController;
            newController.Initialize(this);
            controller = newController;
        }

        private IKinectUiElementController controller;

        public IKinectUiElementController GetKinectUiElementController()
        {
            return controller;
        }

        #endregion KinectUiElementController

        #region KinectActivationTime

        /// <summary>
        /// KinectActivationTime Dependency Property
        /// </summary>
        public static readonly DependencyProperty KinectActivationTimeProperty =
            DependencyProperty.Register("KinectActivationTime", typeof(TimeSpan), typeof(KinectHoverButton),
                new FrameworkPropertyMetadata((TimeSpan)TimeSpan.FromMilliseconds(DefaultActivationTime),
                    new PropertyChangedCallback(OnKinectActivationTimeChanged)));

        /// <summary>
        /// Gets or sets the KinectActivationTime property.  This dependency property
        /// indicates the time required to activate this kinect control.
        /// </summary>
        [Category(KinectCategories.Kinect)]
        public TimeSpan KinectActivationTime
        {
            get { return (TimeSpan)GetValue(KinectActivationTimeProperty); }
            set { SetValue(KinectActivationTimeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the KinectActivationTime property.
        /// </summary>
        private static void OnKinectActivationTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KinectHoverButton)d).OnKinectActivationTimeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KinectActivationTime property.
        /// </summary>
        protected virtual void OnKinectActivationTimeChanged(DependencyPropertyChangedEventArgs e)
        {
            this.ActivationTime = ((TimeSpan)e.NewValue).TotalMilliseconds;
        }

        #endregion KinectActivationTime

        #region IKinectUiControl Members

        public bool IsActivationEnabled
        {
            get { return true; }
        }

        public double? ActivationTime { get; set; }

        public void TriggerCursorMove(IKinectUiEventArgs args)
        {
            OnKinectCursorMove(args);
            if (args.IsHandled) { return; }
            var threadSafeKinectCursorMove = KinectCursorMove;
            if (threadSafeKinectCursorMove != null)
            {
                threadSafeKinectCursorMove(this, KinectUiEventArgs.FromContract(args));
            }
        }

        public void TriggerCursorEnter(IKinectUiEventArgs args)
        {
            OnKinectCursorEnter(args);
            if (args.IsHandled) { return; }
            var threadSafeKinectCursorEnter = KinectCursorEnter;
            if (threadSafeKinectCursorEnter != null)
            {
                threadSafeKinectCursorEnter(this, KinectUiEventArgs.FromContract(args));
            }
        }

        public void TriggerCursorLeave(IKinectUiEventArgs args)
        {
            OnKinectCursorLeave(args);
            if (args.IsHandled) { return; }
            var threadSafeKinectCursorLeave = KinectCursorLeave;
            if (threadSafeKinectCursorLeave != null)
            {
                threadSafeKinectCursorLeave(this, KinectUiEventArgs.FromContract(args));
            }
        }

        public void TriggerActivation(IKinectUiEventArgs args)
        {
            OnKinectActivation(args);
            if (args.IsHandled) { return; }
            var threadSafeKinectActivation = KinectActivation;
            if (threadSafeKinectActivation != null)
            {
                threadSafeKinectActivation(this, KinectUiEventArgs.FromContract(args));
            }
        }

        public event EventHandler<KinectUiEventArgs> KinectCursorEnter;

        protected virtual void OnKinectCursorEnter(IKinectUiEventArgs args)
        {
            VisualStateManager.GoToState(this, KinectVisualStates.HandOverState, true);
        }

        public event EventHandler<KinectUiEventArgs> KinectCursorLeave;

        protected virtual void OnKinectCursorLeave(IKinectUiEventArgs args)
        {
            VisualStateManager.GoToState(this, KinectVisualStates.HandLeaveState, true);
        }

        public event EventHandler<KinectUiEventArgs> KinectCursorMove;

        protected virtual void OnKinectCursorMove(IKinectUiEventArgs args)
        {
        }

        public event EventHandler<KinectUiEventArgs> KinectActivation;

        protected virtual void OnKinectActivation(IKinectUiEventArgs args)
        {
            VisualStateManager.GoToState(this, KinectVisualStates.ActivatedState, true);
        }

        #endregion IKinectUiControl Members

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        ~KinectHoverButton()
        {
            if (this.controller != null)
            {
                this.controller.Detach();
            }
        }
    }
}