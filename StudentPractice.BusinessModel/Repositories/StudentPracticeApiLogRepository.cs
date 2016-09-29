using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StudentPractice.BusinessModel
{
    public class StudentPracticeApiLogRepository : BaseRepository<StudentPracticeApiLog>
    {
        #region [ Base .ctors ]

        public StudentPracticeApiLogRepository() : base() { }

        public StudentPracticeApiLogRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion
    }
}
