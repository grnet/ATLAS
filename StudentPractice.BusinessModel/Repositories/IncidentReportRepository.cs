using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;
using System.Data.Objects;

namespace StudentPractice.BusinessModel
{
    public class IncidentReportRepository : BaseRepository<IncidentReport>
    {
        #region [ Base .ctors ]

        public IncidentReportRepository() : base() { }

        public IncidentReportRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public bool HasIncidentReports(int reporterID)
        {
            return BaseQuery.Any(x => x.ReporterID == reporterID);
        }
    }
}