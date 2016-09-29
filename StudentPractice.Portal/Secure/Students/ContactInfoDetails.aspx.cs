using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class ContactInfoDetails : BaseEntityPortalPage<Student>
    {
        protected override void Fill()
        {
            Entity = new StudentRepository(UnitOfWork).FindByUsername(User.Identity.Name);
            Entity.SaveToCurrentContext();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucStudentInput.FillReadOnlyFields(Entity);
                txtEmail.Text = Entity.Email;
                txtMobilePhone.Text = Entity.ContactMobilePhone;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucStudentInput.Fill(Entity);

            Entity.ContactEmail = txtEmail.Text.ToNull();
            Entity.ContactMobilePhone = txtMobilePhone.Text.ToNull();

            Entity.IsContactInfoCompleted = true;

            Entity.IsEmailVerified = false;
            Entity.EmailVerificationCode = Guid.NewGuid().ToString();

            if (new StudentRepository().StudentByAcademicIDNameAndMobilePhoneExists(Entity.ID, Entity.AcademicID.GetValueOrDefault(), Entity.OriginalFirstName, Entity.OriginalLastName, Entity.ContactMobilePhone))
            {
                Entity.IsActive = false;
            }
            else
            {
                Entity.IsActive = true;

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
            }

            UnitOfWork.Commit();

            Response.Redirect("~/Secure/Students/Default.aspx");
        }
    }
}