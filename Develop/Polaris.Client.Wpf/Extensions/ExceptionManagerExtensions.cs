//-----------------------------------------------------------------------
// <copyright file="ExceptionManagerExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Windows.Extensions
{
    using System;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    public static class ExceptionManagerExtensions
    {
        public static bool TryHandleException(this ExceptionManager exceptionManager, Exception exceptionToHandle, string policyName)
        {
            if (exceptionManager == null)
                // return true to recommend that this exception is re-thrown.
                return true;
            return exceptionManager.HandleException(exceptionToHandle, policyName);
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
    }
}