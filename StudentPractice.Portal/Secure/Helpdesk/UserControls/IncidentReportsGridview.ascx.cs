using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Xml.Linq;
using System.IO;
using System.ComponentModel;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxGridView.Export;
using System.Text;


namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Themeable(true)]
    public partial class IncidentReportsGridview : System.Web.UI.UserControl
    {
        #region [ Events ]

        public event EventHandler<DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs> CustomCallback;
        public event EventHandler<DevExpress.Web.ASPxGridView.ASPxGridViewCustomDataCallbackEventArgs> CustomDataCallback;

        #endregion

        #region [ Properties ]

        public string ClientInstanceName
        {
            get { return gvIncidentReports.ClientInstanceName; }
            set { gvIncidentReports.ClientInstanceName = value; }
        }

        public string DataSourceID
        {
            get
            {
                return gvIncidentReports.DataSourceID;
            }
            set { gvIncidentReports.DataSourceID = value; }
        }

        public int PageIndex
        {
            get { return gvIncidentReports.PageIndex; }
            set { gvIncidentReports.PageIndex = value; }
        }

        public string SearchControlID { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Browsable(false)]
        [DefaultValue("")]
        [TemplateContainer(typeof(GridViewDataItemTemplateContainer))]
        public ITemplate CustomTemplate { get; set; }

        public ASPxGridViewExporter Exporter { get { return gveIncidentReports; } }

        public event EventHandler<ASPxGridViewExportRenderingEventArgs> ExporterRenderBrick;

        #endregion

        #region [ Overrides ]

        protected override void OnLoad(EventArgs e)
        {
            var clm = gvIncidentReports.Columns["Commands"] as GridViewDataTextColumn;
            if (clm != null)
            {
                clm.DataItemTemplate = CustomTemplate;
            }
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            SetUItoDefault();
            HideHiddenColumns();
            base.OnPreRender(e);
        }

        #endregion

        protected void gveIncidentReports_RenderBrick(object sender, DevExpress.Web.ASPxGridView.Export.ASPxGridViewExportRenderingEventArgs e)
        {
            if (e.RowType == GridViewRowType.Data)
            {
                var ir = gvIncidentReports.GetRow(e.VisibleIndex) as IncidentReport;

                if (ir != null)
                {
                    switch (e.Column.Name)
                    {
                        case "CreatedAt":
                            e.TextValue = e.Text = GetReportDetails(ir, true).Replace("<br/>", "\n");
                            break;
                        case "Reporter.ReporterType":
                            IncidentType it = CacheManager.IncidentTypes.Get(ir.IncidentTypeID);
                            e.TextValue = e.Text = string.Format("ID: {0}\n{1}\n{2}\n{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name);
                            break;
                        case "ReporterName":
                            e.TextValue = e.Text = GetContactPersonDetails(ir.Reporter).Replace("<br/>", "\n");
                            break;
                        case "SpecialDetailsOfReporter":
                            e.TextValue = e.Text = GetReporterDetails(ir.Reporter).Replace("<br/>", "\n");
                            break;
                        case "ReportStatus":
                            e.TextValue = e.Text = ir.ReportStatus.GetLabel();
                            break;
                        case "LastPost.PostText":
                            if (ir.LastPost != null)
                            {
                                e.TextValue = e.Text = ir.LastPost.PostText;
                            }
                            break;
                        case "HandlerType":
                            e.TextValue = e.Text = ir.HandlerType.GetLabel();
                            break;
                        default:
                            break;
                    }
                }
            }

            if (ExporterRenderBrick != null)
                ExporterRenderBrick(sender, e);
        }

        protected void gvIncidentReports_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (CustomCallback != null)
                CustomCallback(gvIncidentReports, e);
        }

        protected void gvIncidentReports_CustomDataCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomDataCallbackEventArgs e)
        {
            if (CustomDataCallback != null)
                CustomDataCallback(gvIncidentReports, e);
        }

        public override void DataBind()
        {
            gvIncidentReports.DataBind();
        }

        #region [ Gridview Methods ]

        protected string GetReportDetails(IncidentReport ir, bool export)
        {
            if (ir == null)
                return string.Empty;

            StringBuilder reporterDetails = new StringBuilder();

            reporterDetails.AppendFormat("{0:dd/MM/yyyy HH:mm}<br/>", ir.CreatedAt);
            if (ir.SubmissionType == enReportSubmissionType.Helpdesk)
            {
                reporterDetails
                    .Append(ir.CreatedBy)
                    .Append("<br/>");                
            }

            if (!string.IsNullOrEmpty(ir.UpdatedBy))
            {
                reporterDetails.AppendFormat("<br/>{0}<br/>{1}<br/>{2:dd/MM/yyyy}", export ? "Τροποποίηση" : "<span style=\"font-size:11px; font-weight:bold\">Τροποποίηση</span>", ir.UpdatedBy, ir.UpdatedAt);
            }

            return reporterDetails.ToString();
        }

        protected string GetIncidentTypeDetails(IncidentReport ir)
        {
            if (ir == null)
                return string.Empty;
            string incidentTypeDetails = string.Empty;
            IncidentType it = CacheManager.IncidentTypes.Get(ir.IncidentTypeID);

            if (ir.Reporter is InternshipProvider)
            {
                if (ir.Reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
                {
                    incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/><a runat='server' style='font-weight: bold; color: Blue' target='_blank' href='SearchProviders.aspx?pID={4}'>{2}</a><br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID);
                }
                else
                {
                    incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/>{2}<br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID);
                }
            }
            else if (ir.Reporter is InternshipOffice)
            {
                if (ir.Reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
                {
                    incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/><a runat='server' style='font-weight: bold; color: Blue' target='_blank' href='SearchOffices.aspx?oID={4}&hideFilters=true&isChildAcount={5}'>{2}</a><br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID, !ir.Reporter.IsMasterAccount);
                }
                else
                {
                    incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/>{2}<br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID);
                }
            }
            else if (ir.Reporter is Student)
            {
                if (ir.Reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
                {
                    incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/><a runat='server' style='font-weight: bold; color: Blue' target='_blank' href='SearchStudents.aspx?sID={4}'>{2}</a><br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID);
                }
                else
                {
                    incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/>{2}<br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID);
                }
            }
            else
            {
                incidentTypeDetails = string.Format("ID: {0}<br/>{1}<br/>{2}<br/>{3}", ir.ID, ir.CallType.GetLabel(), ir.Reporter.ReporterType.GetLabel(), it.Name, ir.ReporterID);
            }

            return incidentTypeDetails;
        }

        protected string GetHandlerDetails(IncidentReport ir)
        {
            if (ir == null)
                return string.Empty;

            string handlerDetails = string.Empty;

            if (ir.HandlerType == enHandlerType.Helpdesk)
            {
                handlerDetails = string.Format("{0}", enHandlerType.Helpdesk.GetLabel());
            }
            else if (ir.HandlerType == enHandlerType.Supervisor)
            {
                if (ir.HandlerStatus == enHandlerStatus.Pending)
                {
                    handlerDetails = string.Format("{0}<br/><span style=\"color:Red; font-weight:bold\">{1}</span>", ir.HandlerType.GetLabel(), ir.HandlerStatus.GetLabel());
                }
                else if (ir.HandlerStatus == enHandlerStatus.Closed)
                {
                    handlerDetails = string.Format("{0}<br/><span style=\"color:Green; font-weight:bold\">{1}</span>", ir.HandlerType.GetLabel(), ir.HandlerStatus.GetLabel());
                }
            }

            return handlerDetails;
        }

        protected string GetContactPersonDetails(Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            string reporterDetails = string.Empty;

            reporterDetails = string.Format("{0}<br/>{1}<br/>{2}", reporter.ContactName, reporter.ContactMobilePhone ?? reporter.ContactPhone, reporter.ContactEmail);

            return reporterDetails;
        }

        protected string GetReporterDetails(object reporter)
        {
            if (reporter == null)
                return string.Empty;

            StringBuilder reporterDetails = new StringBuilder();

            if (reporter is InternshipProvider)
            {
                InternshipProvider provider = (InternshipProvider)reporter;

                if (!string.IsNullOrEmpty(provider.Name))
                {
                    reporterDetails.Append(provider.Name);
                }

                if (!string.IsNullOrEmpty(provider.TradeName))
                {
                    if (!string.IsNullOrEmpty(provider.Name))
                    {
                        reporterDetails.Append("<br/>");
                    }

                    reporterDetails.Append(provider.TradeName);
                }

                if (!string.IsNullOrEmpty(provider.AFM))
                {
                    if (!string.IsNullOrEmpty(provider.Name) || !string.IsNullOrEmpty(provider.TradeName))
                    {
                        reporterDetails.Append("<br/>");
                    }

                    reporterDetails.Append(provider.AFM);
                }
            }
            else if (reporter is InternshipOffice)
            {
                InternshipOffice office = (InternshipOffice)reporter;

                if (office.InstitutionID.HasValue)
                {
                    var institution = CacheManager.Institutions.Get(office.InstitutionID.Value);

                    reporterDetails.Append(string.Format("Ίδρυμα: {0}", institution.Name));
                }
            }
            else if (reporter is Student)
            {
                Student student = (Student)reporter;

                if (student.AcademicID.HasValue)
                {
                    var academic = CacheManager.Academics.Get(student.AcademicID.Value);

                    reporterDetails.Append(string.Format("ID Φοιτητή: {0}<br/>Ίδρυμα: {1}<br/>Σχολή: {2}<br/>Τμήμα: {3}", student.ID, academic.Institution, academic.School ?? "-", academic.Department ?? "-"));
                }
            }
            else if (reporter is FacultyMember)
            {
                FacultyMember facultyMember = (FacultyMember)reporter;

                if (facultyMember.InstitutionID.HasValue)
                {
                    var institution = CacheManager.Institutions.Get(facultyMember.InstitutionID.Value);

                    reporterDetails.Append(string.Format("Ίδρυμα: {0}", institution.Name));
                }
            }

            return reporterDetails.ToString();
        }

        #endregion

        private Control GetSearchControl()
        {
            if (string.IsNullOrEmpty(SearchControlID))
                return null;
            var ctrlToFind = Page.FindControlRecursive(SearchControlID);
            if (ctrlToFind == null)
                throw new ArgumentException(string.Format("Control with ID \"{0}\" was not found in the page.", SearchControlID), "SearchControlID");
            return ctrlToFind;
        }
    }
}