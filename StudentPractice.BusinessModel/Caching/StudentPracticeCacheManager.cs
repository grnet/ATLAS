using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using Imis.Domain.EF;

namespace StudentPractice.BusinessModel
{
    public class StudentPracticeCacheManager<TEntity> : DomainCacheManager<DBEntities, TEntity, int>
        where TEntity : DomainEntity<DBEntities>
    {
        protected StudentPracticeCacheManager()
        {
            if (s_CacheStorage.Values.Count == 0)
                Fill();
        }

        #region Thread-safe, lazy Singleton

        public static StudentPracticeCacheManager<TEntity> Current
        {
            get { return Nested._cacheManager; }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private sealed class Nested
        {
            static Nested() { }
            internal static readonly StudentPracticeCacheManager<TEntity> _cacheManager = new StudentPracticeCacheManager<TEntity>();
        }

        #endregion
    }
}
