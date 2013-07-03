//-----------------------------------------------------------------------
// <copyright file="LogWriterExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.EnterpriseEx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Microsoft.Practices.EnterpriseLibrary.Logging;

#if SILVERLIGHT
    using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif

    public static class LogWriterExtensions
    {
        private const string NullLogWriter = "The LogWriter is null, the log message couldn't be written";

        public static void TryWrite(this LogWriter target, object message)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message);
        }

        public static void TryWrite(this LogWriter target, object message, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, GetProperties(properties));
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories);
        }

        public static void TryWrite(this LogWriter target, object message, string category)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category);
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, GetProperties(properties));
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, int priority)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, priority);
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category, priority);
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, int priority, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, priority, GetProperties(properties));
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, int priority, int eventId)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, priority, eventId);
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category, priority, GetProperties(properties));
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, int eventId)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category, priority, eventId);
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, int priority, int eventId, TraceEventType severity)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, priority, eventId, severity);
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, int eventId, TraceEventType severity)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category, priority, eventId, severity);
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, int priority, int eventId, TraceEventType severity, string title)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, priority, eventId, severity, title);
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, int eventId, TraceEventType severity, string title)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category, priority, eventId, severity, title);
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, int eventId, TraceEventType severity, Exception exception)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            Dictionary<String, Object> details = new Dictionary<string, object>();
            details.Add(LogProperties.ExceptionDetail, exception);
            target.Write(message, category, priority, eventId, severity, String.Empty, GetProperties(details));
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, int eventId, TraceEventType severity, Exception exception, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            properties[LogProperties.ExceptionDetail] = exception;
            target.Write(message, category, priority, eventId, severity, String.Empty, GetProperties(properties));
        }

        public static void TryWrite(this LogWriter target, object message, IEnumerable<string> categories, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, categories, priority, eventId, severity, title, GetProperties(properties));
        }

        public static void TryWrite(this LogWriter target, object message, string category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
        {
            if (target == null) { Debug.WriteLine(NullLogWriter); return; }
            target.Write(message, category, priority, eventId, severity, title, GetProperties(properties));
        }

        public static IDictionary<string, object> GetProperties(IDictionary<string, object> originalProperties)
        {
            var properties = new Dictionary<string, object>();
            foreach (var property in originalProperties)
            {
                var collection = property.Value as System.Collections.ICollection;
                if (collection != null)
                {
                    properties.Add(property.Key, new LogCollection(collection));
                }
                else
                {
                    properties.Add(property.Key, property.Value);
                }
            }
            return properties;
        }
    }

    public class LogCollection
    {
        private System.Collections.IEnumerable collection;

        public LogCollection(System.Collections.IEnumerable collection)
        {
            this.collection = collection;
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("{0}---------------Collection----------------{0}", Environment.NewLine);
            foreach (var item in this.collection)
            {
                strBuilder.Append(item.ToString());
                strBuilder.Append(Environment.NewLine);
            }
            strBuilder.AppendFormat("{0}----------------------------------------{0}", Environment.NewLine);
            return strBuilder.ToString();
        }
    }

    public static class LogProperties
    {
        public const string ExceptionDetail = "ExceptionDetail";
        public const string Parameters = "Parameters";
        public const string PublisherSubscriberMessage = "Message";
    }
}