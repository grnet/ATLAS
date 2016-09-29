using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.DataSources;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class IncidentReportFilters : System.Web.UI.UserControl
    {
        public string ReportStatusClientID { get { return ddlReportStatus.ClientID; } }
        public string IncidentReportID { get { return txtIncidentReportID.Text.ToNull(); } }
        public int HandlerType { get { return int.Parse(ddlHandlerType.SelectedItem.Value); } }
        public int HandlerStatus { get { return int.Parse(ddlHandlerStatus.SelectedItem.Value); } }
        public int ReportStatus { get { return int.Parse(ddlReportStatus.SelectedItem.Value); } }
        public int ReporterType { get { return int.Parse(ddlReporterType.SelectedItem.Value); } }
        public int IncidentType
        {
            get
            {
                int incidentType;

                if (int.TryParse(ddlIncidentType.SelectedItem.Value, out incidentType))
                    return incidentType;
                else
                    return -1;
            }
        }
        public string UpdatedBy { get { return ddlUpdatedBy.SelectedValue; } }
        public DateTime? IncidentReportDateFrom
        {
            get
            {
                if (deIncidentReportDateFrom.Value != null)
                    return deIncidentReportDateFrom.Date;
                else
                    return null;
            }
        }
        public DateTime? IncidentReportDateTo
        {
            get
            {
                if (deIncidentReportDateTo.Value != null)
                    return deIncidentReportDateTo.Date;
                else
                    return null;
            }
        }
        public int CallType { get { return int.Parse(ddlCallType.SelectedItem.Value); } }
        public string ReportedBy { get { return ddlReportedBy.SelectedValue; } }
        public string ReporterPhone { get { return txtReporterPhone.Text.ToNull(); } }
        public string ReporterEmail { get { return txtReporterEmail.Text.ToNull(); } }

        public TextBox ReporterPhoneTextBox { get { return txtReporterPhone; } }
        public TextBox ReporterEmailTextBox { get { return txtReporterEmail; } }

        public bool HideFilters
        {
            set
            {
                tbFilters.Visible = !value;
            }
        }

        public bool HelpdeskReports
        {
            set
            {
                tdHelpdeskReports.Visible = value;
            }
        }

        #region [ Control Inits ]

        protected void ddlHandlerType_Init(object sender, EventArgs e)
        {
            ddlHandlerType.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            foreach (enHandlerType item in Enum.GetValues(typeof(enHandlerType)))
            {
                ddlHandlerType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlHandlerStatus_Init(object sender, EventArgs e)
        {
            ddlHandlerStatus.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            ddlHandlerStatus.Items.Add(new ListItem(enHandlerStatus.Pending.GetLabel(), ((int)enHandlerStatus.Pending).ToString()));
            ddlHandlerStatus.Items.Add(new ListItem(enHandlerStatus.Closed.GetLabel(), ((int)enHandlerStatus.Closed).ToString()));
        }

        protected void ddlReportStatus_Init(object sender, EventArgs e)
        {
            ddlReportStatus.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            foreach (enReportStatus item in Enum.GetValues(typeof(enReportStatus)))
            {
                ddlReportStatus.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlReporterType_Init(object sender, EventArgs e)
        {
            ddlReporterType.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            ddlReporterType.Items.Add(new ListItem(enReporterType.InternshipProvider.GetLabel(), ((int)enReporterType.InternshipProvider).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.InternshipOffice.GetLabel(), ((int)enReporterType.InternshipOffice).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.Student.GetLabel(), ((int)enReporterType.Student).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.FacultyMember.GetLabel(), ((int)enReporterType.FacultyMember).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.Other.GetLabel(), ((int)enReporterType.Other).ToString()));
        }

        protected void ddlIncidentType_Init(object sender, EventArgs e)
        {
            ddlIncidentType.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            var incidentTypes = CacheManager.IncidentTypes.GetItems().Where(x => x.SubSystemID == StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID).ToList();

            foreach (var item in incidentTypes)
            {
                ddlIncidentType.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected void ddlUpdatedBy_Init(object sender, EventArgs e)
        {
            ddlUpdatedBy.Items.Add(new ListItem("-- αδιάφορο --", ""));

            Users users = new Users();

            IList<MembershipUser> helpdeskUsers = users.FindUsersInRoles("%", new string[] { RoleNames.Helpdesk, RoleNames.SuperHelpdesk });

            foreach (MembershipUser user in helpdeskUsers)
            {
                ddlUpdatedBy.Items.Add(new ListItem(user.UserName, user.UserName));
            }
        }

        protected void ddlCallType_Init(object sender, EventArgs e)
        {
            ddlCallType.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            foreach (enCallType item in Enum.GetValues(typeof(enCallType)))
            {
                ddlCallType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlReportedBy_Init(object sender, EventArgs e)
        {
            ddlReportedBy.Items.Add(new ListItem("-- αδιάφορο --", ""));

            Users users = new Users();

            IList<MembershipUser> helpdeskUsers = users.FindUsersInRoles("%", new string[] { RoleNames.Helpdesk, RoleNames.SuperHelpdesk });

            foreach (MembershipUser user in helpdeskUsers)
            {
                ddlReportedBy.Items.Add(new ListItem(user.UserName, user.UserName));
            }
        }

        #endregion

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

        }
    }
}