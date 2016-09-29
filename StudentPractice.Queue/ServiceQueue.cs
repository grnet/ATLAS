using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace StudentPractice.Queue
{
    public class ServiceQueue : IDisposable
    {
        #region [ Thread-safe, lazy Singleton ]

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static ServiceQueue Instance
        {
            get
            {
                return Nested.dispatcher;
            }
        }

        ServiceQueueConfiguration config = null;

        public ServiceQueueConfiguration Config { get { return config; } }

        ServiceQueue()
        {
            config = ConfigurationManager.GetSection("serviceQueue") as ServiceQueueConfiguration;
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        class Nested
        {
            static Nested() { }
            internal static readonly ServiceQueue dispatcher = new ServiceQueue();
        }

        #endregion

        #region [ Private Properties ]

        readonly static ILog s_Log = LogManager.GetLogger(typeof(ServiceQueue));

        int _inTimerCallback = 0;

        bool hasInizialize = false;

        Timer _timer;

        IQueueWorker _worker = null;

        readonly object objectLock = new object();

        #endregion

        #region [ Methods ]

        public void Initialize(IQueueWorker worker)
        {
            if (hasInizialize)
                return;
            hasInizialize = true;
            _worker = worker;
            if (Config.ProcessQueueOnInitialize)
            {
                ProcessQueue();
            }
            if (Config.ProcessQueueInterval > 0)
            {
                _timer = new Timer((x) =>
                {
                    try
                    {
                        // if the callback is already being executed, just return 
                        if (Interlocked.Exchange(ref _inTimerCallback, 1) != 0)
                        {
                            return;
                        }
                        lock (objectLock)
                        {
                            ProcessQueue();
                        }
                    }
                    catch (Exception ex)
                    {
                        s_Log.Error("Failure at ProcessingQueue Timer event.", ex);
                    }
                    finally
                    {
                        Interlocked.Exchange(ref _inTimerCallback, 0);
                    }

                }, null, Config.ProcessQueueInterval * 1000, Config.ProcessQueueInterval * 1000);

            }
            s_Log.Info("ServiceQueue initialized at " + DateTime.Now.ToString());
        }

        public void Dispose()
        {
            Timer timer = _timer;
            if (timer != null && Interlocked.CompareExchange(ref _timer, null, timer) == timer)
            {
                timer.Dispose();
            }
        }

        public void ProcessQueue()
        {
            if (string.IsNullOrWhiteSpace(Config.MachineName)
                || Config.MachineName == Environment.MachineName)
            {
                _worker.ProcessQueue((queueProcessed) =>
                {
                    if (queueProcessed)
                    {
                        s_Log.Info("Queue processed at " + DateTime.Now.ToString());
                    }
                });
            }
        }

        #endregion
    }
}
