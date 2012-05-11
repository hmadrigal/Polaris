namespace Polaris.UnityInterceptors.Services
{
    using System;
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
}
