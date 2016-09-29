using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class Evaluation : BaseEntityPortalPage<InternshipProvider>
    {
        private List<SubmittedQuestionnaire> _submittedQuestonnaires = new List<SubmittedQuestionnaire>();

        private List<int> _officeIDs = new List<int>();
        protected override void Fill()
        {
            using (UnitOfWork.SingleConnection())
            {
                var repo = new InternshipProviderRepository(UnitOfWork);

                Entity = repo.FindByUsername(Thread.CurrentPrincipal.Identity.Name);
                _officeIDs = repo.GetParticipatingOfficesIDs(Entity.ID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.Evaluation.NotEmailVerified;
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.Evaluation.NotVerified;
            }
            else
            {
                mvAccount.SetActiveView(vAccountVerified);
            }
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem(Resources.GlobalProvider.DropDownIndifferent, ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvOffices.PageIndex = 0;
            gvOffices.DataBind();
        }

        public void btnSearchStudents_Click(object sender, EventArgs e)
        {
            gvPositions.PageIndex = 0;
            gvPositions.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvOffices.DataBind();
            gvPositions.DataBind();
        }

        protected void odsOffices_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipOffice> criteria = new Criteria<InternshipOffice>();

            criteria.Include(x => x.Academics);

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);
            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, true);

            if (_officeIDs.Count != 0)
            {
                criteria.Expression = criteria.Expression.InMultiSet(x => x.ID, _officeIDs);
            }
            else
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, 0);
            }

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID >= 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.InstitutionID, institutionID);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void odsOffices_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var items = e.ReturnValue as IEnumerable<InternshipOffice>;

            if (items == null)
                return;

            _submittedQuestonnaires = new SubmittedQuestionnaireRepository(UnitOfWork).FindByTypeReporterIDAndEntityIDs(enQuestionnaireType.ProviderForOffice, Entity.ID, items.Select(x => (int?)x.ID));
        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.CanceledStudent);

            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.ProviderID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Completed);

            var orCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            var andCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            orCreationExpression = orCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromProvider);
            andCreationExpression = andCreationExpression
                                        .Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice)
                                        .And(x => x.PositionStatus, enPositionStatus.Completed)
                                        .And(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Published);
            orCreationExpression = orCreationExpression.Or(andCreationExpression);
            criteria.Expression = criteria.Expression.And(orCreationExpression);

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

            _submittedQuestonnaires = new SubmittedQuestionnaireRepository(UnitOfWork).FindByTypeReporterIDAndPositionIDs(enQuestionnaireType.ProviderForStudent, Entity.ID, items.Select(x => (int?)x.ID));

        }

        #region [ Helpers ]

        protected string GetOfficeDetails(InternshipOffice office)
        {
            if (office == null)
                return string.Empty;

            string officeDetails = string.Empty;

            var institution = CacheManager.Institutions.Get(office.InstitutionID.Value);

            switch (office.OfficeType)
            {
                case enOfficeType.None:
                    officeDetails = string.Format("{1}: {0}<br/>{2}: <span style='color: Red'>-</span>", institution.Name, Resources.Evaluation.Institution, Resources.Evaluation.Departments);
                    break;
                case enOfficeType.Institutional:
                    if (office.CanViewAllAcademics.GetValueOrDefault())
                        officeDetails = string.Format("{1}: {0}", institution.Name, Resources.Evaluation.Institution);
                    else
                        officeDetails = string.Format("{2}: {0}<br/>{3}: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={1}\",\"{4}\")'><img src='/_img/iconInformation.png' width='16px' alt='{3}' /></a>", institution.Name, office.ID, Resources.Evaluation.Institution, Resources.Evaluation.Departments, Resources.Evaluation.ShowDepartments);
                    break;
                case enOfficeType.Departmental:
                    var academic = office.Academics.ToList()[0];

                    officeDetails = string.Format("{1}: {0}<br/>{2}: {1}", institution.Name, academic.Department, Resources.Evaluation.Institution, Resources.Evaluation.Department);
                    break;
                case enOfficeType.MultipleDepartmental:
                    officeDetails = string.Format("{2}: {0}<br/>{3}: <a runat='server' href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={1}\",\"{4}\")'><img src='/_img/iconInformation.png' width='16px' alt='{3}' /></a>", institution.Name, office.ID, Resources.Evaluation.Institution, Resources.Evaluation.Departments, Resources.Evaluation.ShowDepartments);
                    break;
                default:
                    break;
            }

            return officeDetails;
        }

        protected bool CanEvaluateOffice(InternshipOffice office)
        {
            if (office == null)
                return false;

            return !_submittedQuestonnaires
                .Any(x => CacheManager.Questionnaires.Get(x.QuestionnaireID).QuestionnaireType == enQuestionnaireType.ProviderForOffice && x.ReporterID == Entity.ID
                    && x.EntityID == office.ID);
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
                .Any(x => CacheManager.Questionnaires.Get(x.QuestionnaireID).QuestionnaireType == enQuestionnaireType.ProviderForStudent && x.ReporterID == Entity.ID
                    && x.EntityID == position.AssignedToStudentID && x.PositionID == position.ID);
        }

        #endregion
    }
}