//-----------------------------------------------------------------------
// <copyright file="WatermarkTextBox.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Provides a Textbox where it's possible to set up a watermark text.
    /// </summary>
    [TemplatePart(Name = WatermarkTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = RemainingTextBlockPartName, Type = typeof(TextBlock))]
    [TemplateVisualState(Name = WatermarkState, GroupName = WatermarkStatesGroupName)]
    [TemplateVisualState(Name = InputState, GroupName = WatermarkStatesGroupName)]
    [TemplateVisualState(Name = RemainingCounterVisibleState, GroupName = RemainingCounterGroupName)]
    [TemplateVisualState(Name = RemainingCounterHiddenState, GroupName = RemainingCounterGroupName)]
    public class WatermarkTextBox : TextBox
    {
        #region Parts

        #region WatermarkTextBlock Template Part

        private const string WatermarkTextBlockPartName = "WatermarkTextBlock";
        private TextBlock WatermarkTextBlockPart;

        #endregion WatermarkTextBlock Template Part

        #region RemainingTextBlock Template Part

        private const string RemainingTextBlockPartName = "RemainingTextBlock";
        private TextBlock RemainingTextBlockPart;

        #endregion RemainingTextBlock Template Part

        #endregion Parts

        #region Visual States

        #region WatermarkStatesGroupName

        /// <summary>
        /// States to be supported in order to indicate whether or not the watermark is visible
        /// </summary>
        protected const string WatermarkStatesGroupName = "WatermarkStates";
        /// <summary>
        ///
        /// </summary>
        protected const string WatermarkState = @"WatermarkState";
        protected const string InputState = @"InputState";

        #endregion WatermarkStatesGroupName

        protected const string RemainingCounterGroupName = "RemainingCounterStates";

        protected const string RemainingCounterVisibleState = "RemainingCounterVisibleState";
        protected const string RemainingCounterHiddenState = "RemainingCounterHiddenState";

        #endregion Visual States

        #region WatermarkText

        /// <summary>
        /// WatermarkText Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkTextBox),
                new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the WatermarkText property.  This dependency property
        /// indicates the text to be displayed by the watermark textbox
        /// </summary>
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        #endregion WatermarkText

        #region WatermarkVisibility

        /// <summary>
        /// WatermarkVisibility Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkVisibilityProperty =
            DependencyProperty.Register("WatermarkVisibility", typeof(WatermarkVisibility), typeof(WatermarkTextBox),
                new FrameworkPropertyMetadata(WatermarkVisibility.FocusBased));

        /// <summary>
        /// Gets or sets the WatermarkVisibility property.  This dependency property
        /// indicates which rules are followed in order to show/hide the watermark
        /// </summary>
        public WatermarkVisibility WatermarkVisibility
        {
            get { return (WatermarkVisibility)GetValue(WatermarkVisibilityProperty); }
            set { SetValue(WatermarkVisibilityProperty, value); }
        }

        #endregion WatermarkVisibility

        #region IsRemainingCounterVisible

        /// <summary>
        /// IsRemainingCounterVisible Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsRemainingCounterVisibleProperty =
            DependencyProperty.Register("IsRemainingCounterVisible", typeof(bool), typeof(WatermarkTextBox),
                new FrameworkPropertyMetadata(true,
                    new PropertyChangedCallback(OnIsRemainingCounterVisibleChanged)));

        /// <summary>
        /// Gets or sets the IsRemainingCounterVisible property.  This dependency property
        /// indicates whether or not to show the remaining character counter
        /// </summary>
        public bool IsRemainingCounterVisible
        {
            get { return (bool)GetValue(IsRemainingCounterVisibleProperty); }
            set { SetValue(IsRemainingCounterVisibleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsRemainingCounterVisible property.
        /// </summary>
        private static void OnIsRemainingCounterVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WatermarkTextBox)d).OnIsRemainingCounterVisibleChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsRemainingCounterVisible property.
        /// </summary>
        protected virtual void OnIsRemainingCounterVisibleChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                VisualStateManager.GoToState(this, RemainingCounterVisibleState, useTransitions: true);
            }
            else
            {
                VisualStateManager.GoToState(this, RemainingCounterHiddenState, useTransitions: true);
            }
        }

        #endregion IsRemainingCounterVisible

        #region IsWhiteSpaceAlsoEmpty

        /// <summary>
        /// IsWhiteSpaceAlsoEmpty Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsWhiteSpaceAlsoEmptyProperty =
            DependencyProperty.Register("IsWhiteSpaceAlsoEmpty", typeof(bool), typeof(WatermarkTextBox),
                new FrameworkPropertyMetadata((bool)false));

        /// <summary>
        /// Gets or sets the IsWhiteSpaceAlsoEmpty property.  This dependency property
        /// indicates whether or not to consider white space as empty
        /// </summary>
        public bool IsWhiteSpaceAlsoEmpty
        {
            get { return (bool)GetValue(IsWhiteSpaceAlsoEmptyProperty); }
            set { SetValue(IsWhiteSpaceAlsoEmptyProperty, value); }
        }

        #endregion IsWhiteSpaceAlsoEmpty

        /// <summary>
        /// Defines the string format to display the remaining text when possible.
        /// </summary>
        [DefaultValue("{0}")]
        public string RemainingTextStringFormat
        {
            get { return remainingTextStringFormat; }
            set { remainingTextStringFormat = value; }
        }

        private string remainingTextStringFormat = "{0}";

        public WatermarkTextBox()
            : base()
        {
            // NOTE: Hook up to the Change text property in the TextBox.
            DependencyPropertyDescriptor textPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(WatermarkTextBox));
            if (textPropertyDescriptor != null)
            {
                textPropertyDescriptor.AddValueChanged(this, delegate { OnTextChanged(); });
            }

            // NOTE: Hook up to the IsReadonly property in the TextBoxBase.
            DependencyPropertyDescriptor isReadonlyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBoxBase.IsReadOnlyProperty, typeof(WatermarkTextBox));
            if (isReadonlyPropertyDescriptor != null)
            {
                isReadonlyPropertyDescriptor.AddValueChanged(this, delegate { OnIsReadonlyChanged(); });
            }
        }

        static WatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            WatermarkTextBlockPart = this.GetTemplateChild(WatermarkTextBlockPartName) as TextBlock;
            RemainingTextBlockPart = this.GetTemplateChild(RemainingTextBlockPartName) as TextBlock;
            ComputeRemainingTextLength(RemainingTextBlockPart);
        }

        protected virtual void OnTextChanged()
        {
            if ((this.WatermarkVisibility & Controls.WatermarkVisibility.TextBased) == Controls.WatermarkVisibility.TextBased)
            {
                ApplyWatermarkStates();
            }
            ComputeRemainingTextLength(this.RemainingTextBlockPart);
        }

        private void ComputeRemainingTextLength(TextBlock remainingTextBlockPart)
        {
            if (this.RemainingTextBlockPart == null)
                return;
            if (string.IsNullOrEmpty(RemainingTextStringFormat))
            {
                remainingTextBlockPart.Text = string.Empty;
            }
            else
            {
                remainingTextBlockPart.Text = string.Format(RemainingTextStringFormat, this.MaxLength - this.Text.Length);
            }
        }

        public event EventHandler ReadonlySet;

        protected virtual void OnReadonlySet()
        {
            var threadSafeReadonlySet = ReadonlySet;
            if (threadSafeReadonlySet != null)
            {
                threadSafeReadonlySet(this, EventArgs.Empty);
            }
        }

        public virtual void OnIsReadonlyChanged()
        {
            if (IsReadOnly)
            {
                OnReadonlySet();
            }
        }

        private void ApplyWatermarkStates()
        {
            if (this.IsEmpty(this.Text))
            {
                VisualStateManager.GoToState(this, WatermarkState, useTransitions: true);
            }
            else
            {
                VisualStateManager.GoToState(this, InputState, useTransitions: true);
            }
        }

        /// <summary>
        /// Returns true if the given text is empty.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual bool IsEmpty(string text)
        {
            return this.IsWhiteSpaceAlsoEmpty ? string.IsNullOrWhiteSpace(text) : string.IsNullOrEmpty(text);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if ((this.WatermarkVisibility & Controls.WatermarkVisibility.FocusBased) == Controls.WatermarkVisibility.FocusBased)
            {
                ApplyWatermarkStates();
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if ((this.WatermarkVisibility & Controls.WatermarkVisibility.FocusBased) == Controls.WatermarkVisibility.FocusBased)
            {
                VisualStateManager.GoToState(this, InputState, useTransitions: true);
            }
        }
    }

    /// <summary>
    /// Identifies which rules should be followed in order to show or hide the watermark
    /// </summary>
    [Flags]
    public enum WatermarkVisibility : int
    {
        FocusBased = 1,
        TextBased = 2
    }
}
