using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using System.Diagnostics;

namespace Polaris.Common
{
    public static class Logging
    {

        public static event EventHandler<EventArgs> LoggingFailed;

        private static readonly int defaultEventId;
        private static readonly int defaultPriority;
        private static readonly string[] defaultCategories;

        static Logging()
        {
            Int32.TryParse(ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.EventId"], out defaultEventId);
            Int32.TryParse(ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.Priority"], out defaultPriority);
            defaultCategories = (ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.Priority"] as String).Split(';');
        }

        public static void LogError(String message)
        {
            LogMessage(message, TraceEventType.Error);
        }

        public static void LogInformation(String message)
        {
            LogMessage(message, TraceEventType.Information);
        }

        public static void LogWarning(String message)
        {
            LogMessage(message, TraceEventType.Error);
        }

        public static void LogMessage(String message, TraceEventType severity)
        {
            LogMessage(message, defaultEventId, defaultPriority, severity, defaultCategories);
        }
        public static void LogMessage(String message, int eventId, int priority, TraceEventType severity)
        {
            LogMessage(message, eventId, priority, severity, defaultCategories);
        }
        public static void LogMessage(String message, int eventId, int priority, TraceEventType severity, params String[] categories)
        {
            try
            {
                // Creates and fills the log entry with user information
                LogEntry logEntry = new LogEntry();
                logEntry.EventId = eventId;
                logEntry.Priority = priority;
                logEntry.Message = message;
                logEntry.Severity = severity;
                logEntry.Categories.Clear();

                // Add the categories selected by the user
                foreach (string category in categories)
                {
                    logEntry.Categories.Add(category);
                }

                // Writes the log entry.
                Logger.Write(logEntry);
            }
            catch (Exception ex)
            {
                OnLoggingFailed(ex);
            }
        }

        private static void OnLoggingFailed(Exception ex)
        {
            if (LoggingFailed != null)
                LoggingFailed(ex, EventArgs.Empty);
        }
    }
}
