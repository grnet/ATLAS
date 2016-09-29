using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using Imis.Domain;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal
{
    public partial class DefaultEN : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Config.AllowForeignRegistration || LanguageService.GetUserLanguage() == enLanguage.Greek)
            {
                Response.Redirect(ResolveUrl("~/Default.aspx"));
            }

            if (!Page.IsPostBack && Page.User.Identity.IsAuthenticated)
            {
                if (Entity.MustChangePassword)
                {
                    Response.Redirect("~/Common/ChangePassword.aspx");
                }

                if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.MasterProvider) ||
                    Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.ProviderUser))
                {
                    Response.Redirect("~/Secure/InternshipProviders/Default.aspx", true);
                }
                else if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.MasterOffice) ||
                         Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.OfficeUser))
                {
                    Response.Redirect("~/Secure/InternshipOffices/Default.aspx", true);
                }
                else if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.Student))
                {
                    Response.Redirect("~/Secure/Students/Default.aspx", true);
                }
                else if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.Helpdesk) ||
                         Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.SuperHelpdesk) ||
                         Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.Supervisor))
                {
                    Response.Redirect("~/Secure/Helpdesk/Default.aspx", true);
                }
                else if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.Reports) ||
                         Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.SuperReports))
                {
                    Response.Redirect("~/Secure/Reports/Default.aspx", true);
                }
                else if (Roles.IsUserInRole(Page.User.Identity.Name, RoleNames.SystemAdministrator))
                {
                    Response.Redirect("~/Secure/Admin/Default.aspx", true);
                }
                else
                {
                    Response.Redirect("~/Common/AccessDenied.aspx", true);
                }
            }

            var loginButton = login.FindControl("LoginButton") as Button;
            var txtUserName = login.FindControl("UserName") as TextBox;
            var txtPassword = login.FindControl("Password") as TextBox;

            txtUserName.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(loginButton, string.Empty));
            txtPassword.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(loginButton, string.Empty));

            Form.DefaultButton = loginButton.UniqueID;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Config.AllowForeignRegistration)
            {
                divForeign.Visible = false;
            }

            TextBox txtUserName = login.FindControl("UserName") as TextBox;
            TextBox txtPassword = login.FindControl("Password") as TextBox;

            txtUserName.Focus();
            txtPassword.Text = string.Empty;
        }

        protected void login_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            MembershipUser user = Membership.GetUser(login.UserName);
            if (user != null)
            {
                if (user.IsLockedOut)
                {
                    login.FailureText = "The user is locked. If you do not remember your password, please, select 'Forgotten Password', or you can contact the Helpdesk.";
                }
                else if (!user.IsApproved)
                {
                    login.FailureText = "The user is locked. Please contact the Central Host who created the account.";
                }
            }
        }

        protected void btnFrProvider_Click(object sender, EventArgs e)
        {
            LanguageService.SetUserLanguageNoRedirect(enLanguage.English);
            Page.Response.Redirect(Page.ResolveUrl("~/Common/ProviderRegistration.aspx?c=2"));
        }

        protected void login_LoggedIn(object sender, EventArgs e)
        {
            var reporter = new ReporterRepository(UnitOfWork).FindByUsername(login.UserName.ToNull());
            if (reporter != null)
            {
                if (reporter.ReporterType == enReporterType.InternshipProvider)
                {
                    LanguageService.SetUserLanguageNoRedirect(reporter.Language.HasValue ? reporter.Language.Value : enLanguage.Greek);
                }

                AuthenticationService.LoginReporter(reporter);
            }
            else
            {
                LanguageService.SetUserLanguageNoRedirect(enLanguage.Greek);
            }
        }
    }
}
