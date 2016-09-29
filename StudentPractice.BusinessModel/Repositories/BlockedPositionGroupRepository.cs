using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq.Expressions;

namespace StudentPractice.BusinessModel
{
    public class BlockedPositionGroupRepository : BaseRepository<BlockedPositionGroup>
    {
        #region [ Base .ctors ]

        public BlockedPositionGroupRepository() : base() { }

        public BlockedPositionGroupRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public BlockedPositionGroup FindByGroupID(int groupID, params Expression<Func<BlockedPositionGroup, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.GroupID == groupID && x.MasterBlockID == null).SingleOrDefault();
        }

        public bool BlockedPositionGroupExists(int groupID, int masterAccountID)
        {
            return BaseQuery.Any(x => x.GroupID == groupID && x.MasterAccountID == masterAccountID);
        }
    }
}