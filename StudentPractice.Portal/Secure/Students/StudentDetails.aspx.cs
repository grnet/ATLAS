using System;
using System.Web.Security;
using System.Web.UI;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using StudentPractice.Mails;
using System.Web;
using System.Collections.Generic;
using System.Web.Services;
using Imis.Domain;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class StudentDetails : BaseEntityPortalPage<Student>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new StudentRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Entity.IsContactInfoCompleted)
            {
                Response.Redirect("~/Secure/Students/ContactInfoDetails.aspx");
            }

            txtEmail.Text = Entity.ContactEmail;
            txtMobilePhone.Text = Entity.ContactMobilePhone;

            if (Entity.IsEmailVerified)
            {
                btnSendEmailVerificationCode.Visible = false;
            }

            ucStudentView.Entity = Entity;
            if (!Page.IsPostBack)
            {
                ucStudentView.Bind();
            }

            base.OnLoad(e);
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnChangeStudentInfo_Click(object sender, EventArgs e)
        {
            if (!Entity.IsAssignedToPosition.GetValueOrDefault())
            {
                ucStudentInput.FillReadOnlyFields(Entity);
                ucStudentInput.Entity = Entity;
                ucStudentInput.Bind();
                mvStudent.SetActiveView(vStudentInput);

                lblChangeStudentInfoError.Visible = false;
            }
            else
            {
                lblChangeStudentInfoError.Visible = true;
            }
        }

        protected void btnUpdateStudentInfo_Click(object sender, EventArgs e)
        {
            if (!Entity.IsAssignedToPosition.GetValueOrDefault())
            {
                ucStudentInput.Fill(Entity);
                UnitOfWork.Commit();

                fm.Text = "Η αλλαγή των στοιχείων του Ον/μου πραγματοποιήθηκε επιτυχώς";
            }
            else
            {
                fm.Text = "Η αλλαγή των στοιχείων του Ον/μου δεν μπορεί να πραγματοποιηθεί γιατί έχετε αντιστοιχιστεί σε θέση Πρακτικής Άσκησης από το Γραφείο Πρακτικής του Ιδρύματός σας. Παρακαλούμε επικοινωνήστε με το Γραφείο Πρακτικής για την εκτέλεση της αλλαγής που επιθυμείτε.";
            }

            ucStudentView.Entity = Entity;
            ucStudentView.Bind();
            mvStudent.SetActiveView(vStudentView);
        }

        protected void btnSendEmailVerificationCode_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            Uri baseURI;
            if (Config.IsSSL)
            {
                baseURI = new Uri("https://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }
            else
            {
                baseURI = new Uri("http://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }

            Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + Entity.EmailVerificationCode);

            var email = MailSender.SendEmailVerification(Entity.ID, Entity.ContactEmail, string.Format("{0} {1}", Entity.GreekFirstName, Entity.GreekLastName), uri);
            UnitOfWork.MarkAsNew(email);
            UnitOfWork.Commit();
        }

        #endregion

    }
}
