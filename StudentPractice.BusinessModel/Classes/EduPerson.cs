using System;

namespace StudentPractice.BusinessModel
{
    public class EduPerson : IEduPerson
    {
        #region IEduPerson Members

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Fullname { get; set; }
        public string AcademicID { get; set; }
        public string StudentCode { get; set; }
        public string Affiliation { get; set; }

        #endregion
    }
}
