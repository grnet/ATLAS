using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPractice.Queue
{
    public interface IQueueEntry
    {
        int QueueEntryTypeInt { get; set; }
        string QueueDataXml { get; set; }
        object ID { get; set; }
        object QueueDataID { get; set; }
        int? MaxNoOfRetries { get; set; }
        int NoOfRetries { get; set; }
    }

    public interface IQueueEntry<T> : IQueueEntry
    {
        T ID { get; set; }
        T QueueDataID { get; set; }
    }
}
