//-----------------------------------------------------------------------
// <copyright file="QuertyKeyboard.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Polaris.Windows.Extensions;
    using Polaris.Windows.Services;
    using Polaris.Wpf.Ui.Extensions;
    using WindowsInput;
using System.ComponentModel;

    [Localizability(LocalizationCategory.Text)]
    [DisplayName("Querty Keyboard")]
    [Description("Allows to create a custom On Screen Keyboard")]
    [TemplatePart(Name = ElementLayoutRootName, Type = typeof(Panel))]
    public class QuertyKeyboard : Control
    {
        #region VirtualKey

        /// <summary>
        /// VirtualKey Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty VirtualKeyProperty =
            DependencyProperty.RegisterAttached("VirtualKey", typeof(DependencyObject), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the VirtualKey property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static DependencyObject GetVirtualKey(DependencyObject d)
        {
            return (DependencyObject)d.GetValue(VirtualKeyProperty);
        }

        /// <summary>
        /// Sets the VirtualKey property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetVirtualKey(DependencyObject d, DependencyObject value)
        {
            d.SetValue(VirtualKeyProperty, value);
        }

        #endregion

        #region UserDefinedKeyHandler

        /// <summary>
        /// UserDefinedKeyHandler Dependency Property
        /// </summary>
        public static readonly DependencyProperty UserDefinedKeyHandlerProperty =
            DependencyProperty.Register("UserDefinedKeyHandler", typeof(IUserDefinedKeyHandler), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(new DefaultUserDefinedKeyHandler()));

        /// <summary>
        /// Gets or sets the UserDefinedKeyHandler property.  This dependency property 
        /// indicates ....
        /// </summary>
        public IUserDefinedKeyHandler UserDefinedKeyHandler
        {
            get { return (IUserDefinedKeyHandler)GetValue(UserDefinedKeyHandlerProperty); }
            set { SetValue(UserDefinedKeyHandlerProperty, value); }
        }

        #endregion

        /// <summary>
        /// Gets or sets the KeyboardLayout property.  This dependency property 
        /// indicates ....
        /// </summary>
        [Description("Enum which will be used to identify the possible layout visual states by name")]
        [Category("Querty OSK")]
        [DisplayName("Keyboard Layout")]
        #region KeyboardLayout
        public Enum KeyboardLayout
        {
            get { return (Enum)GetValue(KeyboardLayoutProperty); }
            set { SetValue(KeyboardLayoutProperty, value); }
        }

        /// <summary>
        /// KeyboardLayout Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyboardLayoutProperty =
            DependencyProperty.Register("KeyboardLayout", typeof(Enum), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(default(Enum),
                    new PropertyChangedCallback(OnKeyboardLayoutChanged)));

        /// <summary>
        /// Handles changes to the KeyboardLayout property.
        /// </summary>
        private static void OnKeyboardLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QuertyKeyboard)d).OnKeyboardLayoutChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the KeyboardLayout property.
        /// </summary>
        protected virtual void OnKeyboardLayoutChanged(DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewValue.ToString(), true);
        }

        #endregion

        /// <summary>
        /// Whether or not use the Debugger Keyboard Service instead of the Virtual Keyboard Service.
        /// <remarks>Other than settings this to true, the app should run attached to a debugger</remarks>
        /// </summary>
        [Description("When set to true it tries to use the IKeyboardInput in debug mode")]
        [Category("Querty OSK")]
        [DisplayName("Latch Debugger")]
        #region LatchDebugger
        public bool LatchDebugger
        {
            get { return (bool)GetValue(LatchDebuggerProperty); }
            set { SetValue(LatchDebuggerProperty, value); }
        }

        /// <summary>
        /// LatchDebugger Dependency Property
        /// </summary>
        public static readonly DependencyProperty LatchDebuggerProperty =
            DependencyProperty.Register("LatchDebugger", typeof(bool), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(false));

        #endregion

        /// <summary>
        /// Gets or sets the InputEventType property.  This dependency property 
        /// indicates ....
        /// </summary>
        [Description("Indicates if the input comes from Mouse Events or Touch events")]
        [Category("Querty OSK")]
        [DisplayName("Input event type")]
        #region InputEventType
        public VirtualKeyboardInputEvent InputEventType
        {
            get { return (VirtualKeyboardInputEvent)GetValue(InputEventTypeProperty); }
            set { SetValue(InputEventTypeProperty, value); }
        }

        /// <summary>
        /// InputEventType Dependency Property
        /// </summary>
        public static readonly DependencyProperty InputEventTypeProperty =
            DependencyProperty.Register("InputEventType", typeof(VirtualKeyboardInputEvent), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata((VirtualKeyboardInputEvent)VirtualKeyboardInputEvent.MouseBasedEvent));
        #endregion

        public IKeyboardInput KeyboardService
        {
            get { return _keyboardService; }
            set { _keyboardService = value; }
        }
        private IKeyboardInput _keyboardService = DebugKeyboardInput.Instance;

        private const String ElementLayoutRootName = "LayoutRoot";
        private Panel _layoutRoot;
        private Dictionary<ContentControl, DependencyLogicalKey> _virtualKeys;
        private readonly List<ModifierKeyBase> _modifierKeys;
        private readonly List<DependencyLogicalKey> _allLogicalKeys;

        static QuertyKeyboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuertyKeyboard), new FrameworkPropertyMetadata(typeof(QuertyKeyboard)));
        }

        public QuertyKeyboard()
        {
            _modifierKeys = new List<ModifierKeyBase>();
            _allLogicalKeys = new List<DependencyLogicalKey>();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            UserDefinedKeyHandler = new DefaultUserDefinedKeyHandler();
            KeyboardLayout = DefaultKeyboardLayout.StandardKeyboard;
        }

        private void HandleLogicKeyPressed(DependencyLogicalKey logicalKey)
        {
            if (logicalKey is ModifierKeyBase)
            {
                var modifierKey = (ModifierKeyBase)logicalKey;
                if (modifierKey.KeyCode == VirtualKeyCode.SHIFT)
                {
                    HandleShiftKeyPressed(modifierKey);
                }
                else if (modifierKey.KeyCode == VirtualKeyCode.CAPITAL)
                {
                    HandleCapsLockKeyPressed(modifierKey);
                }
                else if (modifierKey.KeyCode == VirtualKeyCode.NUMLOCK)
                {
                    HandleNumLockKeyPressed(modifierKey);
                }
            }
            else if (UserDefinedKeyHandler!=null && logicalKey is UserDefinedKey)
            {
                UserDefinedKeyHandler.HandleUserDefinedKey(this, logicalKey, KeyboardService);
            }
            else
            {
                ResetInstantaneousModifierKeys();
            }
            _modifierKeys.OfType<InstantaneousModifierKey>().ToList().ForEach(x => x.SynchroniseKeyState());
            SetKeysContent();
        }

        private void ResetInstantaneousModifierKeys()
        {
            _modifierKeys.OfType<InstantaneousModifierKey>().ToList().ForEach(x => { if (x.IsInEffect) x.Press(); });
        }

        private void SynchroniseModifierKeyState()
        {
            _modifierKeys.ToList().ForEach(x => x.SynchroniseKeyState());
        }

        private void HandleShiftKeyPressed(ModifierKeyBase shiftKey)
        {
            _allLogicalKeys.OfType<CaseSensitiveKey>().ToList().ForEach(x => x.SelectedIndex =
                                                                             InputSimulator.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL) ^ shiftKey.IsInEffect ? 1 : 0);
            _allLogicalKeys.OfType<ShiftSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = shiftKey.IsInEffect ? 1 : 0);
        }

        private void HandleCapsLockKeyPressed(ModifierKeyBase capsLockKey)
        {
            _allLogicalKeys.OfType<CaseSensitiveKey>().ToList().ForEach(x => x.SelectedIndex =
                                                                             capsLockKey.IsInEffect ^ InputSimulator.IsKeyDownAsync(VirtualKeyCode.SHIFT) ? 1 : 0);
        }

        private void HandleNumLockKeyPressed(ModifierKeyBase numLockKey)
        {
            _allLogicalKeys.OfType<NumLockSensitiveKey>().ToList().ForEach(x => x.SelectedIndex = numLockKey.IsInEffect ? 1 : 0);
        }

        public override void OnApplyTemplate()
        {
            _keyboardService = System.Diagnostics.Debugger.IsAttached && LatchDebugger ? DebugKeyboardInput.Instance as IKeyboardInput : VirtualKeyboardInput.Instance as IKeyboardInput;

            base.OnApplyTemplate();

            _layoutRoot = GetTemplateChild(ElementLayoutRootName) as Panel;
            if (_layoutRoot != null)
            {

                _virtualKeys = (from dependencyObject in _layoutRoot.Descendants()
                                let keyConfiguration = dependencyObject.GetValue(QuertyKeyboard.VirtualKeyProperty) as DependencyLogicalKey
                                let element = dependencyObject as ContentControl
                                where keyConfiguration != null && element != null
                                select new KeyValuePair<ContentControl, DependencyLogicalKey>(element, keyConfiguration))
                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                foreach (var element in _virtualKeys.Keys)
                {
                    _virtualKeys[element].KeyboardService = KeyboardService;
                    _virtualKeys[element].LogicalKeyPressed += (s, e) =>
                    {
                        HandleLogicKeyPressed(e.Key);
                    };

                    if (InputEventType == VirtualKeyboardInputEvent.TouchBasedEvent)
                    {
                        var touchUpEventListener = new WeakEventListener<QuertyKeyboard, object, TouchEventArgs>(this);
                        touchUpEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonTouchUp(source, eventArgs);
                        touchUpEventListener.OnDetachAction = (weakEventListenerParameter) => element.TouchUp -= weakEventListenerParameter.OnEvent;
                        element.TouchUp += touchUpEventListener.OnEvent;

                        var touchDownEventListener = new WeakEventListener<QuertyKeyboard, object, TouchEventArgs>(this);
                        touchDownEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonTouchDown(source, eventArgs);
                        touchDownEventListener.OnDetachAction = (weakEventListenerParameter) => element.TouchDown -= weakEventListenerParameter.OnEvent;
                        element.TouchDown += touchDownEventListener.OnEvent;
                    }

                    if (InputEventType == VirtualKeyboardInputEvent.MouseBasedEvent)
                    {
                        var mouseDownEventListener = new WeakEventListener<QuertyKeyboard, object, MouseButtonEventArgs>(this);
                        mouseDownEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonMouseDown(source, eventArgs);
                        mouseDownEventListener.OnDetachAction = (weakEventListenerParameter) => element.MouseDown -= weakEventListenerParameter.OnEvent;
                        element.PreviewMouseDown += mouseDownEventListener.OnEvent;

                        var mouseUpEventListener = new WeakEventListener<QuertyKeyboard, object, MouseButtonEventArgs>(this);
                        mouseUpEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonMouseUp(source, eventArgs);
                        mouseUpEventListener.OnDetachAction = (weakEventListenerParameter) => element.MouseUp -= weakEventListenerParameter.OnEvent;
                        element.PreviewMouseUp += mouseUpEventListener.OnEvent;

                        var mouseDoubleClickEventListener = new WeakEventListener<QuertyKeyboard, object, MouseButtonEventArgs>(this);
                        mouseDoubleClickEventListener.OnDetachAction = (weakEventListenerParameter) => element.PreviewMouseDoubleClick -= weakEventListenerParameter.OnEvent;
                        element.PreviewMouseDoubleClick += mouseDoubleClickEventListener.OnEvent;
                    }
                }

                _allLogicalKeys.AddRange(_virtualKeys.Values);
                _modifierKeys.AddRange(_virtualKeys.Values.OfType<ModifierKeyBase>());
                SynchroniseModifierKeyState();
                SetKeysContent();
            }
        }

        private void SetKeysContent()
        {
            _virtualKeys.Keys.ForEach(element => element.Content = _virtualKeys[element].DisplayName);
        }

        private void OnButtonTouchDown(object sender, TouchEventArgs e)
        {
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            OnHandleButtonDown(virtualKeyConfig);
        }

        private void OnButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            OnHandleButtonDown(virtualKeyConfig);
        }

        private void OnHandleButtonDown(DependencyLogicalKey virtualKeyConfig)
        {
            virtualKeyConfig.Press();
        }

        private void OnButtonTouchUp(object sender, TouchEventArgs e)
        {
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            OnHandleButtonUp(virtualKeyConfig);
        }

        private void OnButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            OnHandleButtonUp(virtualKeyConfig);
        }

        private void OnHandleButtonUp(DependencyLogicalKey virtualKeyConfig)
        {
        }

    }

    public enum VirtualKeyboardInputEvent
    {
        MouseBasedEvent,
        TouchBasedEvent
    }
}
