//-----------------------------------------------------------------------
// <copyright file="InvokeCommandOnKeyAction.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Actions
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using Polaris.Windows.Extensions;

    public class InvokeCommandOnKeyAction : TriggerAction<UIElement>
    {
        #region Key

        /// <summary>
        /// Key Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(Key), typeof(InvokeCommandOnKeyAction),
                new FrameworkPropertyMetadata((Key)Key.Enter,
                    new PropertyChangedCallback(OnKeyChanged)));

        /// <summary>
        /// Gets or sets the Key property.  This dependency property
        /// indicates the key that will trigger the command.
        /// </summary>
        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Key property.
        /// </summary>
        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InvokeCommandOnKeyAction)d).OnKeyChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Key property.
        /// </summary>
        protected virtual void OnKeyChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion Key

        #region KeyEvent

        /// <summary>
        /// KeyEvent Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyEventProperty =
            DependencyProperty.Register("KeyEvent", typeof(KeyEvents), typeof(InvokeCommandOnKeyAction),
                new FrameworkPropertyMetadata((KeyEvents)KeyEvents.KeyDown,
                    new PropertyChangedCallback(OnKeyEventChanged)));

        /// <summary>
        /// Gets or sets the KeyEvent property.  This dependency property
        /// indicates the event that triggers the command.
        /// </summary>
        public KeyEvents KeyEvent
        {
            get { return (KeyEvents)GetValue(KeyEventProperty); }
            set { SetValue(KeyEventProperty, value); }
        }

        /// <summary>
        /// Handles changes to the KeyEvent property.
        /// </summary>
        private static void OnKeyEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InvokeCommandOnKeyAction)d).OnKeyEventChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KeyEvent property.
        /// </summary>
        protected virtual void OnKeyEventChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion KeyEvent

        #region Command

        /// <summary>
        /// Command Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandOnKeyAction),
                new FrameworkPropertyMetadata((ICommand)null,
                    new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Gets or sets the Command property.  This dependency property
        /// indicates the command to execute when the key event happens.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Command property.
        /// </summary>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InvokeCommandOnKeyAction)d).OnCommandChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Command property.
        /// </summary>
        protected virtual void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            var newCommand = e.NewValue as ICommand;
            if (newCommand != null)
            {
                //newCommand.CanExecuteChanged += new System.EventHandler(Command_CanExecuteChanged);
                var canExecuteChangedEventListener = new WeakEventListener<InvokeCommandOnKeyAction, object, EventArgs>(this);
                canExecuteChangedEventListener.OnEventAction = (instance, source, eventArgs) =>
                    instance.Command_CanExecuteChanged(source, eventArgs);
                canExecuteChangedEventListener.OnDetachAction = (weakEventListenerParameter) =>
                    newCommand.CanExecuteChanged -= weakEventListenerParameter.OnEvent;
                newCommand.CanExecuteChanged += canExecuteChangedEventListener.OnEvent;
            }
        }

        private void Command_CanExecuteChanged(object sender, System.EventArgs e)
        {
            AssociatedObject.IsEnabled = Command.CanExecute(CommandParameter);
        }

        #endregion Command

        #region CommandParameter

        /// <summary>
        /// CommandParameter Dependency Property
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandOnKeyAction),
                new FrameworkPropertyMetadata((object)null,
                    new PropertyChangedCallback(OnCommandParameterChanged)));

        /// <summary>
        /// Gets or sets the CommandParameter property.  This dependency property
        /// indicates the parameter to send to the command when it is executed.
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CommandParameter property.
        /// </summary>
        private static void OnCommandParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((InvokeCommandOnKeyAction)d).OnCommandParameterChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the CommandParameter property.
        /// </summary>
        protected virtual void OnCommandParameterChanged(DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion CommandParameter

        protected override void Invoke(object parameter)
        {
            //AssociatedObject.KeyDown += new System.Windows.Input.KeyEventHandler(AssociatedObject_KeyDown);
            var keyDownEventListener = new WeakEventListener<InvokeCommandOnKeyAction, object, KeyEventArgs>(this);
            keyDownEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.AssociatedObject_KeyDown(source, eventArgs);
            keyDownEventListener.OnDetachAction = (weakEventListenerParameter) =>
                AssociatedObject.KeyDown -= weakEventListenerParameter.OnEvent;
            AssociatedObject.KeyDown += keyDownEventListener.OnEvent;

            //AssociatedObject.KeyUp += new KeyEventHandler(AssociatedObject_KeyUp);
            var keyUpEventListener = new WeakEventListener<InvokeCommandOnKeyAction, object, KeyEventArgs>(this);
            keyUpEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.AssociatedObject_KeyUp(source, eventArgs);
            keyUpEventListener.OnDetachAction = (weakEventListenerParameter) =>
                AssociatedObject.KeyUp -= weakEventListenerParameter.OnEvent;
            AssociatedObject.KeyUp += keyUpEventListener.OnEvent;

            //AssociatedObject.PreviewKeyDown += new KeyEventHandler(AssociatedObject_PreviewKeyDown);
            var previewKeyDownEventListener = new WeakEventListener<InvokeCommandOnKeyAction, object, KeyEventArgs>(this);
            previewKeyDownEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.AssociatedObject_PreviewKeyDown(source, eventArgs);
            previewKeyDownEventListener.OnDetachAction = (weakEventListenerParameter) =>
                AssociatedObject.PreviewKeyDown -= weakEventListenerParameter.OnEvent;
            AssociatedObject.PreviewKeyDown += previewKeyDownEventListener.OnEvent;

            //AssociatedObject.PreviewKeyUp += new KeyEventHandler(AssociatedObject_PreviewKeyUp);
            var previewKeyUpEventListener = new WeakEventListener<InvokeCommandOnKeyAction, object, KeyEventArgs>(this);
            previewKeyUpEventListener.OnEventAction = (instance, source, eventArgs) =>
                instance.AssociatedObject_PreviewKeyUp(source, eventArgs);
            previewKeyUpEventListener.OnDetachAction = (weakEventListenerParameter) =>
                AssociatedObject.PreviewKeyUp -= weakEventListenerParameter.OnEvent;
            AssociatedObject.PreviewKeyUp += previewKeyUpEventListener.OnEvent;
        }

        private void AssociatedObject_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (KeyEvent != KeyEvents.PreviewKeyUp) { return; }
            if (e.Key != this.Key) { return; }
            ExecuteCommand();
        }

        private void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyEvent != KeyEvents.PreviewKeyDown) { return; }
            if (e.Key != this.Key) { return; }
            ExecuteCommand();
        }

        private void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
        {
            if (KeyEvent != KeyEvents.KeyUp) { return; }
            if (e.Key != this.Key) { return; }
            ExecuteCommand();
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (KeyEvent != KeyEvents.KeyDown) { return; }
            if (e.Key != this.Key) { return; }
            ExecuteCommand();
        }

        private void ExecuteCommand()
        {
            if (Command != null)
            {
                if (Command.CanExecute(CommandParameter))
                {
                    Command.Execute(CommandParameter);
                }
            }
        }
    }

    public enum KeyEvents
    {
        PreviewKeyUp,
        PreviewKeyDown,
        KeyDown,
        KeyUp,
    }
}