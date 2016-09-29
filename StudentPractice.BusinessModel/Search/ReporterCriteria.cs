using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public class ReporterCriteria : Criteria<Reporter>
    {
        public ReporterCriteria()
        {
            Phone = new CriteriaField<string>();
            ReporterType = new CriteriaField<enReporterType>();
            UnknownReporterType = new CriteriaField<enReporterType>();
            Description = new CriteriaField<string>();
            ProviderAFM = new CriteriaField<string>();
            ProviderName = new CriteriaField<string>();
            ProviderTradeName = new CriteriaField<string>();
            InstitutionID = new CriteriaField<int>();
            StudentNumber = new CriteriaField<string>();
            AcademicID = new CriteriaField<int>();
            CertificationNumber = new CriteriaField<int>();
        }

        public CriteriaField<string> Phone { get; set; }

        public CriteriaField<enReporterType> ReporterType { get; set; }

        public CriteriaField<enReporterType> UnknownReporterType { get; set; }

        public CriteriaField<string> Description { get; set; }

        public CriteriaField<string> ProviderAFM { get; set; }

        public CriteriaField<string> ProviderName { get; set; }

        public CriteriaField<string> ProviderTradeName { get; set; }

        public CriteriaField<int> InstitutionID { get; set; }
        
        public CriteriaField<string> StudentNumber { get; set; }
        
        public CriteriaField<int> AcademicID { get; set; }

        public CriteriaField<int> CertificationNumber { get; set; }
    }
}
