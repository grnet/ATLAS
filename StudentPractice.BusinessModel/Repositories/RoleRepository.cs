using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain;

namespace StudentPractice.BusinessModel
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository() { }

        public RoleRepository(IUnitOfWork uow) : base(uow) { }
    }
}
