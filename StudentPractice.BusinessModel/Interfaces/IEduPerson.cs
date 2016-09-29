using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public interface IEduPerson
    {
        string Username { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string AcademicID { get; set; }
        string StudentCode { get; set; }
        string Affiliation { get; set; }
        string Fullname { get; set; }
    }
}
