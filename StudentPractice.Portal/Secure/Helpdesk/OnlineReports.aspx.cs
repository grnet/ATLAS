using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Xml.Linq;
using System.Web.Security;
using System.IO;
using System.Reflection;
using System.Xml;
using Imis.Domain;
using StudentPractice.Portal.Controls;
using StudentPractice.Mails;
using Microsoft.Data.Extensions;
using System.Web.Services;
using StudentPractice.Portal.Secure.Helpdesk.UserControls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class OnlineReports : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvIncidentReports.DefaultColumns = new List<UserControls.enIncidentReportsGridviewColumns>()
                {
                     enIncidentReportsGridviewColumns.CreatedAt,
                     enIncidentReportsGridviewColumns.LastPost_PostText,
                     enIncidentReportsGridviewColumns.Reporter_ReporterType,
                     enIncidentReportsGridviewColumns.ReporterName,
                     enIncidentReportsGridviewColumns.ReportStatus,
                     enIncidentReportsGridviewColumns.SpecialDetailsOfReporter,
                     enIncidentReportsGridviewColumns.ReportText,
                     enIncidentReportsGridviewColumns.Commands,
                     enIncidentReportsGridviewColumns.HandlerType
                };
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
            Criteria<IncidentReport> criteria = new Criteria<IncidentReport>();

            criteria.Include(x => x.Reporter).Include(x => x.LastPost.LastDispatch);

            criteria.Expression = criteria.Expression.Or(x => x.SubmissionType, enReportSubmissionType.Portal)
                                                     .Or(x => x.SubmissionType, enReportSubmissionType.LoggedInUser);

            criteria.Expression = criteria.Expression.Where(x => x.SubSystemID, StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID);

            int incidentReportID;
            if (int.TryParse(ucIncidentReportFilters.IncidentReportID, out incidentReportID) && incidentReportID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, incidentReportID);
            }

            int handlerType = ucIncidentReportFilters.HandlerType;
            if (handlerType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.HandlerType, (enHandlerType)handlerType);
            }

            int handlerStatus = ucIncidentReportFilters.HandlerStatus;
            if (handlerStatus > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.HandlerStatus, (enHandlerStatus)handlerStatus);
            }

            int reportStatus = ucIncidentReportFilters.ReportStatus;
            if (reportStatus > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ReportStatus, (enReportStatus)reportStatus);
            }

            int reporterType = ucIncidentReportFilters.ReporterType;
            if (reporterType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.Reporter.ReporterType, (enReporterType)reporterType);
            }

            int incidentType = ucIncidentReportFilters.IncidentType;
            if (incidentType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.IncidentTypeID, incidentType);
            }

            if (ucIncidentReportFilters.IncidentReportDateFrom != null)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, ucIncidentReportFilters.IncidentReportDateFrom.Value, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            }

            if (ucIncidentReportFilters.IncidentReportDateTo != null)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, ucIncidentReportFilters.IncidentReportDateTo.Value.Date.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }

            if (!string.IsNullOrEmpty(ucIncidentReportFilters.UpdatedBy))
            {
                criteria.Expression = criteria.Expression.Where(x => x.UpdatedBy, ucIncidentReportFilters.UpdatedBy);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvIncidentReports.Exporter.FileName = String.Format("IncidentReports_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gvIncidentReports.Exporter.WriteXlsxToResponse(true);
        }
    }
}
