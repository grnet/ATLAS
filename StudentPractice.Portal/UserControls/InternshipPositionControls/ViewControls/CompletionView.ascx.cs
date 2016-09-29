using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Drawing;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls
{
    public partial class CompletionView : BaseEntityUserControl<InternshipPosition>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            if (Entity.PositionStatus == enPositionStatus.Completed)
            {
                lblCompletionVerdict.Text = Resources.PositionGroupInput.PositionCompleted;
            }
            else
            {
                lblCompletionVerdict.ForeColor = Color.Red;
                lblCompletionVerdict.Text = Resources.PositionGroupInput.PositionNotCompleted;
            }

            if (Entity.PositionStatus == enPositionStatus.Completed)
            {
                trImplementationStartDate.Visible = true;
                trImplementationEndDate.Visible = true;
                //ToFix: FundingType
                //trFundingType.Visible = true;

                lblImplementationStartDate.Text = Entity.ImplementationStartDate.Value.ToString("dd/MM/yyyy");
                lblImplementationEndDate.Text = Entity.ImplementationEndDate.Value.ToString("dd/MM/yyyy");
                //ToFix: FundingType
                //lblFundingType.Text = Entity.FundingType.GetLabel();

                if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice && Entity.PreAssignedForAcademicID.HasValue)
                {
                    trPreAssignedAcademic.Visible = true;
                    var academic = CacheManager.Academics.Get(Entity.PreAssignedForAcademicID.Value);
                    lblPreAssignAcademic.Text = string.Format("{0} - {1}", academic.Institution, academic.Department);
                }
            }

            if (!string.IsNullOrWhiteSpace(Entity.CompletionComments))
            {
                trCompletionComments.Visible = true;
                txtCompletionComments.Text = Entity.CompletionComments;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}