using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Utils;
using StudentPractice.BusinessModel.Flow;
using System.Threading;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class EditImplementationData : BaseEntityPortalPage<InternshipPosition>
    {
        private Student AssignedStudent;
        private InternshipOffice CurrentOffice;

        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionRepository(UnitOfWork).Load(positionID, x => x.InternshipPositionGroup);
            }

            int studentID;
            if (int.TryParse(Request.QueryString["sID"], out studentID) && studentID > 0)
            {
                AssignedStudent = new StudentRepository(UnitOfWork).Load(studentID);
                AssignedStudent.SaveToCurrentContext();
            }

            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            CurrentOffice.SaveToCurrentContext();

            if (Entity == null || AssignedStudent == null
                || !CurrentOffice.Academics.Any(x => x.ID == AssignedStudent.AcademicID || x.ID == AssignedStudent.PreviousAcademicID))
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity != null)
                ucImplementationInput.Entity = Entity;
            if (!Page.IsPostBack)
            {
                ucImplementationInput.FillReadOnlyFields(AssignedStudent);
                ucImplementationInput.Bind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            DateTime oldImplementationStartDate = Entity.ImplementationStartDate.Value;
            DateTime oldImplementationEndDate = Entity.ImplementationEndDate.Value;
            enFundingType oldFundingType = Entity.FundingType;

            ucImplementationInput.Fill(Entity);

            if (Entity.InternshipPositionGroup.PositionCreationType != enPositionCreationType.FromOffice)
            {
                if (oldImplementationStartDate != Entity.ImplementationStartDate.Value ||
                    oldImplementationEndDate != Entity.ImplementationEndDate.Value ||
                    oldFundingType != Entity.FundingType)
                {

                    enPositionStatus newStatus;

                    if (Entity.PositionStatus == enPositionStatus.Assigned && Entity.ImplementationStartDate.Value <= DateTime.Now.Date)
                    {
                        newStatus = enPositionStatus.UnderImplementation;
                    }
                    else if (Entity.PositionStatus == enPositionStatus.UnderImplementation && Entity.ImplementationStartDate.Value > DateTime.Now.Date)
                    {
                        newStatus = enPositionStatus.Assigned;
                    }
                    else
                    {
                        newStatus = Entity.PositionStatus;
                    }

                    InternshipPositionLog logEntry = new InternshipPositionLog();
                    logEntry.InternshipPositionID = Entity.ID;
                    logEntry.OldStatus = Entity.PositionStatus;
                    logEntry.NewStatus = newStatus;

                    logEntry.AssignedByOfficeID = CurrentOffice.ID;

                    if (CurrentOffice.MasterAccountID.HasValue)
                    {
                        logEntry.AssignedByMasterAccountID = CurrentOffice.MasterAccountID.Value;
                    }
                    else
                    {
                        logEntry.AssignedByMasterAccountID = CurrentOffice.ID;
                    }

                    if (oldImplementationStartDate != Entity.ImplementationStartDate.Value)
                    {
                        logEntry.ImplementationStartDate = Entity.ImplementationStartDate.Value;
                    }

                    if (oldImplementationEndDate != Entity.ImplementationEndDate.Value)
                    {
                        logEntry.ImplementationEndDate = Entity.ImplementationEndDate.Value;
                    }

                    if (oldFundingType != Entity.FundingType)
                    {
                        logEntry.FundingType = Entity.FundingType;
                    }

                    logEntry.CreatedAt = DateTime.Now;
                    logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                    logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                    UnitOfWork.MarkAsNew(logEntry);

                    Entity.PositionStatus = newStatus;
                }
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}