using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using DevExpress.Web.ASPxGridView;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel.Flow;
using System.Threading;
using System.Text;
using StudentPractice.Portal.DataSources;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class StudentPositions : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
                mvAccount.SetActiveView(vAccountNotVerified);
            else
                mvAccount.SetActiveView(vAccountVerified);

            gvPositions.DataBind();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvPositions.DataBind();
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchStudents.aspx");
        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.InternshipPositionGroup)
                .Include(x => x.InternshipPositionGroup.Provider)
                .Include(x => x.InternshipPositionGroup.PhysicalObjects)
                .Include(x => x.PreAssignedForAcademic)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.CanceledStudent);

            criteria.Expression = criteria.Expression.Where(x => x.InternshipPositionGroup.PositionGroupStatus, enPositionGroupStatus.Deleted, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.CanceledGroupCascade, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            criteria.Expression = criteria.Expression.Where(x => x.CancellationReason, enCancellationReason.FromHelpdesk, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);

            var orCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            var andCreationExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            orCreationExpression = orCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromProvider);
            andCreationExpression = andCreationExpression.Where(x => x.InternshipPositionGroup.PositionCreationType, enPositionCreationType.FromOffice)
                                                         .And(x => x.PositionStatus, enPositionStatus.Canceled, Imis.Domain.EF.Search.enCriteriaOperator.NotEquals);
            orCreationExpression = orCreationExpression.Or(andCreationExpression);
            criteria.Expression = criteria.Expression.And(orCreationExpression);

            var orExpression = Imis.Domain.EF.Search.Criteria<InternshipPosition>.Empty;
            orExpression = orExpression.Where(string.Format("it.AssignedToStudent.AcademicID IN MULTISET ({0})", string.Join(",", Entity.Academics.Select(x => x.ID))));
            orExpression = orExpression.Or(string.Format("it.AssignedToStudent.PreviousAcademicID IN MULTISET ({0})", string.Join(",", Entity.Academics.Select(x => x.ID))));
            criteria.Expression = criteria.Expression.And(orExpression);

            int studentID;
            if (int.TryParse(Request.QueryString["sID"], out studentID) && studentID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudentID, studentID);
                e.InputParameters["criteria"] = criteria;
            }
        }

        protected void gvPositions_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            InternshipPosition position = gvPositions.GetRow(e.VisibleIndex) as InternshipPosition;

            if (position != null)
            {
                switch (position.PositionStatus)
                {
                    case enPositionStatus.UnPublished:
                        e.Row.BackColor = Color.LightGray;
                        break;
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
                        break;
                }
            }
        }

        #region [ Helper Methods Get]

        protected string GetProviderDetails(InternshipPosition position)
        {
            if (position == null)
                return string.Empty;

            return string.Format("{0}<br/>{1}<br/>{2}<br/>{3}", position.InternshipPositionGroup.Provider.Name, position.InternshipPositionGroup.Provider.ContactName, position.InternshipPositionGroup.Provider.ContactPhone, position.InternshipPositionGroup.Provider.ContactEmail);
        }

        protected string GetStudentDetails(InternshipPosition position)
        {
            if (position == null || !position.PreAssignedForAcademicID.HasValue)
                return string.Empty;

            Student student;
            if (position.PositionStatus == enPositionStatus.Canceled)
                student = position.CanceledStudent;
            else
                student = position.AssignedToStudent;

            if (student != null)
                return string.Format("{0}<br/><br/><b>{1} {2}<br/>{3}</b>", position.PreAssignedForAcademic.Department, student.GreekFirstName, student.GreekLastName, student.StudentNumber);
            else
                return string.Format("{0}", position.PreAssignedForAcademic.Department);
        }

        protected string GetPositionStatus(InternshipPosition ip)
        {
            if (ip == null)
                return string.Empty;
            else if (ip.PositionStatus == enPositionStatus.Canceled && ip.CancellationReason > enCancellationReason.FromOffice)
                return "Αποσυρμένη";
            else if (ip.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice && ip.PositionStatus == enPositionStatus.UnPublished)
                return "Ημιτελής Ολοκληρωμένη";
            else
                return ip.PositionStatus.GetLabel();
        }

        protected string GetPreAssignedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.PreAssignedAt.HasValue)
                return string.Empty;
            else
                return ip.PreAssignedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetAssignedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.AssignedAt.HasValue || ip.AssignedToStudent == null)
                return string.Empty;
            else
                return ip.AssignedAt.Value.ToString("dd/MM/yyyy");
        }

        protected string GetImplementationStartDate(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return ip.ImplementationStartDate.Value.ToString("dd/MM/yyyy");
        }

        protected string GetImplementationEndDate(InternshipPosition ip)
        {
            if (ip == null || !ip.ImplementationStartDate.HasValue || !ip.ImplementationEndDate.HasValue)
                return string.Empty;
            else
                return ip.ImplementationEndDate.Value.ToString("dd/MM/yyyy");
        }

        protected string GetCompletedAt(InternshipPosition ip)
        {
            if (ip == null || !ip.CompletedAt.HasValue)
                return string.Empty;
            else
                return ip.CompletedAt.Value.ToString("dd/MM/yyyy");
        }

        #endregion
    }
}
