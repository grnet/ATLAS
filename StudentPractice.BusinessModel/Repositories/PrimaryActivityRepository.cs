using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;

namespace StudentPractice.BusinessModel
{
    public class PrimaryActivityRepository : BaseRepository<PrimaryActivity>
    {
        #region [ Base .ctors ]

        public PrimaryActivityRepository() : base() { }

        public PrimaryActivityRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}