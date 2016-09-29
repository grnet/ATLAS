using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using StudentPractice.Utils;

namespace StudentPractice.BusinessModel
{
    public class InternshipPositionGroupRepository : BaseRepository<InternshipPositionGroup>
    {
        #region [ Base .ctors ]

        public InternshipPositionGroupRepository() : base() { }

        public InternshipPositionGroupRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public bool PositionsExistForProvider(int providerID)
        {
            return BaseQuery.Any(x => x.ProviderID == providerID);
        }

        public List<InternshipPositionGroup> FindPublishedPositions(DateTime publishedAt, params Expression<Func<InternshipPositionGroup, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.PositionGroupStatusInt == (int)enPositionGroupStatus.Published
                && x.AvailablePositions > 0
                && x.FirstPublishedAt == publishedAt)
                .ToList();
        }

        public List<InternshipPositionGroup> GetInternshipPositionGroupReport(Criteria<InternshipPositionGroup> criteria, out  int totalRecordCount)
        {
            var query = BaseQuery;

            criteria.Include(x => x.Provider);

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

            var academics = query
                .Where(x => x.IsVisibleToAllAcademics == false)
                .Select(x => new
                {
                    GroupID = x.ID,
                    Academics = x.Academics.Select(a => a.ID)
                })
                .ToList()
                .GroupBy(x => x.GroupID)
                .ToDictionary(x => x.Key, x => x.SelectMany(a => a.Academics).ToList());

            var physicalObjectsIndex = StudentPracticeCacheManager<PhysicalObject>.Current
                .GetItems()
                .Select(p => new PhysicalObject() { ID = p.ID, Name = p.Name })
                .ToDictionary(p => p.ID);

            var physicalObjects = query
                .Select(x => new
                {
                    GroupID = x.ID,
                    PhysicalObjects = x.PhysicalObjects.Select(po => po.ID)
                })
                .ToList()
                .GroupBy(x => x.GroupID)
                .ToDictionary(x => x.Key, x => x.SelectMany(a => a.PhysicalObjects).ToList());

            retValue.ForEach(ip =>
            {
                if (academics.ContainsKey(ip.ID))
                    academics[ip.ID].ForEach(aID =>
                    {
                        if (!ip.IsVisibleToAllAcademics.GetValueOrDefault())
                            ip.Academics.Add(academicsIndex[aID]);
                    });

                if (physicalObjects.ContainsKey(ip.ID))
                    physicalObjects[ip.ID].ForEach(pID => ip.PhysicalObjects.Add(physicalObjectsIndex[pID]));
            });

            totalRecordCount = retValue.Count;
            return retValue;
        }

        public List<int> GetCountryIDsOfProviderPositions(int providerID)
        {
            return BaseQuery.Where(x => x.ProviderID == providerID).Select(x => x.CountryID).Distinct().ToList();
        }

        public bool IsAvailableToOnlyOneAcademic(int groupID)
        {
            return BaseQuery.Any(x => x.ID == groupID && x.Academics.Count == 1);
        }

        public void GetInternshipPositionGroupsAsReader(Action<IDataReader> processReader)
        {
            var con = (SqlConnection)((EntityConnection)GetCurrentObjectContext().Connection).StoreConnection;
            using (var cmd = new SqlCommand("sp_GetInternshipPositionGroups", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                try
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        processReader(reader);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError<InternshipPositionGroupRepository>(ex, this, "Error while exporting InternshipPositionGroups to disc");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        #region [ Service Methods ]

        public List<InternshipPositionGroup> FindByInternshipOffice(InternshipOffice office, int skip, int take, out int count)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();
            criteria.Include(x => x.Provider);
            criteria.UsePaging = true;
            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            criteria.Expression = criteria.Expression.Where(x => x.AvailablePositions, 0, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);

            if (office.IsMasterAccount)
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", office.ID));
            else
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", office.MasterAccountID));

            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);
            if (office.CanViewAllAcademics.GetValueOrDefault())
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", StudentPracticeCacheManager<Academic>.Current.GetItems().Where(x => x.InstitutionID == office.InstitutionID.Value).Select(x => x.ID))));
            else
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", office.Academics.Select(x => x.ID))));

            criteria.Expression = criteria.Expression.And(orExpression);
            criteria.MaximumRows = take;
            criteria.StartRowIndex = skip;

            return FindWithCriteria(criteria, out count);
        }

        public InternshipPositionGroup FindGroupByInternshipOffice(InternshipOffice office, int ID)
        {
            Criteria<InternshipPositionGroup> criteria = new Criteria<InternshipPositionGroup>();
            criteria.Include(x => x.Provider)
                .Include(x => x.City)
                .Include(x => x.Prefecture)
                .Include(x => x.Country)
                .Include(x => x.PhysicalObjects)
                .Include(x => x.Academics)
                .Include(x => x.Positions);

            criteria.Expression = criteria.Expression.Where(x => x.ID, ID);
            criteria.Expression = criteria.Expression.Where(x => x.PositionGroupStatus, enPositionGroupStatus.Published);
            //criteria.Expression = criteria.Expression.Where(x => x.AvailablePositions, 0, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);

            if (office.IsMasterAccount)
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", office.ID));
            else
                criteria.Expression = criteria.Expression.Where(string.Format("NOT EXISTS (SELECT VALUE it1 FROM BlockedPositionGroupSet as it1 WHERE it1.GroupID = it.ID AND it1.MasterAccountID = {0})", office.MasterAccountID));
            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPositionGroup>.Empty;
            orExpression = orExpression.Where(x => x.IsVisibleToAllAcademics, true);

            if (office.CanViewAllAcademics.GetValueOrDefault())
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", StudentPracticeCacheManager<Academic>.Current.GetItems().Where(x => x.InstitutionID == office.InstitutionID.Value).Select(x => x.ID))));
            else
                orExpression = orExpression.Or(string.Format("(it.Academics) OVERLAPS (SELECT VALUE it2 FROM AcademicSet as it2 WHERE it2.ID IN MULTISET ({0}) )", string.Join(",", office.Academics.Select(x => x.ID))));
            criteria.Expression = criteria.Expression.And(orExpression);

            criteria.UsePaging = false;
            int count;
            var group = FindWithCriteria(criteria, out count).FirstOrDefault();
            if (group == null)
                return null;

            var visibleAcademics = new List<Academic>();
            visibleAcademics.AddRange(group.Academics);
            group.Academics.Clear();

            if (!group.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                if (office.Academics != null)
                {
                    foreach (var academic in visibleAcademics)
                    {
                        if (office.Academics.Select(x => x.ID).Contains(academic.ID))
                        {
                            group.Academics.Add(academic);
                        }
                    }
                }
            }
            return group;
        }

        #endregion
    }
}