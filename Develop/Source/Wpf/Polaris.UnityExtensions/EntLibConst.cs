using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.EnterpriseEx
{
    /// <summary>
    /// Holds a set of constant values associated to Enterprise Libraries Settings. These values are available
    /// in the target system or referenced into the configuration files.
    /// </summary>
    public static class EntLibConst
    {
        /// <summary>
        /// Values to be consumed by the Exception Manager Block
        /// </summary>
        public static class ExceptionManager
        {
            /// <summary>
            /// Name of the Logging Exception handler
            /// </summary>
            public const string LOGGING_POLICY = @"LoggingPolicy";
        }

        /// <summary>
        /// Values to be consumes by the Logging Block
        /// </summary>
        public static class LogWriter
        {
            /// <summary>
            /// Set of Categories to log messages
            /// </summary>
            public static class Categories
            {
                /// <summary>
                /// Default Category to log messages
                /// </summary>
                public const string DEFAULT = @"General";
            }

            /// <summary>
            /// Set of EventIds to log messages
            /// </summary>
            public static class EventIDs
            {
                /// <summary>
                /// Default EventId to log messages
                /// </summary>
                public const int DEFAULT = 100;
            }

            /// <summary>
            /// Set of Priorities to log messages
            /// </summary>
            public static class Priorities
            {
                /// <summary>
                /// Default Priority to log messages
                /// </summary>
                public const int DEFAULT = 0;
            }
        }
    }
}