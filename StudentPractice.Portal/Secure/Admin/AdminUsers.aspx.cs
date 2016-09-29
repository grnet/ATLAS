using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using DevExpress.Web.ASPxGridView;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Utils;
using System.Text;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class AdminUsers : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvAdminUsers.DataBind();
        }

        protected void odsAdminUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<AdminUser> criteria = new Criteria<AdminUser>();

            e.InputParameters["criteria"] = criteria;
        }

        public void gvAdminUsers_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            int sID = Convert.ToInt32(e.CommandArgs.CommandArgument);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                AdminUser adminUser = new AdminUserRepository(unitOfWork).Load(sID, x => x.Roles, x => x.Emails);

                if (e.CommandArgs.CommandName == "DeleteAdminUser")
                {
                    int evaluationCount = new IncidentReportPostRepository().GetAnsweredIncidentReportCount(adminUser.UserName);

                    if (evaluationCount > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert_cannotdelete", string.Format("alert('{0}')", "Δεν μπορείτε να διαγράψετε το συγκεκριμένο χρήστη, γιατί έχει ήδη απαντήσει σε συμβάντα.\\n\\nΑν θέλετε ο χρήστης να μην έχει πλέον πρόσβαση στην εφαρμογή μπορείτε να τον απενεργοποιήσετε πατώντας το εικονίδιο με το λουκετάκι κάτω από τη στήλη \"Επεξεργασία Χρήστη\""), true);
                        return;
                    }
                    else
                    {
                        try
                        {
                            var emails = adminUser.Emails.ToList();
                            var roles = adminUser.Roles.ToList();

                            for (int i = emails.Count - 1; i >= 0; i--)
                            {
                                unitOfWork.MarkAsDeleted(emails[i]);
                            }

                            foreach (var role in roles)
                            {
                                adminUser.Roles.Remove(role);
                            }

                            unitOfWork.Commit();

                            unitOfWork.MarkAsDeleted(adminUser);
                            unitOfWork.Commit();

                            Membership.DeleteUser(adminUser.UserName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<AdminUsers>(ex, this, string.Format("Unable to delete AdminUser with username: {0}", adminUser.UserName));
                        }
                    }
                }
                else if (e.CommandArgs.CommandName == "LockAdminUser")
                {
                    if (adminUser.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(adminUser.UserName);
                            mu.IsApproved = false;
                            Membership.UpdateUser(mu);

                            adminUser.IsApproved = false;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<AdminUsers>(ex, this, string.Format("Unable to lock AdminUser with username: {0}", adminUser.UserName));
                        }
                    }
                }
                else if (e.CommandArgs.CommandName == "UnLockAdminUser")
                {
                    if (!adminUser.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(adminUser.UserName);
                            mu.IsApproved = true;
                            Membership.UpdateUser(mu);

                            adminUser.IsApproved = true;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<AdminUsers>(ex, this, string.Format("Unable to unlock AdminUser with username: {0}", adminUser.UserName));
                        }
                    }
                }

                var gridViewAdminUser = gvAdminUsers.GetRow(e.VisibleIndex) as AdminUser;
                if (gridViewAdminUser != null && adminUser != null)
                    gridViewAdminUser.IsApproved = adminUser.IsApproved;
            }

            gvAdminUsers.DataBind();
        }

        protected string GetAccountDetails(AdminUser adminUser)
        {
            if (adminUser == null)
                return string.Empty;

            string accountDetails = string.Empty;

            accountDetails = string.Format("{0}<br/>{1}", adminUser.UserName, adminUser.Email);

            return accountDetails;
        }

        protected string GetContactDetails(AdminUser adminUser)
        {
            if (adminUser == null)
                return string.Empty;

            string contactDetails = string.Empty;

            contactDetails = string.Format("{0}<br/>{1}", adminUser.ContactName, adminUser.ContactMobilePhone);

            return contactDetails;
        }

        protected string GetAccessDetails(AdminUser adminUser)
        {
            if (adminUser == null)
                return string.Empty;

            StringBuilder accessDetails = new StringBuilder();

            if (Roles.IsUserInRole(adminUser.UserName, RoleNames.SystemAdministrator))
            {
                accessDetails.Append("Διαχειριστής Συστήματος");
                accessDetails.Append("<br/>");
            }

            if (Roles.IsUserInRole(adminUser.UserName, RoleNames.SuperHelpdesk))
            {
                accessDetails.Append("Γραφείο Αρωγής");
                accessDetails.Append("<br/>");
            }

            if (Roles.IsUserInRole(adminUser.UserName, RoleNames.SuperReports))
            {
                accessDetails.Append("Επιτελικές Αναφορές (με στοιχεία φοιτητών)");
            }
            else if (Roles.IsUserInRole(adminUser.UserName, RoleNames.Reports))
            {
                accessDetails.Append("Επιτελικές Αναφορές (χωρίς στοιχεία φοιτητών)");
            }

            return accessDetails.ToString();
        }
    }
}
