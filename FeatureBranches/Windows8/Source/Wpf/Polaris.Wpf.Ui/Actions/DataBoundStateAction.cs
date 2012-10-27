//-----------------------------------------------------------------------
// <copyright file="DataBoundStateAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Interactivity;
    using System.Diagnostics;
    using Microsoft.Expression.Interactivity;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Collections.Generic;
    [DefaultTrigger(typeof(FrameworkElement), typeof(System.Windows.EventTrigger), "Loaded")]
    public class DataBoundStateAction : TriggerAction<FrameworkElement>
    {
        #region DataEnum

        /// <summary>
        /// DataEnum Dependency Property
        /// </summary>
        public static readonly DependencyProperty DataEnumProperty =
            DependencyProperty.Register("DataEnum", typeof(Enum), typeof(DataBoundStateAction),
                new PropertyMetadata((Enum)null,
                    new PropertyChangedCallback(OnDataEnumChanged)));

        /// <summary>
        /// Gets or sets the DataEnum property. This dependenc yproperty 
        /// indicates the data enum of the viewmodel that defines the visual state of the associated control.
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
            DataBoundStateAction target = (DataBoundStateAction)d;
            Enum oldDataEnum = (Enum)e.OldValue;
            Enum newDataEnum = target.DataEnum;
            target.OnDataEnumChanged(oldDataEnum, newDataEnum);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DataEnum property.
        /// </summary>
        protected virtual void OnDataEnumChanged(Enum oldDataEnum, Enum newDataEnum)
        {
            UpdateVisualStatesCollection();
            SetVisualState();
        }

        #endregion

        #region FinalStateNameFormat

        /// <summary>
        /// FinalStateNameFormat Dependency Property
        /// </summary>
        public static readonly DependencyProperty FinalStateNameFormatProperty =
            DependencyProperty.Register("FinalStateNameFormat", typeof(String), typeof(DataBoundStateAction),
                new PropertyMetadata((String)String.Empty,
                    new PropertyChangedCallback(OnFinalStateNameFormatChanged)));

        /// <summary>
        /// Gets or sets the FinalStateNameFormat property.  This dependency property 
        /// indicates the format of the final state to set.
        /// </summary>
        public String FinalStateNameFormat
        {
            get { return (String)GetValue(FinalStateNameFormatProperty); }
            set { SetValue(FinalStateNameFormatProperty, value); }
        }

        /// <summary>
        /// Handles changes to the FinalStateNameFormat property.
        /// </summary>
        private static void OnFinalStateNameFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataBoundStateAction)d).OnFinalStateNameFormatChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the FinalStateNameFormat property.
        /// </summary>
        protected virtual void OnFinalStateNameFormatChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateVisualStatesCollection();
        }

        private void UpdateVisualStatesCollection()
        {
            if (IsInDesignModeStatic)
            {
                this.VisualStates = String.Empty;
                if (DataEnum == null) { return; }
                String format = String.IsNullOrEmpty(FinalStateNameFormat) ? "{0}" : FinalStateNameFormat;
                Type enumType = DataEnum.GetType();
                System.Reflection.FieldInfo[] infos;
                infos = enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                foreach (System.Reflection.FieldInfo fi in infos)
                {
                    VisualStates = VisualStates + "  " + String.Format(format, fi.Name);
                }
            }
        }

        #endregion

        #region VisualStates

        /// <summary>
        /// VisualStates Dependency Property
        /// </summary>
        public static readonly DependencyProperty VisualStatesProperty =
            DependencyProperty.Register("VisualStates", typeof(String), typeof(DataBoundStateAction),
                new PropertyMetadata((String)null));

        /// <summary>
        /// Gets or sets the VisualStates property.  This dependency property 
        /// indicates the collection of visual states required by the enum.
        /// </summary>
        public String VisualStates
        {
            get { return (String)GetValue(VisualStatesProperty); }
            set { SetValue(VisualStatesProperty, value); }
        }

        #endregion


        private static bool? _isInDesignMode;

        /// <summary>
        /// Gets a value indicating whether the control is in design mode (running in Blend
        /// or Visual Studio).
        /// </summary>
        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
