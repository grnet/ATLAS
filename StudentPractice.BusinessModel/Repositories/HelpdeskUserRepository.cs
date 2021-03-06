﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Imis.Domain.EF.Extensions;

namespace StudentPractice.BusinessModel
{
    public class HelpdeskUserRepository : BaseRepository<HelpdeskUser>
    {
        #region [ Base .ctors ]

        public HelpdeskUserRepository() : base() { }

        public HelpdeskUserRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public HelpdeskUser FindByUsername(string username, params Expression<Func<HelpdeskUser, object>>[] includeExpressions)
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