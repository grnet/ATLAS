using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.Web.Api
{
    public class Delete
    {
        public int PositionID { get; set; }
    }

    public class Cancellation
    {
        public int PositionID { get; set; }
        public string CancellationReason { get; set; }
    }

    public class Completion
    {
        public int PositionID { get; set; }
        public string CompletionComments { get; set; }

        public DateTime? ImplementationStartDate { get; set; }
        public string ImplementationStartDateString { get; set; }

        public DateTime? ImplementationEndDate { get; set; }
        public string ImplementationEndDateString { get; set; }

        public string ImplementationStartDateStringFormat { get; set; }
        public string ImplementationEndDateStringFormat { get; set; }
    }
}
