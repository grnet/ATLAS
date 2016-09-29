using System;
using Imis.Domain;
using System.Collections.Generic;

namespace StudentPractice.BusinessModel.Flow
{
    public class InternshipPositionTriggersParams
    {
        public int OfficeID { get; set; }
        public int MasterAccountID { get; set; }
        public int InstitutionID { get; set; }
        public Academic Academic { get; set; }
        public int StudentID { get; set; }
        public DateTime ImplementationStartDate { get; set; }
        public DateTime ImplementationEndDate { get; set; }
        public enFundingType FundingType { get; set; }
        public enBlockingReason BlockingReason { get; set; }
        public enCancellationReason CancellationReason { get; set; }
        public List<Academic> Academics { get; set; }
        public string Username { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Comment { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
    }
}