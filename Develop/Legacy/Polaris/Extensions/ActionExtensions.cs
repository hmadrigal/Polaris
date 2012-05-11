//-----------------------------------------------------------------------
// <copyright file="ActionExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Extensions
{
    using System;
    using System.Threading;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using System.Windows.Threading;

    public static class ActionExtensions
    {
        public static void InvokeWithDelay(this Action delayedAction, TimeSpan delay, Dispatcher sourceThreadDispatcher = null)
        {
            if (delayedAction == null) { return; }
            ThreadPool.QueueUserWorkItem(new WaitCallback((target) =>
            {
                Thread.Sleep(delay);
                if (sourceThreadDispatcher != null)
                {
                    sourceThreadDispatcher.BeginInvoke(new Action(() =>
                    {
                        delayedAction();
                    }));
                }
                else
                {
                    delayedAction();
                }
            }));
        }

        public static bool TryInvoke(this Action targetAction, string loggingPolicy = null)
        {
            try
            {
                targetAction.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(loggingPolicy))
                    return false;
                if (GetExceptionManager().TryHandleException(ex, loggingPolicy))
                    throw;
                return false;
            }
        }

        static ExceptionManager exceptionManager;

        private static ExceptionManager GetExceptionManager()
        {
            if (exceptionManager == null)
            {
                try
                {
                    exceptionManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
                }
                catch
                {
                    return null;
                }
            }
            return exceptionManager;
        }
    }
}