namespace FooWPhoneLibrary.Toolkit.Actions
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
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Collections.Generic;
    using System.Xaml.Linq;
    using System.Globalization;
    using Microsoft.Expression.Interactivity;

    [DefaultTrigger(typeof(FrameworkElement), typeof(System.Windows.EventTrigger), "Loaded")]
    public class GoToEnumStateAction : TargetedTriggerAction<FrameworkElement>
    {
        #region DataEnum

        /// <summary>
        /// DataEnum Dependency Property
        /// </summary>
        public static readonly DependencyProperty DataEnumProperty =
            DependencyProperty.Register("DataEnum", typeof(Enum), typeof(GoToEnumStateAction),
                new PropertyMetadata((Enum)null,
                    new PropertyChangedCallback(OnDataEnumChanged)));

        /// <summary>
        /// Gets or sets the DataEnum property. This dependency property 
        /// indicates the data enumeration of the View Model that defines the visual state of the associated control.
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
            GoToEnumStateAction target = (GoToEnumStateAction)d;
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
            DependencyProperty.Register("FinalStateNameFormat", typeof(String), typeof(GoToEnumStateAction),
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
            ((GoToEnumStateAction)d).OnFinalStateNameFormatChanged(e);
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

        #region UseTransitions

        /// <summary>
        /// UseTransitions Dependency Property
        /// </summary>
        public static readonly DependencyProperty UseTransitionsProperty =
            DependencyProperty.Register("UseTransitions", typeof(bool), typeof(GoToEnumStateAction),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the UseTransitions property. This dependency property 
        /// indicates ....
        /// </summary>
        public bool UseTransitions
        {
            get { return (bool)GetValue(UseTransitionsProperty); }
            set { SetValue(UseTransitionsProperty, value); }
        }

        #endregion

        #region VisualStates

        /// <summary>
        /// VisualStates Dependency Property
        /// </summary>
        public static readonly DependencyProperty VisualStatesProperty =
            DependencyProperty.Register("VisualStates", typeof(String), typeof(GoToEnumStateAction),
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

        #region VisualStateChangedCommand

        /// <summary>
        /// VisualStateChangedCommand Dependency Property
        /// </summary>
        public static readonly DependencyProperty VisualStateChangedCommandProperty =
            DependencyProperty.Register("VisualStateChangedCommand", typeof(ICommand), typeof(GoToEnumStateAction),
                new PropertyMetadata(default(ICommand),
                    new PropertyChangedCallback(OnVisualStateChangedCommandChanged)));

        /// <summary>
        /// Gets or sets the VisualStateChangedCommand property. This dependency property 
        /// indicates ....
        /// </summary>
        public ICommand VisualStateChangedCommand
        {
            get { return (ICommand)GetValue(VisualStateChangedCommandProperty); }
            set { SetValue(VisualStateChangedCommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the VisualStateChangedCommand property.
        /// </summary>
        private static void OnVisualStateChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GoToEnumStateAction target = (GoToEnumStateAction)d;
            ICommand oldVisualStateChangedCommand = (ICommand)e.OldValue;
            ICommand newVisualStateChangedCommand = target.VisualStateChangedCommand;
            target.OnVisualStateChangedCommandChanged(oldVisualStateChangedCommand, newVisualStateChangedCommand);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the VisualStateChangedCommand property.
        /// </summary>
        protected virtual void OnVisualStateChangedCommandChanged(ICommand oldVisualStateChangedCommand, ICommand newVisualStateChangedCommand)
        {
        }

        #endregion

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
                    _isInDesignMode = System.ComponentModel.DesignerProperties.IsInDesignTool;
                }

                return _isInDesignMode.Value;
            }
        }
        private static bool? _isInDesignMode;

        private bool IsTargetObjectSet
        {
            get
            {
                return this.ReadLocalValue(TargetedTriggerAction.TargetObjectProperty) != DependencyProperty.UnsetValue;
            }
        }

        private FrameworkElement StateTarget { get; set; }

        private VisualStateGroup _visualStateGroupOfTheStateName;

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

        private void SetVisualState()
        {
            if (AssociatedObject == null || DataEnum == null) { return; }
            var stateName = String.IsNullOrEmpty(FinalStateNameFormat) ? DataEnum.ToString() : String.Format(FinalStateNameFormat, DataEnum.ToString());

            if (StateTarget == null)
                return;
            HookUptoCompletedEvent(StateTarget, stateName);
            VisualStateUtilities.GoToState(StateTarget, stateName, UseTransitions);

        }

        private void HookUptoCompletedEvent(FrameworkElement stateTarget, string stateName)
        {
            if (VisualStateChangedCommand != null)
            {
                _visualStateGroupOfTheStateName = VisualStateUtilities.GetVisualStateGroups(stateTarget)
                                            .OfType<VisualStateGroup>()
                                            .FirstOrDefault(vsg => vsg.States.OfType<VisualState>().Any(vs => vs.Name == stateName));
                if (_visualStateGroupOfTheStateName != null)
                {
                    _visualStateGroupOfTheStateName.CurrentStateChanged += OnVisualStateGroupCurrentStateChanged;
                }
            }
        }

        private void OnVisualStateGroupCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            _visualStateGroupOfTheStateName.CurrentStateChanged -= OnVisualStateGroupCurrentStateChanged;
            _visualStateGroupOfTheStateName = null;
            var parameter = new Tuple<string, string>(e.OldState.Name, e.NewState.Name);
            if (VisualStateChangedCommand == null || !VisualStateChangedCommand.CanExecute(parameter))
            {
                return;
            }
            VisualStateChangedCommand.Execute(parameter);
        }

        /// <summary>
        /// Called when the target changes. If the TargetName property isn't set, this action has custom behavior.
        /// 
        /// </summary>
        /// <param name="oldTarget"/><param name="newTarget"/><exception cref="T:System.InvalidOperationException">Could not locate an appropriate FrameworkElement with states.</exception>
        protected override void OnTargetChanged(FrameworkElement oldTarget, FrameworkElement newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);

            FrameworkElement resolvedControl = (FrameworkElement)null;
            if (string.IsNullOrEmpty(this.TargetName) && !this.IsTargetObjectSet)
            {
                if (!VisualStateUtilities.TryFindNearestStatefulControl(this.AssociatedObject as FrameworkElement, out resolvedControl) && resolvedControl != null)
                    throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.CurrentCulture, "Target has no visual state groups", resolvedControl.Name));
            }
            else
                resolvedControl = this.Target;
            this.StateTarget = resolvedControl;
        }

    }
}
