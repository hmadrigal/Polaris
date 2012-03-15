//-----------------------------------------------------------------------
// <copyright file="CaptureFrameworkElementAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [TypeConstraint(typeof(UIElement))]
    public class CaptureFrameworkElementAction : TargetedTriggerAction<FrameworkElement>
    {
        #region CapturedBitmap

        /// <summary>
        /// CapturedBitmap Dependency Property
        /// </summary>
        public static readonly DependencyProperty CapturedBitmapProperty =
            DependencyProperty.Register("CapturedBitmap", typeof(BitmapSource), typeof(CaptureFrameworkElementAction),
                new FrameworkPropertyMetadata((BitmapSource)null,
                    new PropertyChangedCallback(OnCapturedBitmapChanged)));

        /// <summary>
        /// Gets or sets the CapturedBitmap property.  This dependency property
        /// indicates the bitmap that is captured from the UIElement when the action is invoked.
        /// </summary>
        public BitmapSource CapturedBitmap
        {
            get { return (BitmapSource)GetValue(CapturedBitmapProperty); }
            set { SetValue(CapturedBitmapProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CapturedBitmap property.
        /// </summary>
        private static void OnCapturedBitmapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CaptureFrameworkElementAction)d).OnCapturedBitmapChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CapturedBitmap property.
        /// </summary>
        protected virtual void OnCapturedBitmapChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion CapturedBitmap

        #region CaptureCompletedCommand

        /// <summary>
        /// CaptureCompletedCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty CaptureCompletedCommandProperty =
            DependencyProperty.Register("CaptureCompletedCommand", typeof(ICommand), typeof(CaptureFrameworkElementAction),
                new FrameworkPropertyMetadata((ICommand)null));

        /// <summary>
        /// Gets or sets the CaptureCompletedCommand property.  This dependency property
        /// indicates when the capture process is completed.
        /// </summary>
        public ICommand CaptureCompletedCommand
        {
            get { return (ICommand)GetValue(CaptureCompletedCommandProperty); }
            set { SetValue(CaptureCompletedCommandProperty, value); }
        }

        #endregion CaptureCompletedCommand

        #region CaptureCompletedParameter

        /// <summary>
        /// CaptureCompletedParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty CaptureCompletedParameterProperty =
            DependencyProperty.Register("CaptureCompletedParameter", typeof(object), typeof(CaptureFrameworkElementAction),
                new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the CaptureCompletedParameter property.  This dependency property
        /// indicates the parameter to pass when the command is executed.
        /// </summary>
        public object CaptureCompletedParameter
        {
            get { return (object)GetValue(CaptureCompletedParameterProperty); }
            set { SetValue(CaptureCompletedParameterProperty, value); }
        }

        #endregion CaptureCompletedParameter

        protected override void Invoke(object parameter)
        {
            if (Target == null) { return; }
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)Target.ActualWidth, (int)Target.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(Target);
            CapturedBitmap = BitmapFrame.Create(renderTarget);
            OnCaptureCompleted();
        }

        private void OnCaptureCompleted()
        {
            if (CaptureCompletedCommand == null) { return; }
            if (!CaptureCompletedCommand.CanExecute(CaptureCompletedParameter)) { return; }
            CaptureCompletedCommand.Execute(CaptureCompletedParameter);
        }
    }
}