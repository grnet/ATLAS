using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentPractice.Utils.Worker
{
    public class AsyncWorkerItemCollection : List<AsyncWorkerItem>
    {
        public AsyncWorkerItemCollection() : base() { }
        public AsyncWorkerItemCollection(IEnumerable<AsyncWorkerItem> collection) : base(collection) { }
    }
    public class AsyncWorkerItem
    {
        ///// <summary>
        ///// In seconds
        ///// </summary>
        //public int Interval { get; set; }

        /// <summary>
        /// The task to be executed asyncronously
        /// </summary>
        public Action Task { get; set; }

        public string Name { get; set; }
    }
}