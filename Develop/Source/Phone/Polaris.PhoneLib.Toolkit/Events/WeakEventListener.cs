// Source :http://blog.catenalogic.com/post/2011/11/23/A-weak-event-listener-for-WPF-Silverlight-and-Windows-Phone-7.aspx
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakEventListener.cs" company="Catel development team">
//   Copyright (c) 2008 - 2011 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

// Uncomment if used in Catel
//#define CATEL

namespace Polaris.PhoneLib.Toolkit.Events
{
    using System;
    using System.Reflection;

#if CATEL
    using Logging;
#endif

    /// <summary>
    /// Implements a weak event listener that allows the owner to be garbage
    /// collected if its only remaining link is an event handler.
    /// </summary>
    /// <typeparam name="TSource">Type of source for the event.</typeparam>
    /// <typeparam name="TTarget">Type of target listening for the event.</typeparam>
    /// <typeparam name="TEventArgs">Type of event arguments for the event.</typeparam>
    /// <example>
    /// Initially, the code must be used in this way: 
    /// <para />
    /// <code>
    ///  <![CDATA[
    ///     var source = new EventSource();
    ///     var listener = new EventListener();
    ///
    ///     WeakEventListener<EventListener, EventSource, EventArgs>.SubscribeToWeakEvent(listener, source, "Event", listener.OnEvent);
    /// ]]>
    /// </code>
    /// </example>
    public class WeakEventListener<TTarget, TSource, TEventArgs>
        where TTarget : class
        where TSource : class
        where TEventArgs : EventArgs
    {
        /// <summary>
        /// Open instance delegate which allows the creation of an instance method without an actual reference
        /// to the target.
        /// </summary>
        public delegate void OpenInstanceHandler(TTarget @this, object sender, TEventArgs e);

        #region Variables
#if CATEL
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
#endif

        /// <summary>
        /// The default binding flags used to retrieve the events.
        /// </summary>
        private const BindingFlags DefaultEventBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public;

        /// <summary>
        /// WeakReference to the target listening for the event.
        /// </summary>
        private readonly WeakReference _weakTarget;

        /// <summary>
        /// To hold a reference to source object. With this instance the WeakEventListener 
        /// can guarantee that the handler get unregistered when listener is released.
        /// </summary>
        private readonly WeakReference _weakSource;

        /// <summary>
        /// The event name this listener is automatically subscribed to. If this value is <c>null</c>, the
        /// listener is not automatically registered to any event.
        /// </summary>
        private string _automaticallySubscribedEventName;

        /// <summary>
        /// Delegate that needs to be unsubscribed when registered automatically.
        /// </summary>
        private Delegate _internalEventDelegate;
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instances of the WeakEventListener class.
        /// </summary>
        /// <param name="target">Instance subscribing to the event, should be <c>null</c> for static event handlers.</param>
        /// <param name="source">The source of the event, should be <c>null</c> for static events.</param>
        private WeakEventListener(TTarget target, TSource source)
        {
            IsStaticEventHandler = (target == null);
            if (target != null)
            {
                _weakTarget = new WeakReference(target);
            }

            IsStaticEvent = (source == null);
            if (source != null)
            {
                _weakSource = new WeakReference(source);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the method to call when the event fires.
        /// </summary>
        internal OpenInstanceHandler OnEventAction { get; set; }

        /// <summary>
        /// Gets or sets the method to call when the static event fires.
        /// </summary>
        /// <value>The on static event action.</value>
        internal EventHandler<TEventArgs> OnStaticEventAction { get; set; }

        /// <summary>
        /// Gets the target or <c>null</c> if there is no target.
        /// </summary>
        /// <value>The target.</value>
        private TTarget Target { get { return (_weakTarget != null) ? (TTarget)_weakTarget.Target : null; } }

        /// <summary>
        /// Gets the source or <c>null</c> if there is no source.
        /// </summary>
        /// <value>The target.</value>
        private TSource Source { get { return (_weakSource != null) ? (TSource)_weakSource.Target : null; } }

        /// <summary>
        /// Gets a value indicating whether the event source has not yet been garbage collected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the event source has not yet been garbage collected; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// In case of static events, this property always returns <c>false</c>.
        /// </remarks>
        public bool IsSourceAlive { get { return (_weakSource != null) && _weakSource.IsAlive; } }

        /// <summary>
        /// Gets a value indicating whether the event target has not yet been garbage collected.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the event target has not yet been garbage collected; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// In case of static event handlers, this property always returns <c>false</c>.
        /// </remarks>
        public bool IsTargetAlive { get { return (_weakTarget != null) && _weakTarget.IsAlive; } }

        /// <summary>
        /// Gets a value indicating whether this instance represents a static event.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance represents a static event; otherwise, <c>false</c>.
        /// </value>
        public bool IsStaticEvent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance represents a static event handler.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance represents a static event handler; otherwise, <c>false</c>.
        /// </value>
        public bool IsStaticEventHandler { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Subscribes to a weak event by using one single method. This method also takes care of automatic
        /// unsubscription of the event.
        /// </summary>
        /// <param name="target">Instance subscribing to the event, should be <c>null</c> for static event handlers.</param>
        /// <param name="source">The source of the event, should be <c>null</c> for static events.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="handler">The handler to execute when the event occurs.</param>
        /// <returns>
        /// The created event listener 
        /// </returns>
        /// <remarks>
        /// This is a convenience method. This method wraps the "long" subscription code into one single method call.
        /// </remarks>
        /// <exception cref="ArgumentException">The <paramref name="eventName"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="handler"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="source"/> and <paramref name="target"/> are both <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="eventName"/> does not exist or not accessible.</exception>
        public static WeakEventListener<TTarget, TSource, TEventArgs> SubscribeToWeakEvent(TTarget target, TSource source, string eventName, EventHandler<TEventArgs> handler)
        {
            Argument.IsNotNullOrWhitespace("eventName", eventName);
            Argument.IsNotNull("handler", handler);

            if ((source == null) && (target == null))
            {
                const string error = "Both the source and target are null, which means that a static event handler subscribes to a static event. In such cases, there are no memory leaks, so there is no reason to use this class";
#if CATEL
                Log.Error(error);
#endif

                throw new InvalidOperationException(error);
            }

            var weakListener = new WeakEventListener<TTarget, TSource, TEventArgs>(target, source);

            weakListener.SubscribeToEvent(source, eventName);

            if (weakListener.IsStaticEventHandler)
            {
                var del = Delegate.CreateDelegate(typeof(EventHandler<TEventArgs>), null, handler.Method);
                weakListener.OnStaticEventAction = (EventHandler<TEventArgs>)del;
            }
            else
            {
                var del = Delegate.CreateDelegate(typeof(OpenInstanceHandler), null, handler.Method);
                weakListener.OnEventAction = (OpenInstanceHandler)del;
            }

            return weakListener;
        }

        /// <summary>
        /// Subscribes to the specific event. If the event occurs, the <see cref="OnEvent"/> method will be invoked.
        /// </summary>
        /// <param name="source">The source of the event, should be <c>null</c> for static events.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <exception cref="ArgumentException">The <paramref name="eventName"/> is <c>null</c> or whitespace.</exception>
        private void SubscribeToEvent(object source, string eventName)
        {
            Argument.IsNotNullOrWhitespace("eventName", eventName);

            var eventInfo = typeof(TSource).GetEvent(eventName, DefaultEventBindingFlags);
            if (eventInfo == null)
            {
                string error = string.Format("Event '{0}' is not found on type '{1}'", eventName, typeof(TSource).Name);
#if CATEL
                Log.Error(error);
#endif

                throw new InvalidOperationException(error);
            }

            _internalEventDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, "OnEvent");
            eventInfo.AddEventHandler(source, _internalEventDelegate);

            _automaticallySubscribedEventName = eventName;
        }

        /// <summary>
        /// Unsubscribes from the specific event. If the event occurs, the <see cref="OnEvent"/> method will no longer be invoked.
        /// </summary>
        /// <param name="source">The source of the event, should be <c>null</c> for static events.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="eventName"/> is <c>null</c> or whitespace.</exception>
        private void UnsubscribeFromEvent(object source, string eventName)
        {
            Argument.IsNotNullOrWhitespace("eventName", eventName);

            var eventInfo = typeof(TSource).GetEvent(eventName, DefaultEventBindingFlags);
            if (eventInfo == null)
            {
                string error = string.Format("Event '{0}' is not found on type '{1}'", eventName, typeof(TSource).Name);
#if CATEL
                Log.Error(error);
#endif

                throw new InvalidOperationException(error);
            }

            if (_internalEventDelegate != null)
            {
                eventInfo.RemoveEventHandler(source, _internalEventDelegate);
            }
        }

        /// <summary>
        /// Handler for the subscribed event calls OnEventAction to handle it.
        /// </summary>
        /// <param name="source">Event source.</param>
        /// <param name="eventArgs">Event arguments.</param>
        public void OnEvent(object source, TEventArgs eventArgs)
        {
            TTarget target = Target;
            if (!IsStaticEventHandler && (Target == null))
            {
                Detach();
                return;
            }

            var onEventAction = OnEventAction;
            if (onEventAction != null)
            {
                onEventAction(target, source, eventArgs);
            }

            var onStaticEventAction = OnStaticEventAction;
            if (onStaticEventAction != null)
            {
                onStaticEventAction(source, eventArgs);
            }
        }

        /// <summary>
        /// Detaches from the subscribed event.
        /// </summary>
        public void Detach()
        {
            if (!IsStaticEvent && (Source == null))
            {
#if CATEL
                Log.Warning("Event on source '{0}' is not static, yet the source does no longer exists", typeof(TSource).FullName);
#endif
                return;
            }

            if (_automaticallySubscribedEventName != null)
            {
                UnsubscribeFromEvent(Source, _automaticallySubscribedEventName);
            }
        }
        #endregion
    }

    ///// <summary>
    ///// Overload implement of the <see cref="WeakEventListener{TInstance,TSource,TEventArgs}"/> class that assumes that the
    ///// <c>EventArgs</c> class is of type <see cref="EventArgs"/>.
    ///// </summary>
    ///// <remarks>
    ///// This is a convenience class, the actual logic is implemented in the <see cref="WeakEventListener{TInstance,TSource,TEventArgs}"/> class.
    ///// </remarks>
    //public class WeakEventListener : WeakEventListener<object, object, EventArgs>
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="WeakEventListener{TInstance,TSource,TEventArgs}"/> class.
    //    /// </summary>
    //    /// <param name="target">The target.</param>
    //    /// <param name="source">The source.</param>
    //    public WeakEventListener(object target, object source)
    //        : base(target, source) { }
    //}
}