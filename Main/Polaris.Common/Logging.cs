using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using System.Diagnostics;

namespace Polaris.Common
{
    /// <summary>
    /// This class helps to write log messages
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Event fired when there was an error when trying to write an entry in the log file
        /// </summary>
        public static event EventHandler<EventArgs> LoggingFailed;

        /// <summary>
        /// Default EventId when writing entries into the log
        /// </summary>
        private static readonly int defaultEventId;
        /// <summary>
        /// Default Priority when writing entries into the log
        /// </summary>
        private static readonly int defaultPriority;
        /// <summary>
        /// Default Categories when writing entries into the log
        /// </summary>
        private static readonly string[] defaultCategories;

        /// <summary>
        /// Initializes the default values 
        /// </summary>
        static Logging()
        {
            Int32.TryParse(ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.EventId"], out defaultEventId);
            Int32.TryParse(ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.Priority"], out defaultPriority);
            defaultCategories = (ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.Categories"] as string).Split(';');
            //defaultCategories = ((String)ConfigurationManager.AppSettings["Polaris.Common.Logging.Default.Categories"] ?? String.Empty).Split(';');
        }

        /// <summary>
        /// Logs a error messages 
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(String formatMessage, params object[] args)
        {
            LogMessage(TraceEventType.Error, formatMessage, args);
        }

        /// <summary>
        /// Logs a information message
        /// </summary>
        /// <param name="message"></param>
        public static void LogInformation(String formatMessage, params object[] args)
        {
            LogMessage(TraceEventType.Information, formatMessage, args);
        }

        /// <summary>
        /// Logs a warninig message
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(String formatMessage, params object[] args)
        {
            LogMessage(TraceEventType.Error, formatMessage, args);
        }

        /// <summary>
        /// Allows to log a message and specify the severity of the message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="severity">severity</param>
        public static void LogMessage(TraceEventType severity, String formatMessage, params object[] args)
        {
            LogMessage(defaultEventId, defaultPriority, severity, formatMessage, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        /// <param name="priority"></param>
        /// <param name="severity"></param>
        public static void LogMessage(int eventId, int priority, TraceEventType severity, String formatMessage, params object[] args)
        {
            LogMessage(String.Format(formatMessage, args), eventId, priority, severity, defaultCategories);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="eventId"></param>
        /// <param name="priority"></param>
        /// <param name="severity"></param>
        /// <param name="categories"></param>
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

        /// <summary>
        /// Fires the 'LoggingFailed' event. 
        /// </summary>
        /// <param name="ex"></param>
        private static void OnLoggingFailed(Exception ex)
        {
            if (LoggingFailed != null)
                LoggingFailed(ex, EventArgs.Empty);
        }
    }
}
