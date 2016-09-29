using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;
using System.Data.Objects;

namespace StudentPractice.BusinessModel
{
    public class VerificationLogRepository : BaseRepository<VerificationLog>
    {
        #region [ Base .ctors ]

        public VerificationLogRepository() : base() { }

        public VerificationLogRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}
