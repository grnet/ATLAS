using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls
{
    public partial class ImplementationView : BaseEntityUserControl<InternshipPosition>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            if (Entity.PositionStatusInt > (int)enPositionStatus.UnderImplementation)
            {
                trPositionStatus.Visible = false;
            }
            else
            {
                lblPositionStatus.Text = Entity.PositionStatus.GetLabel();
            }

            lblAssignedAt.Text = Entity.AssignedAt.HasValue ? Entity.AssignedAt.Value.ToString("dd/MM/yyyy") : string.Empty;
            lblImplementationStartDate.Text = Entity.ImplementationStartDate.HasValue ? Entity.ImplementationStartDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            lblImplementationEndDate.Text = Entity.ImplementationEndDate.HasValue ? Entity.ImplementationEndDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            //ToFix: FundingType
            //lblFundingType.Text = Entity.FundingType.GetLabel();

            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice && Entity.PreAssignedForAcademicID.HasValue)
            {
                rowPreAssignedAcademic.Visible = true;
                var academic = CacheManager.Academics.Get(Entity.PreAssignedForAcademicID.Value);
                lblPreAssignAcademic.Text = string.Format("{0} - {1}", academic.Institution, academic.Department);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}