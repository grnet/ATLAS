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
using Microsoft.Data.Extensions;
using StudentPractice.Portal.Secure.Helpdesk.UserControls;
using System.Web.Services;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchIncidentReports : BaseEntityPortalPage<Reporter>
    {
        //private bool _bindDataWhenSearchIsClicked = false;
        protected enReporterType ReporterType
        {
            get
            {
                int typeInt;
                int.TryParse(Request.QueryString["t"], out typeInt);
                return (enReporterType)typeInt;
            }
        }

        private List<Reporter> _childAccounts = new List<Reporter>();

        protected bool HideFilters
        {
            get
            {
                bool hideFilters;

                bool.TryParse(Request.QueryString["hideFilters"], out hideFilters);

                return hideFilters;
            }
        }

        protected bool ShowAllAccounts
        {
            get
            {
                bool showAllAccounts;

                bool.TryParse(Request.QueryString["showAllAccounts"], out showAllAccounts);

                return showAllAccounts;
            }
        }

        protected override void Fill()
        {
            int rID = 0;
            int.TryParse(Request.QueryString["rID"], out rID);

            var rRep = new ReporterRepository(UnitOfWork);
            Entity = rRep.Load(rID);

            if (Entity != null && Entity.IsMasterAccount)
                _childAccounts = rRep.FindChildAccounts(Entity.ID);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HideFilters)
            {
                ucIncidentReportFilters.HideFilters = true;
                btnSearch.Visible = false;
                lnkReportIncident.Visible = false;
            }

            if (Entity != null)
            {
                lnkAddIncidentReportForReporter.Attributes.Add("onclick", string.Format("popUp.show('ReportIncident.aspx?rID={0}','Αναφορά Συμβάντος', cmdRefresh);", Entity.ID));
                lnkAddIncidentReportForReporter.Visible = true;
            }

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
                     // enIncidentReportsGridviewColumns.CallType,
                      enIncidentReportsGridviewColumns.HandlerType
                };

            if (!Page.IsPostBack)
            {
                ucIncidentReportFilters.ReporterPhoneTextBox.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                ucIncidentReportFilters.ReporterEmailTextBox.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvIncidentReports.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvIncidentReports.PageIndex = 0;
            //_bindDataWhenSearchIsClicked = true;
            gvIncidentReports.DataBind();
        }

        protected void odsIncidentReports_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<IncidentReport> criteria = new Criteria<IncidentReport>();

            criteria.Include(x => x.Reporter).Include(x => x.LastPost.LastDispatch);

            criteria.Expression = criteria.Expression.Where(x => x.SubSystemID, StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID);
            if (!HideFilters)
            {
                criteria.Expression = criteria.Expression.Where(x => x.SubmissionType, enReportSubmissionType.Helpdesk);
            }

            if (Entity != null)
            {
                if (ShowAllAccounts)
                {
                    var ids = new List<int>() { Entity.ID };
                    ids.AddRange(_childAccounts.Select(x => x.ID));
                    criteria.Expression = criteria.Expression.InMultiSet(x => x.ReporterID, ids);
                }
                else
                    criteria.Expression = criteria.Expression.Where(x => x.ReporterID, Entity.ID);
            }

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

            int callType = ucIncidentReportFilters.CallType;
            if (callType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CallTypeInt, callType);
            }

            if (!string.IsNullOrEmpty(ucIncidentReportFilters.ReportedBy))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedBy, ucIncidentReportFilters.ReportedBy);
            }

            if (!string.IsNullOrEmpty(ucIncidentReportFilters.ReporterPhone))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ReporterPhone, ucIncidentReportFilters.ReporterPhone);
            }

            if (!string.IsNullOrEmpty(ucIncidentReportFilters.ReporterEmail))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ReporterEmail, ucIncidentReportFilters.ReporterEmail);
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvIncidentReports.Exporter.FileName = String.Format("IncidentReports_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gvIncidentReports.Exporter.WriteXlsxToResponse(true);
        }

        protected bool CanEditIncidentReport(IncidentReport ir)
        {
            if (ir == null)
                return false;

            return ir.SubmissionType == enReportSubmissionType.Helpdesk;
        }
    }
}
