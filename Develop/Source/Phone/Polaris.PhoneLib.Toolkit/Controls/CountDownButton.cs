using System;
using Polaris.PhoneLib.SystemEx;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Expression.Interactivity;
using System.Xaml.Linq;
using System.Windows.Media;
using Polaris.PhoneLib.Toolkit.Extensions;
using System.Windows.Input;
using Polaris.PhoneLib.SystemEx;
using Polaris.PhoneLib.Toolkit.Events;

namespace Polaris.PhoneLib.Toolkit.Controls
{
    [TemplateVisualState(Name = StartCountDownState, GroupName = CountDownStatesGroupName)]
    [TemplateVisualState(Name = CompleteCountDownState, GroupName = CountDownStatesGroupName)]
    [TemplateVisualState(Name = InterruptCountDownState, GroupName = CountDownStatesGroupName)]
    [TemplateVisualState(Name = ResetCountDownState, GroupName = CountDownStatesGroupName)]
    public class CountDownButton : ButtonBase
    {
        #region CurrentCountDownState

        /// <summary>
        /// CurrentCountDownState Dependency Property
        /// </summary>
        public static readonly DependencyProperty CurrentCountDownStateProperty =
            DependencyProperty.Register("CurrentCountDownState", typeof(CountDownState), typeof(CountDownButton),
                new PropertyMetadata(CountDownState.ResetCountDownState,
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
                //_countDownVisualStates[newCurrentCountDownState.ToString()].Activate(this);
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

        protected const string StartCountDownState = @"StartCountDownState";
        protected const string CompleteCountDownState = @"CompleteCountDownState";
        protected const string InterruptCountDownState = @"InterruptCountDownState";
        protected const string ResetCountDownState = @"ResetCountDownState";

        #endregion CountDownStatesGroupName

        #endregion Visual States

        #region CountDownCommand

        /// <summary>
        /// CountDownCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty CountDownCommandProperty =
            DependencyProperty.Register("CountDownCommand", typeof(ICommand), typeof(CountDownButton),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the CountDownCommand property. This dependency property 
        /// indicates ....
        /// </summary>
        public ICommand CountDownCommand
        {
            get { return (ICommand)GetValue(CountDownCommandProperty); }
            set { SetValue(CountDownCommandProperty, value); }
        }

        #endregion

        private Dictionary<string, VisualState> _countDownVisualStates;

       
        #region Counter

        /// <summary>
        /// Counter Dependency Property
        /// </summary>
        public static readonly DependencyProperty CounterProperty =
            DependencyProperty.Register("Counter", typeof(int), typeof(CountDownButton),
                new PropertyMetadata(0));

        /// <summary>
        /// Gets or sets the Counter property. This dependency property 
        /// indicates ....
        /// </summary>
        public int Counter
        {
            get { return (int)GetValue(CounterProperty); }
            set { SetValue(CounterProperty, value); }
        }

        #endregion

        public int InitialCounter
        {
            get { return _initialCounter; }
            set { _initialCounter = value; }
        }
        private int _initialCounter = 0;

        public int Step
        {
            get { return _step; }
            set { _step = value; }
        }
        private int _step = 1;

        public TimeSpan CounterInterval
        {
            get { return _counterInterval; }
            set { _counterInterval = value; }
        }
        private TimeSpan _counterInterval = TimeSpan.FromSeconds(1);

        private DateTime _previousRead = DateTime.UtcNow;
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;
        private bool _isPressed = false;

        public CountDownButton()
        {
            DefaultStyleKey = typeof(CountDownButton);
            //<CountDownButton, object, EventArgs>.SubscribeToWeakEvent(this,null, "System.Windows.Media.CompositionTarget.Rendering ", OnCompositionTargetRendering);
            Unloaded += OnCountDownButtonUnloaded;
            Loaded += OnCountDownButtonLoaded;
        }

        protected virtual void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if (!_isPressed)
            {
                return;
            }
            _elapsedTime = DateTime.UtcNow - _previousRead;
            _totalElapsedTime += _elapsedTime;
            _previousRead = DateTime.UtcNow;
            if (_totalElapsedTime > CounterInterval)
            {
                Counter += Step;
                _totalElapsedTime = TimeSpan.Zero;
            }
            
        }

        public override void OnApplyTemplate()
        {
            RemoveVisualStateEventHandlers();
            base.OnApplyTemplate();
            var countDownStatesVisualStateGroup = VisualStateManager.GetVisualStateGroups(VisualTreeHelper.GetChild(this, 0) as FrameworkElement)
                .OfType<VisualStateGroup>()
                .FirstOrDefault(currentGroup => currentGroup.Name == CountDownStatesGroupName);
            AddVisualStateEventHandlers(countDownStatesVisualStateGroup);
            VisualStateManager.GoToState(this, CurrentCountDownState.ToString(), false);
        }

        protected virtual void OnCountDownButtonLoaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += OnCompositionTargetRendering;
            //var countDownButton = sender as CountDownButton;
            //countDownButton.Loaded -= OnCountDownButtonLoaded;
            //ResetVisualStateEventHandlers();
        }

        protected virtual void OnCountDownButtonUnloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;
            //var countDownButton = sender as CountDownButton;
            //countDownButton.Unloaded -= OnCountDownButtonUnloaded;
            //RemoveVisualStateEventHandlers();
        }

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
                if (visualStateKvp.Value == null || visualStateKvp.Value.Storyboard == null)
                {
                    continue;
                }
                switch (visualStateKvp.Key)
                {
                    case StartCountDownState:
                        visualStateKvp.Value.Storyboard.Completed += OnStartCountingDownStoryboardCompleted;
                        break;
                    case CompleteCountDownState:
                        visualStateKvp.Value.Storyboard.Completed += OnCompleteCountingDownStoryboardCompleted;
                        break;
                    case InterruptCountDownState:
                        visualStateKvp.Value.Storyboard.Completed += OnInterruptCountingDownStoryboardCompleted;
                        break;
                    case ResetCountDownState:
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
                    case StartCountDownState:
                        visualStateKvp.Value.Storyboard.Completed -= OnStartCountingDownStoryboardCompleted;
                        break;
                    case CompleteCountDownState:
                        visualStateKvp.Value.Storyboard.Completed -= OnCompleteCountingDownStoryboardCompleted;
                        break;
                    case InterruptCountDownState:
                        visualStateKvp.Value.Storyboard.Completed -= OnInterruptCountingDownStoryboardCompleted;
                        break;
                }
            }
        }

        protected virtual void OnStartCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
            _totalElapsedTime = TimeSpan.Zero;
            Counter = InitialCounter;
            _isPressed = false;
            CurrentCountDownState = CountDownState.ResetCountDownState;
            CountDownCommand.TryExecute(CommandParameter);
        }

        protected virtual void OnInterruptCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
            CurrentCountDownState = CountDownState.ResetCountDownState;
        }

        protected virtual void OnRestartCountingStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected virtual void OnCompleteCountingDownStoryboardCompleted(object sender, EventArgs e)
        {
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            _totalElapsedTime = TimeSpan.Zero;
            Counter = InitialCounter;
            _isPressed = true;
            base.OnMouseLeftButtonDown(e);
            CurrentCountDownState = CountDownState.StartCountDownState;
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            _isPressed = false;
            Counter = InitialCounter;
            base.OnMouseLeftButtonUp(e);
            if (CurrentCountDownState == CountDownState.StartCountDownState)
            {
                CurrentCountDownState = CountDownState.InterruptCountDownState;
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
        StartCountDownState,
        CompleteCountDownState,
        InterruptCountDownState,
        ResetCountDownState
    }
}