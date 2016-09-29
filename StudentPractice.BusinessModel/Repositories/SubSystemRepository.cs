using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;

namespace StudentPractice.BusinessModel
{
    public class SubSystemRepository : BaseRepository<SubSystem>
    {
        #region [ Base .ctors ]

        public SubSystemRepository() : base() { }

        public SubSystemRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}