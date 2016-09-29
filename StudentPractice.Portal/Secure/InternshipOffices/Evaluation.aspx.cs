using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class Evaluation : BaseEntityPortalPage<InternshipOffice>
    {
        private List<SubmittedQuestionnaire> _submittedQuestonnaires = new List<SubmittedQuestionnaire>();

        private List<int> _providerIDs = new List<int>();
        protected override void Fill()
        {
            using (UnitOfWork.SingleConnection())
            {
                var repo = new InternshipOfficeRepository(UnitOfWork);

                Entity = repo.FindByUsername(Thread.CurrentPrincipal.Identity.Name, x => x.Academics);
                _providerIDs = repo.GetParticipatingProvidersIDs(Entity);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να προβείτε σε αξιολόγηση συνεργαζόμενων Γραφείων Πρακτικής Άσκησης ή Φοιτητών, γιατί δεν έχετε ενεργοποιήσει το e-mail σας.";
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = "Δεν μπορείτε να προβείτε σε αξιολόγηση συνεργαζόμενων Γραφείων Πρακτικής Άσκησης ή Φοιτητών, γιατί δεν έχει πιστοποιηθεί ο λογαριασμός σας.<br/>Παρακαλούμε εκτυπώστε τη Βεβαίωση Συμμετοχής και αποστείλτε τη με ΦΑΞ στο Γραφείο Αρωγής για να πιστοποιηθεί.";
            }
            else
            {
                mvAccount.SetActiveView(vAccountVerified);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvProviders.PageIndex = 0;
            gvProviders.DataBind();
        }

        protected void btnSearchStudents_Click(object sender, EventArgs e)
        {
            gvPositions.PageIndex = 0;
            gvPositions.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvProviders.DataBind();
            gvPositions.DataBind();
        }

        protected void odsProviders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            if (_providerIDs.Count != 0)
            {
                criteria.Expression = criteria.Expression.InMultiSet(x => x.ID, _providerIDs);
            }
            else
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, 0);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AFM, txtProviderAFM.Text.ToNull());
            }

            if (!string.IsNullOrEmpty(txtProviderName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.Name, txtProviderName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void odsProviders_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var items = e.ReturnValue as IEnumerable<InternshipProvider>;

            if (items == null)
                return;

            _submittedQuestonnaires = new SubmittedQuestionnaireRepository(UnitOfWork).FindByTypeReporterIDAndEntityIDs(enQuestionnaireType.OfficeForProvider, Entity.ID, items.Select(x => (int?)x.ID));
        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.CanceledStudent);

            criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.CanceledGroupCascade, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.FromHelpdesk, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            var orCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            var andCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            orCreationExpression = orCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromProvider);
            andCreationExpression = andCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice)
                                                         .And(x => x.PositionStatus, enPositionStatus.Canceled, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            orCreationExpression = orCreationExpression.Or(andCreationExpression);
            criteria.Expression = criteria.Expression.And(orCreationExpression);

            if (Entity.IsMasterAccount)
            {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedByMasterAccountID, Entity.ID);
            }
            else
            {
                Criteria<InternshipPosition> andCrit = new Criteria<InternshipPosition>();
                andCrit.Expression = andCrit.Expression.Where(x => x.PreAssignedByMasterAccountID, Entity.MasterAccountID);
                andCrit.Expression = andCrit.Expression.Where(string.Format("it.PreAssignedForAcademicID IN MULTISET ({0})", string.Join(",", Entity.Academics.Select(x => x.ID))));
                Criteria<InternshipPosition> orCrit = new Criteria<InternshipPosition>();
                orCrit.Expression = andCrit.Expression.Or(x => x.PreAssignedByOfficeID, Entity.ID);
                criteria.Expression = criteria.Expression.And(andCrit.Expression);
            }

            criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Completed);

            int positionID;
            if (int.TryParse(txtPositionID.Text.ToNull(), out positionID) && positionID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, positionID);
            }

            if (!string.IsNullOrEmpty(txtTitle.Text))
            {
                var orTitleExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;

                orTitleExpression = orTitleExpression.Where(x => x.InternshipPositionGroup.Title, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                orTitleExpression = orTitleExpression.Or(x => x.InternshipPositionGroup.Description, txtTitle.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                criteria.Expression = criteria.Expression.And(orTitleExpression);
            }

            if (!string.IsNullOrEmpty(txtFirstName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.GreekFirstName, txtFirstName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtLastName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.GreekLastName, txtLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            }

            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.StudentNumber, txtStudentNumber.Text.ToNull());
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void odsPositions_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var items = e.ReturnValue as IEnumerable<InternshipPosition>;

            if (items == null)
                return;

            _submittedQuestonnaires = new SubmittedQuestionnaireRepository(UnitOfWork).FindByTypeReporterIDAndPositionIDs(enQuestionnaireType.OfficeForStudent, Entity.ID, items.Select(x => (int?)x.ID));

        }

        #region [ Helpers ]

        protected string GetProviderDetails(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            return string.Format("<b>{0}<br/>{1}</b><br/>ΑΦΜ: {2}",
                provider.Name,
                provider.ContactName,
                provider.AFM);
        }

        protected string GetProviderContactDetails(InternshipProvider provider)
        {
            if (provider == null)
                return string.Empty;

            return string.Format("Email: {0}<br/>Tηλ.: {1}",
                provider.ContactEmail,
                provider.ContactPhone);
        }

        protected bool CanEvaluateProvider(InternshipProvider provider)
        {
            if (provider == null)
                return false;

            return !_submittedQuestonnaires
                .Any(x => CacheManager.Questionnaires.Get(x.QuestionnaireID).QuestionnaireType == enQuestionnaireType.OfficeForProvider && x.ReporterID == Entity.ID
                    && x.EntityID == provider.ID);
        }

        protected string GetStudentDetails(InternshipPosition position)
        {
            if (position == null)
                return String.Empty;

            string studentDetails = String.Empty;

            Student student = position.AssignedToStudent;

            if (student != null)
            {
                studentDetails = String.Format("{0}<br/><br/><b>{1} {2}<br/>{3}&nbsp;&nbsp;-&nbsp;&nbsp;{4}</b>",
                    CacheManager.Academics.Get(position.PreAssignedForAcademicID.Value).Department,
                    student.GreekFirstName,
                    student.GreekLastName,
                    student.ContactEmail,
                    student.ContactMobilePhone);
            }
            else
            {
                studentDetails = String.Format("{0}", CacheManager.Academics.Get(position.PreAssignedForAcademicID.Value).Department);
            }

            return studentDetails;
        }

        protected bool CanEvaluateStudent(InternshipPosition position)
        {
            if (position == null)
                return false;

            return !_submittedQuestonnaires
                .Any(x => CacheManager.Questionnaires.Get(x.QuestionnaireID).QuestionnaireType == enQuestionnaireType.OfficeForStudent && x.ReporterID == Entity.ID
                    && x.EntityID == position.AssignedToStudentID && x.PositionID == position.ID);
        }

        #endregion
    }
}