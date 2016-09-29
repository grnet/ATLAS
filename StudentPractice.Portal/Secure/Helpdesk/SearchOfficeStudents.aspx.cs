using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchOfficeStudents : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtStudentName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtStudentLastName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtStudentNumber.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtStudentID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                txtOfficeID.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));

                int officeID = 0;
                if (int.TryParse(Request.QueryString["oID"], out officeID) && officeID > 0)
                    txtOfficeID.Text = officeID.ToString();
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            ucHandledStudentsGridView.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            ucHandledStudentsGridView.PageIndex = 0;
            ucHandledStudentsGridView.DataBind();
        }

        protected void ddlPositionStatus_Init(object sender, EventArgs e)
        {
            ddlPositionStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));
            ddlPositionStatus.Items.Add(new ListItem() { Text = enPositionStatus.Assigned.GetLabel(), Value = ((int)enPositionStatus.Assigned).ToString() });
            ddlPositionStatus.Items.Add(new ListItem() { Text = enPositionStatus.UnderImplementation.GetLabel(), Value = ((int)enPositionStatus.UnderImplementation).ToString() });
            ddlPositionStatus.Items.Add(new ListItem() { Text = enPositionStatus.Completed.GetLabel(), Value = ((int)enPositionStatus.Completed).ToString() });
        }

        protected void odsPositions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipPosition> criteria = new Criteria<InternshipPosition>();

            criteria.Include(x => x.AssignedToStudent)
                .Include(x => x.AssignedToStudent)
                .Include(x => x.PreAssignedByMasterAccount)
                .Include(x => x.PreAssignedByMasterAccount.Academics);

            int positionstatus;
            if (int.TryParse(ddlPositionStatus.SelectedItem.Value, out positionstatus) && positionstatus >= 0)
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, positionstatus);
            else
            {
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Assigned, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
                criteria.Expression = criteria.Expression.Where(x => x.PositionStatusInt, (int)enPositionStatus.Completed, Imis.Domain.EF.Search.enCriteriaOperator.LessThanEquals);
            }

            if (!string.IsNullOrEmpty(txtStudentName.Text))
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.OriginalFirstName, txtStudentName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            if (!string.IsNullOrEmpty(txtStudentLastName.Text))
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.OriginalLastName, txtStudentLastName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
            if (!string.IsNullOrEmpty(txtStudentNumber.Text))
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudent.StudentNumber, txtStudentNumber.Text.ToNull());

            int studentID;
            if (int.TryParse(txtStudentID.Text, out studentID) && studentID > 0)
                criteria.Expression = criteria.Expression.Where(x => x.AssignedToStudentID, studentID);

            int officeID;
            if (int.TryParse(txtOfficeID.Text, out officeID) && officeID > 0)
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedByMasterAccountID, officeID);

            e.InputParameters["criteria"] = criteria;
        }

    }
}