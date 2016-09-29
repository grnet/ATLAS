using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Mails;
using StudentPractice.Portal.UserControls.InternshipProviderControls.InputControls;

namespace StudentPractice.Portal.Common
{
    public partial class ProviderRegistration : BaseEntityPortalPage<InternshipProvider>
    {
        protected override void Fill()
        {
            Entity = new InternshipProvider();
            Entity.UsernameFromLDAP = Guid.NewGuid().ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Config.ProviderRegistrationAllowed)
                mvRegistration.SetActiveView(vNotAllowed);

            int countryOrigin = 0;
            if (int.TryParse(Request.QueryString["c"], out countryOrigin))
            {
                if (!Config.AllowCyprusRegistration && countryOrigin == enCountryOrigin.Cyprus.GetValue())
                {
                    mvRegistration.SetActiveView(vCyprusNotAllowd);
                }

                if (!Config.AllowForeignRegistration && countryOrigin == enCountryOrigin.Foreign.GetValue())
                {
                    mvRegistration.SetActiveView(vForeignNotAllowd);
                }

                if (countryOrigin == enCountryOrigin.Foreign.GetValue())
                {
                    ucLanguageBar.Visible = true;
                }

            }
            base.OnLoad(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                mvRegistration.SetActiveView(vRegister);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            ucProviderInput.Fill(Entity);

            if (Entity.ProviderType != enProviderType.PublicCarrier && new InternshipProviderRepository(UnitOfWork).IsAfmVerified(Entity.ID, Entity.AFM))
            {
                lblErrors.Text = string.Format(Resources.RegistrantionConditions.Provider_Exists, Entity.AFM);
                return;
            }

            string username = null;

            try
            {
                username = ucRegisterUserInput.CreateUser();
                if (string.IsNullOrEmpty(username))
                    throw new MembershipCreateUserException("CreateUser returned empty username");
            }
            catch (MembershipCreateUserException)
            {
                return;
            }

            Entity.DeclarationType = enReporterDeclarationType.FromRegistration;
            Entity.RegistrationType = enRegistrationType.Membership;
            Entity.IsActive = true;
            Entity.IsMasterAccount = true;
            Entity.VerificationStatus = enVerificationStatus.NotVerified;
            Entity.UserName = Entity.CreatedBy = ucRegisterUserInput.Username;
            Entity.Email = ucRegisterUserInput.Email;

            Entity.IsEmailVerified = false;
            Entity.EmailVerificationCode = Guid.NewGuid().ToString();

            try
            {
                UnitOfWork.MarkAsNew(Entity);
                UnitOfWork.Commit();
                var provider = Roles.Provider as StudentPracticeRoleProvider;
                provider.AddUsersToRoles(new[] { Entity.UserName }, new[] { RoleNames.MasterProvider });
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

            Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + Entity.EmailVerificationCode);

            var email = MailSender.SendEmailVerification(Entity.ID, Entity.Email, Entity.ContactName, uri, LanguageService.GetUserLanguage());
            UnitOfWork.MarkAsNew(email);
            UnitOfWork.Commit();

            AuthenticationService.LoginReporter(Entity);

            Response.Redirect("~/Secure/InternshipOffices/Default.aspx");
        }

        protected void cvTerms_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkTerms.Checked;
        }
    }
}
