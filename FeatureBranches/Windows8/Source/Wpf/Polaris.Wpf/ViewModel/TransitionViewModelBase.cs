namespace Polaris
{
    using System;
    using System.Windows.Input;
    using Microsoft.Practices.Prism.Commands;
    using Microsoft.Practices.Unity;

    public abstract class TransitionViewModelBase : ViewModelBase, ITransitionViewModel
    {
        public TransitionViewModelBase(IUnityContainer container)
            : base(container)
        {
            TransitionCompletedCommand = new DelegateCommand<TransitionState?>(OnTransitionCompleted);
        }

        #region ITransitionViewModel implementation

        #region TransitionState (INotifyPropertyChanged Property)

        private TransitionState transitionState;

        public TransitionState TransitionState
        {
            get { return transitionState; }
            set
            {
                if (transitionState != value)
                {
                    transitionState = value;
                    OnPropertyChanged("TransitionState");
                }
            }
        }

        #endregion TransitionState (INotifyPropertyChanged Property)

        public void BeginUnload()
        {
            SetTransitionState(TransitionState.Unloaded);
        }

        public void BeginLoad()
        {
            SetTransitionState(TransitionState.Loaded);
        }

        private void SetTransitionState(TransitionState state)
        {
            if (TransitionState == state)
            {
                OnTransitionCompleted(state);
            }
            else
            {
                TransitionState = state;
            }
        }

        public event EventHandler<TransitionEventArgs> TransitionCompleted;

        protected virtual void OnTransitionCompleted(TransitionState completedState)
        {
            var threadSafeInstance = TransitionCompleted;
            if (threadSafeInstance != null)
            {
                threadSafeInstance(this, new TransitionEventArgs() { CompletedTransition = completedState, });
            }
        }

        protected virtual void OnTransitionCompleted(TransitionState? state)
        {
            if (state == null) { return; }
            OnTransitionCompleted(state.Value);
        }

        #region TransitionCompletedCommand (INotifyPropertyChanged Property)

        private ICommand transitionCompletedCommand;

        public ICommand TransitionCompletedCommand
        {
            get { return transitionCompletedCommand; }
            set
            {
                if (transitionCompletedCommand != value)
                {
                    transitionCompletedCommand = value;
                    OnPropertyChanged("TransitionCompletedCommand");
                }
            }
        }

        #endregion TransitionCompletedCommand (INotifyPropertyChanged Property)

        #endregion ITransitionViewModel implementation
    }
}