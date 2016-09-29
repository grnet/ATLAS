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
    public partial class CompletePosition : BaseEntityPortalPage<InternshipPosition>
    {
        private InternshipOffice CurrentOffice;

        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionRepository(UnitOfWork).Load(positionID,
                    x => x.InternshipPositionGroup,
                    x => x.InternshipPositionGroup.Provider,
                    x => x.AssignedToStudent);
            }

            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);

            if (Entity == null || CurrentOffice == null)
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucCompletionInput.Entity = Entity;
                ucCompletionInput.Bind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucCompletionInput.Fill(Entity);

            if (ucCompletionInput.CompletionVerdict == (int)enPositionStatus.Canceled && string.IsNullOrWhiteSpace(Entity.CompletionComments))
            {
                ClientScript.RegisterStartupScript(GetType(), "alert_nocompletedcomments", string.Format("alert('{0}')", "Θα πρέπει να εισάγετε παρατηρήσεις σχετικά με το λόγο της μη ολοκλήρωσης της Πρακτικής Άσκησης"), true);
                return;
            }

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
            triggersParams.Comment = Entity.CompletionComments;

            if (ucCompletionInput.CompletionVerdict == (int)enPositionStatus.Completed)
            {
                var stateMachine = new InternshipPositionStateMachine(Entity);
                stateMachine.CompleteImplementation(triggersParams);

                if (!string.IsNullOrEmpty(Entity.AssignedToStudent.ContactEmail))
                {
                    var emailToStudent = MailSender.SendCompletedPositionStudentNotification(Entity.AssignedToStudent.ID, Entity.AssignedToStudent.ContactEmail, string.Format("{0} {1}", Entity.AssignedToStudent.GreekFirstName, Entity.AssignedToStudent.GreekLastName), Entity.InternshipPositionGroup.ID, Entity.InternshipPositionGroup.Title, Entity.InternshipPositionGroup.Provider.Name);
                    UnitOfWork.MarkAsNew(emailToStudent);
                }

                UnitOfWork.Commit();
            }
            else if (ucCompletionInput.CompletionVerdict == (int)enPositionStatus.Canceled)
            {
                triggersParams.CancellationReason = enCancellationReason.FromOffice;

                var stateMachine = new InternshipPositionStateMachine(Entity);
                stateMachine.Cancel(triggersParams);
                UnitOfWork.Commit();
            }
            else
            {
                lblValidationErrors.Text = "Παρουσιάστηκε κάποιο λάθος. Παρακαλούμε δοκιμάστε ξανά ή επικοινωνήστε με το Γραφείο Αρωγής Χρηστών για να το αναφέρετε.";
                return;
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}