//-----------------------------------------------------------------------
// <copyright file="ActionExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.EnterpriseEx
{
    using System;
    using System.Threading;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public static class ActionExtensions
    {
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