using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Xml.Linq;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Portal.Controls;
using Imis.Domain;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class IncidentReportInput : BaseUserControl<BaseEntityPortalPage<IncidentReport>>
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

        protected void ddlCallType_Init(object sender, EventArgs e)
        {
            ddlCallType.Items.Add(new ListItem("-- επιλέξτε τύπο κλήσης --", ""));

            foreach (enCallType item in Enum.GetValues(typeof(enCallType)))
            {
                ddlCallType.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }
        }

        protected void ddlReportStatus_Init(object sender, EventArgs e)
        {
            foreach (enReportStatus item in Enum.GetValues(typeof(enReportStatus)))
            {
                ddlReportStatus.Items.Add(new ListItem(item.GetLabel(), ((int)item).ToString()));
            }

            ddlReportStatus.SelectedValue = ((int)enReportStatus.Answered).ToString();
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListItem("-- επιλέξτε ίδρυμα --", ""));

            foreach (var item in CacheManager.Institutions.GetItems())
            {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
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
            txtReporterName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "jsInit", string.Format("hd.init('{0}','{1}','{2}','{3}');", hfSchoolCode.ClientID, txtInstitutionName.ClientID, txtSchoolName.ClientID, txtDepartmentName.ClientID), true);

            txtInstitutionName.Attributes.Add("readonly", "readonly");
            txtSchoolName.Attributes.Add("readonly", "readonly");
            txtDepartmentName.Attributes.Add("readonly", "readonly");

            txtInstitutionName.Attributes.Add("onclick", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής', null, 750, 560);");
            txtInstitutionName.Attributes.Add("onfocus", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής', null, 750, 560);");

            txtSchoolName.Attributes.Add("onclick", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής', null, 750, 560);");
            txtSchoolName.Attributes.Add("onfocus", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής', null, 750, 560);");

            txtDepartmentName.Attributes.Add("onclick", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής', null, 750, 560);");
            txtDepartmentName.Attributes.Add("onfocus", "popUp.show('../../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής', null, 750, 560);");
        }

        public void FillIncidentReport(IncidentReport ir)
        {
            if (ir.IsNew)
            {
                ir.CreatedBy = Page.User.Identity.Name.ToLower().Trim();
                ir.ReportStatus = enReportStatus.Pending;
            }
            else
            {
                ir.UpdatedBy = Page.User.Identity.Name.ToLower().Trim();
                ir.UpdatedAt = DateTime.Now;
            }

            int institutionID;

            if (ir.Reporter == null)
            {
                switch ((enReporterType)Convert.ToInt32(ddlReporterType.SelectedValue))
                {
                    case enReporterType.InternshipProvider:
                        InternshipProvider provider = new InternshipProvider();
                        provider.UsernameFromLDAP = Guid.NewGuid().ToString();

                        if (!string.IsNullOrEmpty(txtProviderName.Text.ToNull()))
                        {
                            provider.Name = txtProviderName.Text.ToNull();
                        }

                        if (!string.IsNullOrEmpty(txtProviderTradeName.Text.ToNull()))
                        {
                            provider.TradeName = txtProviderTradeName.Text.ToNull();
                        }

                        if (!string.IsNullOrEmpty(txtProviderAFM.Text.ToNull()))
                        {
                            provider.AFM = txtProviderAFM.Text.ToNull();
                        }

                        ir.Reporter = provider;
                        break;
                    case enReporterType.InternshipOffice:
                        InternshipOffice office = new InternshipOffice();
                        office.UsernameFromLDAP = Guid.NewGuid().ToString();

                        if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID))
                        {
                            office.InstitutionID = institutionID;
                        }

                        ir.Reporter = office;
                        break;
                    case enReporterType.Student:
                        Student student = new Student();
                        student.UsernameFromLDAP = Guid.NewGuid().ToString();

                        int academicID;
                        if (int.TryParse(hfSchoolCode.Value, out academicID))
                        {
                            student.AcademicID = academicID;
                        }

                        ir.Reporter = student;
                        break;
                    case enReporterType.FacultyMember:
                        FacultyMember facultyMember = new FacultyMember();
                        facultyMember.UsernameFromLDAP = Guid.NewGuid().ToString();

                        if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID))
                        {
                            facultyMember.InstitutionID = institutionID;
                        }

                        ir.Reporter = facultyMember;
                        break;
                    case enReporterType.Other:
                        Other other = new Other();
                        other.UsernameFromLDAP = Guid.NewGuid().ToString();
                        other.Description = txtDescription.Text.ToNull();
                        ir.Reporter = other;
                        break;
                    default:
                        break;
                }

                ir.Reporter.DeclarationType = enReporterDeclarationType.FromHelpdesk;
            }

            enReporterType reporterType = (enReporterType)int.Parse(ddlReporterType.SelectedItem.Value);

            var phone = txtReporterPhone.Text.ToNull();

            if (ir.Reporter.DeclarationType == enReporterDeclarationType.FromHelpdesk)
            {
                ir.Reporter.ContactName = txtReporterName.Text.ToNull();

                if (!string.IsNullOrEmpty(phone))
                {
                    if (phone.StartsWith("69"))
                    {
                        ir.Reporter.ContactPhone = null;
                        ir.Reporter.ContactMobilePhone = phone;
                    }
                    else
                    {
                        ir.Reporter.ContactPhone = phone;
                        ir.Reporter.ContactMobilePhone = null;
                    }
                }
                else
                {
                    ir.Reporter.ContactPhone = null;
                    ir.Reporter.ContactMobilePhone = null;
                }

                ir.Reporter.ContactEmail = txtReporterEmail.Text.ToNull();
            }

            ir.IncidentTypeID = Convert.ToInt32(ddlIncidentType.SelectedValue);

            ir.ReporterName = txtReporterName.Text.ToNull();
            ir.ReporterPhone = phone;
            ir.ReporterEmail = txtReporterEmail.Text.ToNull();

            ir.CallType = (enCallType)Convert.ToInt32(ddlCallType.SelectedValue);
            ir.ReportStatus = (enReportStatus)Convert.ToInt32(ddlReportStatus.SelectedValue);
            ir.ReportText = txtReportText.Text.ToNull();
        }

        public void SetIncidentReport(IncidentReport ir, bool isNew)
        {
            if (ir.Reporter != null)
            {
                tbProviderDetails.Visible = false;
                tbAcademicDetails.Visible = false;
                tbInstitutionDetails.Visible = false;

                if (ir.Reporter is InternshipProvider)
                {
                    InternshipProvider provider = (InternshipProvider)ir.Reporter;

                    lblProviderAFM.Text = provider.AFM;
                    lblProviderName.Text = provider.Name;
                    lblProviderTradeName.Text = provider.TradeName;

                    ddlReporterType.SelectedValue = ((int)enReporterType.InternshipProvider).ToString();

                    tbRegisteredProviderDetails.Visible = true;

                    if (provider.CountryID != StudentPracticeConstants.GreeceCountryID)
                    {
                        txtReporterPhone.MaxLength = 0;
                        valReporterPhone.ValidationExpression = "^\\+?[0-9]{3}-?[0-9]{0,20}$";
                        valReporterPhone.Text = valReporterPhone.ErrorMessage = "Πρέπει να αποτελείται από τουλάχιστον 3 αριθμούς ή και το προαιρετικό '+' στην αρχή";
                    }
                }
                else if (ir.Reporter is InternshipOffice)
                {
                    InternshipOffice office = (InternshipOffice)ir.Reporter;

                    var institution = CacheManager.Institutions.Get(office.InstitutionID.Value);

                    lblRegisteredUserInstitution.Text = institution.Name;

                    switch (office.OfficeType)
                    {
                        case enOfficeType.Departmental:
                            rowRegisteredUserAcademic.Visible = true;
                            cellRegisteredUserSingleAcademic.Visible = true;

                            if (!office.Academics.IsLoaded)
                                office.Academics.Load();

                            var academic = CacheManager.Academics.Get(office.Academics.FirstOrDefault().ID);
                            lblRegisteredUserAcademic.Text = academic.Department;
                            break;
                        case enOfficeType.MultipleDepartmental:
                            rowRegisteredUserAcademic.Visible = true;
                            cellRegisteredUserMultipleAcademic.Visible = true;
                            litRegisteredUserMultipleAcademic.Text = string.Format("<a href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={0}\",\"Προβολή Σχολών/Τμημάτων\", null, 750, 560)'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", office.ID);
                            break;
                        case enOfficeType.Institutional:
                            if (!office.CanViewAllAcademics.GetValueOrDefault())
                            {
                                rowRegisteredUserAcademic.Visible = true;
                                cellRegisteredUserMultipleAcademic.Visible = true;
                                litRegisteredUserMultipleAcademic.Text = string.Format("<a href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={0}\",\"Προβολή Σχολών/Τμημάτων\", null, 750, 560)'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", office.ID);
                            }
                            break;
                        case enOfficeType.None:
                        default:
                            break;
                    }

                    ddlReporterType.SelectedValue = ((int)enReporterType.InternshipOffice).ToString();

                    tbOfficeDetails.Visible = true;
                    tbRegisteredUserInstitutionDetails.Visible = true;
                }
                else if (ir.Reporter is Student)
                {
                    Student student = (Student)ir.Reporter;

                    lblStudentName.Text = string.Format("{0} {1}", student.OriginalFirstName, student.OriginalLastName);
                    lblStudentNumber.Text = student.StudentNumber;

                    var academic = CacheManager.Academics.Get(student.AcademicID.Value);

                    lblInstitution.Text = academic.Institution;
                    lblSchool.Text = academic.School ?? "-";
                    lblDepartment.Text = academic.Department ?? "-";

                    ddlReporterType.SelectedValue = ((int)enReporterType.Student).ToString();

                    tbStudentDetails.Visible = true;
                    tbRegisteredUserAcademicDetails.Visible = true;
                }
                else if (ir.Reporter is FacultyMember)
                {
                    FacultyMember facultyMember = (FacultyMember)ir.Reporter;

                    var institution = CacheManager.Institutions.Get(facultyMember.InstitutionID.Value);

                    lblRegisteredUserInstitution.Text = institution.Name;

                    ddlReporterType.SelectedValue = ((int)enReporterType.FacultyMember).ToString();

                    tbFacultyMemberDetails.Visible = true;
                    tbRegisteredUserInstitutionDetails.Visible = true;
                }
                else if (ir.Reporter is Other)
                {
                    Other other = (Other)ir.Reporter;

                    txtDescription.Text = other.Description;

                    ddlReporterType.SelectedValue = ((int)enReporterType.Other).ToString();
                }

                ddlReporterType.Enabled = false;

                txtReporterName.Text = ir.Reporter.ContactName;
                txtReporterPhone.Text = ir.Reporter.ContactMobilePhone ?? ir.Reporter.ContactPhone;
                txtReporterEmail.Text = ir.Reporter.ContactEmail;

                ddlCallType.SelectedValue = ((int)ir.CallType).ToString();

                if (!isNew)
                {
                    ddlIncidentType.SelectedValue = ir.IncidentType.ID.ToString();
                    cddIncidentType.SelectedValue = ir.IncidentType.ID.ToString();

                    ddlReportStatus.SelectedValue = ((int)ir.ReportStatus).ToString();
                    txtReportText.Text = ir.ReportText;
                }
                else
                {
                    ddlReportStatus.SelectedValue = ((int)enReportStatus.Pending).ToString();
                }
            }
            else
            {
                txtReporterPhone.MaxLength = 0;
                valReporterPhone.Visible = false;
            }
        }

        protected void cvAFM_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = BusinessHelper.CheckAFM(e.Value);
        }
    }
}