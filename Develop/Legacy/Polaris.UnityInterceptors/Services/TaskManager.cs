namespace Polaris.UnityInterceptors.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Polaris.Extensions;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.Unity;

    public class TaskManager : ITaskManager
    {
        private const string LOGGING_POLICY = @"LoggingPolicy";
        private readonly string taskListName;
        private readonly Dictionary<int, TimeSpan> timeOffsets;
        private readonly TimeSpan defaultTimeOffSet;
        private bool isRunningTasks;
        System.Timers.Timer taskExecutionTimer;
        System.Timers.ElapsedEventHandler timerElapsedEventHandler;
        private ExceptionManager exceptionManager;

        [Dependency]
        public IUnityContainer Container { get; set; }

        private ICacheManager cacheManager;

        private TimeSpan expirationTime = TimeSpan.Zero;

        public TaskManager() : this(TimeSpan.Zero, null, null, TimeSpan.Zero, null) { }

        public TaskManager( KeyValuePair<int, TimeSpan>[] timeOffsets, TimeSpan defaultTimeOffSet) :
            this(TimeSpan.Zero, null, null, TimeSpan.Zero, timeOffsets.ToDictionary( kv => kv.Key, kv => kv.Value), defaultTimeOffSet, null)
        {
            
        }


        public TaskManager(TimeSpan dueTime, string taskListName, object cachedData, TimeSpan expirationTime, Dictionary<int, TimeSpan> timeOffsets = null, TimeSpan defaultTimeOffSet = default(TimeSpan), string cacheManagerName = null)
        {
            this.isRunningTasks = false;

            this.taskExecutionTimer = (dueTime == TimeSpan.Zero)
                ? new System.Timers.Timer(10000) { AutoReset = true }
                : new System.Timers.Timer(dueTime.TotalMilliseconds) { AutoReset = true };
            this.timerElapsedEventHandler = new System.Timers.ElapsedEventHandler(OnTimerTick);

            this.taskListName = string.IsNullOrWhiteSpace(taskListName)
                ? GetType().FullName
                : taskListName;

            if (expirationTime != TimeSpan.Zero)
                this.expirationTime = expirationTime;

            try { exceptionManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>(); }
            catch { exceptionManager = default(ExceptionManager); }

#if CRASHTEST
			throw new ApplicationException("CRASH TEST EXCEPTION, PLEASE IGNORE.");
#endif

            try
            {
                this.cacheManager = string.IsNullOrWhiteSpace(cacheManagerName)
                        ? EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>()
                        : EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>(cacheManagerName);
            }
            catch (Exception ex)
            {
                exceptionManager.TryHandleException(ex, LOGGING_POLICY);
                throw new ApplicationException("ICacheManager was not resolved, and could be due to Isolated Storage corruption.  Try deleting the Isolated Storage folder for this application and retry.", ex);
            }

            this.timeOffsets = timeOffsets ?? new Dictionary<int, TimeSpan>(){
                                                                                 {1,TimeSpan.FromMinutes(5)},
                                                                                 {2,TimeSpan.FromMinutes(10)},
                                                                                 {3,TimeSpan.FromMinutes(15)},
                                                                                 {4,TimeSpan.FromMinutes(30)},
                                                                                 {5,TimeSpan.FromMinutes(60)},
                                                                                 {6,TimeSpan.FromMinutes(90)}
                                                                             };
            this.defaultTimeOffSet = defaultTimeOffSet == default(TimeSpan) ? TimeSpan.FromHours(2) : defaultTimeOffSet;
        }

        private void OnTimerTick(object sender, System.Timers.ElapsedEventArgs e)
        {
            RunTasks();
        }

        private void AddToCache(string key, object valueToCache)
        {
            if (key == null)
            {
                //When the method uses input that is not serializable
                //we cannot create a cache key and can therefore not
                //cache the data.
                return;
            }

            if (expirationTime.Equals(TimeSpan.Zero))
            {
                cacheManager.Add(key, valueToCache);
            }
            else
            {
                var expiry = new AbsoluteTime(expirationTime);
                cacheManager.Add(key,
                    valueToCache,
                    CacheItemPriority.Normal,
                    null,
                    new ICacheItemExpiration[] { expiry });
            }
        }

        public ITask GetTask(Guid id)
        {
            ITask task = default(ITask);
            task = cacheManager[id.ToString()] as ITask;
            return task;
        }
        
        /// <summary>
        /// Performs the task execution in a separated thread. 
        /// Moreover, it attempts to execute the task immediately.  
        /// So caller does not have to wait. 
        /// </summary>
        /// <param name="task"></param>
        public void BeginRunOrAdd(ITask task)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((userState) =>
            {
                var taskUserState = userState as ITask;
                try
                {
                    taskUserState.Invoke(this.Container);
                }
                catch
                {
                    AddTask(taskUserState);
                }
            }, task);
        }

        
        public void AddTask(ITask newTask)
        {
            // Adds the newTask to the queue
            SaveTask(newTask);

            // Add the newTask to the isolated storage
            var taskList = cacheManager[taskListName] as List<Guid>;
            if (taskList == null) { taskList = new List<Guid>(); }
            taskList.Add(newTask.Id);
            AddToCache(taskListName, taskList);

            // Restarts the timer and triggers
            //this.taskExecutionTimer.Stop();
            //this.taskExecutionTimer.Start();
        }

        public void RemoveTask(Guid id)
        {
            // Gets task from cache
            cacheManager.Remove(id.ToString());

            // remove newTask from Isolated storage
            var taskList = cacheManager[taskListName] as List<Guid>;
            if (taskList == null)
            {
                taskList = new List<Guid>();
            }
            else //if (taskList.Contains(id))
            {
                taskList.Remove(id);
            }
            //cacheManager.Add(taskListName, taskList);
            AddToCache(taskListName, taskList);
        }

        public Guid[] GetTaskIds()
        {
            var ids = new Guid[0];

            var taskList = cacheManager[taskListName] as List<Guid>;
            if (taskList == null) { taskList = new List<Guid>(); }
            ids = taskList.ToArray();
            return ids;
        }

        private void RunTasks()
        {
            if (isRunningTasks)
            {
                return;
            }

            isRunningTasks = true;
            ITask task = default(ITask);
            Guid taskId = default(Guid);

            var taskIds = GetTaskIds();
            if (taskIds.Length == 0)
            {
                isRunningTasks = false;
                return;
            }

            for (int i = 0; i < taskIds.Length; i++)
            {
                taskId = taskIds[i];
                task = GetTask(taskId);
                if (task == null) continue;
                if (task.DueDate > DateTime.Now)
                {
                    continue;
                }
                try
                {
                    task.Invoke(Container);
                    RemoveTask(taskId);
                }
                catch (Exception ex)
                {
                    exceptionManager.TryHandleException(ex, LOGGING_POLICY);
                    task.RetryCount++;
                    task.DueDate = GetNextDueDate(task);
                    SaveTask(task);
                }
            }
            isRunningTasks = false;
        }

        public void Start()
        {
            taskExecutionTimer.Elapsed += timerElapsedEventHandler;
            taskExecutionTimer.Start();
        }

        public void Stop()
        {
            taskExecutionTimer.Stop();
            taskExecutionTimer.Elapsed -= timerElapsedEventHandler;
        }

        public void SaveTask(ITask task)
        {
            AddToCache(task.Id.ToString(), task);
        }

        public DateTime GetNextDueDate(ITask task)
        {
            if (task == null)
            {
                return DateTime.Now.Add(defaultTimeOffSet);
            }
            if (timeOffsets.Count == 0)
            {
                return task.DueDate.Add(defaultTimeOffSet);
            }

            if (timeOffsets.ContainsKey(task.RetryCount))
            {
                return task.DueDate.Add(timeOffsets[task.RetryCount]);
            }

            var longestTimeOffset = timeOffsets.Values.Max();
            return task.DueDate.Add(longestTimeOffset);
        }
    }
}