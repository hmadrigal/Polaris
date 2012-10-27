namespace Polaris
{
    using System;
    using System.ComponentModel;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public interface ITransitionViewModel : INotifyPropertyChanged
    {
        TransitionState TransitionState { get; }

        void BeginUnload();

        void BeginLoad();

        event EventHandler<TransitionEventArgs> TransitionCompleted;

        ICommand TransitionCompletedCommand { get; }
    }

    public static class ITransitionViewModelExtensions
    {
        public static IObservable<EventPattern<TransitionEventArgs>> GetTransitionCompleted(this ITransitionViewModel target)
        {
            return Observable.FromEventPattern<TransitionEventArgs>(target, "TransitionCompleted");
        }

        public static IObservable<EventPattern<TransitionEventArgs>> GetTransitionCompleted(this ITransitionViewModel target, TransitionState targetState)
        {
            var observable = Observable.FromEventPattern<TransitionEventArgs>(target, "TransitionCompleted");
            var filter = from EventPattern<TransitionEventArgs> c in observable
                         where c.EventArgs.CompletedTransition == targetState
                         select c;
            return filter;
        }
    }

    public class TransitionEventArgs : EventArgs
    {
        public TransitionState CompletedTransition { get; set; }
    }

    public enum TransitionState
    {
        BeforeLoaded,
        Loaded,
        Unloaded,
    }
}