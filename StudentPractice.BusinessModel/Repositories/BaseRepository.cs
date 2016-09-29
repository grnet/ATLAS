using Imis.Domain.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPractice.BusinessModel
{
    public class BaseRepository<TEntity> : DomainRepository<DBEntities, TEntity, int>
        where TEntity : DomainEntity<DBEntities>
    {
        public BaseRepository() : base() { }

        public BaseRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }
    }
}
