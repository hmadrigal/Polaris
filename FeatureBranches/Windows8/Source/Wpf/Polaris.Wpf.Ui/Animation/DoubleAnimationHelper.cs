//-----------------------------------------------------------------------
// <copyright file="DoubleAnimationHelper.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
#if NETFX_CORE
namespace Polaris.Animation
#else
namespace Polaris.Windows.Animation
#endif
{
    using System;
    using Windows.UI.Xaml.Media.Animation;
#if NETFX_CORE
#else
    using System.Windows.Media.Animation;
#endif

    public class DoubleAnimationHelper
    {
        private DateTime? StartTime { get; set; }

        public double InitialValue { get; private set; }

        public double FinalValue { get; private set; }

        private Action OnAnimationCompleted { get; set; }

        private Action<double> OnAnimationProgress { get; set; }

        public TimeSpan Duration { get; set; }

#if NETFX_CORE
        public EasingFunctionBase EasingFunction { get; set; }
#else
        public IEasingFunction EasingFunction { get; set; }
#endif

        public void Update()
        {
            if (StartTime == null) { return; }
            var startTime = StartTime.Value;
            var initialValue = InitialValue;
            var finalValue = FinalValue;
            var duration = Duration;
            var easingFunction = EasingFunction;

            var elapsedTime = DateTime.Now.Subtract(startTime);

            if (elapsedTime < TimeSpan.Zero) { return; }

            var magnitude = Math.Abs(finalValue - initialValue);

            if (elapsedTime > duration)
            {
                StopAnimation();
                return;
            }

            var normalizedTime = (Double)elapsedTime.Ticks / (Double)duration.Ticks;

            var easing =
                (easingFunction == null ?
                    normalizedTime // if there is no easing function, use linear animation.
                    :
                    easingFunction.Ease(normalizedTime));

            var currentStepValue = initialValue + (((finalValue - initialValue) * easing));

            OnAnimationProgress(currentStepValue);
        }

        public DoubleAnimationHelper(Action onCompletedAction, Action<double> onProgressAction)
        {
            OnAnimationCompleted = onCompletedAction;
            OnAnimationProgress = onProgressAction;
        }

        public void StartAnimation(double initialValue = 0.0, double finalValue = 1.0)
        {
            InitialValue = initialValue;
            FinalValue = finalValue;
            StartTime = DateTime.Now;
        }

        public void StopAnimation()
        {
            if (StartTime == null) { return; }
            StartTime = null;
            OnAnimationCompleted();
        }
    }
}
