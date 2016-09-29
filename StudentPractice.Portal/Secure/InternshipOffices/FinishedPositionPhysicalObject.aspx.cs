using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class FinishedPositionPhysicalObject : BaseEntityPortalPage<InternshipOffice>
    {   
        private InternshipPositionGroup CurrentGroup;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.PhysicalObjects, x => x.Positions);
                if (CurrentGroup == null || CurrentGroup.PositionCreationType != enPositionCreationType.FromOffice)
                    Response.Redirect("FinishedPositions.aspx");
            }
            else
            {
                Response.Redirect("FinishedPositions.aspx");
            }
        }
        
        protected void Page_Init(object sender, EventArgs e)
        {
            ucPhysicalObjectsInput.Entity = CurrentGroup;
            ucPhysicalObjectsInput.UnitOfWork = UnitOfWork;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || Entity.VerificationStatus != enVerificationStatus.Verified)
                Response.Redirect("Default.aspx");

            btnPositionProviders.HRef = string.Format("FinishedPositionProvider.aspx?gID={0}", CurrentGroup.ID);
            btnPositionDetails.HRef = string.Format("FinishedPositionDetails.aspx?gID={0}", CurrentGroup.ID);

            if (!Page.IsPostBack)
                ucPhysicalObjectsInput.Bind();
        }

        protected void ucPhysicalObjectsInput_Complete(object sender, EventArgs e)
        {
            CurrentGroup.Positions.First().UpdatedAt = DateTime.Now;
            CurrentGroup.Positions.First().UpdatedBy = Page.User.Identity.Name;

            Response.Redirect(string.Format("FinishedPositionStudent.aspx?pID={0}", CurrentGroup.Positions.First().ID));
        }

        protected void ucPhysicalObjectsInput_Cancel(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FinishedPositionDetails.aspx?gID={0}", CurrentGroup.ID));
        }
    }
}