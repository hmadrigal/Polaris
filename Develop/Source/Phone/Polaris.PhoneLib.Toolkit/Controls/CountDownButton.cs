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
    [TemplateVisualState(Name = RestartCountingDown, GroupName = CountDownStatesGroupName)]
    public class CountDownButton : ButtonBase
    {
        #region CurrentCountDownState

        /// <summary>
        /// CurrentCountDownState Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentCountDownStateProperty =
            DependencyProperty.Register("CurrentCountDownState", typeof(CountDownState), typeof(CountDownButton),
                new PropertyMetadata(CountDownState.RestartCountingDown,
                    new PropertyChangedCallback(OnCurrentCountDownStateChanged)));

        /// <summary>
        /// Gets or sets the CurrentCountDownState property. This dependency property 
        /// indicates ....
        /// </summary>
        public CountDownState CurrentCountDownState
        {
            get { return (CountDownState)GetValue(CurrentCountDownStateProperty); }
            set { SetValue(CurrentCountDownStateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CurrentCountDownState property.
        /// </summary>
        private static void OnCurrentCountDownStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CountDownButton target = (CountDownButton)d;
            CountDownState oldCurrentCountDownState = (CountDownState)e.OldValue;
            CountDownState newCurrentCountDownState = target.CurrentCountDownState;
            target.OnCurrentCountDownStateChanged(oldCurrentCountDownState, newCurrentCountDownState);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CurrentCountDownState property.
        /// </summary>
        protected virtual void OnCurrentCountDownStateChanged(CountDownState oldCurrentCountDownState, CountDownState newCurrentCountDownState)
        {
            if (oldCurrentCountDownState != newCurrentCountDownState)
            {
                VisualStateManager.GoToState(this, newCurrentCountDownState.ToString(), true);
            }
        }

        #endregion

        #region Visual States

        #region CountDownStatesGroupName

        /// <summary>
        /// States to be supported in order when counting down starts or stops
        /// </summary>
        protected const string CountDownStatesGroupName = "CountDownStates";

        protected const string StartCountingDown = @"StartCountingDownState";
        protected const string CompleteCountingDown = @"CompleteCountingDownState";
        protected const string InterruptCountingDown = @"InterruptCountingDownState";
        protected const string RestartCountingDown = @"RestartCountingDown";

        #endregion CountDownStatesGroupName

        #endregion Visual States

        private Dictionary<string, VisualState> _countDownVisualStates;

        public CountDownButton()
        {
            //Unloaded += OnCountDownButtonUnloaded;
            Loaded += OnCountDownButtonLoaded;
        }

        public override void OnApplyTemplate()
        {
            RemoveVisualStateEventHandlers();
            base.OnApplyTemplate();
            var countDownStatesVisualStateGroup = VisualStateManager.GetVisualStateGroups(this).OfType<VisualStateGroup>().FirstOrDefault(vsg => vsg.Name == CountDownStatesGroupName);
            AddVisualStateEventHandlers(countDownStatesVisualStateGroup);
        }

        protected virtual void OnCountDownButtonLoaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, CurrentCountDownState.ToString(), false);
            //var countDownButton = sender as CountDownButton;
            //countDownButton.Loaded -= OnCountDownButtonLoaded;
            //ResetVisualStateEventHandlers();
        }

        //protected virtual void OnCountDownButtonUnloaded(object sender, RoutedEventArgs e)
        //{
        //    var countDownButton = sender as CountDownButton;
        //    countDownButton.Unloaded -= OnCountDownButtonUnloaded;
        //    RemoveVisualStateEventHandlers();
        //}

        private void ResetVisualStateEventHandlers()
        {
            RemoveVisualStateEventHandlers();
            var countDownStatesVisualStateGroup = VisualStateManager.GetVisualStateGroups(this).OfType<VisualStateGroup>().FirstOrDefault(vsg => vsg.Name == CountDownStatesGroupName);
            AddVisualStateEventHandlers(countDownStatesVisualStateGroup);
        }

        private void AddVisualStateEventHandlers(VisualStateGroup countDownStatesVisualStateGroup)
        {
            if (countDownStatesVisualStateGroup != null)
            {
                _countDownVisualStates = countDownStatesVisualStateGroup.States.OfType<VisualState>().ToDictionary(vs => vs.Name, vs => vs);
                SubscribeEvents(_countDownVisualStates);
            }
        }

        private void RemoveVisualStateEventHandlers()
        {
            if (_countDownVisualStates != null)
            {
                UnsubscribeEvents(_countDownVisualStates);
                _countDownVisualStates.Clear();
                _countDownVisualStates = null;
            }
        }

        private void SubscribeEvents(Dictionary<string, VisualState> countDownVisualStates)
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
                    case RestartCountingDown:
                        visualStateKvp.Value.Storyboard.Completed += OnRestartCountingStoryboardCompleted;
                        break;
                }
            }
        }

        private void UnsubscribeEvents(Dictionary<string, VisualState> countDownVisualStates)
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

        protected virtual void OnStartCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
            CurrentCountDownState = CountDownState.RestartCountingDown;
        }

        protected virtual void OnInterruptCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
            CurrentCountDownState = CountDownState.RestartCountingDown;
        }

        protected virtual void OnRestartCountingStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected virtual void OnCompleteCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnMouseLeftButtonDown(e);
            CurrentCountDownState = CountDownState.StartCountingDown;
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (CurrentCountDownState == CountDownState.StartCountingDown)
            {
                CurrentCountDownState = CountDownState.InterruptCountingDown;
            }
        }

        //protected override void OnIsPressedChanged(System.Windows.DependencyPropertyChangedEventArgs e)
        //{
        //    var newIsPressedValue = (bool)e.NewValue;
        //    var oldIsPressedValue = (bool)e.OldValue;
        //    if (newIsPressedValue && !oldIsPressedValue)
        //    {
        //        // Start counting down
        //    }
        //    base.OnIsPressedChanged(e);
        //}
    }

    public enum CountDownState
    {
        StartCountingDown,
        CompleteCountingDown,
        InterruptCountingDown,
        RestartCountingDown
    }
}
