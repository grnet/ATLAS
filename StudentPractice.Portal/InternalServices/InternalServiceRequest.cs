using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentPractice.Portal.InternalServices
{
    public class AcademicIDCardRequest
    {
        public int ServiceCallerID { get; set; }
        public int AcademicID { get; set; }
        public string StudentNumber { get; set; }
        public string NewAcademicIDNumber { get; set; }
        public int AcademicIDStatus { get; set; }
        public DateTime? AcademicIDSubmissionDate { get; set; }
    }
}