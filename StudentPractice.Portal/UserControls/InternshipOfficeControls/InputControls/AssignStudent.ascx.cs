using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Mails;
using StudentPractice.Portal.Controls;
using StudentPractice.Portal.UserControls.GenericControls;
using StudentPractice.Utils;

namespace StudentPractice.Portal.UserControls.InternshipOfficeControls.InputControls
{
    public partial class AssignStudent : BaseEntityUserControl<InternshipOffice>
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public InternshipPosition CurrentPosition { get; set; }

        public bool AllowAllOfficeAcademics { get; set; }

        public bool AllowInactiveAcademics { get; set; }

        public bool ShowCompletePositionButton
        {
            get { return btnFinishPosition.Visible; }
            set { btnFinishPosition.Visible = value; }
        }

        #region [ Events ]

        public event EventHandler StudentAssignmentComplete;

        public event EventHandler StudentAssignmentCanceled;

        public event EventHandler PositionCompleted;

        private void FireStudentAssignmentCompleteEvent()
        {
            if (StudentAssignmentComplete != null)
                StudentAssignmentComplete(this, EventArgs.Empty);
        }

        private void FireStudentAssignmentCanceledEvent()
        {
            if (StudentAssignmentCanceled != null)
                StudentAssignmentCanceled(this, EventArgs.Empty);
        }

        private void FirePositionCompletedEvent()
        {
            if (PositionCompleted != null)
                PositionCompleted(this, EventArgs.Empty);
        }

        #endregion

        #region [ DataBind ]

        public void ReBindGrid()
        {
            ucSearchAndRegisterStudent.ReBindGrid();
        }

        public override void Bind()
        {
            if (CurrentPosition.AssignedToStudentID.HasValue)
            {
                ucStudentView.Entity = CurrentPosition.AssignedToStudent;
                ucStudentView.Bind();

                ucImplementationView.Entity = CurrentPosition;
                ucImplementationView.Bind();

                btnChangeImplementationData.Attributes.Add("onclick", string.Format("popUp.show('EditImplementationData.aspx?sID={0}&pID={1}', 'Προσθήκη Στοιχείων Εκτέλεσης Πρακτικής Άσκησης',cmdRefresh);", CurrentPosition.AssignedToStudentID, CurrentPosition.ID));

                mvSelectStudent.SetActiveView(vStudentSelected);
            }
            else
            {
                mvAssignPosition.SetActiveView(vSelectStudent);
            }

            ucSearchAndRegisterStudent.UnitOfWork = UnitOfWork;
            ucSearchAndRegisterStudent.CurrentPosition = CurrentPosition;
            ucSearchAndRegisterStudent.Bind();

            ucPositionView.Entity = CurrentPosition;
            if (CurrentPosition.InternshipPositionGroup.PositionCreationType == enPositionCreationType.FromOffice)
            {
                ucPositionView.HideAcademics = true;
            }
            ucPositionView.UserAssociatedAcademics = Entity.Academics.ToList();
            ucPositionView.Bind();
        }

        #endregion

        #region [ Button events ]

        protected void btnFinishPosition_Click(object sender, EventArgs e)
        {
            FirePositionCompletedEvent();
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            FireStudentAssignmentCompleteEvent();
        }

        protected void btnSelectStudent_Click(object sender, EventArgs e)
        {
            mvAssignPosition.SetActiveView(vSelectStudent);
        }

        protected void btnChangeStudent_Click(object sender, EventArgs e)
        {
            mvAssignPosition.SetActiveView(vSelectStudent);
        }

        protected void btnReturnToSelectStudent_Click(object sender, EventArgs e)
        {
            mvAssignPosition.SetActiveView(vSelectStudent);
        }

        #endregion

        #region [ SearchAndRegister Student Events ]

        protected void ucSearchAndRegisterStudent_InitAcademics(object sender, InitAcademicsEventArgs e)
        {
            e.AllowAllAcademics = AllowAllOfficeAcademics;
            if (AllowAllOfficeAcademics)
            {
                if (Entity == null)
                    Entity = HttpContext.Current.LoadOffice();
                if (AllowInactiveAcademics)
                    e.Academics = Entity.Academics.ToList();
                else
                    e.Academics = Entity.Academics.Where(x => x.IsActive == true).ToList();
            }
            else
            {
                if (CurrentPosition == null)
                    CurrentPosition = HttpContext.Current.LoadPosition();

                e.Academics = new List<Academic>();
                e.Academics.Add(CacheManager.Academics.Get(CurrentPosition.PreAssignedForAcademicID.GetValueOrDefault()));
            }
        }

        protected void ucSearchAndRegisterStudent_StudentAssigned(object sender, StudentAssignedEventArgs e)
        {
            CurrentPosition.AssignedToStudent.IsAssignedToPosition = false;

            var newAssignedStudent = new StudentRepository(UnitOfWork).Load(e.StudentID);
            newAssignedStudent.IsAssignedToPosition = true;

            CurrentPosition.AssignedToStudentID = newAssignedStudent.ID;

            InternshipPositionLog logEntry = new InternshipPositionLog();
            logEntry.InternshipPositionID = CurrentPosition.ID;
            logEntry.OldStatus = CurrentPosition.PositionStatus;
            logEntry.NewStatus = CurrentPosition.PositionStatus;
            logEntry.AssignedByOfficeID = Entity.ID;

            if (Entity.MasterAccountID.HasValue)
            {
                logEntry.AssignedByMasterAccountID = Entity.MasterAccountID.Value;
            }
            else
            {
                logEntry.AssignedByMasterAccountID = Entity.ID;
            }

            logEntry.AssignedToStudentID = e.StudentID;
            logEntry.CreatedAt = DateTime.Now;
            logEntry.CreatedAtDateOnly = DateTime.Now.Date;
            logEntry.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
            UnitOfWork.MarkAsNew(logEntry);

            var provider = CurrentPosition.InternshipPositionGroup.Provider;
            var academic = CacheManager.Academics.Get(newAssignedStudent.AcademicID.Value);

            var emailToStudent = MailSender.SendAssignedPositionStudentNotification(newAssignedStudent.ID, newAssignedStudent.ContactEmail, string.Format("{0} {1}", newAssignedStudent.GreekFirstName, newAssignedStudent.GreekLastName), CurrentPosition.InternshipPositionGroup.ID, CurrentPosition.InternshipPositionGroup.Title, provider.Name);
            UnitOfWork.MarkAsNew(emailToStudent);

            var emailToProvider = MailSender.SendAssignedPositionProviderNotification(provider.ID, provider.ContactEmail, provider.UserName, CurrentPosition.InternshipPositionGroup.ID, CurrentPosition.InternshipPositionGroup.Title, string.Format("{0} {1}", newAssignedStudent.GreekFirstName, newAssignedStudent.GreekLastName), academic.Institution, academic.School ?? "-", academic.Department, newAssignedStudent.StudentNumber, provider.Language.GetValueOrDefault());

            UnitOfWork.MarkAsNew(emailToProvider);
            UnitOfWork.Commit();

            FireStudentAssignmentCompleteEvent();
        }

        protected void ucSearchAndRegisterStudent_Cancel(object sender, EventArgs e)
        {
            FireStudentAssignmentCanceledEvent();
        }

        #endregion

    }
}