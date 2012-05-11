using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Polaris.Services;

namespace Polaris.Kinect
{
    public class KinectUiElementController : IKinectUiElementController
    {
        public IKinectUiControl AssociatedControl { get; set; }

        public WeakReference associatedFrameworkElementReference;

        public FrameworkElement AssociatedFrameworkElement
        {
            get
            {
                if (!associatedFrameworkElementReference.IsAlive)
                {
                    return null;
                }
                var frameworkElement = associatedFrameworkElementReference.Target as FrameworkElement;
                return frameworkElement;
            }
        }

        public IKinectUiService KinectUiService { get; set; }

        public KinectUiElementController(IKinectUiService kinectUiService)
        {
            this.KinectUiService = kinectUiService;
        }

        public IKinectUiElementController Initialize(IKinectUiControl element)
        {
            this.associatedFrameworkElementReference = new WeakReference(element);
            this.AssociatedControl = element;
            if (AssociatedFrameworkElement == null)
            {
                throw new InvalidOperationException("IKinectUiControl must be a FrameworkElement");
            }
            this.KinectUiService.RegisterControl(this.AssociatedControl);
            return this;
        }

        #region IKinectUiControl Members

        public int ZIndex
        {
            get { return Panel.GetZIndex(this.AssociatedFrameworkElement); }
        }

        public bool IsKinectVisible
        {
            get
            {
                return this.AssociatedFrameworkElement.IsHitTestVisible && this.AssociatedFrameworkElement.IsVisible && this.AssociatedFrameworkElement.IsEnabled;
            }
        }

        public bool Visible
        {
            get { return (this.AssociatedFrameworkElement.Visibility == Visibility.Visible); }
        }

        public bool ContainsPoint(double normalizedX, double normalizedY)
        {
            var positionX = Application.Current.MainWindow.ActualWidth * normalizedX;
            var positionY = Application.Current.MainWindow.ActualHeight * normalizedY;

            var transform = Application.Current.MainWindow.TranslatePoint(new Point(positionX, positionY), this.AssociatedFrameworkElement);
            var result = VisualTreeHelper.HitTest(this.AssociatedFrameworkElement, transform);
            var containsPoint = result != null && result.VisualHit != null;
            return containsPoint;
        }

        public bool IsActivationEnabled { get; protected set; }

        #endregion IKinectUiControl Members

        public void Detach()
        {
            if (this.AssociatedControl != null)
            {
                this.KinectUiService.UnregisterControl(this.AssociatedControl);
            }
        }
    }
}