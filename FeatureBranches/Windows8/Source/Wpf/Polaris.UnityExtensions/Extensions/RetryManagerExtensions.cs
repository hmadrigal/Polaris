using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling;
using Microsoft.Practices.TransientFaultHandling;

namespace Polaris.Wpf.UnityExtensions.Extensions
{
    public static class RetryManagerExtensions
    {
        private class AcceptAllErrorsDetectionStrategy : ITransientErrorDetectionStrategy
        {
            public bool IsTransient(Exception ex)
            {
                return true;
            }
        }
        public static RetryPolicy GetRetryPolicy(this RetryManager retryManager)
        {
            if (retryManager==null)
            {
                throw new ArgumentNullException("retryManager");
            }
            return retryManager.GetRetryPolicy<AcceptAllErrorsDetectionStrategy>();
        }
    }
}
