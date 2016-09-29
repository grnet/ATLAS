using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Web.Security;
using StudentPractice.Utils;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class AddAdminUser : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Attributes["onblur"] = "RemoveTags(this)";
            txtPassword1.Attributes["onblur"] = "RemoveTags(this)";
            txtPassword2.Attributes["onblur"] = "RemoveTags(this)";
            txtEmail.Attributes["onblur"] = "RemoveTags(this)";
            txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            AdminUser reporter = new AdminUser();

            reporter.UsernameFromLDAP = Guid.NewGuid().ToString();
            reporter.ContactName = txtContactName.Text.ToNull();
            reporter.ContactMobilePhone = txtContactMobilePhone.Text.ToNull();
            reporter.IsApproved = true;
            reporter.DeclarationType = enReporterDeclarationType.FromRegistration;

            string username = txtUsername.Text.ToNull();
            string password = txtPassword1.Text.ToNull();
            string email = txtEmail.Text.ToNull();

            reporter.UserName = username;
            reporter.Email = reporter.ContactEmail = email;

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

            MembershipUser mu;

            try
            {
                MembershipCreateStatus status;
                mu = Membership.CreateUser(username, password, email, null, null, true, out status);

                if (mu == null)
                    throw new MembershipCreateUserException(status);
            }
            catch (MembershipCreateUserException ex)
            {
                LogHelper.LogError<AddAdminUser>(ex, this, string.Format("Error while creating User with username: {0}", username));
            }

            reporter.MustChangePassword = true;

            try
            {
                UnitOfWork.MarkAsNew(reporter);
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                Membership.DeleteUser(username);
                throw;
            }

            var roles = new RoleRepository(UnitOfWork).LoadAll();

            if (chbxHelpdesk.Checked)
            {
                reporter.Roles.Add(roles.Where(r => r.RoleName == RoleNames.SuperHelpdesk).FirstOrDefault());
            }

            if (chbxSuperReports.Checked)
            {
                reporter.Roles.Add(roles.Where(r => r.RoleName == RoleNames.SuperReports).FirstOrDefault());
            }

            if (chbxReports.Checked)
            {
                reporter.Roles.Add(roles.Where(r => r.RoleName == RoleNames.Reports).FirstOrDefault());
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}