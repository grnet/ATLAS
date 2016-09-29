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
    public partial class FinishedPositionProvider : BaseEntityPortalPage<InternshipOffice>
    {
        private InternshipPositionGroup CurrentGroup;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                CurrentGroup = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.Positions);
                if (CurrentGroup == null || CurrentGroup.PositionCreationType != enPositionCreationType.FromOffice)
                    Response.Redirect("FinishedPositions.aspx");
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ucInternshipPositionProviderSelect.Entity = CurrentGroup;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || Entity.VerificationStatus != enVerificationStatus.Verified)
                Response.Redirect("Default.aspx");

            if (!Page.IsPostBack)
                ucInternshipPositionProviderSelect.Bind();
        }

        public void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SelectedPositions.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            InternshipPosition position;
            if (CurrentGroup == null)
            {
                CurrentGroup = new InternshipPositionGroup();
                CurrentGroup.ProviderID = Entity.ID;
                CurrentGroup.AvailablePositions = 0;
                CurrentGroup.PreAssignedPositions = 1;
                CurrentGroup.TotalPositions = 1;
                CurrentGroup.NoTimeLimit = true;
                CurrentGroup.IsVisibleToAllAcademics = false;
                CurrentGroup.PositionGroupStatus = enPositionGroupStatus.UnPublished;
                CurrentGroup.PositionCreationType = enPositionCreationType.FromOffice;
                UnitOfWork.MarkAsNew(CurrentGroup);

                CurrentGroup.Title = "";
                CurrentGroup.CountryID = StudentPracticeConstants.GreeceCountryID;

                position = new InternshipPosition();
                position.InternshipPositionGroup = CurrentGroup;
                position.PreAssignedByMasterAccountID = Entity.IsMasterAccount ? Entity.ID : Entity.MasterAccountID;
                position.PreAssignedByOfficeID = Entity.ID;
            }
            else
                position = CurrentGroup.Positions.First();

            position.UpdatedAt = DateTime.Now;
            position.UpdatedBy = Page.User.Identity.Name;
            ucInternshipPositionProviderSelect.Fill(CurrentGroup);



            if (CurrentGroup == null || CurrentGroup.ProviderID == 0)
            {
                lblErrors.Visible = true;
                lblErrors.Text = "Πρέπει να έχετε επιλέξει Φορέα Υποδοχής για την θέση πρακτικής άσκησης για να μπορέσετε να συνεχίσετε.";
                return;
            }

            InternshipPositionGroupLog log = new InternshipPositionGroupLog();
            log.InternshipPositionGroup = CurrentGroup;
            log.OldStatus = CurrentGroup.PositionGroupStatus;
            log.NewStatus = CurrentGroup.PositionGroupStatus;
            log.CreatedAt = DateTime.Now;
            log.CreatedAtDateOnly = DateTime.Now.Date;
            log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
            log.UpdatedAt = DateTime.Now;
            log.UpdatedAtDateOnly = DateTime.Now.Date;
            log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

            UnitOfWork.Commit();

            Response.Redirect(string.Format("FinishedPositionDetails.aspx?gID={0}", CurrentGroup.ID));
        }
    }
}