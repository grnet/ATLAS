using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Security;
using Imis.Domain;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Linq.Expressions;

namespace StudentPractice.BusinessModel
{
    public class StudentRepository : BaseRepository<Student>
    {
        #region [ Base .ctors ]

        public StudentRepository() : base() { }

        public StudentRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Student FindActiveByID(int id, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.ID == id && x.IsActive).SingleOrDefault();
        }

        public int? FindAcademicIDByUsername(string username)
        {
            return BaseQuery
                .Where(student => student.UserName == username)
                .Select(student => student.AcademicID)
                .FirstOrDefault();
        }

        public Student FindByUsernameFromLDAP(string usernameFromLDAP, int academicID, string studentNumber)
        {
            Criteria<Student> criteria = new Criteria<Student>();

            criteria.UsePaging = false;
            criteria.Expression = criteria.Expression.Where(x => x.UsernameFromLDAP, usernameFromLDAP);
            criteria.Expression = criteria.Expression.Where(x => x.AcademicID, academicID);
            criteria.Expression = criteria.Expression.Where(x => x.StudentNumber, studentNumber);

            int studentCount;
            IList<Student> students = FindWithCriteria(criteria, out studentCount);

            if (studentCount == 1)
            {
                return students[0];
            }

            if (studentCount > 1)
                throw new Exception(
                    string.Format("The DB is inconsistent : {0} students found for usernameFromLDAP {1}, academicID {2} and studentNumber {3}", studentCount, usernameFromLDAP, academicID, studentNumber));

            return null;
        }

