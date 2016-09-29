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
using StudentPractice.Portal.DataSources;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchSupervisorReports : BaseEntityPortalPage
    {
        private IUnitOfWork unitOfWork = null;
        private Reporter _currentReporter = null;
        //private bool _bindDataWhenSearchIsClicked = false;


        protected Reporter CurrentReporter
        {
            get
            {
                if (_currentReporter != null)
                    return _currentReporter;
                try
                {
                    int reporterID = Convert.ToInt32(Request.QueryString["rID"]);
                    if (reporterID > 0)
                        _currentReporter = new ReporterRepository(unitOfWork).Load(reporterID);
                }
                catch (FormatException)
                {
                }

                return _currentReporter;
            }
        }

        protected bool HideFilters
        {
            get
            {
                bool hideFilters;

                bool.TryParse(Request.QueryString["hideFilters"], out hideFilters);

                return hideFilters;
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            IList<IncidentType> incidentTypes = CacheManager.IncidentTypes.GetItems();
            Page.ClientScript.RegisterForEventValidation(ddlIncidentType.UniqueID, string.Empty);
            foreach (IncidentType incidentType in incidentTypes)
            {
                Page.ClientScript.RegisterForEventValidation(ddlIncidentType.UniqueID, incidentType.ID.ToString());
            }

            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            unitOfWork = UnitOfWorkFactory.Create();
            if (HideFilters)
            {
                tbFilters.Visible = false;
                btnSearch.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                deIncidentReportDateFrom.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
                deIncidentReportDateTo.Attributes["onkeydown"] = string.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, string.Empty));
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
        }

        protected void ddlReportedBy_Init(object sender, EventArgs e)
        {
            ddlReportedBy.Items.Add(new ListItem("-- αδιάφορο --", ""));

            Users users = new Users();

            IList<MembershipUser> StudentTransfersUsers = users.FindUsersInRoles("%", new string[] { RoleNames.Helpdesk });

            foreach (MembershipUser user in StudentTransfersUsers)
            {
                ddlReportedBy.Items.Add(new ListItem(user.UserName, user.UserName));
            }
        }

        protected void ddlReportStatus_Init(object sender, EventArgs e)
        {
            ddlReportStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (enReportStatus item in Enum.GetValues(typeof(enReportStatus)))
            {
                ddlReportStatus.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlHandlerStatus_Init(object sender, EventArgs e)
        {
            ddlHandlerStatus.Items.Add(new ListItem("-- αδιάφορο --", ""));

            ddlHandlerStatus.Items.Add(new ListItem(enHandlerStatus.Pending.GetLabel(), ((int)enHandlerStatus.Pending).ToString()));
            ddlHandlerStatus.Items.Add(new ListItem(enHandlerStatus.Closed.GetLabel(), ((int)enHandlerStatus.Closed).ToString()));
        }

        protected void ddlCallType_Init(object sender, EventArgs e)
        {
            ddlCallType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (enCallType item in Enum.GetValues(typeof(enCallType)))
            {
                ddlCallType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlReporterType_Init(object sender, EventArgs e)
        {
            ddlReporterType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            //foreach (enReporterType item in AppUser.ReporterTypes)
            //{
            //    ddlReporterType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            //}
        }

        protected void ddlIncidentType_Init(object sender, EventArgs e)
        {
            ddlIncidentType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            //foreach (var item in AppUser.IncidentTypes)
            //{
            //    ddlIncidentType.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            //}
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
            //var ctrl = btnSearch;
            //if (ctrl != null && Request.Form["__EVENTTARGET"] == ctrl.UniqueID)
            //{
            //    if (!_bindDataWhenSearchIsClicked)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //}
            IncidentReportCriteria criteria = new IncidentReportCriteria();

            criteria.Include(x => x.Reporter).Include(x => x.LastPost.LastDispatch);

            criteria.Expression = criteria.Expression.Where(x => x.SubSystem.ID, StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID);
            criteria.Expression = criteria.Expression.Where(x => x.HandlerType, enHandlerType.Supervisor);

            if (CurrentReporter != null)
            {
                criteria.Expression = criteria.Expression.Where(x => x.Reporter.ID, CurrentReporter.ID);
            }

            int reportStatus;
            if (int.TryParse(ddlReportStatus.SelectedItem.Value, out reportStatus) && reportStatus > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ReportStatus, (enReportStatus)reportStatus);
            }

            if (!string.IsNullOrEmpty(ddlReportedBy.SelectedValue))
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedBy, ddlReportedBy.SelectedValue);
            }

            int reporterType;
            if (int.TryParse(ddlReporterType.SelectedItem.Value, out reporterType) && reporterType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.Reporter.ReporterType, (enReporterType)reporterType);
            }

            int incidentType;
            if (int.TryParse(ddlIncidentType.SelectedItem.Value, out incidentType) && incidentType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.IncidentType.ID, incidentType);
            }

            int callType;
            if (int.TryParse(ddlCallType.SelectedItem.Value, out callType) && callType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CallType, (enCallType)callType);
            }

            if (deIncidentReportDateFrom.Value != null)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, deIncidentReportDateFrom.Date, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            }

            if (deIncidentReportDateTo.Value != null)
            {
                criteria.Expression = criteria.Expression.Where(x => x.CreatedAt, deIncidentReportDateTo.Date.AddDays(1), Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            }

            int handlerStatus;
            if (int.TryParse(ddlHandlerStatus.SelectedItem.Value, out handlerStatus) && handlerStatus > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.HandlerStatus, (enHandlerStatus)handlerStatus);
            }


            e.InputParameters["criteria"] = criteria;
        }

    }
}
