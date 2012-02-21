//-----------------------------------------------------------------------
// <copyright file="ActionExtensions.cs" company="">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Client.Wpf.Extensions
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public static class ActionExtensions
    {
        public static bool TryInvoke(this Action targetAction)
        {
            try
            {
                targetAction.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                var exceptionManager = GetExceptionManager();
                if (exceptionManager.TryHandleException(ex, EntLibConst.ExceptionManager.LOGGING_POLICY))
                {
                    throw;
                }
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