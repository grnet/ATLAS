using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Portal.Controls;
using System.Drawing;
using Microsoft.Data.Extensions;
using System.Text;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchReporters : BaseEntityPortalPage
    {
        #region [ Control Inits ]
        protected void ddlReporterType_Init(object sender, EventArgs e)
        {
            ddlReporterType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            ddlReporterType.Items.Add(new ListItem(enReporterType.InternshipProvider.GetLabel(), ((int)enReporterType.InternshipProvider).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.InternshipOffice.GetLabel(), ((int)enReporterType.InternshipOffice).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.Student.GetLabel(), ((int)enReporterType.Student).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.FacultyMember.GetLabel(), ((int)enReporterType.FacultyMember).ToString()));
            ddlReporterType.Items.Add(new ListItem(enReporterType.Other.GetLabel(), ((int)enReporterType.Other).ToString()));
        }

        protected void ddlDeclarationType_Init(object sender, EventArgs e)
        {
            ddlDeclarationType.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (enReporterDeclarationType item in Enum.GetValues(typeof(enReporterDeclarationType)))
            {
                ddlDeclarationType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtContactName.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtContactPhone.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtContactEmail.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtCertificationNumber.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtProviderName.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
                txtProviderAFM.Attributes["onkeydown"] = String.Format("Imis.Lib.EnterHandler(event, function(){{{0};}})",
                    Page.ClientScript.GetPostBackEventReference(btnSearch, String.Empty));
            }
        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvReporters.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvReporters.PageIndex = 0;
            gvReporters.DataBind();
        }

        protected void odsReporters_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!Page.IsPostBack)
                e.Cancel = true;

            ReporterCriteria criteria = new ReporterCriteria();

            bool criteriaEntered = false;

            if (!string.IsNullOrEmpty(txtContactName.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ContactName, txtContactName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);

                criteriaEntered = true;
            }

            if (!string.IsNullOrEmpty(txtContactPhone.Text))
            {
                criteria.Phone.FieldValue = txtContactPhone.Text.ToNull();

                criteriaEntered = true;
            }

            if (!string.IsNullOrEmpty(txtContactEmail.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.ContactEmail, txtContactEmail.Text.ToNull());

                criteriaEntered = true;
            }

            int reporterType;
            if (int.TryParse(ddlReporterType.SelectedItem.Value, out reporterType) && reporterType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ReporterTypeInt, reporterType);
                criteriaEntered = true;
            }
            else
            {
                criteria.Expression = criteria.Expression.Where(x => x.ReporterTypeInt, (int)enReporterType.InternshipProvider, Imis.Domain.EF.Search.enCriteriaOperator.GreaterThanEquals);
            }

            int reporterDeclarationType;
            if (int.TryParse(ddlDeclarationType.SelectedItem.Value, out reporterDeclarationType) && reporterDeclarationType > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.DeclarationTypeInt, reporterDeclarationType);
                criteriaEntered = true;
            }

            if (!string.IsNullOrEmpty(txtProviderName.Text))
            {
                criteria.ProviderName.FieldValue = txtProviderName.Text.ToNull();
                criteriaEntered = true;
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text))
            {
                criteria.ProviderAFM.FieldValue = txtProviderAFM.Text.ToNull();
                criteriaEntered = true;
            }

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID > 0)
            {
                criteria.InstitutionID.FieldValue = institutionID;
                criteriaEntered = true;
            }

            if (!criteriaEntered)
            {
                e.Cancel = true;
            }

            e.InputParameters["criteria"] = criteria;
        }

        protected void gvReporters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            Reporter reporter = e.Row.DataItem as Reporter;

            if (reporter != null)
            {
                if (reporter is InternshipProvider)
                {
                    InternshipProvider provider = (InternshipProvider)reporter;

                    if (reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
                    {
                        switch (provider.VerificationStatus)
                        {
                            case enVerificationStatus.NotVerified:
                                e.Row.BackColor = Color.DarkGray;
                                break;
                            case enVerificationStatus.Verified:
                                e.Row.BackColor = Color.LightGreen;
                                break;
                            case enVerificationStatus.CannotBeVerified:
                                e.Row.BackColor = Color.Tomato;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (reporter is InternshipOffice)
                {
                    InternshipOffice office = (InternshipOffice)reporter;

                    if (reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
                    {
                        switch (office.VerificationStatus)
                        {
                            case enVerificationStatus.NotVerified:
                                e.Row.BackColor = Color.DarkGray;
                                break;
                            case enVerificationStatus.Verified:
                                e.Row.BackColor = Color.LightGreen;
                                break;
                            case enVerificationStatus.CannotBeVerified:
                                e.Row.BackColor = Color.Tomato;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (reporter is Student)
                {
                    Student student = (Student)reporter;

                    if (reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
                    {
                        e.Row.BackColor = Color.LightGreen;
                    }
                }
            }
        }

        protected string GetReporterTypeDetails(Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            string reporterTypeDetails = string.Empty;

            if (reporter.DeclarationType == enReporterDeclarationType.FromRegistration)
            {
                if (reporter is InternshipProvider)
                {
                    reporterTypeDetails = string.Format("<a runat='server' style='font-weight: bold; color: Blue' target='_blank' href='SearchProviders.aspx?sID={1}'>{0}</a>", reporter.ReporterType.GetLabel(), reporter.ID);
                }
                else if (reporter is InternshipOffice)
                {
                    reporterTypeDetails = string.Format("<a runat='server' style='font-weight: bold; color: Blue' target='_blank' href='SearchOffices.aspx?sID={1}'>{0}</a>", reporter.ReporterType.GetLabel(), reporter.ID);
                }
                else if (reporter is Student)
                {
                    reporterTypeDetails = string.Format("<a runat='server' style='font-weight: bold; color: Blue' target='_blank' href='SearchStudents.aspx?sID={1}'>{0}</a>", reporter.ReporterType.GetLabel(), reporter.ID);
                }
            }
            else
            {
                reporterTypeDetails = string.Format("{0}", reporter.ReporterType.GetLabel());
            }

            return reporterTypeDetails;
        }

        protected string GetContactDetails(Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            StringBuilder contactDetails = new StringBuilder();

            if (!string.IsNullOrEmpty(reporter.ContactName))
            {
                contactDetails.Append(reporter.ContactName);

                if (!string.IsNullOrEmpty(reporter.ContactPhone) || !string.IsNullOrEmpty(reporter.ContactMobilePhone) || !string.IsNullOrEmpty(reporter.ContactEmail))
                {
                    contactDetails.Append("<br/>");
                }
            }

            if (!string.IsNullOrEmpty(reporter.ContactPhone) && !string.IsNullOrEmpty(reporter.ContactMobilePhone))
            {
                contactDetails.Append(string.Format("{0} / {1}", reporter.ContactPhone, reporter.ContactMobilePhone));

                if (!string.IsNullOrEmpty(reporter.ContactEmail))
                {
                    contactDetails.Append("<br/>");
                }
            }
            else if (!string.IsNullOrEmpty(reporter.ContactPhone))
            {
                contactDetails.Append(string.Format("{0}", reporter.ContactPhone));

                if (!string.IsNullOrEmpty(reporter.ContactEmail))
                {
                    contactDetails.Append("<br/>");
                }
            }
            else if (!string.IsNullOrEmpty(reporter.ContactMobilePhone))
            {
                contactDetails.Append(string.Format("{0}", reporter.ContactMobilePhone));

                if (!string.IsNullOrEmpty(reporter.ContactEmail))
                {
                    contactDetails.Append("<br/>");
                }
            }

            if (!string.IsNullOrEmpty(reporter.ContactEmail))
            {
                contactDetails.Append(reporter.ContactEmail);
            }

            return contactDetails.ToString();
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

                    if (student.DeclarationType == enReporterDeclarationType.FromRegistration)
                    {
                        reporterDetails.Append(string.Format("Αρ. Μητρώου: {0}<br/>Ίδρυμα: {1}<br/>Σχολή: {2}<br/>Τμήμα: {3}", student.StudentNumber, academic.Institution, academic.School ?? "-", academic.Department ?? "-"));
                    }
                    else
                    {
                        reporterDetails.Append(string.Format("Ίδρυμα: {0}<br/>Σχολή: {1}<br/>Τμήμα: {2}", academic.Institution, academic.School ?? "-", academic.Department ?? "-"));
                    }
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
            else if (reporter is Other)
            {
                Other other = (Other)reporter;

                if (!string.IsNullOrEmpty(other.Description))
                {
                    reporterDetails.Append(string.Format("Λοιπά Στοιχεία: {0}", other.Description));
                }
            }

            return reporterDetails.ToString();
        }

        protected string GetSearchIncidentReportLink(Reporter reporter)
        {
            if (reporter == null)
                return string.Empty;

            string link = string.Empty;

            link = string.Format("~/Secure/Helpdesk/SearchIncidentReports.aspx?hideFilters=true&rID={0}", reporter.ID);

            return link;
        }

        protected bool CanEditAccountDetails(Reporter reporter)
        {
            if (reporter == null)
                return false;

            return reporter.DeclarationType == enReporterDeclarationType.FromRegistration &&
                    !(reporter is Student);
        }

        protected bool CanEditReporter(Reporter reporter)
        {
            if (reporter == null)
                return false;

            return reporter.DeclarationType == enReporterDeclarationType.FromHelpdesk;
        }
    }
}