        public Student FindByUsernameFromLDAP(string username, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.UsernameFromLDAP == username).SingleOrDefault();
        }

        public Student FindActiveByUsernameFromLDAP(string username, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.UsernameFromLDAP == username && x.IsActive).SingleOrDefault();
        }

        public Student FindByStudentNumber(string studentNumber, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.StudentNumber == studentNumber || x.PreviousStudentNumber == studentNumber).SingleOrDefault();
        }

        public Student FindByUsername(string username, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.UserName == username).SingleOrDefault();
        }

        public Student FindByAcademicIDNumber(string academicIDNumber, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.AcademicIDNumber == academicIDNumber).SingleOrDefault();
        }

        public Student FindActiveByAcademicIDNumber(string academicIDNumber, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => x.AcademicIDNumber == academicIDNumber && x.IsActive).SingleOrDefault();
        }

        public Student FindByStudentNumberAndAcademicID(string studentNumber, int academicID, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => (x.StudentNumber == studentNumber && x.AcademicID == academicID)
                || (x.PreviousStudentNumber == studentNumber && x.PreviousAcademicID == academicID)).SingleOrDefault();
        }

        public Student FindActiveByStudentNumberAndAcademicID(string studentNumber, int academicID, params Expression<Func<Student, object>>[] includes)
        {
            var query = BaseQuery;

            if (includes.Length != 0)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query.Where(x => ((x.StudentNumber == studentNumber && x.AcademicID == academicID)
                || (x.PreviousStudentNumber == studentNumber && x.PreviousAcademicID == academicID)) && x.IsActive).SingleOrDefault();
        }

        public bool ShibbolizedStudentExists(string username)
        {
            return BaseQuery.Any(x => x.UsernameFromLDAP == username);
        }

        public bool ShibbolizedStudentExists(int academicID, string studentNumber)
        {
            return BaseQuery.Any(x => x.AcademicID == academicID && x.StudentNumber == studentNumber);
        }

        public List<Student> FindShibbolizedStudentsByUsernameNameAndStudentNumber(string username, string firstName, string lastName, string studentNumber)
        {
            return BaseQuery.Where(x =>
                x.UsernameFromLDAP == username
                && x.OriginalFirstName == firstName
                && x.OriginalLastName == lastName
                && (x.StudentNumber == studentNumber || x.PreviousStudentNumber == studentNumber))
                .ToList();
        }

        public List<Student> FindShibbolizedStudentsByAcademicIDNameAndStudentNumber(int academicID, string firstName, string lastName, string studentNumber)
        {
            return BaseQuery.Where(x => x.OriginalFirstName == firstName && x.OriginalLastName == lastName &&
                ((x.AcademicID == academicID && x.StudentNumber == studentNumber)
                || (x.PreviousAcademicID == academicID && x.PreviousStudentNumber == studentNumber))).ToList();
        }

        public List<Student> FindShibbolizedStudentsByUsername(string username)
        {
            return BaseQuery.Where(x => x.UsernameFromLDAP == username).ToList();
        }

        public List<Student> FindShibbolizedStudentsByAcademicIDAndStudentNumber(int academicID, string studentNumber)
        {
            return BaseQuery.Where(x => (x.AcademicID == academicID && x.StudentNumber == studentNumber)
                || (x.PreviousAcademicID == academicID && x.PreviousStudentNumber == studentNumber)).ToList();
        }

        public Student FindShibbolizedStudentByAcademicIDAndStudentNumber(int academicID, string studentNumber)
        {
            try
            {
                return BaseQuery
                        .Where(s => s.AcademicID == academicID && s.StudentNumber == studentNumber)
                        .SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new Exception(string.Format("The DB is inconsistent : More than one (1) students found for studentNumber {0} and AcademicID {1}", studentNumber, academicID));
            }
        }

        public Student CreateStudent(StudentDetailsFromAcademicID studentDetailsFromAcademicID, bool fromService = false)
        {
            Student student = new Student();


            student.DeclarationType = enReporterDeclarationType.FromRegistration;
            if (fromService)
                student.RegistrationType = enRegistrationType.Services;
            else
                student.RegistrationType = enRegistrationType.AcademicIDNumber;

            student.OriginalFirstName = studentDetailsFromAcademicID.OriginalFirstName;
            student.OriginalLastName = studentDetailsFromAcademicID.OriginalLastName;
            student.IsNameLatin = studentDetailsFromAcademicID.IsNameLatin;
            student.GreekFirstName = studentDetailsFromAcademicID.GreekFirstName;
            student.GreekLastName = studentDetailsFromAcademicID.GreekLastName;
            student.LatinFirstName = studentDetailsFromAcademicID.LatinFirstName;
            student.LatinLastName = studentDetailsFromAcademicID.LatinLastName;
            student.ContactMobilePhone = studentDetailsFromAcademicID.ContactMobilePhone;
            student.ContactEmail = studentDetailsFromAcademicID.ContactEmail;
            student.IsEmailVerified = studentDetailsFromAcademicID.IsEmailVerified;
            student.AcademicID = studentDetailsFromAcademicID.AcademicID;
            student.StudentNumber = studentDetailsFromAcademicID.StudentNumber;
            student.UsernameFromLDAP = studentDetailsFromAcademicID.UsernameFromLDAP;
            student.AcademicIDNumber = studentDetailsFromAcademicID.AcademicIDNumber;
            student.PreviousAcademicID = studentDetailsFromAcademicID.PreviousAcademicID;
            student.PreviousStudentNumber = studentDetailsFromAcademicID.PreviousStudentNumber;
            student.AcademicIDStatus = (enAcademicIDApplicationStatus)studentDetailsFromAcademicID.AcademicIDStatus;
            student.AcademicIDSubmissionDate = studentDetailsFromAcademicID.AcademicIDSubmissionDate;

            student.IsAssignedToPosition = false;

            student.UserName = student.CreatedBy = Guid.NewGuid().ToString();
            student.ContactName = student.OriginalFirstName + " " + student.OriginalLastName;
            student.IsContactInfoCompleted = true;

            student.IsActive = true;

            var uow = GetCurrentObjectContext() as IUnitOfWork;
            uow.MarkAsNew(student);
            uow.Commit();

            return student;
        }

        public Student CreateOrGetStudent(ShibDetails shibDetails)
        {
            var student = FindByUsernameFromLDAP(shibDetails.Username);
            if (student == null)
                return CreateStudent(shibDetails);
            else
            {
                if (student.AcademicID != int.Parse(shibDetails.AcademicID))
                {
                    student.PreviousAcademicID = student.AcademicID;
                    student.AcademicID = int.Parse(shibDetails.AcademicID);
                }

                if (student.StudentNumber != shibDetails.StudentCode.Trim())
                {
                    student.PreviousStudentNumber = student.StudentNumber;
                    student.StudentNumber = shibDetails.StudentCode.Trim();
                }

                ((IUnitOfWork)GetCurrentObjectContext()).Commit();
                return student;
            }
        }

        public Student CreateStudent(ShibDetails shibDetails)
        {
            Student student = new Student();

            student.DeclarationType = enReporterDeclarationType.FromRegistration;
            student.RegistrationType = enRegistrationType.Shibboleth;

            student.OriginalFirstName = shibDetails.FirstName;
            student.OriginalLastName = shibDetails.LastName;
            student.AcademicID = int.Parse(shibDetails.AcademicID);
            student.StudentNumber = shibDetails.StudentCode.Trim();
            student.IsNameLatin = false;
            student.IsAssignedToPosition = false;
            //student.PositionCount = 0;

            student.UsernameFromLDAP = shibDetails.Username;
            student.UserName = student.CreatedBy = Guid.NewGuid().ToString();
            student.ContactName = student.OriginalFirstName + " " + student.OriginalLastName;
            student.IsContactInfoCompleted = false;

            student.IsActive = true;

            var uow = GetCurrentObjectContext() as IUnitOfWork;
            uow.MarkAsNew(student);
            uow.Commit();

            return student;
        }

        public void UpdateShibDetails(Student student, ShibDetails shibDetails)
        {
            student.UsernameFromLDAP = shibDetails.Username;
            student.OriginalFirstName = shibDetails.FirstName;
            student.OriginalLastName = shibDetails.LastName;
            student.ContactName = student.OriginalFirstName + " " + student.OriginalLastName;
            student.StudentNumber = shibDetails.StudentCode;

            int aID = -1;
            if (int.TryParse(shibDetails.AcademicID, out aID))
                student.AcademicID = aID;

            GetCurrentObjectContext().SaveChanges();
        }

        public bool StudentByAcademicIDNameAndMobilePhoneExists(int studentID, int academicID, string firstName, string lastName, string mobilePhone)
        {
            return BaseQuery.Any(x => x.ID != studentID
                                    && x.AcademicID == academicID
                                    && x.OriginalFirstName == firstName
                                    && x.OriginalLastName == lastName
                                    && x.ContactMobilePhone == mobilePhone);
        }

        public virtual List<Student> FindWithOfficePositionCount(Criteria<Student> criteria, out  int totalRecordCount)
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

            if (string.IsNullOrEmpty(criteria.Sort.Expression))
                criteria.Sort.Expression = "it.ID ASC";
            else
                criteria.Sort.Expression = "it." + criteria.Sort.Expression.Replace(",", ",it.");

            query.MergeOption = System.Data.Objects.MergeOption.NoTracking;

            if (criteria.UsePaging)
            {
                totalRecordCount = query.Count();
                var results = query
                    .OrderBy(criteria.Sort.Expression)
                    .Skip(criteria.StartRowIndex)
                    .Take(criteria.MaximumRows)
                    .Select(x => new
                    {
                        Student = x,
                        PositionCount = x.Positions.Count(y => y.InternshipPositionGroup.PositionGroupStatusInt != (int)enPositionGroupStatus.Deleted
                            && y.CancellationReasonInt != (int)enCancellationReason.CanceledGroupCascade
                            && y.CancellationReasonInt != (int)enCancellationReason.FromHelpdesk
                            && (y.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromProvider
                            || (y.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromOffice && y.PositionStatusInt != (int)enPositionStatus.Canceled)))
                    }).ToList();

                foreach (var item in results)
                    item.Student.PositionCount = item.PositionCount;
                return results.Select(x => x.Student).ToList();
            }
            var retValue = query
                .Select(x => new
                {
                    Student = x,
                    PositionCount = x.Positions.Count(y => y.InternshipPositionGroup.PositionGroupStatusInt != (int)enPositionGroupStatus.Deleted
                        && y.CancellationReasonInt != (int)enCancellationReason.CanceledGroupCascade
                        && y.CancellationReasonInt != (int)enCancellationReason.FromHelpdesk
                        && (y.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromProvider
                        || (y.InternshipPositionGroup.PositionCreationTypeInt == (int)enPositionCreationType.FromOffice && y.PositionStatusInt != (int)enPositionStatus.Canceled)))
                }).ToList();
            totalRecordCount = retValue.Count;

            foreach (var item in retValue)
                item.Student.PositionCount = item.PositionCount;
            return retValue.Select(x => x.Student).ToList();
        }

        public bool HasCompletedPositions(int studentID)
        {
            var positionsSet = GetCurrentObjectContext().InternshipPositionSet;

            return positionsSet
                .Where(x => x.AssignedToStudentID == studentID)
                .Where(x => x.PositionStatusInt == (int)enPositionStatus.Completed)
                .Any();
        }

        #region [ Service Methods ]

        public List<Student> FindByOffice(InternshipOffice office, int skip, int take)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Include(x => x.Academic);

            criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);
            criteria.Expression = criteria.Expression.And(GetAcademicsExpression(office));

            criteria.Sort.OrderBy(x => x.IsAssignedToPosition)
                         .ThenBy(x => x.Academic.DepartmentInGreek)
                         .ThenBy(x => x.GreekLastName)
                         .ThenBy(x => x.GreekFirstName)
                         .ThenBy(x => x.OriginalLastName)
                         .ThenBy(x => x.OriginalFirstName);
            criteria.UsePaging = true;
            criteria.MaximumRows = take;
            criteria.StartRowIndex = skip;

            int count;
            return FindWithCriteria(criteria, out count);
        }

        public Student FindByStudentNumber(string studentNumber, int academicID)
        {
            var query = BaseQuery;
            query = query.Include(x => x.Academic);
            return query.Where(x =>
                (x.StudentNumber == studentNumber && x.AcademicID == academicID) ||
                (x.PreviousStudentNumber == studentNumber && x.PreviousAcademicID == academicID)).SingleOrDefault();
        }

        public Student FindByStudentNumber(InternshipOffice office, string studentNumber, int academicID)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Include(x => x.Academic);

            var expressionA = Imis.Domain.EF.Search.Criteria<Student>.Empty;
            var expressionB = Imis.Domain.EF.Search.Criteria<Student>.Empty;

            expressionA = expressionA.Where(x => x.StudentNumber, studentNumber);
            expressionA = expressionA.Where(x => x.AcademicID, academicID);

            expressionB = expressionB.Where(x => x.PreviousStudentNumber, studentNumber);
            expressionB = expressionB.Where(x => x.PreviousAcademicID, academicID);

            criteria.Expression = criteria.Expression.And(expressionA.Or(expressionB));
            criteria.Expression = criteria.Expression.And(GetAcademicsExpression(office));
            criteria.UsePaging = false;

            int count;
            return FindWithCriteria(criteria, out count).FirstOrDefault();
        }

        public Student LoadByOfficeWithID(InternshipOffice office, int ID)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Include(x => x.Academic);

            criteria.Expression = criteria.Expression.Where(x => x.ID, ID);
            criteria.Expression = criteria.Expression.And(GetAcademicsExpression(office));
            criteria.UsePaging = false;

            int count;
            return FindWithCriteria(criteria, out count).FirstOrDefault();
        }

        public Student LoadByOfficeWithAcademicIDNumber(InternshipOffice office, string AcademicIDNumber)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Include(x => x.Academic);

            criteria.Expression = criteria.Expression.Where(x => x.AcademicIDNumber, AcademicIDNumber);
            criteria.Expression = criteria.Expression.And(GetAcademicsExpression(office));
            criteria.UsePaging = false;

            int count;
            return FindWithCriteria(criteria, out count).FirstOrDefault();
        }

        public Student LoadByOfficeWithPrincipalName(InternshipOffice office, string PrincipalName)
        {
            Criteria<Student> criteria = new Criteria<Student>();
            criteria.Include(x => x.Academic);

            criteria.Expression = criteria.Expression.Where(x => x.UsernameFromLDAP, PrincipalName);
            criteria.UsePaging = false;

            int count;
            return FindWithCriteria(criteria, out count).FirstOrDefault();
        }

        private Imis.Domain.EF.Search.Criteria<Student> GetAcademicsExpression(InternshipOffice office)
        {
            var orExpression = Imis.Domain.EF.Search.Criteria<Student>.Empty;
            if (office.CanViewAllAcademics.GetValueOrDefault())
            {
                var officeAcademics = string.Join(",", StudentPracticeCacheManager<Academic>.Current.GetItems().Where(x => x.InstitutionID == office.InstitutionID.Value).Select(x => x.ID));

                orExpression = orExpression.Where(string.Format("it.AcademicID IN MULTISET ({0})", officeAcademics));
                orExpression = orExpression.Or(string.Format("it.PreviousAcademicID IN MULTISET ({0})", officeAcademics));
                return orExpression;
            }

            else
            {
                var officeAcademics = string.Join(",", office.Academics.Select(x => x.ID));

                orExpression = orExpression.Where(string.Format("it.AcademicID IN MULTISET ({0})", officeAcademics));
                orExpression = orExpression.Or(string.Format("it.PreviousAcademicID IN MULTISET ({0})", officeAcademics));
                return orExpression;
            }
        }

        #endregion
    }
}