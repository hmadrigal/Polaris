//-----------------------------------------------------------------------
// <copyright file="ExceptionManagerExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.EnterpriseEx
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public static class ExceptionManagerExtensions
    {

        /// <summary>
        /// Tries to handle an exception by applying the respective exception handling
        /// </summary>
        /// <param name="exceptionManager"></param>
        /// <param name="exceptionToHandle"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static bool TryHandleException(this ExceptionManager exceptionManager, Exception exceptionToHandle, string policyName)
        {
            return
                // If null then it returns true to recommend that this exception is re-thrown.
                exceptionManager == null
                // Otherwise it invokes HandleException 
                || exceptionManager.HandleException(exceptionToHandle, policyName);
        }

        public static bool TryHandleException(this ExceptionManager exceptionManager, Exception exceptionToHandle, string policyName, out Exception exceptionToThrow)
        {
            if (exceptionManager == null)
            {
                exceptionToThrow = default(Exception);
                // return true to recommend that this exception is re-thrown.
                return true;
            }
            return exceptionManager.HandleException(exceptionToHandle, policyName, out exceptionToThrow);
        }

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/> and handles
        ///             any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// 
        /// </summary>
        /// <param name="exceptionManager"> </param>
        /// <param name="action">The delegate to execute.</param><param name="policyName">The name of the policy to handle.</param>
        /// <example>
        /// The following code shows the usage of this method.
        /// 
        /// <code>
        /// exceptionManager.Process(() =&gt; { DoWork(); }, "policy");
        /// 
        /// </code>
        /// 
        /// </example>
        /// <seealso cref="M:Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionManager.HandleException(System.Exception,System.String)"/>
        public static void TryProcess(this ExceptionManager exceptionManager, Action action, string policyName)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                if (exceptionManager != null && exceptionManager.HandleException(exception, policyName))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/>, and handles
        ///             any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// 
        /// </summary>
        /// <typeparam name="TResult">Type of return value from <paramref name="action"/>.</typeparam>
        /// <param name="exceptionManager"> </param>
        /// <param name="action">The delegate to execute.</param><param name="defaultResult">The value to return if an exception is thrown and the
        ///             exception policy swallows it instead of rethrowing.</param><param name="policyName">The name of the policy to handle.</param>
        /// <returns>
        /// If no exception occurs, returns the result from executing <paramref name="action"/>. If
        ///             an exception occurs and the policy does not re-throw, returns <paramref name="defaultResult"/>.
        /// </returns>
        public static TResult TryProcess<TResult>(this ExceptionManager exceptionManager, Func<TResult> action, TResult defaultResult, string policyName)
        {
            TResult result;
            try
            {
                result = action.Invoke();
            }
            catch (Exception exception)
            {
                if (exceptionManager != null && exceptionManager.HandleException(exception, policyName))
                {
                    throw;
                }
                result = default(TResult);
            }
            return result;
        }

        /// <summary>
        /// Executes the supplied delegate <paramref name="action"/>, and handles
        ///             any thrown exception according to the rules configured for <paramref name="policyName"/>.
        /// 
        /// </summary>
        /// <typeparam name="TResult">Type of return value from <paramref name="action"/>.</typeparam>
        /// <param name="exceptionManager"> </param>
        /// <param name="action">The delegate to execute.</param><param name="policyName">The name of the policy to handle.</param>
        /// <returns>
        /// If no exception occurs, returns the result from executing <paramref name="action"/>. If
        ///             an exception occurs and the policy does not re-throw, returns the default value for <typeparamref name="TResult"/>.
        /// </returns>
        public static TResult Process<TResult>(this ExceptionManager exceptionManager, Func<TResult> action, string policyName)
        {
            TResult result;
            try
            {
                result = action.Invoke();
            }
            catch (Exception exception)
            {
                if (exceptionManager != null && exceptionManager.HandleException(exception, policyName))
                {
                    throw;
                }
                result = default(TResult);
            }
            return result;
        }
    }
}