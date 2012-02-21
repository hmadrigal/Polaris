//-----------------------------------------------------------------------
// <copyright file="AnimatedLayoutControl.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Controls.Wpf.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    [TemplateVisualStateAttribute(Name = StateBeforeLoadedName, GroupName = StateGroupLayoutStatesName)]
    [TemplateVisualStateAttribute(Name = StateLoadedName, GroupName = StateGroupLayoutStatesName)]
    [TemplateVisualStateAttribute(Name = StateUnloadedName, GroupName = StateGroupLayoutStatesName)]
    [TemplatePart(Name = ElementLayoutRootName, Type = typeof(Panel))]
    public class AnimatedLayoutControl : ContentControl
    {

        internal Random RandomGenerator { get; set; }
        private Double? NormalizedIndex { get; set; }

        internal TimeSpan BeginTime { get; private set; }

        #region Template parts

        private const String ElementLayoutRootName = "LayoutRoot";

        private const String StateGroupLayoutStatesName = "LayoutStates";
        private const String StateBeforeLoadedName = "BeforeLoaded";
        private const String StateLoadedName = "Loaded";
        private const String StateUnloadedName = "Unloaded";

        private Panel LayoutRoot { get; set; }

        internal VisualStateGroup LayoutStatesGroup { get; set; }
        internal VisualState BeforeLoadedState { get; set; }
        internal VisualState LoadedState { get; set; }
        internal VisualState UnloadedState { get; set; }

        #endregion

        #region LayoutState

        /// <summary>
        /// LayoutState Dependency Property
        /// </summary>
        public static readonly DependencyProperty LayoutStateProperty =
            DependencyProperty.Register("LayoutState", typeof(LayoutState), typeof(AnimatedLayoutControl),
                new PropertyMetadata((LayoutState.BeforeLoaded),
                    new PropertyChangedCallback(OnLayoutStateChanged)));

        /// <summary>
        /// Gets or sets the LayoutState property.  This dependency property 
        /// indicates the current loading state of the control and will trigger
        /// the visual state changes of the "LayoutStates" visual state group.
        /// </summary>
        public LayoutState LayoutState
        {
            get { return (LayoutState)GetValue(LayoutStateProperty); }
            set { SetValue(LayoutStateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the LayoutState property.
        /// </summary>
        private static void OnLayoutStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedLayoutControl)d).OnLayoutStateChanged(e);
        }


        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the LayoutState property.
        /// </summary>
        private void OnLayoutStateChanged(DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewValue.ToString(), true);
        }

        public event EventHandler<LayoutStateChangeEventArgs> LayoutStateChangeCompleted;
        protected virtual void OnLayoutStateChangeCompleted(object sender, LayoutStateChangeEventArgs args)
        {
            var threadSafeInstance = this.LayoutStateChangeCompleted;
            if (threadSafeInstance != null)
            {
                threadSafeInstance(this, args);
            }
        }



        #endregion

        #region LoadedDelayMode

        /// <summary>
        /// LoadedDelayMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty LoadedDelayModeProperty =
            DependencyProperty.Register("LoadedDelayMode", typeof(DelayMode), typeof(AnimatedLayoutControl),
                new PropertyMetadata((DelayMode.None),
                    new PropertyChangedCallback(OnLoadedDelayModeChanged)));

        /// <summary>
        /// Gets or sets the LoadedDelayMode property.  This dependency property 
        /// indicates the methodology the control will use to delay the 
        /// visual state animations.
        /// </summary>
        public DelayMode LoadedDelayMode
        {
            get { return (DelayMode)GetValue(LoadedDelayModeProperty); }
            set { SetValue(LoadedDelayModeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the LoadedDelayMode property.
        /// </summary>
        private static void OnLoadedDelayModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedLayoutControl)d).OnLoadedDelayModeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the LoadedDelayMode property.
        /// </summary>
        protected virtual void OnLoadedDelayModeChanged(DependencyPropertyChangedEventArgs e)
        {

            var delayMode = (DelayMode)e.NewValue;

            //Initialize(delayMode);

        }

        #endregion

        #region UnloadedDelayMode

        /// <summary>
        /// UnloadedDelayMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty UnloadedDelayModeProperty =
            DependencyProperty.Register("UnloadedDelayMode", typeof(DelayMode), typeof(AnimatedLayoutControl),
                new PropertyMetadata((DelayMode.None),
                    new PropertyChangedCallback(OnUnloadedDelayModeChanged)));

        /// <summary>
        /// Gets or sets the UnloadedDelayMode property.  This dependency property 
        /// indicates the methodology the control will use to delay the 
        /// visual state animations.
        /// </summary>
        public DelayMode UnloadedDelayMode
        {
            get { return (DelayMode)GetValue(UnloadedDelayModeProperty); }
            set { SetValue(UnloadedDelayModeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the UnloadedDelayMode property.
        /// </summary>
        private static void OnUnloadedDelayModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedLayoutControl)d).OnUnloadedDelayModeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the UnloadedDelayMode property.
        /// </summary>
        protected virtual void OnUnloadedDelayModeChanged(DependencyPropertyChangedEventArgs e)
        {

            var delayMode = (DelayMode)e.NewValue;

            //Initialize(delayMode);

        }

        #endregion


        #region MinDelay

        /// <summary>
        /// MinDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinDelayProperty =
            DependencyProperty.Register("MinDelay", typeof(TimeSpan), typeof(AnimatedLayoutControl),
                new PropertyMetadata((TimeSpan.Zero),
                    new PropertyChangedCallback(OnMinDelayChanged)));

        /// <summary>
        /// Gets or sets the MinDelay property.  This dependency property 
        /// indicates the minimum delay the visual state transitions 
        /// will have when the delay mode is set to random or sequential.
        /// </summary>
        public TimeSpan MinDelay
        {
            get { return (TimeSpan)GetValue(MinDelayProperty); }
            set { SetValue(MinDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the MinDelay property.
        /// </summary>
        private static void OnMinDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedLayoutControl)d).OnMinDelayChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the MinDelay property.
        /// </summary>
        protected virtual void OnMinDelayChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region MaxDelay

        /// <summary>
        /// MaxDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaxDelayProperty =
            DependencyProperty.Register("MaxDelay", typeof(TimeSpan), typeof(AnimatedLayoutControl),
                new PropertyMetadata((TimeSpan.Zero),
                    new PropertyChangedCallback(OnMaxDelayChanged)));

        /// <summary>
        /// Gets or sets the MaxDelay property.  This dependency property 
        /// indicates the maximum delay the visual state transitions will 
        /// have when the delay mode is set to random or sequential.
        /// </summary>
        public TimeSpan MaxDelay
        {
            get { return (TimeSpan)GetValue(MaxDelayProperty); }
            set { SetValue(MaxDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the MaxDelay property.
        /// </summary>
        private static void OnMaxDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedLayoutControl)d).OnMaxDelayChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the MaxDelay property.
        /// </summary>
        protected virtual void OnMaxDelayChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #region InitialBeginTime

        /// <summary>
        /// InitialBeginTime Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty InitialBeginTimeProperty =
            DependencyProperty.RegisterAttached("InitialBeginTime", typeof(TimeSpan?), typeof(AnimatedLayoutControl),
                new PropertyMetadata(null,
                    new PropertyChangedCallback(OnInitialBeginTimeChanged)));

        /// <summary>
        /// Gets the InitialBeginTime property.  This dependency property 
        /// indicates the begin time the element had before the 
        /// modifications started.
        /// </summary>
        private static TimeSpan? GetInitialBeginTime(DependencyObject d)
        {
            return (TimeSpan?)d.GetValue(InitialBeginTimeProperty);
        }

        /// <summary>
        /// Sets the InitialBeginTime property.  This dependency property 
        /// indicates the begin time the element had before the modifications started.
        /// </summary>
        private static void SetInitialBeginTime(DependencyObject d, TimeSpan? value)
        {
            d.SetValue(InitialBeginTimeProperty, value);
        }

        /// <summary>
        /// Handles changes to the InitialBeginTime property.
        /// </summary>
        private static void OnInitialBeginTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion


        internal event EventHandler ApplyTemplateCompleted;
        protected void OnApplyTemplateCompleted()
        {
            var hldr = this.ApplyTemplateCompleted;
            if (hldr != null)
                hldr(this, EventArgs.Empty);
        }


        public AnimatedLayoutControl()
            : base()
        {
            DefaultStyleKey = typeof(AnimatedLayoutControl);
            RandomGenerator = new Random();
            Loaded += new RoutedEventHandler(AnimatedLayoutControl_Loaded);
        }

        void AnimatedLayoutControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Initialize();
            //LayoutState = LayoutState.Loaded;
        }

        private void SetItemIndex()
        {
            if (NormalizedIndex != null && !Double.IsNaN(NormalizedIndex.Value)) { return; }

            Panel parentPanel = null;
            DependencyObject parentContainer = null;
            AnimatedLayoutControl target = this;

            GetParentElements(target, ref parentPanel, ref parentContainer);

            UIElement child = null;
            if (parentContainer != null)
            {
                child = parentContainer as UIElement;
            }
            else
            {
                child = this;
            }

            if (parentPanel != null)
            {

                var itemIndex = parentPanel.Children.IndexOf(child);
                var itemCount = parentPanel.Children.Count - 1;
                if (itemCount == 0)
                {
                    NormalizedIndex = 0;
                }
                else
                {
                    NormalizedIndex = (Double)itemIndex / (Double)itemCount;
                }
            }

        }

        private void GetParentElements(DependencyObject target, ref Panel parentPanel, ref DependencyObject parentContainer)
        {
            var visualParent = VisualTreeHelper.GetParent(target);
            if (visualParent == null) { return; }
            if (visualParent is Panel)
            {
                parentPanel = visualParent as Panel;
            }
            else
            {
                parentContainer = visualParent;
                GetParentElements(parentContainer, ref parentPanel, ref parentContainer);
            }
        }

        private void SetInitialBeginTime()
        {

            if (LayoutRoot == null) { return; }

            var groups = VisualStateManager.GetVisualStateGroups(LayoutRoot);

            foreach (var group in groups)
            {
                foreach (var item in ((VisualStateGroup)group).Transitions)
                {
                    var transition = item as VisualTransition;
                    if (transition != null)
                    {
                        if (transition.GeneratedDuration.HasTimeSpan)
                        {
                            var beginTime = GetInitialBeginTime(transition);
                            if (beginTime == null)
                            {
                                SetInitialBeginTime(transition, transition.GeneratedDuration.TimeSpan);
                            }
                        }
                    }

                }
                foreach (var state in ((VisualStateGroup)group).States)
                {
                    var storyboard = ((VisualState)state).Storyboard;
                    if (storyboard != null)
                    {
                        if (storyboard.BeginTime != null)
                        {
                            var beginTime = GetInitialBeginTime(storyboard);
                            if (beginTime == null)
                            {
                                SetInitialBeginTime(storyboard, storyboard.BeginTime.Value);
                            }
                        }
                    }
                }
            }
        }

        private void CalculateDelay()
        {

            if ((LoadedDelayMode == DelayMode.None) && (UnloadedDelayMode == DelayMode.None)) { return; }
            if (LayoutRoot == null) { return; }

            var groups = VisualStateManager.GetVisualStateGroups(LayoutRoot);

            var minTicks = (double)MinDelay.Ticks;
            var maxTicks = (double)MaxDelay.Ticks;


            var loadedNormalizedValue = 0.0;
            var unloadedNormalizedValue = 0.0;

            if (NormalizedIndex == null || Double.IsNaN(NormalizedIndex.Value))
            {
                SetItemIndex();
            }


            switch (LoadedDelayMode)
            {
                case DelayMode.Random:
                    loadedNormalizedValue = RandomGenerator.NextDouble();
                    break;
                case DelayMode.Sequential:
                    loadedNormalizedValue = NormalizedIndex.Value;
                    break;
                case DelayMode.ReverseSequential:
                    loadedNormalizedValue = 1.0 - NormalizedIndex.Value;
                    break;
            }


            switch (UnloadedDelayMode)
            {
                case DelayMode.Random:
                    unloadedNormalizedValue = RandomGenerator.NextDouble();
                    break;
                case DelayMode.Sequential:
                    unloadedNormalizedValue = NormalizedIndex.Value;
                    break;
                case DelayMode.ReverseSequential:
                    unloadedNormalizedValue = 1.0 - NormalizedIndex.Value;
                    break;
            }



#if DEBUG
            System.Diagnostics.Debug.WriteLine(String.Format("Loaded Normalized value: {0}", loadedNormalizedValue));
            System.Diagnostics.Debug.WriteLine(String.Format("Unloaded Normalized value: {0}", unloadedNormalizedValue));
#endif


            var delta = maxTicks - minTicks;

            delta = loadedNormalizedValue * delta;
            delta += minTicks;

            var loadedTimeDelta = new TimeSpan((long)delta);



            delta = maxTicks - minTicks;
            delta = unloadedNormalizedValue * delta;
            delta += minTicks;

            var unloadedTimeDelta = new TimeSpan((long)delta);


            BeginTime = new TimeSpan(Math.Max(loadedTimeDelta.Ticks, unloadedTimeDelta.Ticks));

#if DEBUG
            System.Diagnostics.Debug.WriteLine(String.Format("BeginTime: {0}", BeginTime));
#endif


            foreach (var group in groups)
            {

                foreach (var item in ((VisualStateGroup)group).Transitions)
                {
                    var transition = item as VisualTransition;
                    if (transition != null)
                    {
                        if (transition.GeneratedDuration.HasTimeSpan)
                        {
                            var initialBeginTime = GetInitialBeginTime(transition);
                            if (initialBeginTime == null)
                            {
                                initialBeginTime = TimeSpan.Zero;
                            }
                            transition.GeneratedDuration =
                                new Duration(initialBeginTime.Value + loadedTimeDelta);
                        }
                    }

                }

                foreach (var state in ((VisualStateGroup)group).States)
                {
                    var storyboard = ((VisualState)state).Storyboard;
                    if (storyboard != null)
                    {
                        var initialBeginTime = GetInitialBeginTime(storyboard);
                        if (initialBeginTime == null)
                        {
                            initialBeginTime = TimeSpan.Zero;
                        }

                        if (state == LoadedState)
                        {
                            storyboard.BeginTime = initialBeginTime.Value + loadedTimeDelta;
                        }
                        else if (state == UnloadedState)
                        {
                            storyboard.BeginTime = initialBeginTime.Value + unloadedTimeDelta;
                        }
                    }
                }
            }
        }


        private void Initialize(Boolean setIndex)
        {
            if (setIndex)
            {
                SetItemIndex();
            }

            SetInitialBeginTime();
            CalculateDelay();
        }

        public void Initialize()
        {
            var setIndex = (LoadedDelayMode == DelayMode.Sequential || LoadedDelayMode == DelayMode.ReverseSequential || UnloadedDelayMode == DelayMode.Sequential || UnloadedDelayMode == DelayMode.ReverseSequential);
            Initialize(setIndex);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRoot = GetTemplateChild(ElementLayoutRootName) as Panel;

            LayoutStatesGroup = GetTemplateChild(StateGroupLayoutStatesName) as VisualStateGroup;

            LayoutStatesGroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(LayoutStatesGroup_CurrentStateChanging);
            LayoutStatesGroup.CurrentStateChanged += new EventHandler<VisualStateChangedEventArgs>(LayoutStatesGroup_CurrentStateChanged);

            BeforeLoadedState = GetTemplateChild(StateBeforeLoadedName) as VisualState;
            LoadedState = GetTemplateChild(StateLoadedName) as VisualState;
            UnloadedState = GetTemplateChild(StateUnloadedName) as VisualState;

            if (BeforeLoadedState != null)
            {
                if (BeforeLoadedState.Storyboard != null)
                {
                    BeforeLoadedState.Storyboard.Completed += new EventHandler(OnBeforeLoadedStateCompleted);
                }
            }

            if (LoadedState != null)
            {
                if (LoadedState.Storyboard != null)
                {
                    LoadedState.Storyboard.Completed += new EventHandler(OnLoadedStateCompleted);
                }
            }

            if (UnloadedState != null)
            {
                if (UnloadedState.Storyboard != null)
                {
                    UnloadedState.Storyboard.Completed += new EventHandler(OnUnloadedStateCompleted);
                }
            }

            OnApplyTemplateCompleted();
        }

        void LayoutStatesGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            //var newState = Enum.Parse(typeof(LayoutState), e.NewState.Name);
            //OnLayoutStateChangeCompleted(this, new LayoutStateChangeEventArgs() { NewState = LayoutState.Unloaded, });
        }

        void LayoutStatesGroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
        }

        void OnUnloadedStateCompleted(object sender, EventArgs e)
        {
            OnLayoutStateChangeCompleted(this, new LayoutStateChangeEventArgs() { NewState = LayoutState.Unloaded, });
        }

        void OnBeforeLoadedStateCompleted(object sender, EventArgs e)
        {
            OnLayoutStateChangeCompleted(this, new LayoutStateChangeEventArgs() { NewState = LayoutState.BeforeLoaded, });
        }


        void OnLoadedStateCompleted(object sender, EventArgs e)
        {
            OnLayoutStateChangeCompleted(this, new LayoutStateChangeEventArgs() { NewState = LayoutState.Loaded, });
        }

    }

    public enum LayoutState
    {
        BeforeLoaded,
        Loaded,
        Unloaded,
    }

    public enum DelayMode
    {
        None,
        Random,
        Sequential,
        ReverseSequential,
    }

    public class LayoutStateChangeEventArgs : EventArgs
    {
        public LayoutState NewState { get; set; }
    }
}
