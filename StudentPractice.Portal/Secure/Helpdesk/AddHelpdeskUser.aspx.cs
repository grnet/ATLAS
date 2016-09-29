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

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class AddHelpdeskUser : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            HelpdeskUser helpdeskUser = new HelpdeskUser();
            helpdeskUser.UsernameFromLDAP = Guid.NewGuid().ToString();

            helpdeskUser = ucHelpdeskUserInput.Fill(helpdeskUser);

            string username = ucHelpdeskUserInput.Username;
            string password = ucHelpdeskUserInput.Password;
            string email = ucHelpdeskUserInput.Email;

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
                MembershipUser mu = Membership.CreateUser(username, ucHelpdeskUserInput.Password, ucHelpdeskUserInput.Email, null, null, true, out status);

                if (mu == null)
                    throw new MembershipCreateUserException(status);
            }
            catch (MembershipCreateUserException ex)
            {
                LogHelper.LogError<AddHelpdeskUser>(ex, this, string.Format("Error while creating HelpdeskUser with username: {0}", username));
            }

            helpdeskUser.DeclarationType = enReporterDeclarationType.FromRegistration;
            helpdeskUser.UserName = username;
            helpdeskUser.IsApproved = true;
            helpdeskUser.MustChangePassword = true;

            try
            {
                UnitOfWork.MarkAsNew(helpdeskUser);
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                Membership.DeleteUser(username);
                throw;
            }

            Roles.AddUserToRole(username, RoleNames.Helpdesk);

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}