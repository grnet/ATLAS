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
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class ChangeAssignedStudent : BaseEntityPortalPage<InternshipPosition>
    {
        private Student NewAssignedStudent;
        private InternshipOffice CurrentOffice;

        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.Provider,
                    x => x.InternshipPositionGroup.Academics,
                    x => x.AssignedToStudent);
            }

            int studentID;
            if (int.TryParse(Request.QueryString["sID"], out studentID) && studentID > 0)
            {
                NewAssignedStudent = new StudentRepository(UnitOfWork).FindActiveByID(studentID, x => x.Academic);
            }

            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);

            if (Entity == null || NewAssignedStudent == null)
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentOffice.Academics.Any(x => x.ID == NewAssignedStudent.AcademicID || x.ID == NewAssignedStudent.PreviousAcademicID))
                mvAccess.ActiveViewIndex = 0;

            if (!Page.IsPostBack)
            {
                ucStudentInput.FillReadOnlyFields(NewAssignedStudent);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                //Entity.AssignedToStudent.PositionCount--;
                //NewAssignedStudent.PositionCount++;

                ucStudentInput.Fill(NewAssignedStudent);
                Entity.AssignedToStudentID = NewAssignedStudent.ID;
                Entity.InternshipPositionGroup.Academics.Clear();
                Entity.InternshipPositionGroup.Academics.Add(NewAssignedStudent.Academic);
            }

            else
            {
                Entity.AssignedToStudent.IsAssignedToPosition = false;
                //Entity.AssignedToStudent.PositionCount--;

                ucStudentInput.Fill(NewAssignedStudent);

                NewAssignedStudent.IsAssignedToPosition = true;
                //NewAssignedStudent.PositionCount++;
                Entity.AssignedToStudentID = NewAssignedStudent.ID;

                InternshipPositionLog logEntry = new InternshipPositionLog();
                logEntry.InternshipPositionID = Entity.ID;
                logEntry.OldStatus = Entity.PositionStatus;
                logEntry.NewStatus = Entity.PositionStatus;
                logEntry.AssignedByOfficeID = CurrentOffice.ID;

                if (CurrentOffice.MasterAccountID.HasValue)
                    logEntry.AssignedByMasterAccountID = CurrentOffice.MasterAccountID.Value;
                else
                    logEntry.AssignedByMasterAccountID = CurrentOffice.ID;

                logEntry.AssignedToStudentID = NewAssignedStudent.ID;
                logEntry.CreatedAt = DateTime.Now;
                logEntry.CreatedAtDateOnly = DateTime.Now.Date;
                logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
                UnitOfWork.MarkAsNew(logEntry);

                var provider = Entity.InternshipPositionGroup.Provider;
                var academic = CacheManager.Academics.Get(NewAssignedStudent.AcademicID.Value);

                if (!string.IsNullOrWhiteSpace(NewAssignedStudent.ContactEmail))
                {
                    var emailToStudent = MailSender.SendAssignedPositionStudentNotification(NewAssignedStudent.ID, NewAssignedStudent.ContactEmail, string.Format("{0} {1}", NewAssignedStudent.GreekFirstName, NewAssignedStudent.GreekLastName), Entity.InternshipPositionGroup.ID, Entity.InternshipPositionGroup.Title, provider.Name);
                    UnitOfWork.MarkAsNew(emailToStudent);
                }

                if (Entity.ImplementationEndDate > DateTime.Today)
                {
                    if (!NewAssignedStudent.SMSSentCount.HasValue)
                        NewAssignedStudent.SMSSentCount = 0;

                    if (!string.IsNullOrWhiteSpace(NewAssignedStudent.ContactMobilePhone))
                    {
                        var smsToStudent = SMSSender.SendInternshipPositionAssignment(NewAssignedStudent.ID, NewAssignedStudent.GreekFirstName, NewAssignedStudent.GreekLastName, provider.Name, NewAssignedStudent.ContactMobilePhone);
                        UnitOfWork.MarkAsNew(smsToStudent);
                        NewAssignedStudent.SMSSentCount++;
                    }
                }

                if (!string.IsNullOrWhiteSpace(provider.ContactEmail))
                {
                    var emailToProvider = MailSender.SendAssignedPositionProviderNotification(provider.ID, provider.ContactEmail, provider.UserName, Entity.InternshipPositionGroup.ID, Entity.InternshipPositionGroup.Title, string.Format("{0} {1}", NewAssignedStudent.GreekFirstName, NewAssignedStudent.GreekLastName), academic.Institution, academic.School ?? "-", academic.Department, NewAssignedStudent.StudentNumber, provider.Language.GetValueOrDefault());
                    UnitOfWork.MarkAsNew(emailToProvider);
                }
            }
            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}