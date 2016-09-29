using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Imis.Domain;
using DevExpress.Web.ASPxGridView;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.BusinessModel;
using StudentPractice.Utils;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class ProviderUsers : BaseEntityPortalPage<InternshipProvider>
    {
        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Entity.IsEmailVerified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.ProviderUser.EmailNotVerified;
            }
            else if (Entity.VerificationStatus != enVerificationStatus.Verified)
            {
                mvAccount.SetActiveView(vAccountNotVerified);
                lblVerificationError.Text = Resources.ProviderUser.AccountNotVerified;
            }
            else
            {
                mvAccount.SetActiveView(vAccountVerified);
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvProviderUsers.DataBind();
        }

        protected void odsProviderUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, false);
            criteria.Expression = criteria.Expression.Where(x => x.MasterAccountID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvProviderUsers_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var oID = int.Parse(parameters[1]);

            if (action == "delete")
            {
                using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
                {
                    InternshipProvider providerUser = new InternshipProviderRepository(unitOfWork).Load(oID);
                    if (new InternshipPositionGroupRepository().PositionsExistForProvider(oID) || new IncidentReportRepository(unitOfWork).HasIncidentReports(oID))
                        e.Result = "CANNOTDELETE";
                    else
                        e.Result = string.Format("CANDELETE:{0}", providerUser.ID);
                }
            }
        }

        protected void gvProviderUsers_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var oID = int.Parse(parameters[1]);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                InternshipProvider providerUser = new InternshipProviderRepository(unitOfWork).Load(oID, x => x.Emails, x => x.Roles);

                if (action == "delete")
                {
                    if (!new InternshipPositionGroupRepository().PositionsExistForProvider(oID)
                        && !new IncidentReportRepository(unitOfWork).HasIncidentReports(oID))
                    {
                        try
                        {
                            var emails = providerUser.Emails.ToList();
                            var roles = providerUser.Roles.ToList();

                            for (int i = emails.Count - 1; i >= 0; i--)
                            {
                                unitOfWork.MarkAsDeleted(emails[i]);
                            }

                            foreach (var role in roles)
                            {
                                providerUser.Roles.Remove(role);
                            }

                            unitOfWork.Commit();

                            unitOfWork.MarkAsDeleted(providerUser);
                            unitOfWork.Commit();

                            Membership.DeleteUser(providerUser.UserName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<ProviderUsers>(ex, this, string.Format("Unable to delete Provider with username: {0}", providerUser.UserName));
                        }
                    }
                }
                else if (action == "lock")
                {
                    if (providerUser.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(providerUser.UserName);
                            mu.IsApproved = false;
                            Membership.UpdateUser(mu);

                            providerUser.IsApproved = false;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<ProviderUsers>(ex, this, string.Format("Unable to lock Provider with username: {0}", providerUser.UserName));
                        }
                    }
                }
                else if (action == "unlock")
                {
                    if (!providerUser.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(providerUser.UserName);
                            mu.IsApproved = true;
                            Membership.UpdateUser(mu);

                            providerUser.IsApproved = true;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<ProviderUsers>(ex, this, string.Format("Unable to unlock Provider with username: {0}", providerUser.UserName));
                        }
                    }
                }
            }
            gvProviderUsers.DataBind();
        }
    }
}