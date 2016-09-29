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

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class HelpdeskUsers : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvHelpdeskUsers.DataBind();
        }

        protected void odsHelpdeskUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<HelpdeskUser> criteria = new Criteria<HelpdeskUser>();

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvHelpdeskUsers_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var sID = int.Parse(parameters[1]);

            if (action == "delete")
            {
                using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
                {
                    HelpdeskUser hUser = new HelpdeskUserRepository(unitOfWork).Load(sID);
                    int evaluationCount = new IncidentReportPostRepository().GetAnsweredIncidentReportCount(hUser.UserName);
                    if (evaluationCount > 0)
                        e.Result = "CANNOTDELETE";
                    else
                        e.Result = string.Format("CANDELETE:{0}", hUser.ID);
                }
            }
        }

        protected void gvHelpdeskUsers_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var sID = int.Parse(parameters[1]);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                HelpdeskUser hUser = new HelpdeskUserRepository(unitOfWork).Load(sID);

                if (action == "delete")
                {
                    int evaluationCount = new IncidentReportPostRepository().GetAnsweredIncidentReportCount(hUser.UserName);
                    if (evaluationCount == 0)
                    {
                        try
                        {
                            Roles.RemoveUserFromRoles(hUser.UserName, new string[] { RoleNames.Helpdesk });
                            unitOfWork.MarkAsDeleted(hUser);
                            unitOfWork.Commit();

                            Membership.DeleteUser(hUser.UserName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<HelpdeskUsers>(ex, this, string.Format("Unable to delete HelpdeskUser with username: {0}", hUser.UserName));
                        }
                    }
                }
                else if (action == "lock")
                {
                    if (hUser.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(hUser.UserName);
                            mu.IsApproved = false;
                            Membership.UpdateUser(mu);

                            hUser.IsApproved = false;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<HelpdeskUsers>(ex, this, string.Format("Unable to lock HelpdeskUser with username: {0}", hUser.UserName));
                        }
                    }
                }
                else if (action == "unlock")
                {
                    if (!hUser.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(hUser.UserName);
                            mu.IsApproved = true;
                            Membership.UpdateUser(mu);

                            hUser.IsApproved = true;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<HelpdeskUsers>(ex, this, string.Format("Unable to unlock HelpdeskUser with username: {0}", hUser.UserName));
                        }
                    }
                }
            }

            gvHelpdeskUsers.DataBind();
        }

        protected string GetAccountDetails(HelpdeskUser hUser)
        {
            if (hUser == null)
                return string.Empty;

            string accountDetails = string.Empty;

            accountDetails = string.Format("{0}<br/>{1}", hUser.UserName, hUser.Email);

            return accountDetails;
        }

        protected string GetContactDetails(HelpdeskUser helpdeskUser)
        {
            if (helpdeskUser == null)
                return string.Empty;

            string contactDetails = string.Empty;

            contactDetails = string.Format("{0}<br/>{1}", helpdeskUser.ContactName, helpdeskUser.ContactMobilePhone);

            return contactDetails;
        }

        protected string GetAnsweredIncidentReports(HelpdeskUser helpdeskUser)
        {
            if (helpdeskUser == null)
                return string.Empty;

            int evaluationCount = new IncidentReportPostRepository().GetAnsweredIncidentReportCount(helpdeskUser.UserName);

            return string.Format("{0:D}", evaluationCount);
        }
    }
}
