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
    public partial class FinishedPositionStudent : BaseEntityPortalPage<InternshipOffice>
    {
        private InternshipPosition CurrentPosition;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();

            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                CurrentPosition = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.PhysicalObjects,
                    x => x.InternshipPositionGroup.Academics,
                    x => x.InternshipPositionGroup.Provider,
                    x => x.AssignedToStudent);

                if (CurrentPosition == null || CurrentPosition.InternshipPositionGroup.PositionCreationType != enPositionCreationType.FromOffice)
                    Response.Redirect("FinishedPositions.aspx");

                CurrentPosition.SaveToCurrentContext();
            }
            else
            {
                Response.Redirect("FinishedPositions.aspx");
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ucAssignStudent.Entity = Entity;
            ucAssignStudent.UnitOfWork = UnitOfWork;
            ucAssignStudent.CurrentPosition = CurrentPosition;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified || Entity.VerificationStatus != enVerificationStatus.Verified)
                Response.Redirect("Default.aspx");

            btnPositionProviders.HRef = string.Format("FinishedPositionProvider.aspx?gID={0}", CurrentPosition.GroupID);
            btnPositionDetails.HRef = string.Format("FinishedPositionDetails.aspx?gID={0}", CurrentPosition.GroupID);
            btnPositionPhysicalObject.HRef = string.Format("FinishedPositionPhysicalObject.aspx?gID={0}", CurrentPosition.GroupID);

            if (!Page.IsPostBack)
                ucAssignStudent.Bind();
        }

        protected void ucAssignStudent_StudentAssignmentComplete(object sender, EventArgs e)
        {
            if (CurrentPosition.AssignedToStudentID.HasValue)
            {
                Response.Redirect(string.Format("FinishedPositionStudent.aspx?pID={0}", CurrentPosition.ID));
            }
            else
            {
                ucAssignStudent.ReBindGrid();
            }
        }

        protected void ucAssignStudent_StudentAssignmentCanceled(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FinishedPositionStudent.aspx?pID={0}", CurrentPosition.ID));
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("FinishedPositionPhysicalObject.aspx?gID={0}", CurrentPosition.GroupID));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SavePosition();
        }

        protected void ucAssignStudent_PositionCompleted(object sender, EventArgs e)
        {
            SavePosition();
        }

        private void SavePosition()
        {
            if (!CurrentPosition.AssignedToStudentID.HasValue)
            {
                lblErrors.Visible = true;
                lblErrors.Text = "Πρέπει να έχετε επιλέξει φοιτητή  για να μπορέσετε να καταχωρίσετε την θέση πρακτικής άσκησης.";
                return;
            }
            CurrentPosition.InternshipPositionGroup.PositionGroupStatus = enPositionGroupStatus.Published;
            CurrentPosition.PositionStatus = enPositionStatus.Completed;
            UnitOfWork.Commit();

            Response.Redirect("SelectedPositions.aspx");
        }
    }
}