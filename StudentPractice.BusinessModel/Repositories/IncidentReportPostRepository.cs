using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq.Expressions;

namespace StudentPractice.BusinessModel
{
    public class IncidentReportPostRepository : BaseRepository<IncidentReportPost>
    {
        #region [ Base .ctors ]

        public IncidentReportPostRepository() : base() { }

        public IncidentReportPostRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public List<IncidentReportPost> FindByIncidentReportID(int incidentReportID, params Expression<Func<IncidentReportPost, object>>[] includes)
        {
            var query = BaseQuery;
            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query
                .Where(x => x.IncidentReport.ID == incidentReportID)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public List<IncidentReportPost> FindDispatchedByIncidentReportID(int incidentReportID, params Expression<Func<IncidentReportPost, object>>[] includes)
        {
            var query = BaseQuery;
            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query
                .Where(x => x.IncidentReportID == incidentReportID && x.LastDispatchID != null)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }

        public int GetAnsweredIncidentReportCount(string helpdeskUserName)
        {
            return BaseQuery.Count(x => x.CreatedBy == helpdeskUserName);
        }
    }
}