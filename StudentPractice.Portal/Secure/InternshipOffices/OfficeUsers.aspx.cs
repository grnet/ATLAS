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

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class OfficeUsers : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Entity.CanViewAllAcademics.Value && Entity.Academics.Count == 0)
                {
                    Response.Redirect("OfficeDetails.aspx");
                }

                if (!Entity.IsEmailVerified)
                {
                    mvAccount.SetActiveView(vAccountNotVerified);
                    lblVerificationError.Text = "Δεν μπορείτε να διαχειριστείτε τους χρήστες του Γραφείου Πρακτικής Άσκησης, γιατί δεν έχετε ενεργοποιήσει το e-mail σας.";
                }
                else if (Entity.VerificationStatus != enVerificationStatus.Verified)
                {
                    mvAccount.SetActiveView(vAccountNotVerified);
                    lblVerificationError.Text = "Δεν μπορείτε να διαχειριστείτε τους χρήστες του Γραφείου Πρακτικής Άσκησης, γιατί δεν έχει πιστοποιηθεί ο λογαριασμός σας.<br/>Παρακαλούμε εκτυπώστε τη Βεβαίωση Συμμετοχής και αποστείλτε τη με ΦΑΞ στο Γραφείο Αρωγής για να πιστοποιηθεί.";
                }
                else
                {
                    mvAccount.SetActiveView(vAccountVerified);

                    if (Entity.OfficeType == enOfficeType.Departmental)
                    {
                        gvOfficeUsers.Columns[3].Visible = false;
                        gvOfficeUsers.Columns[4].Visible = false;
                    }
                }
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvOfficeUsers.DataBind();
        }

        protected void odsOfficeUsers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipOffice> criteria = new Criteria<InternshipOffice>();

            criteria.Include(x => x.Academics);

            criteria.Expression = criteria.Expression.Where(x => x.IsMasterAccount, false);
            criteria.Expression = criteria.Expression.Where(x => x.MasterAccountID, Entity.ID);
            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvOfficeUsers_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var oID = int.Parse(parameters[1]);

            if (action == "delete")
            {
                using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
                {
                    InternshipOffice office = new InternshipOfficeRepository(unitOfWork).Load(oID);
                    if (new InternshipPositionLogRepository(unitOfWork).HasMadePreAssignemnts(oID) || new IncidentReportRepository(unitOfWork).HasIncidentReports(oID))
                        e.Result = "CANNOTDELETE";
                    else
                        e.Result = string.Format("CANDELETE:{0}", office.ID);
                }
            }
        }

        public void gvOfficeUsers_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            var parameters = e.Parameters.Split(':');
            var action = parameters[0].ToLower();
            var oID = int.Parse(parameters[1]);

            using (IUnitOfWork unitOfWork = UnitOfWorkFactory.Create())
            {
                InternshipOffice office = new InternshipOfficeRepository(unitOfWork).Load(oID, x => x.Academics, y => y.Emails, z => z.IncidentReports);

                if (action == "delete")
                {
                    if (!new InternshipPositionLogRepository(unitOfWork).HasMadePreAssignemnts(oID)
                        && !new IncidentReportRepository(unitOfWork).HasIncidentReports(oID))
                    {
                        try
                        {
                            var emails = office.Emails.ToList();
                            var incidentReports = office.IncidentReports.ToList();

                            office.Academics.Clear();

                            for (int i = emails.Count - 1; i >= 0; i--)
                            {
                                unitOfWork.MarkAsDeleted(emails[i]);
                            }

                            for (int i = incidentReports.Count - 1; i >= 0; i--)
                            {
                                unitOfWork.MarkAsDeleted(incidentReports[i]);
                            }

                            unitOfWork.Commit();

                            Roles.RemoveUserFromRoles(office.UserName, new string[] { RoleNames.OfficeUser });
                            unitOfWork.MarkAsDeleted(office);
                            unitOfWork.Commit();

                            Membership.DeleteUser(office.UserName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<OfficeUsers>(ex, this, string.Format("Unable to delete Office with username: {0}", office.UserName));
                        }
                    }
                }
                else if (action == "lock")
                {
                    if (office.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(office.UserName);
                            mu.IsApproved = false;
                            Membership.UpdateUser(mu);

                            office.IsApproved = false;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<OfficeUsers>(ex, this, string.Format("Unable to lock Office with username: {0}", office.UserName));
                        }
                    }
                }
                else if (action == "unlock")
                {
                    if (!office.IsApproved)
                    {
                        try
                        {
                            MembershipUser mu = Membership.GetUser(office.UserName);
                            mu.IsApproved = true;
                            Membership.UpdateUser(mu);

                            office.IsApproved = true;
                            unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.LogError<OfficeUsers>(ex, this, string.Format("Unable to unlock Office with username: {0}", office.UserName));
                        }
                    }
                }
            }

            gvOfficeUsers.DataBind();
        }
    }
}
