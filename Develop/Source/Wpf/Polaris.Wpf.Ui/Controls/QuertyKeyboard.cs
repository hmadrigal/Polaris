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

    [TemplatePart(Name = ElementLayoutRootName, Type = typeof(Panel))]
    [TemplateVisualState(Name = VisualStateQuertyName, GroupName = VisualStateGroupKeyboardLayoutName)]
    [TemplateVisualState(Name = VisualStateNumericName, GroupName = VisualStateGroupKeyboardLayoutName)]
    public class QuertyKeyboard : Control
    {

        #region VirtualKey

        /// <summary>
        /// VirtualKey Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty VirtualKeyProperty =
            DependencyProperty.RegisterAttached("VirtualKey", typeof(VirtualKeyConfig), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the VirtualKey property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static VirtualKeyConfig GetVirtualKey(DependencyObject d)
        {
            return (VirtualKeyConfig)d.GetValue(VirtualKeyProperty);
        }

        /// <summary>
        /// Sets the VirtualKey property.  This dependency property 
        /// indicates ....
        /// </summary>
        public static void SetVirtualKey(DependencyObject d, VirtualKeyConfig value)
        {
            d.SetValue(VirtualKeyProperty, value);
        }

        #endregion

        #region Visual states

        private const String VisualStateGroupKeyboardLayoutName = "KeyboardLayoutStates";
        private const String VisualStateQuertyName = "QuertyState";
        private const String VisualStateNumericName = "NumericState";

        private VisualStateGroup KeyboardLayoutVisualStateGroup;
        private VisualState QuertyVisualState;
        private VisualState NumericVisualState;

        #endregion Visual states

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

        #region CustomVirtualKeyHandler

        /// <summary>
        /// CustomVirtualKeyHandler Dependency Property
        /// </summary>
        public static readonly DependencyProperty CustomVirtualKeyHandlerProperty =
            DependencyProperty.Register("CustomVirtualKeyHandler", typeof(IVirtualKeyHandler), typeof(QuertyKeyboard),
                new FrameworkPropertyMetadata(default(IVirtualKeyHandler)));

        /// <summary>
        /// Gets or sets the CustomVirtualKeyHandler property.  This dependency property 
        /// indicates ....
        /// </summary>
        public IVirtualKeyHandler CustomVirtualKeyHandler
        {
            get { return (IVirtualKeyHandler)GetValue(CustomVirtualKeyHandlerProperty); }
            set { SetValue(CustomVirtualKeyHandlerProperty, value); }
        }

        #endregion

        public IVirtualKeyboardService KeyboardService
        {
            get { return _keyboardService; }
            set { _keyboardService = value; }
        }
        private IVirtualKeyboardService _keyboardService = VirtualKeyboardService.Instance;

        private const String ElementLayoutRootName = "LayoutRoot";
        private Panel _layoutRoot;
        private Dictionary<ContentControl, VirtualKeyConfig> _virtualKeys;

        public bool IsShiftPressed
        {
            get { return _isShiftPressed; }
            set
            {
                if (_isShiftPressed == value) return;
                _isShiftPressed = value;
                OnIsShiftPressedChanged();
            }
        }
        private bool _isShiftPressed = false;

        public bool IsCapsLockActivated
        {
            get { return _isCapsLockActivated; }
            set
            {
                if (_isCapsLockActivated == value)
                    return;
                _isCapsLockActivated = value;
                OnIsCapsLockActivatedChanged();
            }
        }
        private bool _isCapsLockActivated = false;

        //public bool IsShiftSticked
        //{
        //    get { return _isShiftSticked; }
        //    set { _isShiftSticked = value; }
        //}
        private bool _isShiftSticked = false;

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
            _timer.Interval = TimeSpan.FromMilliseconds(PauseOnKeyPressedInitial);
            _timer.Tick += OnTimerTick;
            Loaded += QuertyKeyboard_Loaded;
            Application.Current.Startup += OnApplicationStartup;
            Application.Current.Exit += OnApplicationExit;
        }

        void QuertyKeyboard_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= QuertyKeyboard_Loaded;
            CustomVirtualKeyHandler = new DefaultCustomVirtualKeyHandler();
            KeyboardLayout = DefaultKeyboardLayout.StandardKeyboard;
        }

        void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            KeyboardService.ReleaseStickyKeys();
            Application.Current.Startup -= OnApplicationStartup;
        }

        void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Application.Current.Exit -= OnApplicationExit;
            KeyboardService.ReleaseStickyKeys();
        }

        private void OnSendKeyStroke(VirtualKeyConfig virtualKeyConfig)
        {
            //if (!OnCanExecuteStringStroke(keyStats.DefaultContent)) { return; }
            switch (virtualKeyConfig.KeyCode)
            {
                case KeysEx.None:
                    OnCustomKeyStroke(virtualKeyConfig);
                    break;
                case KeysEx.VK_CAPITAL:
                    IsCapsLockActivated = !IsCapsLockActivated;
                    KeyboardService.PressAndRelease(KeysEx.VK_CAPITAL);
                    break;
                case KeysEx.VK_LSHIFT:
                case KeysEx.VK_RSHIFT:
                case KeysEx.VK_SHIFT:
                    KeyboardService.PressAndHold(KeysEx.VK_LSHIFT);
                    IsShiftPressed = !IsShiftPressed;
                    if (!_isShiftSticked)
                        KeyboardService.ReleaseStickyKeys();
                    break;
                default:
                    if (_isShiftSticked)
                        KeyboardService.PressAndHold(KeysEx.VK_LSHIFT);    
                    KeyboardService.PressAndRelease(virtualKeyConfig.KeyCode);
                    if (!_isShiftSticked)
                        IsShiftPressed = false; 
                    break;
            }
        }

        private void OnCustomKeyStroke(VirtualKeyConfig virtualKeyConfig)
        {
            if (CustomVirtualKeyHandler == null)
                return;
            CustomVirtualKeyHandler.HandleCustomKeyStroke(this, virtualKeyConfig, KeyboardService);
        }

        private void OnIsShiftPressedChanged()
        {
            SetVirtualKeysContent();
        }

        private void OnIsCapsLockActivatedChanged()
        {
            SetVirtualKeysContent();
        }

        private void SetVirtualKeysContent()
        {
            foreach (var element in _virtualKeys.Keys)
            {
                var virtualKeyConfig = _virtualKeys[element];
                var content = GetContent(element, virtualKeyConfig, virtualKeyConfig.DefaultContent);
                element.Content = content;
            }
        }

        private object GetContent(ContentControl element, VirtualKeyConfig virtualKeyConfig, object fallbackContent = null)
        {
            var content =
                virtualKeyConfig.CapitalizedContent != null && IsCapsLockActivated && !IsShiftPressed ? virtualKeyConfig.CapitalizedContent :
                virtualKeyConfig.CapitalizedContent != null && IsCapsLockActivated && IsShiftPressed ? virtualKeyConfig.DefaultContent :
                virtualKeyConfig.ShiftContent != null && (IsShiftPressed || _isShiftSticked) && !IsCapsLockActivated ? virtualKeyConfig.ShiftContent :
                virtualKeyConfig.DefaultContent != null && IsShiftPressed && IsCapsLockActivated ? virtualKeyConfig.DefaultContent :
                fallbackContent ?? element.Content;
            return content;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _layoutRoot = GetTemplateChild(ElementLayoutRootName) as Panel;
            if (_layoutRoot != null)
            {
                _virtualKeys = (from dependencyObject in _layoutRoot.Descendants()
                                let keyConfiguration = dependencyObject.GetValue(QuertyKeyboard.VirtualKeyProperty) as VirtualKeyConfig
                                let element = dependencyObject as ContentControl
                                where keyConfiguration != null && element != null
                                select new KeyValuePair<ContentControl, VirtualKeyConfig>(element, keyConfiguration))
                                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                foreach (var element in _virtualKeys.Keys)
                {
                    var keyConfiguration = _virtualKeys[element];
                    element.Content = GetContent(element, keyConfiguration, keyConfiguration.DefaultContent);

                    //var touchUpEventListener = new WeakEventListener<QuertyKeyboard, object, TouchEventArgs>(this);
                    //touchUpEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonTouchUp(source, eventArgs);
                    //touchUpEventListener.OnDetachAction = (weakEventListenerParameter) => element.TouchUp -= weakEventListenerParameter.OnEvent;
                    //element.TouchUp += touchUpEventListener.OnEvent;

                    //var touchDownEventListener = new WeakEventListener<QuertyKeyboard, object, TouchEventArgs>(this);
                    //touchDownEventListener.OnEventAction = (instance, source, eventArgs) => instance.OnButtonTouchDown(source, eventArgs);
                    //touchDownEventListener.OnDetachAction = (weakEventListenerParameter) => element.TouchDown -= weakEventListenerParameter.OnEvent;
                    //element.TouchDown += touchDownEventListener.OnEvent;

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

                    //OnShowCapitalize();
                    //OnShiftPressed();
                }
            }

            KeyboardLayoutVisualStateGroup = GetTemplateChild(VisualStateGroupKeyboardLayoutName) as VisualStateGroup;
            QuertyVisualState = GetTemplateChild(VisualStateQuertyName) as VisualState;
            NumericVisualState = GetTemplateChild(VisualStateNumericName) as VisualState;
        }

        private void OnButtonTouchDown(object sender, TouchEventArgs e)
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
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            if (!virtualKeyConfig.IsSticky)
                return;
            _isShiftSticked = !_isShiftSticked;
            if (_isShiftSticked)
            {
                KeyboardService.PressAndHold(KeysEx.VK_LSHIFT);
            }
            else
            {
                KeyboardService.ReleaseStickyKeys();
            }
            IsShiftPressed = _isShiftSticked;
        }

        private void OnButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as ContentControl;
            var virtualKeyConfig = _virtualKeys[element];
            HandleButtonDown(virtualKeyConfig);
        }

        private void HandleButtonUp()
        {
            _timer.IsEnabled = false;
        }

        private void HandleButtonDown(VirtualKeyConfig virtualKeyConfig)
        {
            _timer.Tag = virtualKeyConfig;
            SendKeyStroke(virtualKeyConfig);
            if (virtualKeyConfig.IsRepeatable && !virtualKeyConfig.IsSticky && !_timer.IsEnabled)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(PauseOnKeyPressedInitial);
                _timer.IsEnabled = true;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            var virtualKeyConfig = timer.Tag as VirtualKeyConfig;
            if (timer.Interval.TotalMilliseconds > PauseOnKeyPressedMinimum)
            {
                timer.Interval = TimeSpan.FromMilliseconds(timer.Interval.TotalMilliseconds / 2);
            }
            SendKeyStroke(virtualKeyConfig);
        }

        private void SendKeyStroke(VirtualKeyConfig keyStroke)
        {
            OnSendKeyStroke(keyStroke);
        }

        public void ReleaseKeys()
        {
            IsShiftPressed = false;
            KeyboardService.ReleaseStickyKeys();
        }
    }

    public class VirtualKeyConfig
    {
        public string KeyName { get; set; }
        public KeysEx KeyCode { get; set; }
        public bool IsSticky { get; set; }
        public bool IsRepeatable { get; set; }
        public object DefaultContent { get; set; }
        public object ShiftContent { get; set; }
        public object CapitalizedContent { get; set; }

        public VirtualKeyConfig()
        {
            KeyCode = KeysEx.None;
            IsSticky = false;
            IsRepeatable = true;
        }
    }
}
