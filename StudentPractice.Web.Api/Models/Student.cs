using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public class StudentAcademic
    {
        public int? ID { get; set; }
        public string School { get; set; }
        public string Institution { get; set; }
        public int InstitutionID { get; set; }
        public string Department { get; set; }
    }

    public class StudentResult
    {
        public int ID { get; set; }
        public string PrincipalUserName { get; set; }
        public string AcademicIDNumber { get; set; }
        public string StudentNumber { get; set; }

        public StudentAcademic Academic { get; set; }
        public StudentAcademic PreviousAcademic { get; set; }

        public bool? IsNameLatin { get; set; }
        public string SystemFirstName { get; set; }
        public string SystemLastName { get; set; }
        public string GreekFirstName { get; set; }
        public string GreekLastName { get; set; }
        public string LatinFirstName { get; set; }
        public string LatinLastName { get; set; }

        public bool IsActive { get; set; }
    }

    public class StudentUpdateRequest
    {
        public int? ID { get; set; }
        public string AcademicIDNumber { get; set; }
    }

    public class StudentRequest
    {
        public string AcademicIDNumber { get; set; }
    }

    public class StudentAcademicIDNumberRequest
    {
        public string StudentNumber { get; set; }
        public int? AcademicID { get; set; }
    }

    public class GetStudentCardNumberModel
    {
        public string StudentNumber { get; set; }
        public int AcademicID { get; set; }
        public int ServiceCallerID { get; set; }
    }

    public class StudentResultFromLDAP
    {
        public string Comment { get; set; }

        public string PrincipalUserName { get; set; }
        public string AcademicIDNumber { get; set; }
        public string StudentNumber { get; set; }

        public StudentAcademic Academic { get; set; }
        public StudentAcademic PreviousAcademic { get; set; }

        public bool? IsNameLatin { get; set; }
        public string SystemFirstName { get; set; }
        public string SystemLastName { get; set; }
        public string GreekFirstName { get; set; }
        public string GreekLastName { get; set; }
        public string LatinFirstName { get; set; }
        public string LatinLastName { get; set; }

        public bool IsActive { get; set; }
    }

    public static class Student
    {
        public static StudentResult ToStudentResult(this BusinessModel.Student student)
        {
            Academic academic = null;            
            if (student.AcademicID.HasValue)
                academic = StudentPracticeCacheManager<Academic>.Current.Get(student.AcademicID.Value);

            Academic previousAcademic = null;
            if (student.PreviousAcademicID.HasValue)
                previousAcademic = StudentPracticeCacheManager<Academic>.Current.Get(student.PreviousAcademicID.Value);

            return new StudentResult()
            {
                ID = student.ID,
                PrincipalUserName = student.UsernameFromLDAP,
                AcademicIDNumber = student.AcademicIDNumber,
                StudentNumber = student.StudentNumber,

                Academic = academic != null ? new StudentAcademic()
                {
                    ID = academic.ID,
                    School = academic.School,
                    Institution = academic.Institution,
                    InstitutionID = academic.InstitutionID,
                    Department = academic.Department
                } : null,

                PreviousAcademic = previousAcademic != null ? new StudentAcademic()
                {
                    ID = previousAcademic.ID,
                    School = previousAcademic.School,
                    Institution = previousAcademic.Institution,
                    InstitutionID = previousAcademic.InstitutionID,
                    Department = previousAcademic.Department
                } : null,

                IsNameLatin = student.IsNameLatin,
                GreekFirstName = student.GreekFirstName,
                GreekLastName = student.GreekLastName,
                LatinFirstName = student.LatinFirstName,
                LatinLastName = student.LatinLastName,
                SystemFirstName = student.OriginalFirstName,
                SystemLastName = student.OriginalLastName,

                IsActive = student.IsActive
            };
        }

        public static StudentResultFromLDAP ToStudentResult(this BusinessModel.Student student, string comment)
        {
            Academic academic = null;
            if (student.AcademicID.HasValue)
                academic = StudentPracticeCacheManager<Academic>.Current.Get(student.AcademicID.Value);

            Academic previousAcademic = null;
            if (student.PreviousAcademicID.HasValue)
                previousAcademic = StudentPracticeCacheManager<Academic>.Current.Get(student.PreviousAcademicID.Value);

            return new StudentResultFromLDAP()
            {
                Comment = comment,
                PrincipalUserName = student.UsernameFromLDAP,
                AcademicIDNumber = student.AcademicIDNumber,
                StudentNumber = student.StudentNumber,

                Academic = academic != null ? new StudentAcademic()
                {
                    ID = academic.ID,
                    School = academic.School,
                    Institution = academic.Institution,
                    InstitutionID = academic.InstitutionID,
                    Department = academic.Department
                } : null,

                PreviousAcademic = previousAcademic != null ? new StudentAcademic()
                {
                    ID = previousAcademic.ID,
                    School = previousAcademic.School,
                    Institution = previousAcademic.Institution,
                    InstitutionID = previousAcademic.InstitutionID,
                    Department = previousAcademic.Department
                } : null,

                IsNameLatin = student.IsNameLatin,
                GreekFirstName = student.GreekFirstName,
                GreekLastName = student.GreekLastName,
                LatinFirstName = student.LatinFirstName,
                LatinLastName = student.LatinLastName,
                SystemFirstName = student.OriginalFirstName,
                SystemLastName = student.OriginalLastName,

                IsActive = student.IsActive
            };
        }

        public static StudentResultFromLDAP ToStudentResult(this StudentDetailsFromAcademicID student, string comment)
        {
            var academic = StudentPracticeCacheManager<Academic>.Current.Get(student.AcademicID);
            Academic previousAcademic = null;
            if (student.PreviousAcademicID.HasValue)
                previousAcademic = StudentPracticeCacheManager<Academic>.Current.Get(student.PreviousAcademicID.Value);

            return new StudentResultFromLDAP()
            {
                Comment = comment,
                PrincipalUserName = student.UsernameFromLDAP,
                AcademicIDNumber = student.AcademicIDNumber,
                StudentNumber = student.StudentNumber,

                Academic = academic != null ? new StudentAcademic()
                {
                    ID = academic.ID,
                    School = academic.School,
                    Institution = academic.Institution,
                    InstitutionID = academic.InstitutionID,
                    Department = academic.Department
                } : null,

                PreviousAcademic = previousAcademic != null ? new StudentAcademic()
                {
                    ID = previousAcademic.ID,
                    School = previousAcademic.School,
                    Institution = previousAcademic.Institution,
                    InstitutionID = previousAcademic.InstitutionID,
                    Department = previousAcademic.Department
                } : null,

                IsNameLatin = student.IsNameLatin,
                GreekFirstName = student.GreekFirstName,
                GreekLastName = student.GreekLastName,
                LatinFirstName = student.LatinFirstName,
                LatinLastName = student.LatinLastName,
                SystemFirstName = student.OriginalFirstName,
                SystemLastName = student.OriginalLastName
            };
        }
    }
}