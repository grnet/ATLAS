using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;

namespace StudentPractice.BusinessModel
{
    public class IncidentTypeRepository : BaseRepository<IncidentType>
    {
        #region [ Base .ctors ]

        public IncidentTypeRepository() : base() { }

        public IncidentTypeRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}