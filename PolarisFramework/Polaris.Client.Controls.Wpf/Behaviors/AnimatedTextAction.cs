//-----------------------------------------------------------------------
// <copyright file="AnimatedTextAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Controls.Wpf.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interactivity;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Polaris.Client.Controls.Wpf.Extensions;

    public class AnimatedTextAction : TriggerAction<TextBlock>
    {

        #region Value

        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(String), typeof(AnimatedTextAction),
                new PropertyMetadata((String)"",
                    new PropertyChangedCallback(OnValueChanged)));

        /// <summary>
        /// Gets or sets the Value property. This dependency property 
        /// indicates the text displayed by the TextBlock.
        /// </summary>
        public String Value
        {
            get { return (String)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AnimatedTextAction)d).OnValueChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Value property.
        /// </summary>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            this.PreviousTextValue = e.OldValue.ToString();
            var isAnimating = GetIsAnimating();
            if (isAnimating) { return; }
            StartTime = DateTime.Now;
        }

        #endregion

        #region Duration

        /// <summary>
        /// Duration Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AnimatedTextAction),
                new PropertyMetadata((TimeSpan)new TimeSpan(0, 0, 0, 0, 100)));

        /// <summary>
        /// Gets or sets the Duration property. This dependency property 
        /// indicates duration of the animation played when the value changed.
        /// </summary>
        [Category(CategoryNames.Animation),
           Description("Indicates the duration of the animation played when the values change.")]
        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        #endregion

        #region ChangesPerSecond

        /// <summary>
        /// ChangesPerSecondProperty Dependency Property
        /// </summary>
        public static readonly DependencyProperty ChangesPerSecondProperty =
            DependencyProperty.Register("ChangesPerSecond", typeof(Double), typeof(AnimatedTextAction),
                new PropertyMetadata((Double)1000));

        /// <summary>
        /// Gets or sets the ChangesPerSecondProperty property. This dependency property 
        /// indicates the quantity of changes of the text when the animation is played
        /// </summary>
        public Double ChangesPerSecond
        {
            get { return (Double)GetValue(ChangesPerSecondProperty); }
            set { SetValue(ChangesPerSecondProperty, value); }
        }

        #endregion

        #region EasingFunction

        /// <summary>
        /// EasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(AnimatedTextAction),
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

        #region TextChangePolicy

        /// <summary>
        /// TextChangePolicy Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextChangePolicyProperty =
            DependencyProperty.Register("TextChangePolicy", typeof(TextChangePolicy), typeof(AnimatedTextAction),
                new PropertyMetadata(TextChangePolicy.Always));

        /// <summary>
        /// Gets or sets the TextChangePolicy property. This dependency property 
        /// indicates policy of the character replacement in the animation.
        /// </summary>
        public TextChangePolicy TextChangePolicy
        {
            get { return (TextChangePolicy)GetValue(TextChangePolicyProperty); }
            set { SetValue(TextChangePolicyProperty, value); }
        }

        #endregion




        private DateTime StartTime { get; set; }

        private Double PreviousTimeValue { get; set; }

        private String PreviousTextValue { get; set; }

        private Random RandomGen { get; set; }

        protected override void Invoke(object parameter) { }



        protected override void OnAttached()
        {
            RandomGen = new Random();
            this.PreviousTextValue = "";
            var _weakEventListener = new WeakEventListener<AnimatedTextAction, object, EventArgs>(this);
            _weakEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.CompositionTarget_Rendering(source, eventArgs);
            _weakEventListener.OnDetachAction = (weakEventListener) =>
                CompositionTarget.Rendering -= weakEventListener.OnEvent;
            CompositionTarget.Rendering += _weakEventListener.OnEvent;
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            var elapsedTime = DateTime.Now.Subtract(StartTime);
            if (elapsedTime < TimeSpan.Zero) { return; }

            var totalTicks = (long)Duration.Ticks;

            var totalDuration = new TimeSpan(totalTicks);

            if (elapsedTime < totalDuration)
            {
                var normalizedTime = (Double)elapsedTime.Ticks / (Double)totalDuration.Ticks;
                var easing =
                    (EasingFunction == null ?
                        normalizedTime // if there is no easing function, use linear animation.
                        :
                        EasingFunction.Ease(normalizedTime)); //Ease(normalizedTime);

                var deltaTime = normalizedTime - PreviousTimeValue;


                //If the delta easing indicates that threshold is exceeded.
                if (Math.Abs(deltaTime) > (Double)Duration.TotalSeconds / (ChangesPerSecond))
                {
                    AssociatedObject.Text = GetNewRandomText(easing);
                    PreviousTimeValue = normalizedTime;
                }
            }
            else
            {
                AssociatedObject.Text = Value;
            }
        }

        private string GetNewRandomText(Double easing)
        {

            char[] values = Value.ToCharArray();
            char[] oldValues = this.PreviousTextValue.ToCharArray();
            int startChar = (int)(easing * values.Length);
            for (int i = startChar; i < values.Length; i++)
            {
                char oldChar = (oldValues.Length > i) ? oldValues[i] : (char)0;
                values[i] = GetCharacterAccordingChangePolicy(oldChar, values[i]);
            }
            return new String(values);
        }

        private Char GetCharacterAccordingChangePolicy(Char oldValue, Char newValue)
        {
            char result;
            switch (this.TextChangePolicy)
            {
                case TextChangePolicy.Always:
                    {
                        result = GetRandomCharacter(newValue);
                        break;
                    }
                case TextChangePolicy.WhenChange:
                    {
                        if (oldValue.Equals(newValue))
                        {
                            result = newValue;
                        }
                        else
                        {
                            result = GetRandomCharacter(newValue);
                        }
                        break;
                    }

                default:
                    result = ' ';
                    break;
            }
            return result;
        }

        private Char GetRandomCharacter(Char newValue)
        {
            char result = ' ';
            if (newValue >= 48 && newValue <= 57)
            {
                result = (char)RandomGen.Next(48, 58);
            }
            else if (newValue >= 65 && newValue <= 90)
            {
                result = (char)RandomGen.Next(65, 91);
            }
            else if (newValue >= 97 && newValue <= 122)
            {
                result = (char)RandomGen.Next(97, 122);
            }
            else if (newValue.Equals(' '))
            {
                //Keep the space 
                result = ' ';
            }
            else
            {
                result = (char)RandomGen.Next(33, 126);
            }
            return result;
        }


        private Boolean GetIsAnimating()
        {
            return StartTime.Add(Duration) >= DateTime.Now;
        }
    }

    /// <summary>
    /// Enumeration of policies to change the text when a change occurs
    /// </summary>
    public enum TextChangePolicy : int
    {
        /// <summary>
        /// Animate all the characters.
        /// </summary>
        Always,

        /// <summary>
        /// Animate only the characters that change.
        /// </summary>
        WhenChange
    }
}
