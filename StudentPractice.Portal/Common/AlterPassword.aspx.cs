using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.ComponentModel;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Common
{
    public partial class AlterPassword : System.Web.UI.Page
    {
        [DefaultValue(true)]
        private bool _requestOldPassword;
        public bool RequestOldPassword
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["r"]))
                    _requestOldPassword = Convert.ToBoolean(Request.QueryString["r"]);

                return _requestOldPassword;
            }
            set
            {
                _requestOldPassword = value;
            }
        }

        private MembershipUser _currentUser = null;
        protected MembershipUser CurrentUser
        {
            get
            {
                if (_currentUser != null)
                    return _currentUser;

                try
                {
                    _currentUser = Membership.GetUser(Request.QueryString["username"]);
                }
                catch (FormatException)
                {
                }

                return _currentUser;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            changePassword.RequestOldPassword = RequestOldPassword;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string oldPassword = changePassword.OldPassword;

            if (!RequestOldPassword)
            {
                oldPassword = CurrentUser.ResetPassword();
            }

            if (CurrentUser.ChangePassword(oldPassword, changePassword.NewPassword))
            {
                AuthenticationService.InvalidateCookie(CurrentUser.UserName, true);
                Session["flash"] = "Η αλλαγή του κωδικού πρόσβασης πραγματοποιήθηκε επιτυχώς";
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
            else
            {
                lblErrors.Visible = true;
            }
        }
    }
}
