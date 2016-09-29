using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StudentPractice.BusinessModel;

namespace StudentPractice.Web.Api
{
    public class AcademicResult
    {
        public int ID { get; set; }
        public int InstitutionID { get; set; }
        public string Institution { get; set; }
        public string School { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
    }

    public class InstitutionResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameInLatin { get; set; }
    }

    public class CountryResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class PrefectureResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class CityResult
    {
        public int ID { get; set; }
        public int PrefectureID { get; set; }
        public string Name { get; set; }
    }

    public class PhysicalObjectResult
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public static class Lookup
    {
        public static AcademicResult ToServiceLookup(this Academic academic)
        {
            return new AcademicResult()
            {
                ID = academic.ID,
                InstitutionID = academic.InstitutionID,
                Institution = academic.Institution,
                School = academic.School,
                Department = academic.Department,
                IsActive = academic.IsActive
            };
        }

        public static InstitutionResult ToServiceLookup(this Institution institution)
        {
            return new InstitutionResult()
            {
                ID = institution.ID,
                Name = institution.Name,
                NameInLatin = institution.NameInLatin
            };
        }

        public static CountryResult ToServiceLookup(this Country country)
        {
            return new CountryResult()
            {
                ID = country.ID,
                Name = country.Name
            };
        }

        public static PrefectureResult ToServiceLookup(this Prefecture prefecture)
        {
            return new PrefectureResult()
            {
                ID = prefecture.ID,
                Name = prefecture.Name
            };
        }

        public static CityResult ToServiceLookup(this City city)
        {
            return new CityResult()
            {
                ID = city.ID,
                PrefectureID = city.PrefectureID,
                Name = city.Name
            };
        }

        public static PhysicalObjectResult ToServiceLookup(this PhysicalObject phObject)
        {
            return new PhysicalObjectResult()
            {
                ID = phObject.ID,
                Name = phObject.Name
            };
        }

    }
}
