using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;

namespace StudentPractice.BusinessModel
{
    public class InternshipOfficeRepository : BaseRepository<InternshipOffice>
    {
        #region [ Base .ctors ]

        public InternshipOfficeRepository() : base() { }

        public InternshipOfficeRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public InternshipOffice FindByInstitutionID(int institutionID)
        {
            return BaseQuery.Where(x => x.InstitutionID == institutionID && x.IsMasterAccount).FirstOrDefault();
        }

        public int FindInstitutionIDByUsername(string username)
        {
            return BaseQuery.Where(x => x.UserName == username).Select(x => x.InstitutionID.Value).FirstOrDefault();
        }

        public InternshipOffice FindByUsername(string username, params Expression<Func<InternshipOffice, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.UserName == username).SingleOrDefault();
        }

        public bool IsInstitutionVerified(int currentReporterID, int institutionID)
        {
            return FindOfficesByVerificationStatus(institutionID, enVerificationStatus.Verified).Any(x => x.ID != currentReporterID);
        }

        //public bool VerifiedOfficeExists(int officeID, enOfficeType officeType, int institutionID, List<int> academicIDs)
        //{
        //    bool verifiedOfficeExists = false;
        //
        //    if (officeType == enOfficeType.Institutional)
        //    {
        //        verifiedOfficeExists = BaseQuery.Any(x =>
        //            x.ID != officeID
        //            && x.IsMasterAccount
        //            && x.OfficeTypeInt == (int)officeType
        //            && x.InstitutionID == institutionID
        //            && x.VerificationStatusInt == (int)enVerificationStatus.Verified);
        //    }
        //    else
        //    {
        //        foreach (int academicID in academicIDs)
        //        {
        //            if (BaseQuery.Any(x =>
        //                x.ID != officeID
        //                && x.IsMasterAccount
        //                && x.OfficeTypeInt != (int)officeType
        //                && x.InstitutionID == institutionID
        //                && x.VerificationStatusInt == (int)enVerificationStatus.Verified
        //                && x.Academics.Any(y => y.ID == academicID)))
        //            {
        //                verifiedOfficeExists = true;
        //            }
        //        }
        //    }
        //
        //    return verifiedOfficeExists;
        //}

        public bool VerifiedOfficeExists(int officeID, enOfficeType officeType, int institutionID, List<int> academicIDs)
        {
            if (officeType == enOfficeType.Institutional)
            {
                return BaseQuery.Any(x =>
                    x.ID != officeID
                    && x.IsMasterAccount
                    && x.OfficeTypeInt == (int)enOfficeType.Institutional
                    && x.InstitutionID == institutionID
                    && x.VerificationStatusInt == (int)enVerificationStatus.Verified);
            }
            else
            {
                return BaseQuery.Any(x =>
                    x.ID != officeID
                    && x.IsMasterAccount
                    && (x.OfficeTypeInt == (int)enOfficeType.Departmental || x.OfficeTypeInt == (int)enOfficeType.MultipleDepartmental)
                    && x.InstitutionID == institutionID
                    && x.VerificationStatusInt == (int)enVerificationStatus.Verified
                    && x.Academics.Any(y => academicIDs.Contains(y.ID)));
            }
        }

        public bool InstitutionalVerifiedOfficeExists(int institutionID)
        {
            return BaseQuery.Any(x => x.OfficeTypeInt == (int)enOfficeType.Institutional
                && x.InstitutionID == institutionID
                && x.IsMasterAccount
                && x.VerificationStatusInt == (int)enVerificationStatus.Verified);
        }

        public bool DepartmentalVerifiedOfficeExists(int institutionID)
        {
            return BaseQuery.Any(x => x.OfficeTypeInt != (int)enOfficeType.Institutional
                && x.InstitutionID == institutionID
                && x.IsMasterAccount
                && x.VerificationStatusInt == (int)enVerificationStatus.Verified);
        }

        public List<int> GetVerifiedOfficeIDs(int institutionID, int academicID)
        {
            return BaseQuery.Where(x =>
                x.IsMasterAccount
                && x.InstitutionID == institutionID
                && x.VerificationStatusInt == (int)enVerificationStatus.Verified
                && ((x.CanViewAllAcademics.HasValue && x.CanViewAllAcademics.Value) || x.Academics.Select(y => y.ID).Contains(academicID)))
                .Select(x => x.ID).ToList();

            //List<int> verifiedOfficeIDs = new List<int>();
            //
            //var verifiedInstitutionalOffice = BaseQuery.Where(x => x.OfficeTypeInt == (int)enOfficeType.Institutional && x.InstitutionID == institutionID && x.IsMasterAccount && x.VerificationStatusInt == (int)enVerificationStatus.Verified).FirstOrDefault();
            //
            //if (verifiedInstitutionalOffice != null)
            //{
            //    verifiedOfficeIDs.Add(verifiedInstitutionalOffice.ID);
            //}
            //
            //var verifiedDepartmentalOffice = BaseQuery.Where(x => x.OfficeTypeInt != (int)enOfficeType.Institutional && x.InstitutionID == institutionID && x.IsMasterAccount && x.Academics.Select(y => y.ID).Contains(academicID) && x.VerificationStatusInt == (int)enVerificationStatus.Verified).FirstOrDefault();
            //
            //if (verifiedDepartmentalOffice != null)
            //{
            //    verifiedOfficeIDs.Add(verifiedDepartmentalOffice.ID);
            //}
            //
            //return verifiedOfficeIDs;
        }

