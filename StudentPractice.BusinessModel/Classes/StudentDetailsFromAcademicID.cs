using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public class StudentDetailsFromAcademicID
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string OriginalFirstName { get; set; }
        public string OriginalLastName { get; set; }
        public bool? IsNameLatin { get; set; }
        public string GreekFirstName { get; set; }
        public string GreekLastName { get; set; }
        public string LatinFirstName { get; set; }
        public string LatinLastName { get; set; }
        public string ContactMobilePhone { get; set; }
        public string ContactEmail { get; set; }
        public bool IsEmailVerified { get; set; }
        public int AcademicID { get; set; }
        public string StudentNumber { get; set; }
        public int? PreviousAcademicID { get; set; }
        public string PreviousStudentNumber { get; set; }
        public string UsernameFromLDAP { get; set; }
        public string AcademicIDNumber { get; set; }
        public int AcademicIDStatus { get; set; }
        public DateTime? AcademicIDSubmissionDate { get; set; }
    }
}
