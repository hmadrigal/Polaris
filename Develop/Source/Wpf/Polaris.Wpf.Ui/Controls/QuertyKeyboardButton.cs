//-----------------------------------------------------------------------
// <copyright file="QuertyKeyboardButton.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Media.Animation;
    using System.Windows.Controls;
using System.Windows.Threading;

    [TemplatePart(Name = PressedStoryboardPartName, Type = typeof(Storyboard))]
    public class QuertyKeyboardButton : Button
    {

        #region PressedStoryboard Template Part
        private const string PressedStoryboardPartName = "PressedStoryboard";
        private Storyboard PressedStoryboardPart;
        #endregion

        DispatcherTimer _timer;

        public QuertyKeyboardButton()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += _timer_Tick;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            if (timer.Interval.TotalMilliseconds > 100)
	        {
                timer.Interval = TimeSpan.FromMilliseconds(timer.Interval.TotalMilliseconds / 2);
	        }
            OnMouseDown(default(System.Windows.Input.MouseButtonEventArgs));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PressedStoryboardPart = this.GetTemplateChild(PressedStoryboardPartName) as Storyboard;
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            TryPlayPressedStoryboard();
            base.OnMouseDown(e);
            if (!_timer.IsEnabled)
            {
                _timer.IsEnabled = true;
            }
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            _timer.IsEnabled = false;
        }

        private void TryPlayPressedStoryboard()
        {
            if (PressedStoryboardPart != null)
            {
                PressedStoryboardPart.Begin();
            }
        }


    }
}
