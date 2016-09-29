using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain;

namespace StudentPractice.BusinessModel
{
    class QueueEntryRepository : BaseRepository<QueueEntry>
    {
        public QueueEntryRepository() { }

        public QueueEntryRepository(IUnitOfWork uow) : base(uow) { }
    }
}
