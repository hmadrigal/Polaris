using Polaris.EnterpriseEx;

namespace Polaris.EnterpriseEx
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting.Contexts;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Polaris.Services;

    /// <summary>
    /// This handler intercepts a given method. When the method does not return a value it's added to the
    /// task manager. The task manager will execute the task when possible.
    /// </summary>
    [ConfigurationElementType(typeof(PessimisticCacheHandler)), Synchronization]
    public class TaskManagerHandler : ICallHandler
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public ITaskManager TaskManager { get; set; }

        private GetNextHandlerDelegate getNext;

        private IMethodInvocation input;

        private IMethodReturn LockedInvoke()
        {
            //We need to synchronize calls to the CacheHandler on method level
            //to prevent duplicate calls to methods that could be cached.
            lock (input.MethodBase)
            {
                // If the method returns a value then it's invoked immediately OR
                // if the method has been intercepted before by this handler
                if (
                    System.Environment.StackTrace.Contains(this.TaskManager.GetType().FullName)
                    || !TargetMethodReturnsVoid(input))
                {
                    return getNext()(input, getNext);
                }

                // creates a task instance
                var args = (from object argument in input.Inputs
                            select argument).ToArray();
                var taskId = Guid.NewGuid();

                var newTask = Container.Resolve<Task>();
                newTask.Id = taskId;
                newTask.ContractType = input.MethodBase.ReflectedType;
                newTask.MethodName = input.MethodBase.Name;
                newTask.Arguments = args;

                // Performs the task execution in a separated thread.
                // Moreover, it attempts to execute the task immediately.
                // So caller does not have to wait.
                TaskManager.BeginRunOrAdd(newTask);

                return input.CreateMethodReturn(null);
            }
        }

        #region ICallHandler

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            lock (input.MethodBase)
            {
                this.input = input;
                this.getNext = getNext;
                return LockedInvoke();
            }
        }

        public int Order
        {
            get { return 0; }
            set { }
        }

        #endregion ICallHandler

        private static bool TargetMethodReturnsVoid(IMethodInvocation input)
        {
            var targetMethod = input.MethodBase as MethodInfo;
            return targetMethod != null && targetMethod.ReturnType == typeof(void);
        }

        #region class Task : ITask

        /// <summary>
        /// Represents a Task item for the task manager
        /// </summary>
        [Serializable]
        private class Task : ITask
        {
            public Guid Id { get; set; }

            public Type ContractType { get; set; }

            public string MethodName { get; set; }

            public object[] Arguments { get; set; }

            public DateTime DueDate { get; set; }

            public int RetryCount { get; set; }

            public Task()
            {
                RetryCount = 0;
                DueDate = DateTime.Now;
            }

            public object Invoke(IUnityContainer container)
            {
                var newInstance = container.Resolve(ContractType);

                var methodInfo = newInstance.GetType().GetMethod(MethodName);
                object methodResult = null;

                try
                {
                    methodResult = methodInfo.Invoke(newInstance, Arguments);
                }
                catch //(Exception ex)
                {
                    //var sb = new System.Text.StringBuilder();
                    //sb.AppendFormat("Failed invoked method: {0}.{1}(", ContractType.FullName, MethodName);
                    //Arguments.Select(arg => sb.AppendFormat(" {0},", arg.ToString())).ToArray();
                    //sb.Remove(sb.Length - 1, 1);
                    //sb.AppendFormat(")");
                    throw;
                }
                return methodResult;
            }

            public override string ToString()
            {
                return string.Format("Id:{0} Method:{1}.{2} DueDate:{3} RetryCount:{4}", this.Id, this.ContractType.FullName, this.MethodName, this.DueDate, this.RetryCount);
            }
        }

        #endregion class Task : ITask
    }
}