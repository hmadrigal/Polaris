using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Carousel.Rt
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        TaskScheduler _uiTaskScheduler;

        DateTime? latestMouseWheelEvent;

        TimeSpan idleTimeout = TimeSpan.FromMilliseconds(200);

        int latestMouseWheelDelta;

        public MainPage()
        {
            this.InitializeComponent();
            _uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            StartCarouselDemoProcessAsync();
            Window.Current.CoreWindow.PointerWheelChanged += CoreWindow_PointerWheelChanged;
        }



        void CoreWindow_PointerWheelChanged(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.PointerEventArgs args)
        {

            if (Carousel.IsAnimating) { return; }

            latestMouseWheelEvent = DateTime.Now;
            latestMouseWheelDelta = args.CurrentPoint.Properties.MouseWheelDelta;

            var animatedScrollPosition = Carousel.AnimatedScrollPosition;
            animatedScrollPosition += latestMouseWheelDelta * 0.1;
            if (animatedScrollPosition == -360)
            {
                Carousel.ScrollPosition = 30;
                animatedScrollPosition = 0;
            }
            Carousel.AnimatedScrollPosition = animatedScrollPosition;

        }

        private async void StartCarouselDemoProcessAsync()
        {
            Action updateScrollPosition = new Action(() =>
            {
                var targetScrollPosition = Carousel.ScrollPosition;
                targetScrollPosition = ((int)(targetScrollPosition / 30)) * 30;

                if (latestMouseWheelDelta > 0)
                {
                    targetScrollPosition += 30;
                }
                else
                {
                    targetScrollPosition -= 30;
                }

                var animatedScrollPosition = targetScrollPosition;
                if (animatedScrollPosition == -360)
                {
                    Carousel.ScrollPosition = 30;
                    animatedScrollPosition = 0;
                }
                Carousel.AnimatedScrollPosition = animatedScrollPosition;
            });

            while (true)
            {
                await Task.Delay(60);

                var isAutoScrollingEnabled = false;

                if (latestMouseWheelEvent != null)
                {
                    var elapsedTimeSinceLatestMouseWheelEvent = DateTime.Now - latestMouseWheelEvent.Value;
                    if (elapsedTimeSinceLatestMouseWheelEvent > idleTimeout)
                    {
                        isAutoScrollingEnabled = true;
                        latestMouseWheelEvent = null;
                    }
                }

                if (isAutoScrollingEnabled)
                {
                    var updateScrollPositionTask = new Task(updateScrollPosition);
                    updateScrollPositionTask.Start(_uiTaskScheduler);
                    isAutoScrollingEnabled = false;
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
