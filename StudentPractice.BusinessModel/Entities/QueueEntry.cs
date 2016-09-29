using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.Queue;

namespace StudentPractice.BusinessModel
{
    public partial class QueueEntry : IQueueEntry
    {
        public enQueueEntryType QueueEntryType
        {
            get { return (enQueueEntryType)QueueEntryTypeInt; }
            set
            {
                int intValue = (int)value;
                if (intValue != QueueEntryTypeInt)
                    QueueEntryTypeInt = intValue;
            }
        }

        object IQueueEntry.ID
        {
            get { return ID; }
            set { }
        }

        object IQueueEntry.QueueDataID
        {
            get { return QueueDataID; }
            set { QueueDataID = (int)value; }
        }
    }
}
