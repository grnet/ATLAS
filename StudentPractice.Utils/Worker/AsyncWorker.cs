using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Threading;
using System.Configuration;
using StudentPractice.Utils.Worker;
using StudentPractice.Utils;

namespace StudentPractice.Utils.Worker
{
    public class AsyncWorker
    {
        #region [ Thread-safe, lazy Singleton ]

        public static AsyncWorker Instance
        {
            get
            {
                return Nested.asyncWorker;
            }
        }

        public WorkerConfiguration config = null;

        private AsyncWorker()
        {
            Items = new AsyncWorkerItemCollection();
            config = ConfigurationManager.GetSection("asyncWorker") as WorkerConfiguration;
            if (config == null)
                config = new WorkerConfiguration();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly AsyncWorker asyncWorker = new AsyncWorker();
        }

        #endregion

        public AsyncWorkerItemCollection Items { get; set; }

        public static void Initialize()
        {
            AsyncWorker.Instance.InitializeInternal();
        }

        #region [ Private Properties ]

        readonly static ILog s_Log = LogManager.GetLogger(typeof(AsyncWorker));

        int _inTimerCallback = 0;

        bool hasInizialize = false;

        Timer _timer;

        readonly object objectLock = new object();

        #endregion

        #region [ Methods ]

        void InitializeInternal()
        {
            if (hasInizialize)
                return;

            hasInizialize = true;

            if (Items.Count == 0)
                return;

            if (config.ProcessQueueOnInitialize)
            {
                ThreadPool.QueueUserWorkItem((y) =>
                {
                    Items.ForEach(x => ProcessItem(x));
                    InitializeTimer();
                });
            }
            else
            {
                InitializeTimer();
            }
        }

        void InitializeTimer()
        {
            if (config.ProcessQueueInterval > 0)
            {
                if (_timer != null)
                    _timer.Dispose();
                s_Log.Info("Initializing Timer at " + DateTime.Now.ToString());
                _timer = new Timer((x) =>
                {

                    try
                    {
                        // if the callback is already being executed, just return 
                        if (Interlocked.Exchange(ref _inTimerCallback, 1) != 0)
                        {
                            LogHelper.LogMessage("Callback already being executed", this);
                            return;
                        }
                    }
                    catch { }
                    try
                    {
                        lock (objectLock)
                        {
                            DateTime processingStartedAt = DateTime.Now;
                            foreach (var i in Items)
                            {
                                ProcessItem(i, processingStartedAt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        s_Log.Error("Failure at ProcessingItem Timer event.", ex);
                    }
                    finally
                    {
                        Interlocked.Exchange(ref _inTimerCallback, 0);
                    }

                }, null, config.ProcessQueueInterval * 1000, config.ProcessQueueInterval * 1000);

            }
            s_Log.Info("AsyncWorker initialized at " + DateTime.Now.ToString());
        }

        public void Dispose()
        {
            //s_Log.Info("AsyncWorker disposing at " + DateTime.Now.ToString());
            Timer timer = _timer;
            if (timer != null && Interlocked.CompareExchange(ref _timer, null, timer) == timer)
            {
                timer.Dispose();
            }
        }

        void ProcessItem(AsyncWorkerItem item, DateTime? processingStartedAt = null)
        {
            try
            {
                if (string.IsNullOrEmpty(config.MachineName)
                    || config.MachineName.ToLower() == Environment.MachineName.ToLower())
                {
                    var workerItem = config.WorkerItems.OfType<WorkerItem>().FirstOrDefault(x => x.Name == item.Name);
                    if (workerItem != null)
                    {//Checking if date has passed     
                        if (!processingStartedAt.HasValue)
                        {
                            processingStartedAt = DateTime.Now;
                        }
                        if (processingStartedAt.Value > workerItem.RunAt && workerItem.RunAt > processingStartedAt.Value.AddSeconds(-config.ProcessQueueInterval))
                        {
                            if (item.Task != null)
                            {
                                s_Log.Info("In Progress for task" + item.Name + " Thread:" + Thread.CurrentThread.ManagedThreadId);
                                item.Task();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                s_Log.Error(ex);
            }
        }

        #endregion
    }

}