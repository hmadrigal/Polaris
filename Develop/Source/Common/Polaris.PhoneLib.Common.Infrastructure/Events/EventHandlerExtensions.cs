﻿// Source :http://blog.catenalogic.com/post/2011/11/23/A-weak-event-listener-for-WPF-Silverlight-and-Windows-Phone-7.aspx
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventHandlerExtensions.cs" company="Catel development team">
//   Copyright (c) 2011 - 2011 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Polaris.PhoneLib.Events
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;

    /// <summary>
    /// Extensions for event handlers.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invokes the specified <paramref name="handler"/> in a thread-safe manner with <see cref="EventArgs.Empty"/>
        /// as parameter for the event args. Where normally one has to write the following code:
        /// <para />
        /// <code>
        /// <![CDATA[
        /// var handler = MyEvent;
        /// if (handler != null)
        /// {
        ///     handler(this, EventArgs.Empty);
        /// }
        /// ]]>
        /// </code>
        /// <para />
        /// One can now write:
        /// <para />
        /// <code>
        /// MyEvent.SafeInvoke(this);
        /// </code>
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <returns><c>true</c> if the event handler was not <c>null</c>; otherwise <c>false</c>.</returns>
        public static bool SafeInvoke(this EventHandler handler, object sender)
        {
            return SafeInvoke(handler, sender, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the specified <paramref name="handler"/> in a thread-safe manner with <see cref="EventArgs.Empty"/>
        /// as parameter for the event args. Where normally one has to write the following code:
        /// <para />
        /// <code>
        /// <![CDATA[
        /// var handler = MyEvent;
        /// if (handler != null)
        /// {
        ///     handler(this, EventArgs.Empty);
        /// }
        /// ]]>
        /// </code>
        /// <para />
        /// One can now write:
        /// <para />
        /// <code>
        /// MyEvent.SafeInvoke(this);
        /// </code>
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <returns><c>true</c> if the event handler was not <c>null</c>; otherwise <c>false</c>.</returns>
        public static bool SafeInvoke(this EventHandler<EventArgs> handler, object sender)
        {
            return SafeInvoke(handler, sender, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the specified <paramref name="handler"/> in a thread-safe manner with <see cref="EventArgs.Empty"/>
        /// as parameter for the event args. Where normally one has to write the following code:
        /// <para />
        /// <code>
        /// <![CDATA[
        /// var handler = MyEvent;
        /// if (handler != null)
        /// {
        ///     handler(this, EventArgs.Empty);
        /// }
        /// ]]>
        /// </code>
        /// <para />
        /// One can now write:
        /// <para />
        /// <code>
        /// MyEvent.SafeInvoke(this);
        /// </code>
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        /// <returns><c>true</c> if the event handler was not <c>null</c>; otherwise <c>false</c>.</returns>
        public static bool SafeInvoke(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Invokes the specified <paramref name="handler"/> in a thread-safe manner. Where normally one
        /// has to write the following code:
        /// <para />
        /// <code>
        /// <![CDATA[
        /// var handler = MyEvent;
        /// if (handler != null)
        /// {
        ///     handler(this, e);
        /// }
        /// ]]>
        /// </code>
        /// <para />
        /// One can now write:
        /// <para />
        /// <code>
        /// MyEvent.SafeInvoke(this, e);
        /// </code>
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the <see cref="EventArgs"/> class.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        /// <returns><c>true</c> if the event handler was not <c>null</c>; otherwise <c>false</c>.</returns>
        public static bool SafeInvoke<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs e)
            where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                handler(sender, e);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Invokes the specified <paramref name="handler"/> in a thread-safe manner. Where normally one
        /// has to write the following code:
        /// <para />
        /// <code>
        /// <![CDATA[
        /// var handler = PropertyChanged;
        /// if (handler != null)
        /// {
        ///     handler(this, e, new PropertyChangedEventArgs("propertyName"));
        /// }
        /// ]]>
        /// </code>
        /// <para />
        /// One can now write:
        /// <para />
        /// <code>
        /// PropertyChanged.SafeInvoke(this, e, new PropertyChangedEventArgs("propertyName"));
        /// </code>
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        /// <returns><c>true</c> if the event handler was not <c>null</c>; otherwise <c>false</c>.</returns>
        public static bool SafeInvoke(this PropertyChangedEventHandler handler, object sender, PropertyChangedEventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Invokes the specified <paramref name="handler"/> in a thread-safe manner. Where normally one
        /// has to write the following code:
        /// <para />
        /// <code>
        /// <![CDATA[
        /// var handler = CollectionChanged;
        /// if (handler != null)
        /// {
        ///     handler(this, e, new NotifyCollectionChangedEventArgs(...));
        /// }
        /// ]]>
        /// </code>
        /// <para />
        /// One can now write:
        /// <para />
        /// <code>
        /// CollectionChanged.SafeInvoke(this, e, new NotifyCollectionChangedEventArgs(...));
        /// </code>
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        /// <returns><c>true</c> if the event handler was not <c>null</c>; otherwise <c>false</c>.</returns>
        public static bool SafeInvoke(this NotifyCollectionChangedEventHandler handler, object sender, NotifyCollectionChangedEventArgs e)
        {
            if (handler != null)
            {
                handler(sender, e);
                return true;
            }

            return false;
        }
    }
}