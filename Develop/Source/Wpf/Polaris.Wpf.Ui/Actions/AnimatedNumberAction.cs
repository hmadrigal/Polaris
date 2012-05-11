//-----------------------------------------------------------------------
// <copyright file="AnimatedNumberAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Polaris.Windows.Extensions;

    public class AnimatedNumberAction : TriggerAction<TextBlock>
    {

        const String DefaultFormat = "{0:#,###,##0}";

        #region Value

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Double), typeof(AnimatedNumberAction),
                new PropertyMetadata((Double)0,
                    new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// Gets or sets the Value property.  This dependency property 
        /// indicates the value to present on the target TextBlock.
        /// </summary>
        public Double Value
        {
            get { return (Double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedNumberAction)d).OnValueChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var isAnimating = GetIsAnimating();
            if (isAnimating) { return; }
            PreviousValue = (Double)e.OldValue;
            StartTime = DateTime.Now;
            IsAnimationFinished = false;
        }

        #endregion

        #region Duration

        /// <summary>
        /// Duration Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AnimatedNumberAction),
                new PropertyMetadata((TimeSpan)new TimeSpan(0, 0, 0, 0, 100)));

        /// <summary>
        /// Gets or sets the Duration property.  This dependency property 
        /// indicates the duration of the animation played when the values change.
        /// </summary>
        [Category(CategoryNames.Animation),
           Description("Indicates the duration of the animation played when the values change.")]
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        #endregion

        #region MaxDuration

        /// <summary>
        /// MaxDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaxDurationProperty =
            DependencyProperty.Register("MaxDuration", typeof(TimeSpan), typeof(AnimatedNumberAction),
                new PropertyMetadata((TimeSpan)new TimeSpan(0, 0, 10)));

        /// <summary>
        /// Gets or sets the MaxDuration property.  This dependency property 
        /// indicates the maximum duration of the animation regardless of the magnitude of the change.
        /// </summary>
        public TimeSpan MaxDuration
        {
            get { return (TimeSpan)GetValue(MaxDurationProperty); }
            set { SetValue(MaxDurationProperty, value); }
        }

        #endregion

        #region StepSize

        /// <summary>
        /// StepSize Dependency Property
        /// </summary>
        public static readonly DependencyProperty StepSizeProperty =
            DependencyProperty.Register("StepSize", typeof(Double), typeof(AnimatedNumberAction),
                new PropertyMetadata((Double)10));

        /// <summary>
        /// Gets or sets the StepSize property.  This dependency property 
        /// indicates the size of the step the duration will apply to.
        /// </summary>
        public Double StepSize
        {
            get { return (Double)GetValue(StepSizeProperty); }
            set { SetValue(StepSizeProperty, value); }
        }

        #endregion

        #region EasingFunction

        /// <summary>
        /// EasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(AnimatedNumberAction),
                new PropertyMetadata((IEasingFunction)null));

        /// <summary>
        /// Gets or sets the EasingFunction property.  This dependency property 
        /// indicates the easing function to use in order to perform the value changes.
        /// </summary>
        [Category(CategoryNames.Animation),
            Description("Indicates the easing function to use in order to perform the value changes.")]
        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        #endregion

        #region Format

        /// <summary>
        /// Format Dependency Property
        /// </summary>
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(String), typeof(AnimatedNumberAction),
                new PropertyMetadata((String)DefaultFormat));

        /// <summary>
        /// Gets or sets the Format property.  This dependency property 
        /// indicates ....
        /// </summary>
        public String Format
        {
            get { return (String)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        #endregion

        [CustomPropertyValueEditorAttribute(CustomPropertyValueEditor.Storyboard)]
        public Storyboard FinalStoryboard { get; set; }

        private Double PreviousValue { get; set; }
        private DateTime StartTime { get; set; }
        private Boolean IsAnimationFinished { get; set; }

        protected override void Invoke(object parameter)
        {
        }

        protected override void OnAttached()
        {
            var _weakEventListener = new WeakEventListener<AnimatedNumberAction, object, EventArgs>(this);
            _weakEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.CompositionTarget_Rendering(source, eventArgs);
            _weakEventListener.OnDetachAction = (weakEventListener) =>
                CompositionTarget.Rendering -= weakEventListener.OnEvent;
            CompositionTarget.Rendering += _weakEventListener.OnEvent;
        }


        void CompositionTarget_Rendering(object sender, EventArgs e)
        {

            if (IsAnimationFinished) { return; }

            var elapsedTime = DateTime.Now.Subtract(StartTime);

            if (elapsedTime < TimeSpan.Zero) { return; }

            var magnitude = Math.Abs(Value - PreviousValue);

            var totalTicks = (long)(((Double)Duration.Ticks) * (magnitude / Math.Max(1, StepSize)));

            totalTicks = Math.Min(totalTicks, MaxDuration.Ticks);

            var totalDuration = new TimeSpan(totalTicks);

            if (elapsedTime < totalDuration)
            {

                if (FinalStoryboard != null)
                {
                    FinalStoryboard.Stop();
                }


                var normalizedTime = (Double)elapsedTime.Ticks / (Double)totalDuration.Ticks;

                var easing =
                    (EasingFunction == null ?
                        normalizedTime // if there is no easing function, use linear animation.
                        :
                        EasingFunction.Ease(normalizedTime)); //Ease(normalizedTime);

                var currentDisplacement = ((Value - PreviousValue) * easing);

                SetText(PreviousValue + currentDisplacement);

            }
            else
            {
                SetText(Value);
                if (FinalStoryboard != null)
                {
                    FinalStoryboard.Begin();
                }
                IsAnimationFinished = true;
            }

        }

        protected void SetText(Double value)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var format = DefaultFormat;
            if (!String.IsNullOrEmpty(Format))
            {
                format = Format;
            }

            AssociatedObject.Text = String.Format(culture, format, value);
        }

        protected Boolean GetIsAnimating()
        {
            return StartTime.Add(Duration) >= DateTime.Now;
        }
    }
}
