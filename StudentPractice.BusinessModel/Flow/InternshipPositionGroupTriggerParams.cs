using System;
using System.Collections.Generic;
using Imis.Domain;

namespace StudentPractice.BusinessModel.Flow
{
    public class InternshipPositionGroupTriggerParams
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public enCancellationReason CancellationReason { get; set; }
        public string Username { get; set; }
        public DateTime ExecutionDate { get; set; }
        public List<InternshipPosition> Positions { get; set; }
    }
}
