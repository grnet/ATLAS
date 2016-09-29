using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;


namespace StudentPractice.Portal.Secure.UserControls
{
    public partial class HelpdeskContactFormInput : BaseUserControl<BaseEntityPortalPage<Reporter>>
    {
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

        protected void ddlReporterType_Init(object sender, EventArgs e)
        {
            ddlReporterType.Items.Add(new ListItem(enReporterType.InternshipProvider.GetLabel(), ((int)enReporterType.InternshipProvider).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.InternshipOffice.GetLabel(), ((int)enReporterType.InternshipOffice).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.Student.GetLabel(), ((int)enReporterType.Student).ToString()));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtReporterName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
            }
        }

        public void FillContactForm(IncidentReport ir)
        {
            ir.SubSystemID = StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID;
            ir.IncidentTypeID = Convert.ToInt32(ddlIncidentType.SelectedValue);
            ir.SubmissionType = enReportSubmissionType.LoggedInUser;

            ir.ReporterName = txtReporterName.Text.ToNull();
            ir.ReporterPhone = txtReporterPhone.Text.ToNull();
            ir.ReporterEmail = txtReporterEmail.Text.ToNull();

            ir.ReportText = txtReportText.Text;
            ir.CallType = enCallType.Incoming;
            ir.ReportStatus = enReportStatus.Pending;
        }

        public void SetContactForm(Reporter reporter)
        {
            if (reporter is InternshipProvider)
            {
                InternshipProvider provider = (InternshipProvider)reporter;

                ddlReporterType.SelectedValue = ((int)enReporterType.InternshipProvider).ToString();
                txtReporterName.Text = provider.ContactName;
                txtReporterPhone.Text = provider.ContactMobilePhone;
                txtReporterEmail.Text = provider.ContactEmail;

                if (provider.CountryID != StudentPracticeConstants.GreeceCountryID)
                {
                    txtReporterPhone.MaxLength = 0;
                    revReporterPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                    revReporterPhone.ErrorMessage = Resources.HelpdeskContact.Form_PhoneRegex;
                    revReporterPhoneTip.Attributes.Add("title", Resources.HelpdeskContact.Form_PhoneRegex);
                }

            }
            else if (reporter is InternshipOffice)
            {
                InternshipOffice office = (InternshipOffice)reporter;

                ddlReporterType.SelectedValue = ((int)enReporterType.InternshipOffice).ToString();
                txtReporterName.Text = office.ContactName;
                txtReporterPhone.Text = office.ContactMobilePhone;
                txtReporterEmail.Text = office.ContactEmail;
            }
            else if (reporter is Student)
            {
                Student student = (Student)reporter;

                ddlReporterType.SelectedValue = ((int)enReporterType.Student).ToString();
                txtReporterName.Text = string.Format("{0} {1}", student.GreekFirstName ?? student.OriginalFirstName, student.GreekLastName ?? student.OriginalLastName);
                txtReporterPhone.Text = student.ContactMobilePhone;
                txtReporterEmail.Text = student.ContactEmail;
            }

            ddlReporterType.Enabled = false;
        }

        public string ValidationGroup
        {
            get
            {
                return rfvReporterName.ValidationGroup;
            }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }
    }
}