#if SILVERLIGHT
                    _isInDesignMode = System.ComponentModel.DesignerProperties.IsInDesignTool;
#else
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode
                    = (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
#endif
                }

                return _isInDesignMode.Value;
            }
        }

        protected override void Invoke(object parameter)
        {
            SetVisualState();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            SetVisualState();
            UpdateVisualStatesCollection();
        }

        Storyboard storyboard;

        private void SetVisualState()
        {
            if (AssociatedObject == null || !AssociatedObject.IsLoaded || DataEnum == null) { return; }

            var stateName = String.IsNullOrEmpty(FinalStateNameFormat) ? DataEnum.ToString() : String.Format(FinalStateNameFormat, DataEnum.ToString());

            FrameworkElement stateTarget;
            var found = VisualStateUtilities.TryFindNearestStatefulControl(AssociatedObject, out stateTarget);


            if (!found)
            {
                stateTarget = AssociatedObject;
            }
            bool result = false;
            if (stateTarget != null)
            {
                var stateControl = stateTarget as Control;

                if (stateControl == null && found)
                {
                    var groups = VisualStateManager.GetVisualStateGroups(stateTarget);
                    if (groups.Count == 0)
                    {
                        groups = VisualStateManager.GetVisualStateGroups(AssociatedObject);
                    }
                    FindStoryboard(stateName, groups);

                    result = VisualStateUtilities.GoToState(stateTarget, stateName, true);
                }
                else
                {
                    var groups = VisualStateManager.GetVisualStateGroups(stateControl);
                    if (groups.Count == 0)
                    {
                        groups = VisualStateManager.GetVisualStateGroups(AssociatedObject);
                    }
                    FindStoryboard(stateName, groups);

                    result = VisualStateManager.GoToState(stateControl, stateName, true);
                }
            }

            //if (found)
            //{
            //    //bool useTransitions = _initialized;
            //    VisualStateUtilities.GoToState(stateTarget, DataEnum.ToString(), true);
            //    //_initialized = true;
            //}
            //else
            //{
            //    stateTarget = AssociatedObject as Control;
            //    if (stateTarget != null) {
            //        var result = VisualStateManager.GoToState(stateTarget, DataEnum.ToString(), true);
            //    }
            //}

#if DEBUG && DataBoundStateActionDEBUG
                Debug.WriteLine("DataBoundStateAction {0}, result={1}", DataEnum, result);
#endif
        }

        private void FindStoryboard(string stateName, System.Collections.IList groups)
        {
            var visualState = (from VisualStateGroup stateGroup in groups
                               from VisualState state in stateGroup.States
                               where state.Name == stateName
                               select state).FirstOrDefault();

            if (visualState == null) { return; }

            storyboard = visualState.Storyboard;

            if (storyboard == null) { return; }

            storyboard.Completed += new EventHandler(Storyboard_Completed);
        }

        void Storyboard_Completed(object sender, EventArgs e)
        {
            if (storyboard != null)
            {
                storyboard.Completed -= new EventHandler(Storyboard_Completed);
            }

            if (StateCompletedCommand != null)
            {
                if (StateCompletedCommand.CanExecute(DataEnum))
                {
                    StateCompletedCommand.Execute(DataEnum);
                }
            }

        }

        #region StateCompletedCommand

        /// <summary>
        /// StateCompletedCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty StateCompletedCommandProperty =
            DependencyProperty.Register("StateCompletedCommand", typeof(ICommand), typeof(DataBoundStateAction),
                new FrameworkPropertyMetadata((ICommand)null,
                    new PropertyChangedCallback(OnStateCompletedCommandChanged)));

        /// <summary>
        /// Gets or sets the StateCompletedCommand property.  This dependency property 
        /// indicates the command to execute when the state animation is completed.
        /// </summary>
        public ICommand StateCompletedCommand
        {
            get { return (ICommand)GetValue(StateCompletedCommandProperty); }
            set { SetValue(StateCompletedCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the StateCompletedCommand property.
        /// </summary>
        private static void OnStateCompletedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataBoundStateAction)d).OnStateCompletedCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the StateCompletedCommand property.
        /// </summary>
        protected virtual void OnStateCompletedCommandChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion



    }
}
