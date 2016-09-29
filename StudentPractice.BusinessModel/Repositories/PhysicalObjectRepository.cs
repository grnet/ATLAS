using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;

namespace StudentPractice.BusinessModel
{
    public class PhysicalObjectRepository : BaseRepository<PhysicalObject>
    {
        #region [ Base .ctors ]

        public PhysicalObjectRepository() : base() { }

        public PhysicalObjectRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}