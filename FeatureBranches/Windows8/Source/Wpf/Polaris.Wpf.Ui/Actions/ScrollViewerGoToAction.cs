//-----------------------------------------------------------------------
// <copyright file="ScrollViewerGoToAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;

    [TypeConstraint(typeof(ScrollViewer))]
    public class ScrollViewerGoToAction : TargetedTriggerAction<UIElement>
    {
        #region ScrollTo

        /// <summary>
        /// ScrollTo Dependency Property
        /// </summary>
        public static readonly DependencyProperty ScrollToProperty =
            DependencyProperty.Register("ScrollTo", typeof(ScrollViewerGoToOptions), typeof(ScrollViewerGoToAction),
                new FrameworkPropertyMetadata(ScrollViewerGoToOptions.NonSet));

        /// <summary>
        /// Gets or sets the ScrollTo property.  This dependency property
        /// indicates ....
        /// </summary>
        public ScrollViewerGoToOptions ScrollTo
        {
            get { return (ScrollViewerGoToOptions)GetValue(ScrollToProperty); }
            set { SetValue(ScrollToProperty, value); }
        }

        #endregion ScrollTo

        #region HorizontalOffset

        /// <summary>
        /// HorizontalOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(ScrollViewerGoToAction),
                new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the HorizontalOffset property.  This dependency property
        /// indicates ....
        /// </summary>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        #endregion HorizontalOffset

        #region VerticalOffset

        /// <summary>
        /// VerticalOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register("VerticalOffset", typeof(double), typeof(ScrollViewerGoToAction),
                new FrameworkPropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the VerticalOffset property.  This dependency property
        /// indicates ....
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        #endregion VerticalOffset

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

        protected override void Invoke(object parameter)
        {
            if (this.AssociatedScrollViewer != null && ScrollTo != ScrollViewerGoToOptions.NonSet)
            {
                if ((ScrollTo & ScrollViewerGoToOptions.Bottom) == ScrollViewerGoToOptions.Bottom)
                {
                    this.AssociatedScrollViewer.ScrollToBottom();
                }
                if ((ScrollTo & ScrollViewerGoToOptions.End) == ScrollViewerGoToOptions.End)
                {
                    this.AssociatedScrollViewer.ScrollToEnd();
                }
                if ((ScrollTo & ScrollViewerGoToOptions.Home) == ScrollViewerGoToOptions.Home)
                {
                    this.AssociatedScrollViewer.ScrollToHome();
                }
                if ((ScrollTo & ScrollViewerGoToOptions.HorizontalOffSet) == ScrollViewerGoToOptions.HorizontalOffSet)
                {
                    this.AssociatedScrollViewer.ScrollToHorizontalOffset(this.HorizontalOffset);
                }
                if ((ScrollTo & ScrollViewerGoToOptions.LeftEnd) == ScrollViewerGoToOptions.LeftEnd)
                {
                    this.AssociatedScrollViewer.ScrollToLeftEnd();
                }
                if ((ScrollTo & ScrollViewerGoToOptions.RightEnd) == ScrollViewerGoToOptions.RightEnd)
                {
                    this.AssociatedScrollViewer.ScrollToRightEnd();
                }
                if ((ScrollTo & ScrollViewerGoToOptions.Top) == ScrollViewerGoToOptions.Top)
                {
                    this.AssociatedScrollViewer.ScrollToTop();
                }
                if ((ScrollTo & ScrollViewerGoToOptions.VerticalOffset) == ScrollViewerGoToOptions.VerticalOffset)
                {
                    this.AssociatedScrollViewer.ScrollToVerticalOffset(this.VerticalOffset);
                }
            }
        }
    }

    [Flags]
    public enum ScrollViewerGoToOptions : int
    {
        NonSet = 0,
        Bottom = 1,
        End = 2,
        Home = 4,
        HorizontalOffSet = 8,
        LeftEnd = 16,
        RightEnd = 32,
        Top = 64,
        VerticalOffset = 128
    }
}