using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;

namespace StudentPractice.BusinessModel
{
    public class InternshipProviderRepository : BaseRepository<InternshipProvider>
    {
        #region [ Base .ctors ]

        public InternshipProviderRepository() : base() { }

        public InternshipProviderRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public InternshipProvider FindByUsername(string username, params Expression<Func<InternshipProvider, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.UserName == username).SingleOrDefault();
        }

        public bool IsAfmVerified(int currentReporterID, string afm)
        {
            return FindProvidersByVerificationStatus(afm, enVerificationStatus.Verified).Any(x => x.ID != currentReporterID);
        }

        public List<InternshipProvider> FindProvidersByVerificationStatus(string afm, enVerificationStatus verificationStatus)
        {
            return BaseQuery.Where(x => x.AFM == afm
                && x.IsMasterAccount
                && x.VerificationStatusInt == (int)verificationStatus)
                .ToList();
        }

        public List<InternshipProvider> FindAvailableProviders()
        {
            return BaseQuery.Where(x => x.DeclarationTypeInt == (int)enReporterDeclarationType.FromRegistration
                && x.VerificationStatusInt == (int)enVerificationStatus.Verified)
                .OrderBy(x => x.TradeName)
                .ThenBy(x => x.Name)
                .ToList();
        }

        public List<int> GetParticipatingOfficesIDs(int providerID)
        {
            var positionsSet = GetCurrentObjectContext().InternshipPositionSet;

            return positionsSet
                .Where(x => x.InternshipPositionGroup.ProviderID == providerID)
                .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                .Where(x => x.PreAssignedByOfficeID.HasValue)
                .Select(x => x.PreAssignedByOfficeID.Value)
                .Distinct()
                .ToList();
        }

        public bool HasCompletedPositions(int providerID)
        {
            var positionsSet = GetCurrentObjectContext().InternshipPositionSet;

            return positionsSet
                .Where(x => x.InternshipPositionGroup.ProviderID == providerID)
                .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                .Any();
        }

        #region [ Service Methods ]
        public InternshipProvider FindByIDVerified(int ID)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.ID, ID);
            criteria.Expression = criteria.Expression.Where(x => x.VerificationStatusInt, (int)enVerificationStatus.Verified);
            criteria.UsePaging = false;

            int count;
            return FindWithCriteria(criteria, out count).FirstOrDefault();
        }

        public List<InternshipProvider> FindByAFM(string AFM)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.AFM, AFM);
            criteria.Expression = criteria.Expression.Where(x => x.VerificationStatusInt, (int)enVerificationStatus.Verified);
            criteria.UsePaging = false;

            int count;
            return FindWithCriteria(criteria, out count).ToList();
        }


        #endregion
    }
}