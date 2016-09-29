using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Microsoft.Data.Extensions;

namespace StudentPractice.Portal.Secure.Common
{
    public partial class HelpdeskContact : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Thread.CurrentPrincipal.Identity.Name);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string link = ResolveClientUrl("~/Secure/Common/ContactForm.aspx");
            lnkSubmitQuestion.NavigateUrl = link;
            lnkSubmitQuestion.Attributes["onclick"] = string.Format("popUp.show('{0}','{1}', cmdRefresh);", link, Resources.HelpdeskContact.HelpdeskQuestion);
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvIncidentReports.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvIncidentReports.PageIndex = 0;
            gvIncidentReports.DataBind();
        }

        protected void odsIncidentReports_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            IncidentReportCriteria criteria = new IncidentReportCriteria();

            criteria.Include(x => x.Reporter).Include(x => x.LastDispatchedPost.LastDispatch);

            criteria.Expression = criteria.Expression.Where(x => x.SubSystem.ID, StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID);
            criteria.Expression = criteria.Expression.Where(x => x.SubmissionType, enReportSubmissionType.LoggedInUser);
            criteria.Expression = criteria.Expression.Where(x => x.Reporter.ID, Entity.ID);

            e.InputParameters["criteria"] = criteria;
        }

        protected string GetIncidentTypeDetails(IncidentReport ir)
        {
            if (ir == null)
                return string.Empty;

            string incidentTypeDetails = string.Empty;
            IncidentType it = CacheManager.IncidentTypes.Get((int)ir.IncidentTypeReference.GetKey());
            incidentTypeDetails = string.Format("{0}", it.Name);
            return incidentTypeDetails;
        }

        protected string GetLastAnswer(IncidentReport ir)
        {
            if (ir == null)
                return string.Empty;

            string lastAnswer = string.Empty;
            if (ir.LastDispatchedPost != null && ir.LastDispatchedPost.LastDispatch != null)
            {
                Dispatch lastDispatch = ir.LastDispatchedPost.LastDispatch;
                lastAnswer = string.Format("<span style=\"font-size: 11px; font-weight: bold\">{0}</span><br />{1}<br/><br/>{2}", 
                    Resources.HelpdeskContact.AnswerDatetime,
                    lastDispatch.DispatchSentAt, 
                    lastDispatch.DispatchText);
            }
            return lastAnswer;
        }

        protected string GetContactHistoryLink(IncidentReport ir)
        {
            if (ir == null)
                return string.Empty;

            return string.Format("popUp.show('{0}?iID={1}','{2}', cmdRefresh);", 
                ResolveClientUrl("~/Secure/Common/ContactHistory.aspx"), 
                ir.ID,
                Resources.HelpdeskContact.HelpdeskQuestion);
        }
    }
}