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
using System.Data;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class AddProviderUser : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
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

            InternshipProvider providerUser = new InternshipProvider();
            providerUser.UsernameFromLDAP = Guid.NewGuid().ToString();

            providerUser = ucProviderUserInput.Fill(providerUser);

            if (string.IsNullOrEmpty(providerUser.AFM))
                providerUser.AFM = Entity.AFM;

            if (string.IsNullOrEmpty(providerUser.DOY))
                providerUser.DOY = Entity.DOY;

            string username = ucProviderUserInput.Username;
            string password = ucProviderUserInput.Password;
            string email = ucProviderUserInput.Email;

            if (Membership.GetUser(username) != null)
            {
                lblValidationErrors.Text = Resources.ProviderUser.UserExists;
                return;
            }

            if (!string.IsNullOrEmpty(Membership.GetUserNameByEmail(email)))
            {
                lblValidationErrors.Text = Resources.ProviderUser.EmailExists;
                return;
            }

            try
            {
                MembershipCreateStatus status;
                MembershipUser mu = Membership.CreateUser(username, ucProviderUserInput.Password, ucProviderUserInput.Email, null, null, true, out status);

                if (mu == null)
                    throw new MembershipCreateUserException(status);
            }
            catch (MembershipCreateUserException ex)
            {
                LogHelper.LogError<AddProviderUser>(ex, this, string.Format("Error while creating ProviderUser with username: {0}", username));
            }

            providerUser.IsActive = true;
            providerUser.IsMasterAccount = false;
            providerUser.MasterAccountID = Entity.ID;
            providerUser.UserName = username;
            providerUser.Email = email;
            providerUser.IsEmailVerified = false;
            providerUser.EmailVerificationCode = Guid.NewGuid().ToString();
            providerUser.VerificationStatus = enVerificationStatus.Verified;
            providerUser.VerificationDate = DateTime.Now;
            providerUser.IsApproved = true;
            providerUser.MustChangePassword = true;
            providerUser.DeclarationType = enReporterDeclarationType.FromRegistration;
            providerUser.RegistrationType = enRegistrationType.Membership;
            providerUser.CreatedBy = Entity.UserName;

            try
            {
                UnitOfWork.MarkAsNew(providerUser);
                UnitOfWork.Commit();

                Roles.AddUserToRole(username, RoleNames.ProviderUser);
            }
            catch (Exception)
            {
                Membership.DeleteUser(username);
                throw;
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

            Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + providerUser.EmailVerificationCode);

            var emailToSend = MailSender.SendProviderUserEmailVerification(providerUser.ID, providerUser.Email, providerUser.ContactName, uri, LanguageService.GetUserLanguage());
            UnitOfWork.MarkAsNew(emailToSend);
            UnitOfWork.Commit();

            Response.Redirect("ProviderUsers.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProviderUsers.aspx");
        }
    }
}