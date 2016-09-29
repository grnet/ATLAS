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
    public partial class AddImplementationData : BaseEntityPortalPage<InternshipPosition>
    {
        private Student AssignedStudent;
        private InternshipOffice CurrentOffice;
        private int AcademicID;

        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.Provider,
                    x => x.PreAssignedForAcademic);
            }

            int studentID;
            if (int.TryParse(Request.QueryString["sID"], out studentID) && studentID > 0)
            {
                AssignedStudent = new StudentRepository(UnitOfWork).Load(studentID, x => x.Academic);
                AssignedStudent.SaveToCurrentContext();
            }

            int.TryParse(Request.QueryString["aID"], out AcademicID);

            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            CurrentOffice.SaveToCurrentContext();

            if (Entity == null || AssignedStudent == null)
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CurrentOffice.Academics.Any(x => x.ID == AssignedStudent.AcademicID || x.ID == AssignedStudent.PreviousAcademicID))
                mvAccess.ActiveViewIndex = 0;

            if (Entity != null)
            {
                ucImplementationInput.Entity = Entity;
                ucImplementationInput.AcademicID = AcademicID;
            }

            if (!Page.IsPostBack)
                ucImplementationInput.FillReadOnlyFields(AssignedStudent);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            Entity.AssignedToStudent = AssignedStudent;
            ucImplementationInput.Fill(Entity);

            if (Entity.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                Entity.PreAssignedAt = DateTime.Now;
                Entity.AssignedAt = DateTime.Now;
                Entity.PreAssignedByOfficeID = CurrentOffice.ID;
                Entity.DaysLeftForAssignment = 0;
                Entity.CompletedAt = Entity.ImplementationEndDate;

                if (CurrentOffice.MasterAccountID.HasValue)
                    Entity.PreAssignedByMasterAccountID = CurrentOffice.MasterAccountID.Value;
                else
                    Entity.PreAssignedByMasterAccountID = CurrentOffice.ID;
            }
            else
            {
                InternshipPositionTriggersParams triggersParams = new InternshipPositionTriggersParams();
                triggersParams.OfficeID = CurrentOffice.ID;

                if (CurrentOffice.MasterAccountID.HasValue)
                {
                    triggersParams.MasterAccountID = CurrentOffice.MasterAccountID.Value;
                }
                else
                {
                    triggersParams.MasterAccountID = CurrentOffice.ID;
                }

                triggersParams.Username = Thread.CurrentPrincipal.Identity.Name;
                triggersParams.ExecutionDate = DateTime.Now;
                triggersParams.UnitOfWork = UnitOfWork;

                var stateMachine = new InternshipPositionStateMachine(Entity);

                if (Entity.ImplementationStartDate <= DateTime.Now.Date)
                {
                    stateMachine.AssignAndBeginImplementation(triggersParams);
                }
                else
                {
                    stateMachine.Assign(triggersParams);
                }

                AssignedStudent.IsAssignedToPosition = true;

                var provider = Entity.InternshipPositionGroup.Provider;
                var academic = CacheManager.Academics.Get(AssignedStudent.AcademicID.Value);

                if (!string.IsNullOrWhiteSpace(AssignedStudent.ContactEmail))
                {
                    var emailToStudent = MailSender.SendAssignedPositionStudentNotification(AssignedStudent.ID, AssignedStudent.ContactEmail, string.Format("{0} {1}", AssignedStudent.GreekFirstName, AssignedStudent.GreekLastName), Entity.InternshipPositionGroup.ID, Entity.InternshipPositionGroup.Title, provider.Name);
                    UnitOfWork.MarkAsNew(emailToStudent);
                }

                if (Entity.ImplementationEndDate > DateTime.Today)
                {
                    if (!AssignedStudent.SMSSentCount.HasValue)
                        AssignedStudent.SMSSentCount = 0;

                    if (!string.IsNullOrWhiteSpace(AssignedStudent.ContactMobilePhone))
                    {
                        var smsToStudent = SMSSender.SendInternshipPositionAssignment(AssignedStudent.ID, AssignedStudent.GreekFirstName, AssignedStudent.GreekLastName, provider.Name, AssignedStudent.ContactMobilePhone);
                        UnitOfWork.MarkAsNew(smsToStudent);
                        AssignedStudent.SMSSentCount++;
                    }
                }

                if (!string.IsNullOrWhiteSpace(provider.ContactEmail))
                {
                    var emailToProvider = MailSender.SendAssignedPositionProviderNotification(provider.ID, provider.ContactEmail, provider.UserName, Entity.InternshipPositionGroup.ID, Entity.InternshipPositionGroup.Title, string.Format("{0} {1}", AssignedStudent.GreekFirstName, AssignedStudent.GreekLastName), academic.Institution, academic.School ?? "-", academic.Department, AssignedStudent.StudentNumber, provider.Language.GetValueOrDefault());
                    UnitOfWork.MarkAsNew(emailToProvider);
                }
            }
            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}