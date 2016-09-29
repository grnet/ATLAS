using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class PositionPreview : BaseEntityPortalPage<InternshipProvider>
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
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.PhysicalObjects, x => x.Academics);

                if (CurrentGroup == null)
                    Response.Redirect("InternshipPositions.aspx");
            }
            else
            {
                Response.Redirect("InternshipPositions.aspx");
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || !Config.InternshipPositionCreationAllowed)
            {
                Response.Redirect("Default.aspx");
            }

            btnPositionDetails.HRef = string.Format("PositionDetails.aspx?gID={0}", CurrentGroup.ID);
            btnPositionPhysicalObject.HRef = string.Format("PositionPhysicalObject.aspx?gID={0}", CurrentGroup.ID);
            btnPositionAcademics.HRef = string.Format("PositionAcademics.aspx?gID={0}", CurrentGroup.ID);

            ucPositionGroupView.Entity = CurrentGroup;
            ucPositionGroupView.Bind();
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("PositionAcademics.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (CurrentGroup.PositionGroupStatus != enPositionGroupStatus.Published)
                Session["flash"] = Resources.PositionPages.PositionPreview_FlashMessage;

            Response.Redirect("InternshipPositions.aspx");
        }
    }
}