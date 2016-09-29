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
    public partial class FinishedPositionDetails : BaseEntityPortalPage<InternshipOffice>
    {
        private InternshipPositionGroup CurrentGroup;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();

            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.Provider, x => x.Positions);
                if (CurrentGroup == null || CurrentGroup.PositionCreationType != enPositionCreationType.FromOffice)
                    Response.Redirect("FinishedPositions.aspx");
            }
            else
            {
                Response.Redirect("FinishedPositions.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || Entity.VerificationStatus != enVerificationStatus.Verified)
                Response.Redirect("Default.aspx");

            btnPositionProviders.HRef = string.Format("FinishedPositionProvider.aspx?gID={0}", CurrentGroup.ID);

            if (!IsPostBack)
            {
                if (CurrentGroup != null)
                {
                    ucPositionGroupInput.Entity = CurrentGroup;
                    ucPositionGroupInput.CountryID = CurrentGroup.Provider.CountryID.GetValueOrDefault();
                    ucPositionGroupInput.Bind();
                }
            }
        }

        public void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FinishedPositionProvider.aspx?gID={0}", CurrentGroup.ID));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucPositionGroupInput.Fill(CurrentGroup);
            CurrentGroup.Positions.First().UpdatedAt = DateTime.Now;
            CurrentGroup.Positions.First().UpdatedBy = Page.User.Identity.Name;
            UnitOfWork.Commit();

            Response.Redirect(string.Format("FinishedPositionPhysicalObject.aspx?gID={0}", CurrentGroup.ID));
        }
    }
}