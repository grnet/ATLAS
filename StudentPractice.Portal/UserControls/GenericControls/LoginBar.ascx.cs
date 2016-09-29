using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using AjaxControlToolkit;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class LoginBar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If user if a Student's user
            if (Roles.IsUserInRole(RoleNames.Student))
            {
                loginName.Visible = false;
            }
        }

        #region Controls in templates

        public void SetUserDetails(string userdetails)
        {
            ((Literal)loginView.FindControl("txtUserDetails")).Text = userdetails;
        }

        private ModalPopupExtender mpeChangePassword
        {
            get { return (ModalPopupExtender)loginView.FindControl("mpeChangePassword"); }
        }

        public LinkButton ChangePasswordButton
        {
            get { return (LinkButton)loginView.FindControl("lnkChangePassword"); }
        }

        public LoginStatus LogoutButton
        {
            get { return (LoginStatus)loginView.FindControl("loginStatus"); }
        }

        private ChangePassword cp
        {
            get { return (ChangePassword)loginView.FindControl("cp"); }
        }

        private Label lblErrorMessage
        {
            get { return (Label)loginView.FindControl("lblErrorMessage"); }
        }

        private LoginName loginName
        {
            get { return (LoginName)loginView.FindControl("loginName"); }
        }

        private MultiView mv
        {
            get { return (MultiView)loginView.FindControl("mv"); }
        }

        #endregion

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                MembershipUser mu = Membership.GetUser();
                if (mu.ChangePassword(cp.OldPassword, cp.NewPassword))
                {
                    lblErrorMessage.Text = "";
                    cp.ClearInput();
                    mv.ActiveViewIndex = 1; // success
                    mpeChangePassword.Show();
                }
                else
                {
                    mpeChangePassword.Show();
                    lblErrorMessage.Text = "Ο παλιός κωδικός πρόσβασης δεν είναι σωστός. Ελέγξτε ότι τον πληκτρολογείτε σωστά.";
                }
            }
        }

        protected void btnSuccess_Click(object sender, EventArgs e)
        {
            mv.ActiveViewIndex = 0; // restore default view
            mpeChangePassword.Hide();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cp.ClearInput();
            mpeChangePassword.Hide();
        }

        protected void LoginStatus1_OnLoggingOut(object sender, LoginCancelEventArgs e)
        {
            var c = Response.Cookies[Roles.CookieName + "_"];
            if (c != null)
                c.Expires = DateTime.Now.AddDays(-1);
        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            AuthenticationService.ClearRoleCookie();
        }
    }
}