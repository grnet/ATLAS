using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Web.Security;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class EditAdminUser : BaseEntityPortalPage<AdminUser>
    {
        protected override void Fill()
        {
            Entity = new AdminUserRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["sID"]), x => x.Roles);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtUsername.Text = Entity.UserName;
                txtEmail.Text = Entity.ContactEmail;
                txtContactName.Text = Entity.ContactName;
                txtContactMobilePhone.Text = Entity.ContactMobilePhone;

                if (Roles.IsUserInRole(Entity.UserName, RoleNames.SuperHelpdesk))
                {
                    chbxHelpdesk.Checked = true;
                }

                if (Roles.IsUserInRole(Entity.UserName, RoleNames.SuperReports))
                {
                    chbxSuperReports.Checked = true;
                }

                if (Roles.IsUserInRole(Entity.UserName, RoleNames.Reports))
                {
                    chbxReports.Checked = true;
                }
            }

            base.OnPreRender(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Attributes["onblur"] = "RemoveTags(this)";
            txtEmail.Attributes["onblur"] = "RemoveTags(this)";
            txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string oldEmail = Entity.Email;

            if (Entity.Email != txtEmail.Text.ToNull())
                Entity.Email = Entity.ContactEmail = txtEmail.Text.ToNull();

            if (Entity.ContactName != txtContactName.Text.ToNull())
                Entity.ContactName = txtContactName.Text.ToNull();

            if (Entity.ContactMobilePhone != txtContactMobilePhone.Text.ToNull())
                Entity.ContactMobilePhone = txtContactMobilePhone.Text.ToNull();

            var roles = new RoleRepository(UnitOfWork).LoadAll();

            if (chbxHelpdesk.Checked)
            {
                if (!Roles.IsUserInRole(Entity.UserName, RoleNames.SuperHelpdesk))
                {
                    Entity.Roles.Add(roles.Where(r => r.RoleName == RoleNames.SuperHelpdesk).FirstOrDefault());
                }
            }
            else
            {
                if (Roles.IsUserInRole(Entity.UserName, RoleNames.SuperHelpdesk))
                {
                    Entity.Roles.Remove(roles.Where(r => r.RoleName == RoleNames.SuperHelpdesk).FirstOrDefault());
                }
            }

            if (chbxSuperReports.Checked)
            {
                if (!Roles.IsUserInRole(Entity.UserName, RoleNames.SuperReports))
                {
                    Entity.Roles.Add(roles.Where(r => r.RoleName == RoleNames.SuperReports).FirstOrDefault());
                }
            }
            else
            {
                if (Roles.IsUserInRole(Entity.UserName, RoleNames.SuperReports))
                {
                    Entity.Roles.Remove(roles.Where(r => r.RoleName == RoleNames.SuperReports).FirstOrDefault());
                }
            }

            if (chbxReports.Checked)
            {
                if (!Roles.IsUserInRole(Entity.UserName, RoleNames.Reports))
                {
                    Entity.Roles.Add(roles.Where(r => r.RoleName == RoleNames.Reports).FirstOrDefault());
                }
            }
            else
            {
                if (Roles.IsUserInRole(Entity.UserName, RoleNames.Reports))
                {
                    Entity.Roles.Remove(roles.Where(r => r.RoleName == RoleNames.Reports).FirstOrDefault());
                }
            }

            string newEmail = txtEmail.Text.ToNull();

            if (oldEmail != newEmail && !string.IsNullOrEmpty(Membership.GetUserNameByEmail(newEmail)))
            {
                lblValidationErrors.Text = "Το E-mail χρησιμοποιείται ήδη από κάποιο άλλο χρήστη του Πληροφοριακού Συστήματος. Παρακαλούμε επιλέξτε κάποιο άλλο.";
                return;
            }

            UnitOfWork.Commit();

            MembershipUser mu = Membership.GetUser(Entity.UserName);
            if (oldEmail != newEmail)
            {
                mu.Email = newEmail;
                Membership.UpdateUser(mu);
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
