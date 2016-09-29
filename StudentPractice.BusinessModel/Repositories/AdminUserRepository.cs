using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Imis.Domain.EF.Extensions;

namespace StudentPractice.BusinessModel
{
    public class AdminUserRepository : BaseRepository<AdminUser>
    {
        #region [ Base .ctors ]

        public AdminUserRepository() : base() { }

        public AdminUserRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public AdminUser FindByUsername(string username, params Expression<Func<AdminUser, object>>[] includeExpressions)
        {
            var query = BaseQuery;
            if (includeExpressions.Length != 0)
            {
                foreach (var item in includeExpressions)
                    query = query.Include(item);
            }
            return query.Where(x => x.UserName == username).SingleOrDefault();
        }
    }
}