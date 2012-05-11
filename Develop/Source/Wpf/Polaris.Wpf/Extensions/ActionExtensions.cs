//-----------------------------------------------------------------------
// <copyright file="ActionExtensions.cs" company="Polaris Community">
//     This code is distributed under the Microsoft Public License (MS-PL).
// </copyright>
//-----------------------------------------------------------------------
namespace Polaris.Extensions
{
    using System;
    using System.Threading;
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
    }
}