using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;
using System.Data.Objects;

namespace StudentPractice.BusinessModel
{
    public class InternshipPositionLogRepository : BaseRepository<InternshipPositionLog>
    {
        #region [ Base .ctors ]

        public InternshipPositionLogRepository() : base() { }

        public InternshipPositionLogRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public bool HasMadePreAssignemnts(int officeID)
        {
            return BaseQuery.Any(x => x.PreAssignedByOfficeID == officeID);
        }
    }
}