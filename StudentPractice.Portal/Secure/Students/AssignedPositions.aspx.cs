using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using DevExpress.Web.ASPxGridView;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class AssignedPositions : BaseEntityPortalPage<Student>
    {
        private List<SubmittedQuestionnaire> _submittedQuestonnaires = new List<SubmittedQuestionnaire>();

        protected override void Fill()
        {
            Entity = new StudentRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Entity.IsContactInfoCompleted)
            {
                Response.Redirect("~/Secure/Students/ContactInfoDetails.aspx");
            }

            base.OnLoad(e);
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositions.DataBind();
        }

        #region [ Grid Methods ]

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.PreAssignedForAcademic)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.InternshipPositionGroup.Academics);

            criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudentID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.PositionStatus, enPositionStatus.Canceled, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            e.InputParameters["criteria"] = criteria;
        }

        protected void odsPositions_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var items = e.ReturnValue as IEnumerable<InternshipPosition>;

            if (items == null)
                return;

            var repo = new SubmittedQuestionnaireRepository(UnitOfWork);
            _submittedQuestonnaires = repo.FindByTypeReporterIDAndPositionIDs(enQuestionnaireType.StudentForOffice, Entity.ID, items.Select(x => (int?)x.ID));
            _submittedQuestonnaires.AddRange(repo.FindByTypeReporterIDAndPositionIDs(enQuestionnaireType.StudentForProvider, Entity.ID, items.Select(x => (int?)x.ID)));
        }

        protected void gvPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data)
                return;

            InternshipPosition position = (InternshipPosition)gvPositions.GetRow(e.VisibleIndex);

            if (position != null)
            {
                switch (position.PositionStatus)
                {
                    case enPositionStatus.Assigned:
                        e.Row.BackColor = Color.LightPink;
                        break;
                    case enPositionStatus.UnderImplementation:
                        e.Row.BackColor = Color.Yellow;
                        break;
                    case enPositionStatus.Completed:
                        e.Row.BackColor = Color.LightGreen;
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.LightBlue;
                        break;
                    case enPositionStatus.Canceled:
                        e.Row.BackColor = Color.Tomato;
                        if (position.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
                            e.Row.BackColor = Color.Purple;
                        break;
                }
            }
        }

        protected void gvPositions_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var groupID = int.Parse(parameters[1]);
        }

        #endregion

        #region [ Helpder Methods ]

        protected string GetProviderDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            string providerDetails = string.Empty;

            if (!string.IsNullOrEmpty(position.InternshipPositionGroup.Provider.TradeName))
            {
                providerDetails = string.Format("<b>{0}</b> <br/>{1} <br/>{2}", position.InternshipPositionGroup.Provider.Name, position.InternshipPositionGroup.Provider.TradeName, position.InternshipPositionGroup.Provider.AFM);
            }
            else
            {
                providerDetails = string.Format("<b>{0}</b> <br/>{1}", position.InternshipPositionGroup.Provider.Name, position.InternshipPositionGroup.Provider.AFM);
            }

            return providerDetails;
        }

        protected string GetPositionStatus(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else if (ip.PositionStatus == enPositionStatus.Canceled && ip.CancellationReason > enCancellationReason.FromOffice)
                return "Αποσυρμένη";
            else
                return ip.PositionStatus.GetLabel();
        }

        protected string GetImplementationInfo(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return string.Join("<br />εώς<br />",
                    ip.ImplementationStartDate.Value.ToString("dd/MM/yyyy"),
                    ip.ImplementationEndDate.Value.ToString("dd/MM/yyyy"));
        }

        protected string GetCompletedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.CompletedAt.HasValue)
                return string.Empty;
            else
                return ip.CompletedAt.Value.ToString("dd/MM/yyyy");
        }

        protected bool CanEvaluateOffice(InternshipPosition ip)
        {
            if (ip == null)
                return false;


            return !_submittedQuestonnaires
                .Any(x => CacheManager.Questionnaires.Get(x.QuestionnaireID).QuestionnaireType == enQuestionnaireType.StudentForOffice && x.ReporterID == Entity.ID
                    && x.EntityID == ip.PreAssignedByMasterAccountID && x.PositionID == ip.ID);
        }

        protected bool CanEvaluateProvider(InternshipPosition ip)
        {
            if (ip == null)
                return false;


            return !_submittedQuestonnaires
                .Any(x => CacheManager.Questionnaires.Get(x.QuestionnaireID).QuestionnaireType == enQuestionnaireType.StudentForProvider && x.ReporterID == Entity.ID
                    && x.EntityID == ip.InternshipPositionGroup.ProviderID && x.PositionID == ip.ID);
        }

        #endregion
    }
}
