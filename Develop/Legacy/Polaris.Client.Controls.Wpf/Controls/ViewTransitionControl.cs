//-----------------------------------------------------------------------
// <copyright file="ViewTransitionControl.cs" company="Polaris Community">
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
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using Microsoft.Expression.Interactivity;
    using Polaris.Windows.Extensions;
    using System.Windows.Media.Animation;

    [TemplateVisualState(Name = BeforeLoadedStateName, GroupName = TransitionStatesGroupName)]
    [TemplateVisualState(Name = LoadedStateName, GroupName = TransitionStatesGroupName)]
    [TemplateVisualState(Name = UnloadedStateName, GroupName = TransitionStatesGroupName)]
    [TemplatePart(Name = LayoutRootPartName, Type = typeof(Panel))]
    public class ViewTransitionControl : ContentControl
    {

        #region LayoutRoot Template Part
        private const String LayoutRootPartName = "LayoutRoot";
        private Panel LayoutRootPart;
        //  [TemplatePart(Name = LayoutRootPartName, Type=typeof(Panel))]
        //  LayoutRootPart = this.GetTemplateChild(LayoutRootPartName) as Panel;
        #endregion


        #region Visual States

        #region TransitionStates
        private const string TransitionStatesGroupName = "TransitionStates";

        private const string BeforeLoadedStateName = "BeforeLoaded";
        private const string LoadedStateName = "Loaded";
        private const string UnloadedStateName = "Unloaded";
        #endregion

        #endregion

        #region Properties

        #region DataEnum

        /// <summary>
        /// DataEnum Dependency Property
        /// </summary>
        public static readonly DependencyProperty DataEnumProperty =
            DependencyProperty.Register("DataEnum", typeof(Enum), typeof(ViewTransitionControl),
                new FrameworkPropertyMetadata((Enum)null,
                    new PropertyChangedCallback(OnDataEnumChanged)));

        /// <summary>
        /// Gets or sets the DataEnum property.  This dependency property 
        /// indicates the enumeration value on the view model to match the states to.
        /// </summary>
        public Enum DataEnum
        {
            get { return (Enum)GetValue(DataEnumProperty); }
            set { SetValue(DataEnumProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DataEnum property.
        /// </summary>
        private static void OnDataEnumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewTransitionControl)d).OnDataEnumChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DataEnum property.
        /// </summary>
        protected virtual void OnDataEnumChanged(DependencyPropertyChangedEventArgs e)
        {
            Enum state = (Enum)e.NewValue;
            SetVisualState(state);
        }

        #endregion

        #region TransitionCompletedCommand

        /// <summary>
        /// TransitionCompletedCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty TransitionCompletedCommandProperty =
            DependencyProperty.Register("TransitionCompletedCommand", typeof(ICommand), typeof(ViewTransitionControl),
                new FrameworkPropertyMetadata((ICommand)null,
                    new PropertyChangedCallback(OnTransitionCompletedCommandChanged)));

        /// <summary>
        /// Gets or sets the TransitionCompletedCommand property.  This dependency property 
        /// indicates the command to execute when a transition is completed.
        /// </summary>
        public ICommand TransitionCompletedCommand
        {
            get { return (ICommand)GetValue(TransitionCompletedCommandProperty); }
            set { SetValue(TransitionCompletedCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TransitionCompletedCommand property.
        /// </summary>
        private static void OnTransitionCompletedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ViewTransitionControl)d).OnTransitionCompletedCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the TransitionCompletedCommand property.
        /// </summary>
        protected virtual void OnTransitionCompletedCommandChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion



        #endregion

        static ViewTransitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ViewTransitionControl), new FrameworkPropertyMetadata(typeof(ViewTransitionControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRootPart = this.GetTemplateChild(LayoutRootPartName) as Panel;
            SetVisualState(DataEnum);
        }

        #region WeakStoryboardEvents

        WeakStoryboardEvents beforeLoadedEvents;
        WeakStoryboardEvents loadedEvents;
        WeakStoryboardEvents unloadedEvents;

        /// <summary> 
        ///     WeakBitmapSourceEvents acts as a proxy between events on BitmapSource 
        ///     and handlers on Image.  It is used to respond to events on BitmapSource
        ///     without preventing Image's to be collected. 
        /// </summary>
        private class WeakStoryboardEvents : WeakReference
        {
            public WeakStoryboardEvents(ViewTransitionControl viewTransitionControl, Storyboard storyboard)
                : base(viewTransitionControl)
            {
                this.storyboard = storyboard;
                this.storyboard.Completed += this.OnStoryboardCompleted;
            }

            public void OnStoryboardCompleted(object sender, EventArgs e)
            {
                ViewTransitionControl control = this.Target as ViewTransitionControl;
                if (control != null)
                {
                    control.OnStoryboardCompleted(sender, e);
                }
                else
                {
                    Dispose();
                }
            }

            public void Dispose()
            {
                // We can't remove handlers from frozen sources. 
                if (!storyboard.IsFrozen)
                {
                    storyboard.Completed -= this.OnStoryboardCompleted;
                }
            }

            private Storyboard storyboard;
        }

        #endregion WeakBitmapSourceEvents

        private void DisposeEventHandlers()
        {
            if (beforeLoadedEvents != null)
            {
                beforeLoadedEvents.Dispose();
                beforeLoadedEvents = null;
            }
            if (loadedEvents != null)
            {
                loadedEvents.Dispose();
                loadedEvents = null;
            }
            if (unloadedEvents != null)
            {
                unloadedEvents.Dispose();
                unloadedEvents = null;
            }
        }

        void OnStoryboardCompleted(object sender, EventArgs e)
        {
            if (TransitionCompletedCommand != null)
            {
                if (TransitionCompletedCommand.CanExecute(DataEnum))
                {
                    TransitionCompletedCommand.Execute(DataEnum);
                }
            }
        }


        private void SetVisualState(Enum stateEnum)
        {

            if (stateEnum == null) { return; }
            var stateName = stateEnum.ToString();
            if (stateName == null || LayoutRootPart == null) { return; }

            FindStoryboard(stateName);

            VisualStateManager.GoToState(this, stateName, true);
        }

        private void FindStoryboard(string stateName)
        {
            DisposeEventHandlers();

            var groups = VisualStateManager.GetVisualStateGroups(LayoutRootPart);

            var transitionState = (from VisualStateGroup visualStateGroup in groups
                                   from VisualState state in visualStateGroup.States
                                   where visualStateGroup.Name == TransitionStatesGroupName
                                   where state.Name == stateName
                                   select state).FirstOrDefault();
            if (transitionState == null) { return; }

            switch (stateName)
            {
                case BeforeLoadedStateName:
                    beforeLoadedEvents = new WeakStoryboardEvents(this, transitionState.Storyboard);
                    break;
                case LoadedStateName:
                    loadedEvents = new WeakStoryboardEvents(this, transitionState.Storyboard);
                    break;
                case UnloadedStateName:
                    unloadedEvents = new WeakStoryboardEvents(this, transitionState.Storyboard);
                    break;
            }

        }

    }
}