        public InternshipOffice GetVerifiedInstitutionalOffice(int institutionID)
        {
            return BaseQuery.Where(x => x.OfficeTypeInt == (int)enOfficeType.Institutional
                && x.InstitutionID == institutionID
                && x.IsMasterAccount
                && x.VerificationStatusInt == (int)enVerificationStatus.Verified)
                .FirstOrDefault();
        }

        public List<InternshipOffice> GetVerifiedDepartmentalOffices(int institutionID)
        {
            return BaseQuery.Where(x => x.OfficeTypeInt != (int)enOfficeType.Institutional
                && x.InstitutionID == institutionID
                && x.IsMasterAccount
                && x.VerificationStatusInt == (int)enVerificationStatus.Verified)
                .ToList();
        }

        public List<InternshipOffice> FindOfficesByVerificationStatus(int institutionID, enVerificationStatus verificationStatus)
        {
            return BaseQuery.Where(x => x.IsMasterAccount
                && x.InstitutionID == institutionID
                && x.VerificationStatusInt == (int)verificationStatus)
                .ToList();
        }

        public List<InternshipOffice> FindVerifiedOffices(params Expression<Func<InternshipOffice, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.IsMasterAccount && x.VerificationStatusInt == (int)enVerificationStatus.Verified).ToList();
        }

        public List<InternshipOffice> GetInternshipOfficeUsersReport(Criteria<InternshipOffice> criteria, out  int totalRecordCount)
        {
            var query = BaseQuery;

            criteria.Include(x => x.MasterAccount);

            if (criteria.Includes != null)
            {
                criteria.Includes.ForEach(x => query = query.Include(x));
            }

            if (!string.IsNullOrEmpty(criteria.Expression.CommandText))
            {
                query = query.Where(criteria.Expression.CommandText, criteria.Expression.Parameters);
            }

            if (string.IsNullOrEmpty(criteria.Sort.Expression))
                criteria.Sort.Expression = "it.ID ASC";
            else
                criteria.Sort.Expression = "it." + criteria.Sort.Expression.Replace(",", ",it.");

            query.MergeOption = MergeOption.NoTracking;
            var retValue = query.OrderBy(criteria.Sort.Expression).ToList();

            var academicsIndex = StudentPracticeCacheManager<Academic>.Current
                .GetItems()
                .Select(a => new Academic() { ID = a.ID, Institution = a.Institution, Department = a.Department })
                .ToDictionary(a => a.ID);

            var masterQuery = BaseQuery;
            masterQuery.MergeOption = MergeOption.NoTracking;
            var masterAccountIDs = retValue.Select(y => y.MasterAccountID).Distinct().ToList();
            masterQuery = (ObjectQuery<InternshipOffice>)masterQuery.Where(x => masterAccountIDs.Contains(x.ID));
            var masterOffices = masterQuery.ToList();

            var masterAcademics = masterQuery
                .Select(x => new
                {
                    ID = x.ID,
                    Academics = x.Academics.Select(a => a.ID)
                })
                .ToList()
                .GroupBy(x => x.ID)
                .ToDictionary(x => x.Key, x => x.SelectMany(a => a.Academics).ToList());

            masterOffices.ForEach(mo =>
            {
                if (masterAcademics.ContainsKey(mo.ID))
                    masterAcademics[mo.ID].ForEach(maID => mo.Academics.Add(academicsIndex[maID]));
            });

            var academics = query
                .Select(x => new
                {
                    ID = x.ID,
                    Academics = x.Academics.Select(a => a.ID)
                })
                .ToList()
                .GroupBy(x => x.ID)
                .ToDictionary(x => x.Key, x => x.SelectMany(a => a.Academics).ToList());

            retValue.ForEach(o =>
            {
                if (academics.ContainsKey(o.ID))
                    academics[o.ID].ForEach(aID => o.Academics.Add(academicsIndex[aID]));

                o.MasterAccount = masterOffices.FirstOrDefault(x => x.ID == o.MasterAccountID);
            });

            totalRecordCount = retValue.Count;
            return retValue;
        }

        public List<int> GetParticipatingProvidersIDs(InternshipOffice office)
        {
            var academicIDs = office.Academics.Select(x => (int?)x.ID);
            var positionsSet = GetCurrentObjectContext().InternshipPositionSet;

            if (office.IsMasterAccount)
            {
                return positionsSet
                    .Where(x => x.PreAssignedByMasterAccountID == office.ID)
                    .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                    .Select(x => x.InternshipPositionGroup.ProviderID)
                    .Distinct()
                    .ToList();
            }
            else
            {
                return positionsSet
                    .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                    .Where(x => (x.PreAssignedByMasterAccountID == office.MasterAccountID && academicIDs.Contains(x.PreAssignedForAcademicID))
                        || x.PreAssignedByOfficeID == office.ID)
                    .Select(x => x.InternshipPositionGroup.ProviderID)
                    .Distinct()
                    .ToList();
            }
        }

        public bool HasCompletedPositions(InternshipOffice office)
        {
            var academicIDs = office.Academics.Select(x => (int?)x.ID);
            var positionsSet = GetCurrentObjectContext().InternshipPositionSet;

            if (office.IsMasterAccount)
            {
                return positionsSet
                    .Where(x => x.PreAssignedByMasterAccountID == office.ID)
                    .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                    .Any();
            }
            else
            {
                return positionsSet
                    .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                    .Where(x => (x.PreAssignedByMasterAccountID == office.MasterAccountID && academicIDs.Contains(x.PreAssignedForAcademicID))
                        || x.PreAssignedByOfficeID == office.ID)
                    .Any();
            }
        }
    }
}