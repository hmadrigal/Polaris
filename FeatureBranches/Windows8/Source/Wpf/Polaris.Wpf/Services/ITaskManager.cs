using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polaris.Services
{
    using System;
    using Microsoft.Practices.Unity;

    public interface ITaskManager
    {
        ITask GetTask(Guid id);

        void RemoveTask(Guid id);

        void BeginRunOrAdd(ITask task);

        void AddTask(ITask newTask);

        void SaveTask(ITask task);

        void Start();

        void Stop();

        DateTime GetNextDueDate(ITask task);
    }

    public interface ITask
    {
        Guid Id { get; set; }

        object[] Arguments { get; set; }

        Type ContractType { get; set; }

        object Invoke(IUnityContainer container);

        string MethodName { get; set; }

        DateTime DueDate { get; set; }

        int RetryCount { get; set; }
    }
}