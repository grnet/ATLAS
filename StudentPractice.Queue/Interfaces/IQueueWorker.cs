using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPractice.Queue
{
    public interface IQueueWorker
    {
        void AddToQueue(IQueueEntry entry);

        void ProcessQueue(Action<bool> callback);

        void RemoveFromQueue(IQueueEntry entry);

        T GetQueueData<T>(IQueueEntry entry);
        void SetQueueData(IQueueEntry entry, object queueData);

    }
}
