using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Imis.Domain;
using System.Threading;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class PositionPhysicalObject : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        private InternshipPositionGroup CurrentGroup;

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.PhysicalObjects);

                if (CurrentGroup == null)
                    Response.Redirect("InternshipPositions.aspx");
            }
            else
            {
                Response.Redirect("InternshipPositions.aspx");
            }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            ucPhysicalObjectsInput.Entity = CurrentGroup;
            ucPhysicalObjectsInput.UnitOfWork = UnitOfWork;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || !Config.InternshipPositionCreationAllowed)
            {
                Response.Redirect("Default.aspx");
            }

            if (CurrentGroup != null && CurrentGroup.PositionGroupStatus == enPositionGroupStatus.Published && CurrentGroup.AvailablePositions != CurrentGroup.TotalPositions)
            {
                Response.Redirect(string.Format("EditPreAssignedPosition.aspx?gID={0}", CurrentGroup.ID));
            }

            btnPositionDetails.HRef = string.Format("PositionDetails.aspx?gID={0}", CurrentGroup.ID);

            if (!Page.IsPostBack)
                ucPhysicalObjectsInput.Bind();
        }

        protected void ucPhysicalObjectsInput_Complete(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("PositionAcademics.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void ucPhysicalObjectsInput_Cancel(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("PositionDetails.aspx?gID={0}", CurrentGroup.ID));
        }
    }
}
