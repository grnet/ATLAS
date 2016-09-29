using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.Web.Api
{
    public class Assignment
    {
        public int PositionID { get; set; }
        public int StudentID { get; set; }

        public DateTime? ImplementationStartDate { get; set; }
        public DateTime? ImplementationEndDate { get; set; }

        public string ImplementationStartDateString { get; set; }
        public string ImplementationEndDateString { get; set; }

        public string ImplementationStartDateStringFormat { get; set; }
        public string ImplementationEndDateStringFormat { get; set; }
    }

    public class AssignStudent
    {
        public int PositionID { get; set; }
        public int StudentID { get; set; }
    }

    public class Reassignment
    {
        public int PositionID { get; set; }
        public DateTime? ImplementationStartDate { get; set; }
        public DateTime? ImplementationEndDate { get; set; }

        public string ImplementationStartDateString { get; set; }
        public string ImplementationEndDateString { get; set; }

        public string ImplementationStartDateStringFormat { get; set; }
        public string ImplementationEndDateStringFormat { get; set; }
    }
}
