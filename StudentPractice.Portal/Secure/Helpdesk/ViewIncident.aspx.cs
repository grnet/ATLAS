using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Mails;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ViewIncident : BaseEntityPortalPage<IncidentReport>
    {
        protected override void Fill()
        {
            Entity = new IncidentReportRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["id"]), x => x.Reporter, y => y.IncidentType, z => z.LastPost.LastDispatch);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                if (Entity.SubmissionType == enReportSubmissionType.Helpdesk || Entity.LastPost == null || string.IsNullOrWhiteSpace(Entity.ReporterEmail))
                {
                    btnSendEmail.Visible = false;
                }

                incidentReportView.SetIncidentReport(Entity);
                incidentReportView.DataBind();

                incidentReportPostsView.DataSource = odsIncidentReportPosts;
                incidentReportPostsView.DataBind();

                phFromSuccess.DataBind();
            }
            else
            {
                //ErrorHelper.ShowBackofficeError(Response, "Δε βρέθηκε η αναφορά με κωδικό " + Request.QueryString["id"]);
            }
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            if (Entity.LastPost != null && Entity.LastPost.LastDispatch != null && DateTime.Now < Entity.LastPost.LastDispatch.DispatchSentAt.AddMinutes(30))
            {
                phCannotSendEmail.Visible = true;
                return;
            }

            var email = MailSender.SendIncidentReportAnswer(Entity.ReporterID, Entity.ReporterEmail, Entity.ID.ToString(), Entity.ReportText, Entity.LastPost.PostText, Entity.Reporter.Language.GetValueOrDefault());

            Dispatch d = new Dispatch();

            d.IncidentReportPost = Entity.LastPost;
            d.DispatchType = enDispatchType.Email;
            d.DispatchText = Entity.LastPost.PostText;
            d.DispatchSentAt = DateTime.Now;
            d.DispatchSentBy = Page.User.Identity.Name;

            UnitOfWork.MarkAsNew(d);
            UnitOfWork.MarkAsNew(email);

            Entity.ReportStatus = enReportStatus.Closed;
            Entity.LastPost.LastDispatch = d;

            Entity.LastDispatchedPostID = Entity.LastPostID;

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
