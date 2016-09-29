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

namespace StudentPractice.BusinessModel
{
    public class InternshipPositionRepository : BaseRepository<InternshipPosition>
    {
        #region [ Base .ctors ]

        public InternshipPositionRepository() : base() { }

        public InternshipPositionRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public List<InternshipPosition> FindByGroupID(int groupID, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.GroupID == groupID).ToList();
        }

        public bool ExistingPreAssignedInternshipPositions(int groupID)
        {
            return BaseQuery.Any(x => x.GroupID == groupID && x.PositionStatusInt >= (int)enPositionStatus.PreAssigned);
        }

        public List<InternshipPosition> FindByStudent(int studentID, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.AssignedToStudentID == studentID).ToList();
        }

        public List<InternshipPosition> FindActiveByStudent(int studentID, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.AssignedToStudentID == studentID)
                        .Where(x => x.PositionStatusInt == (int)enPositionStatus.UnderImplementation || x.PositionStatusInt == (int)enPositionStatus.Assigned)
                        .ToList();
        }

        public List<InternshipPosition> FindPreAssignedInternshipPositions(int skip, int take, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.PositionStatusInt >= (int)enPositionStatus.PreAssigned).OrderBy(x => x.ID).Skip(skip).Take(take).ToList();
        }

        public List<InternshipPosition> FindAssignedInternshipPositions(int skip, int take, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.PositionStatusInt == (int)enPositionStatus.Assigned).OrderBy(x => x.ID).Skip(skip).Take(take).ToList();
        }

        public List<InternshipPosition> FindUnPreAssignedInternshipPositions(int groupID, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.GroupID == groupID && x.PositionStatusInt < (int)enPositionStatus.PreAssigned).ToList();
        }

        public List<InternshipPosition> GetInternshipPositionReport(Criteria<InternshipPosition> criteria, out  int totalRecordCount)
        {
            var query = BaseQuery;

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.PreAssignedForAcademic)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.CanceledStudent)
                .Include(x => x.PreAssignedByOffice)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.InternshipPositionGroup.Provider);

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
                .Where(x => x.InternshipPositionGroup.IsVisibleToAllAcademics == false)
                .Select(x => new
                {
                    GroupID = x.GroupID,
                    Academics = x.InternshipPositionGroup.Academics.Select(a => a.ID)
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
                    GroupID = x.GroupID,
                    PhysicalObjects = x.InternshipPositionGroup.PhysicalObjects.Select(po => po.ID)
                })
                .ToList()
                .GroupBy(x => x.GroupID)
                .ToDictionary(x => x.Key, x => x.SelectMany(a => a.PhysicalObjects).ToList());

            retValue.ForEach(ip =>
            {
                if (academics.ContainsKey(ip.GroupID))
                    academics[ip.GroupID].ForEach(aID =>
                    {
                        if (!ip.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
                            ip.InternshipPositionGroup.Academics.Add(academicsIndex[aID]);
                    });

                if (physicalObjects.ContainsKey(ip.GroupID))
                    physicalObjects[ip.GroupID].ForEach(pID => ip.InternshipPositionGroup.PhysicalObjects.Add(physicalObjectsIndex[pID]));
            });

            totalRecordCount = retValue.Count;
            return retValue;
        }

        public InternshipPosition FindOneByStudentID(int pID, int studentID, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return query.Where(x => x.ID == pID
                && x.AssignedToStudentID == studentID
                && (x.PositionStatusInt == (int)enPositionStatus.Assigned
                    || x.PositionStatusInt == (int)enPositionStatus.UnderImplementation
                    || x.PositionStatusInt == (int)enPositionStatus.Completed
                    || (x.PositionStatusInt == (int)enPositionStatus.Canceled && x.CancellationReasonInt == (int)enCancellationReason.FromOffice)))
                .FirstOrDefault();
        }

        #region [ Service Methods ]

        public InternshipPosition LoadByOffice(int positionID, InternshipOffice office, int status, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            var position = query.Where(x => x.ID == positionID)
                .Where(x => x.PositionStatusInt == status)
                .Where(x => (x.PreAssignedByOfficeID == office.ID) || (x.PreAssignedByMasterAccountID == office.ID))
                .FirstOrDefault();

            if (position != null)
            {
                var visibleAcademics = new List<Academic>();
                visibleAcademics.AddRange(position.InternshipPositionGroup.Academics);
                position.InternshipPositionGroup.Academics.Clear();

                if (!position.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
                {
                    if (office.Academics != null)
                    {
                        foreach (var academic in visibleAcademics)
                        {
                            if (office.Academics.Select(x => x.ID).Contains(academic.ID))
                            {
                                position.InternshipPositionGroup.Academics.Add(academic);
                            }
                        }
                    }
                }
            }
            return position;
        }

        public InternshipPosition LoadMoreThanPreassignedByOffice(int positionID, InternshipOffice office, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            
            var position = query.Where(x => x.ID == positionID)
                .Where(x => x.PositionStatusInt >= (int)enPositionStatus.Assigned)
                .Where(x => (x.PreAssignedByOfficeID == office.ID) || (x.PreAssignedByMasterAccountID == office.ID))
                .FirstOrDefault();
            
            return position;
        }

        public InternshipPosition LoadAssignedByOffice(int positionID, InternshipOffice office, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            var position = query.Where(x => x.ID == positionID)
                .Where(x => (x.PositionStatusInt == (int)enPositionStatus.Assigned) || (x.PositionStatusInt == (int)enPositionStatus.UnderImplementation))
                .Where(x => (x.PreAssignedByOfficeID == office.ID) || (x.PreAssignedByMasterAccountID == office.ID))
                .Where(x => x.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromProvider)
                .FirstOrDefault();

            if (position == null)
                return null;

            var visibleAcademics = new List<Academic>();
            visibleAcademics.AddRange(position.InternshipPositionGroup.Academics);
            position.InternshipPositionGroup.Academics.Clear();

            if (!position.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
            {
                if (office.Academics != null)
                {
                    foreach (var academic in visibleAcademics)
                    {
                        if (office.Academics.Select(x => x.ID).Contains(academic.ID))
                        {
                            position.InternshipPositionGroup.Academics.Add(academic);
                        }
                    }
                }
            }
            return position;
        }

        public List<InternshipPosition> FindByOffice(InternshipOffice office, int status, int skip, int take, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            var positions = query.Where(x => x.PositionStatusInt == status)
                .Where(x => x.PreAssignedByMasterAccountID == office.ID || x.PreAssignedByOfficeID == office.ID)
                .Where(x => x.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromProvider)
                .OrderBy(x => x.ID)
                .Skip(skip)
                .Take(take)
                .ToList();

            foreach (var position in positions)
            {
                var visibleAcademics = new List<Academic>();
                visibleAcademics.AddRange(position.InternshipPositionGroup.Academics);
                position.InternshipPositionGroup.Academics.Clear();

                if (!position.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
                {
                    if (office.Academics != null)
                    {
                        foreach (var academic in visibleAcademics)
                        {
                            if (office.Academics.Select(x => x.ID).Contains(academic.ID))
                            {
                                position.InternshipPositionGroup.Academics.Add(academic);
                            }
                        }
                    }
                }
            }
            return positions;
        }

        public List<InternshipPosition> FindAssignedByOffice(InternshipOffice office, int skip, int take, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            var positions = query.Where(x => x.PositionStatusInt == (int)enPositionStatus.Assigned || x.PositionStatusInt == (int)enPositionStatus.UnderImplementation)
                .Where(x => x.PreAssignedByMasterAccountID == office.ID || x.PreAssignedByOfficeID == office.ID)
                .Where(x => x.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromProvider)
                .OrderBy(x => x.ID)
                .Skip(skip)
                .Take(take)
                .ToList();


            foreach (var position in positions)
            {
                var visibleAcademics = new List<Academic>();
                visibleAcademics.AddRange(position.InternshipPositionGroup.Academics);
                position.InternshipPositionGroup.Academics.Clear();

                if (!position.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
                {
                    if (office.Academics != null)
                    {
                        foreach (var academic in visibleAcademics)
                        {
                            if (office.Academics.Select(x => x.ID).Contains(academic.ID))
                            {
                                position.InternshipPositionGroup.Academics.Add(academic);
                            }
                        }
                    }
                }
            }
            return positions;
        }

        public List<InternshipPosition> FindFinishedPositions(InternshipOffice office, int skip, int take, params Expression<Func<InternshipPosition, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            var positions = query.Where(x => x.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromOffice)
                .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                .Where(x => x.PreAssignedByMasterAccountID == office.ID || x.PreAssignedByOfficeID == office.ID)
                .OrderBy(x => x.ID)
                .Skip(skip)
                .Take(take)
                .ToList();

            foreach (var position in positions)
            {
                var visibleAcademics = new List<Academic>();
                visibleAcademics.AddRange(position.InternshipPositionGroup.Academics);
                position.InternshipPositionGroup.Academics.Clear();

                if (!position.InternshipPositionGroup.IsVisibleToAllAcademics.GetValueOrDefault())
                {
                    if (office.Academics != null)
                    {
                        foreach (var academic in visibleAcademics)
                        {
                            if (office.Academics.Select(x => x.ID).Contains(academic.ID))
                            {
                                position.InternshipPositionGroup.Academics.Add(academic);
                            }
                        }
                    }
                }
            }
            return positions;
        }

        public void GetInternshipPositionsAsReader(Action<IDataReader> processReader)
        {
            var con = (SqlConnection)((EntityConnection)GetCurrentObjectContext().Connection).StoreConnection;
            using (var cmd = new SqlCommand("sp_GetInternshipPositions", con))
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
                finally
                {
                    con.Close();
                }
            }
        }

        #endregion
    }
}
