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
    public class ReporterRepository : BaseRepository<Reporter>
    {
        #region [ Base .ctors ]

        public ReporterRepository() : base() { }

        public ReporterRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public List<Reporter> FindWithCriteria(ReporterCriteria criteria, out int totalRecordCount)
        {
            var query = BaseQuery;

            if (criteria.Includes != null)
            {
                criteria.Includes.ForEach(x => query = query.Include(x));
            }

            if (!string.IsNullOrEmpty(criteria.Expression.CommandText))
            {
                query = query.Where(criteria.Expression.CommandText, criteria.Expression.Parameters);
            }

            if (!criteria.Phone.IsEmpty)
            {
                query = query.Where("it.ContactPhone = @phone OR it.ContactMobilePhone = @phone", new ObjectParameter("phone", criteria.Phone.FieldValue));
            }

            if (!criteria.ProviderName.IsEmpty)
            {
                query = query.OfType<InternshipProvider>().Where("it.Name LIKE N\"%" + criteria.ProviderName.FieldValue + "%\" OR it.TradeName LIKE N\"%" + criteria.ProviderName.FieldValue + "%\"").OfType<Reporter>();
            }

            if (!criteria.ProviderAFM.IsEmpty)
            {
                query = query.OfType<InternshipProvider>().Where("it.AFM = @providerAFM", new ObjectParameter("providerAFM", criteria.ProviderAFM.FieldValue)).OfType<Reporter>();
            }

            if (!criteria.InstitutionID.IsEmpty)
            {
                query = query.OfType<InternshipOffice>().Where("it.InstitutionID = " + criteria.InstitutionID.FieldValue).OfType<Reporter>();
            }

            if (string.IsNullOrEmpty(criteria.Sort.Expression))
                criteria.Sort.Expression = "it.CreatedAt DESC";
            else
                criteria.Sort.Expression = "it." + criteria.Sort.Expression.Replace(",", ",it.");

            if (criteria.UsePaging)
            {
                totalRecordCount = query.Count();
                return query.OrderBy(criteria.Sort.Expression).Skip(criteria.StartRowIndex).Take(criteria.MaximumRows).ToList();
            }
            var retValue = query.ToList();
            totalRecordCount = retValue.Count;
            return retValue;
        }

        public Reporter FindByID<T>(int id, params Expression<Func<T, object>>[] includes)
            where T : Reporter
        {
            var query = BaseQuery.OfType<T>();

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.FirstOrDefault(x => x.ID == id);
        }

        public List<Reporter> FindChildAccounts(int masterAccountID, params Expression<Func<Reporter, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.MasterAccountID == masterAccountID).ToList();
        }

        public Reporter FindByEmail(string email, params Expression<Func<Reporter, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.FirstOrDefault(x => x.Email == email);
        }

        public Reporter FindByUsername(string username, params Expression<Func<Reporter, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.FirstOrDefault(x => x.UserName == username || x.UsernameFromLDAP == username);
        }

        public Reporter FindByEmailVerificationCode(string emailVerificationCode, params Expression<Func<Reporter, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.EmailVerificationCode == emailVerificationCode).SingleOrDefault();
        }
    }
}