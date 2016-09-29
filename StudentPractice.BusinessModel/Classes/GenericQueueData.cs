using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public class GenericQueueDataCollection : List<GenericQueueData>
    {

    }

    public class GenericQueueData
    {
        public int NoOfRetry { get; set; }
        public string Message { get; set; }
        public string ServerName { get; set; }
    }
}
