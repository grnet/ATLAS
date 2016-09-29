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
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class AddOfficeUser : BaseEntityPortalPage<InternshipOffice>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            InternshipOffice officeUser = new InternshipOffice();
            officeUser.UsernameFromLDAP = Guid.NewGuid().ToString();
            officeUser.OfficeType = Entity.OfficeType;

            officeUser = ucOfficeUserInput.Fill(officeUser);

            string username = ucOfficeUserInput.Username;
            string password = ucOfficeUserInput.Password;
            string email = ucOfficeUserInput.Email;

            if (Membership.GetUser(username) != null)
            {
                lblValidationErrors.Text = "Το Όνομα Χρήστη χρησιμοποιείται ήδη από κάποιο άλλο χρήστη του Πληροφοριακού Συστήματος. Παρακαλούμε επιλέξτε κάποιο άλλο.";
                return;
            }

            if (!string.IsNullOrEmpty(Membership.GetUserNameByEmail(email)))
            {
                lblValidationErrors.Text = "Το E-mail χρησιμοποιείται ήδη από κάποιο άλλο χρήστη του Πληροφοριακού Συστήματος. Παρακαλούμε επιλέξτε κάποιο άλλο.";
                return;
            }

            try
            {
                MembershipCreateStatus status;
                MembershipUser mu = Membership.CreateUser(username, ucOfficeUserInput.Password, ucOfficeUserInput.Email, null, null, true, out status);

                if (mu == null)
                    throw new MembershipCreateUserException(status);
            }
            catch (MembershipCreateUserException ex)
            {
                LogHelper.LogError<AddOfficeUser>(ex, this, string.Format("Error while creating OfficeUser with username: {0}", username));
            }

            officeUser.InstitutionID = Entity.InstitutionID;
            officeUser.CanViewAllAcademics = Entity.CanViewAllAcademics;
            officeUser.IsActive = true;
            officeUser.IsMasterAccount = false;
            officeUser.MasterAccountID = Entity.ID;
            officeUser.UserName = username;
            officeUser.Email = email;
            officeUser.IsEmailVerified = false;
            officeUser.VerificationStatus = enVerificationStatus.Verified;
            officeUser.VerificationDate = DateTime.Now;
            officeUser.IsApproved = true;
            officeUser.MustChangePassword = true;
            officeUser.DeclarationType = enReporterDeclarationType.FromRegistration;
            officeUser.RegistrationType = enRegistrationType.Membership;
            officeUser.CreatedBy = Entity.UserName;

            officeUser.IsEmailVerified = false;
            officeUser.EmailVerificationCode = Guid.NewGuid().ToString();
            officeUser.EmailVerificationDate = null;

            try
            {
                UnitOfWork.MarkAsNew(officeUser);
                UnitOfWork.Commit();

                Roles.AddUserToRole(username, RoleNames.OfficeUser);
            }
            catch (Exception)
            {
                Membership.DeleteUser(username);
                throw;
            }

            foreach (var academic in Entity.Academics)
            {
                officeUser.Academics.Add(academic);
            }

            Uri baseURI;
            if (Config.IsSSL)
            {
                baseURI = new Uri("https://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }
            else
            {
                baseURI = new Uri("http://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }

            Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + officeUser.EmailVerificationCode);

            var verificationEmail = MailSender.SendOfficeUserEmailEmailVerification(officeUser.ID, officeUser.Email, officeUser.ContactName, uri);
            UnitOfWork.MarkAsNew(verificationEmail);
            UnitOfWork.Commit();

            Response.Redirect("OfficeUsers.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("OfficeUsers.aspx");
        }
    }
}