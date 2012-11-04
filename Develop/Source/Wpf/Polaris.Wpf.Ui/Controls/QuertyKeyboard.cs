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

        #region KeyboardLayout

        /// <summary>
        /// KeyboardLayout Dependency Property
        /// </summary>
        public static readonly DependencyProperty KeyboardLayoutProperty =
            DependencyProperty.Register("KeyboardLayout", typeof(Enum), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(default(Enum),
                    new PropertyChangedCallback(OnKeyboardLayoutChanged)));

        /// <summary>
        /// Gets or sets the KeyboardLayout property.  This dependency property 
        /// indicates ....
        /// </summary>
        public Enum KeyboardLayout
        {
            get { return (Enum)GetValue(KeyboardLayoutProperty); }
            set { SetValue(KeyboardLayoutProperty, value); }
        }

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

        private VirtualKeyboardInputEvent _inputEventType = VirtualKeyboardInputEvent.MouseBasedEvent;
        public VirtualKeyboardInputEvent InputEventType
        {
            get { return _inputEventType; }
            set { 
                _inputEventType = value; 
            }
        }

        public IKeyboardInput KeyboardService
        {
            get { return _keyboardService; }
            set { _keyboardService = value; }
        }
        private IKeyboardInput _keyboardService = VirtualKeyboardInput.Instance;

        private const String ElementLayoutRootName = "LayoutRoot";
        private Panel _layoutRoot;
        private Dictionary<ContentControl, DependencyLogicalKey> _virtualKeys;
        private readonly List<ModifierKeyBase> _modifierKeys;
        private readonly List<DependencyLogicalKey> _allLogicalKeys;

        private readonly DispatcherTimer _timer;
        public int PauseOnKeyPressedInitial
        {
            get { return _pauseOnKeyPressedInitial; }
            set { _pauseOnKeyPressedInitial = value; }
        }
        private int _pauseOnKeyPressedInitial = 500;
        public int PauseOnKeyPressedMinimum
        {
            get { return _pauseOnKeyPressedMinimum; }
            set { _pauseOnKeyPressedMinimum = value; }
        }
        private int _pauseOnKeyPressedMinimum = 50;

        static QuertyKeyboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuertyKeyboard), new FrameworkPropertyMetadata(typeof(QuertyKeyboard)));
        }

        public QuertyKeyboard()
        {
            _timer = new DispatcherTimer();
            _modifierKeys = new List<ModifierKeyBase>();
            _allLogicalKeys = new List<DependencyLogicalKey>();
            _timer.Interval = TimeSpan.FromMilliseconds(PauseOnKeyPressedInitial);
            _timer.Tick += OnTimerTick;
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
                        mouseDoubleClickEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonMouseDoubleClick(source, eventArgs);
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
            HandleButtonDown(virtualKeyConfig);
        }

        private void OnButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            HandleButtonDown(virtualKeyConfig);
        }

        private void OnButtonTouchUp(object sender, TouchEventArgs e)
        {
            HandleButtonUp();
        }

        private void OnButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            HandleButtonUp();
        }

        private void OnButtonMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var element = sender as ContentControl;
            //var virtualKeyConfig = _virtualKeys[element];
            //if (!virtualKeyConfig.IsSticky)
            //    return;
            //_isShiftSticked = !_isShiftSticked;
            //if (_isShiftSticked)
            //{
            //    KeyboardService.PressAndHold(VirtualKeyCode .VK_LSHIFT);
            //}
            //else
            //{
            //    KeyboardService.ReleaseStickyKeys();
            //}
            //IsShiftPressed = _isShiftSticked;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var virtualKeyConfig = timer.Tag as DependencyLogicalKey;
            if (timer.Interval.TotalMilliseconds > PauseOnKeyPressedMinimum)
            {
                timer.Interval = TimeSpan.FromMilliseconds(timer.Interval.TotalMilliseconds / 2);
            }
            virtualKeyConfig.Press();
        }

        private void HandleButtonUp()
        {
            _timer.IsEnabled = false;
        }

        private void HandleButtonDown(DependencyLogicalKey virtualKeyConfig)
        {
            _timer.Tag = virtualKeyConfig;
            virtualKeyConfig.Press();
            //HandleLogicKeyPressed(virtualKeyConfig);
            if (!_timer.IsEnabled && !(virtualKeyConfig is ModifierKeyBase))
            {
                _timer.Interval = TimeSpan.FromMilliseconds(PauseOnKeyPressedInitial);
                _timer.IsEnabled = true;
            }
        }
    }

    public enum VirtualKeyboardInputEvent
    {
        MouseBasedEvent,
        TouchBasedEvent
    }
}
