using System;
using Polaris.PhoneLib.SystemEx;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Expression.Interactivity;

namespace Polaris.PhoneLib.Toolkit.Controls
{
    [TemplateVisualState(Name = StartCountingDown, GroupName = CountDownStatesGroupName)]
    [TemplateVisualState(Name = CompleteCountingDown, GroupName = CountDownStatesGroupName)]
    [TemplateVisualState(Name = InterruptCountingDown, GroupName = CountDownStatesGroupName)]
    public class CountDownButton : ButtonBase
    {

        #region Visual States

        #region CountDownStatesGroupName

        /// <summary>
        /// States to be supported in order when counting down starts or stops
        /// </summary>
        protected const string CountDownStatesGroupName = "CountDownStates";

        protected const string StartCountingDown = @"StartCountingDownState";
        protected const string CompleteCountingDown = @"CompleteCountingDownState";
        protected const string InterruptCountingDown = @"InterruptCountingDownState";

        #endregion CountDownStatesGroupName



        #endregion Visual States

        private Dictionary<string, VisualState> _countDownVisualStates;

        public CountDownButton()
        {
            Unloaded += OnCountDownButtonUnloaded;
            Loaded += OnCountDownButtonLoaded;
        }

        void OnCountDownButtonLoaded(object sender, RoutedEventArgs e)
        {
            if (_countDownVisualStates != null)
            {
                Unsubscribe(_countDownVisualStates);
                _countDownVisualStates.Clear();
                _countDownVisualStates = null;
            }

            var countDownStatesVisualStateGroup = VisualStateManager.GetVisualStateGroups(this).OfType<VisualStateGroup>().FirstOrDefault(vsg => vsg.Name == CountDownStatesGroupName);
            
            if (countDownStatesVisualStateGroup != null)
            {
                _countDownVisualStates = countDownStatesVisualStateGroup.States.OfType<VisualState>().ToDictionary(vs => vs.Name, vs => vs);
                Subscribe(_countDownVisualStates);
            }
        }

        void OnCountDownButtonUnloaded(object sender, RoutedEventArgs e)
        {
            var countDownButton = sender as CountDownButton;
            countDownButton.Unloaded += OnCountDownButtonUnloaded;
            if (_countDownVisualStates != null)
            {
                Unsubscribe(_countDownVisualStates);
                _countDownVisualStates.Clear();
                _countDownVisualStates = null;
            }
        }

        

        private void Subscribe(Dictionary<string, VisualState> countDownVisualStates)
        {
            foreach (var visualStateKvp in countDownVisualStates)
            {
                switch (visualStateKvp.Key)
                {
                    case StartCountingDown:
                        visualStateKvp.Value.Storyboard.Completed += OnStartCountingDownStoryboardCompleted;
                        break;
                    case CompleteCountingDown:
                        visualStateKvp.Value.Storyboard.Completed += OnCompleteCountingDownStoryboardCompleted;
                        break;
                    case InterruptCountingDown:
                        visualStateKvp.Value.Storyboard.Completed += OnInterruptCountingDownStoryboardCompleted;
                        break;
                }
            }
        }

        private void Unsubscribe(Dictionary<string, VisualState> countDownVisualStates)
        {
            foreach (var visualStateKvp in countDownVisualStates)
            {
                switch (visualStateKvp.Key)
                {
                    case StartCountingDown:
                        visualStateKvp.Value.Storyboard.Completed -= OnStartCountingDownStoryboardCompleted;
                        break;
                    case CompleteCountingDown:
                        visualStateKvp.Value.Storyboard.Completed -= OnCompleteCountingDownStoryboardCompleted;
                        break;
                    case InterruptCountingDown:
                        visualStateKvp.Value.Storyboard.Completed -= OnInterruptCountingDownStoryboardCompleted;
                        break;
                }
            }
        }

        protected virtual void OnInterruptCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected virtual void OnCompleteCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected virtual void OnStartCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected override void OnIsPressedChanged(System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var newIsPressedValue = (bool)e.NewValue;
            var oldIsPressedValue = (bool)e.OldValue;
            if (newIsPressedValue && !oldIsPressedValue)
            {
                // Start counting down
            }
            base.OnIsPressedChanged(e);
        }
    }
}
