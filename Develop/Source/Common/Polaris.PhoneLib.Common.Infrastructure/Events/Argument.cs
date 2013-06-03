// Source :http://blog.catenalogic.com/post/2011/11/23/A-weak-event-listener-for-WPF-Silverlight-and-Windows-Phone-7.aspx
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Argument.cs" company="Catel development team">
//   Copyright (c) 2008 - 2011 Catel development team. All rights reserved.
// </copyright>
// <summary>
//   Argument validator class to help validating arguments that are passed into a method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Uncomment if used in Catel
//#define CATEL

namespace Polaris.PhoneLib.Events
{
    using System;
    using System.Diagnostics;
    using System.Linq;

#if CATEL
    using Generics;
    using Logging;
#endif

    /// <summary>
    /// Argument validator class to help validating arguments that are passed into a method.
    /// <para />
    /// This class automatically adds thrown exceptions to the log file.
    /// </summary>
    public static class Argument
    {
        #region Variables
#if CATEL
        /// <summary>
        /// The <see cref="ILog">log</see> object.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
#endif
        #endregion

        #region Methods
        /// <summary>
        /// Determines whether the specified argument is not <c>null</c>.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="paramValue"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static void IsNotNull(string paramName, object paramValue)
        {
            EnsureValidParamName(paramName);

            if (paramValue == null)
            {
#if CATEL
                Log.Error("Argument '{0}' cannot be null", paramName);
#endif
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c> or empty.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramValue"/> is <c>null</c> or empty.</exception>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty(string paramName, string paramValue)
        {
            EnsureValidParamName(paramName);

            if (string.IsNullOrEmpty(paramValue))
            {
                string error = string.Format("Argument '{0}' cannot be null or empty", paramName);

#if CATEL
                Log.Error(error);
#endif
                throw new ArgumentException(error, paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c> or a whitespace.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramValue"/> is <c>null</c> or a whitespace.</exception>
        [DebuggerStepThrough]
        public static void IsNotNullOrWhitespace(string paramName, string paramValue)
        {
            EnsureValidParamName(paramName);

            if (string.IsNullOrEmpty(paramValue) || (string.Compare(paramValue.Trim(), string.Empty) == 0))
            {
                string error = string.Format("Argument '{0}' cannot be null or whitespace", paramName);

#if CATEL
                Log.Error(error);
#endif
                throw new ArgumentException(error, paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not <c>null</c> or an empty array (.Length == 0).
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramValue"/> is <c>null</c> or an empty array.</exception>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmptyArray(string paramName, Array paramValue)
        {
            EnsureValidParamName(paramName);

            if ((paramValue == null) || (paramValue.Length == 0))
            {
                string error = string.Format("Argument '{0}' cannot be null or an empty array", paramName);

#if CATEL
                Log.Error(error);
#endif
                throw new ArgumentException(error, paramName);
            }
        }

        /// <summary>
        /// Determines whether the specified argument is not out of range.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="validation">The validation function to call for validation.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsNotOutOfRange(string paramName, object paramValue, Func<object, bool> validation)
        {
            EnsureValidParamName(paramName);

            IsNotNull("validation", validation);

            if (!validation(paramValue))
            {
#if CATEL
                Log.Error("Argument '{0}' is out of range", paramName);
#endif
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

#if CATEL
        /// <summary>
        /// Determines whether the specified argument is not out of range.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="minimumValue">The minimum value.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsNotOutOfRange<T>(string paramName, T paramValue, T minimumValue, T maximumValue)
        {
            EnsureValidParamName(paramName);

            if (Operator<T>.LessThan(paramValue, minimumValue) || Operator<T>.GreaterThan(paramValue, maximumValue))
            {
                string error = string.Format("Argument '{0}' should be between {1} and {2}", paramName, minimumValue, maximumValue);

                Log.Error(error);
                throw new ArgumentOutOfRangeException(paramName, error);
            }
        }

        /// <summary>
        /// Determines whether the specified argument has a minimum value.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="minimumValue">The minimum value.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsMinimal<T>(string paramName, T paramValue, T minimumValue)
        {
            EnsureValidParamName(paramName);

            if (Operator<T>.LessThan(paramValue, minimumValue))
            {
                string error = string.Format("Argument '{0}' should be at least {1}", paramName, minimumValue);

                Log.Error(error);
                throw new ArgumentOutOfRangeException(paramName, error);
            }
        }

        /// <summary>
        /// Determines whether the specified argument has a maximum value.
        /// </summary>
        /// <typeparam name="T">Type of the argument.</typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="paramValue">Value of the parameter.</param>
        /// <param name="maximumValue">The maximum value.</param>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="paramValue"/> is out of range.</exception>
        [DebuggerStepThrough]
        public static void IsMaximum<T>(string paramName, T paramValue, T maximumValue)
        {
            EnsureValidParamName(paramName);

            if (Operator<T>.GreaterThan(paramValue, maximumValue))
            {
                string error = string.Format("Argument '{0}' should be at maximum {1}", paramName, maximumValue);

                Log.Error(error);
                throw new ArgumentOutOfRangeException(paramName, error);
            }
        }
#endif

        /// <summary>
        /// Checks whether the specified <paramref name="instance"/> implements the specified <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="instance">The instance to check.</param>
        /// <param name="interfaceType">The type of the interface to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="interfaceType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="instance"/> does not implement the <paramref name="interfaceType"/>.</exception>
        [DebuggerStepThrough]
        public static void ImplementsInterface(string paramName, object instance, Type interfaceType)
        {
            Argument.IsNotNull("instance", instance);

            ImplementsInterface(paramName, instance.GetType(), interfaceType);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> implements the specified <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="type">The type to check.</param>
        /// <param name="interfaceType">The type of the interface to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="interfaceType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> does not implement the <paramref name="interfaceType"/>.</exception>
        [DebuggerStepThrough]
        public static void ImplementsInterface(string paramName, Type type, Type interfaceType)
        {
            EnsureValidParamName(paramName);
            Argument.IsNotNull("type", type);
            Argument.IsNotNull("interfaceType", interfaceType);

            if (type.GetInterfaces().Any(iType => iType == interfaceType))
            {
                return;
            }

            string error = string.Format("Type '{0}' should implement interface '{1}', but does not", type.Name, interfaceType.Name);

#if CATEL
            Log.Error(error);
#endif

            throw new ArgumentException(error, "type");
        }

        /// <summary>
        /// Checks whether the specified <paramref name="instance"/> is of the specified <paramref name="requiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="instance">The instance to check.</param>
        /// <param name="requiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="requiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="instance"/> is not of type <paramref name="requiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsOfType(string paramName, object instance, Type requiredType)
        {
            Argument.IsNotNull("instance", instance);

            IsOfType(paramName, instance.GetType(), requiredType);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> is of the specified <paramref name="requiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="type">The type to check.</param>
        /// <param name="requiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="requiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is not of type <paramref name="requiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsOfType(string paramName, Type type, Type requiredType)
        {
            EnsureValidParamName(paramName);
            Argument.IsNotNull("type", type);
            Argument.IsNotNull("requiredType", requiredType);

            if (requiredType.IsAssignableFrom(type))
            {
                return;
            }

            string error = string.Format("Type '{0}' should be of type '{1}', but is not", type.Name, requiredType.Name);

#if CATEL
            Log.Error(error);
#endif

            throw new ArgumentException(error, "type");
        }

        /// <summary>
        /// Checks whether the specified <paramref name="instance"/> is not of the specified <paramref name="notRequiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="instance">The instance to check.</param>
        /// <param name="notRequiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="notRequiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="instance"/> is of type <paramref name="notRequiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsNotOfType(string paramName, object instance, Type notRequiredType)
        {
            Argument.IsNotNull("instance", instance);

            IsNotOfType(paramName, instance.GetType(), notRequiredType);
        }

        /// <summary>
        /// Checks whether the specified <paramref name="type"/> is not of the specified <paramref name="notRequiredType"/>.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <param name="type">The type to check.</param>
        /// <param name="notRequiredType">The type to check for.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="type"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="notRequiredType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="type"/> is of type <paramref name="notRequiredType"/>.</exception>
        [DebuggerStepThrough]
        public static void IsNotOfType(string paramName, Type type, Type notRequiredType)
        {
            EnsureValidParamName(paramName);
            Argument.IsNotNull("type", type);
            Argument.IsNotNull("notRequiredType", notRequiredType);

            if (!notRequiredType.IsAssignableFrom(type))
            {
                return;
            }

            string error = string.Format("Type '{0}' should not be of type '{1}', but is", type.Name, notRequiredType.Name);

#if CATEL
            Log.Error(error);
#endif

            throw new ArgumentException(error, "type");
        }

        /// <summary>
        /// Checks whether the passed in boolean check is <c>true</c>. If not, this method will throw a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="isSupported">if set to <c>true</c>, the action is supported; otherwise <c>false</c>.</param>
        /// <param name="errorFormat">The error format.</param>
        /// <param name="args">The arguments for the string format.</param>
        /// <exception cref="NotSupportedException">The <paramref name="isSupported"/> is <c>false</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="errorFormat"/> is <c>null</c> or whitespace.</exception>
        public static void IsSupported(bool isSupported, string errorFormat, params object[] args)
        {
            Argument.IsNotNullOrWhitespace("errorFormat", errorFormat);

            if (!isSupported)
            {
                string error = string.Format(errorFormat, args);

#if CATEL
                Log.Error(error);
#endif

                throw new NotSupportedException(error);
            }
        }

        /// <summary>
        /// Ensures that the name of the param is valid.
        /// </summary>
        /// <param name="paramName">Name of the param.</param>
        /// <exception cref="ArgumentException">If <paramref name="paramName"/> is <c>null</c> or whitespace.</exception>
        [DebuggerStepThrough]
        private static void EnsureValidParamName(string paramName)
        {
            if ((paramName == null) || string.IsNullOrEmpty(paramName.Trim()))
            {
                string error = string.Format("Argument '{0}' cannot be null or whitespace", "paramName");

#if CATEL
                Log.Error(error);
#endif

                throw new ArgumentException(error, "paramName");
            }
        }
        #endregion
    }
}
