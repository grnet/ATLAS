using System;

namespace StudentPractice.BusinessModel
{
    public class IncidentReportCriteria : Criteria<IncidentReport>
    {
        public IncidentReportCriteria()
        {
            OnlineReporterType = new CriteriaField<enReporterType>();
        }

        public CriteriaField<enReporterType> OnlineReporterType { get; set; }
    }
}
