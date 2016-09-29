using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class ReporterInput : BaseUserControl<BaseEntityPortalPage<Reporter>>
    {
        #region [ Control Inits ]

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
            txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";

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

        public void FillReporter(Reporter reporter)
        {
            int institutionID;

            if (reporter is InternshipProvider)
            {
                InternshipProvider provider = (InternshipProvider)reporter;

                if (!string.IsNullOrEmpty(txtProviderName.Text.ToNull()))
                {
                    provider.Name = txtProviderName.Text.ToNull();
                }

                if (!string.IsNullOrEmpty(txtProviderTradeName.Text.ToNull()))
                {
                    provider.TradeName = txtProviderTradeName.Text.ToNull();
                }

                if (!string.IsNullOrEmpty(txtProviderAFM.Text.ToNull())) {
                    provider.AFM = txtProviderAFM.Text.ToNull();
                }

                reporter = provider;
            }
            else if (reporter is InternshipOffice)
            {
                InternshipOffice office = (InternshipOffice)reporter;

                if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID))
                {
                    office.InstitutionID = institutionID;
                }
                else
                {
                    office.InstitutionID = null;
                }

                reporter = office;
            }
            else if (reporter is Student)
            {
                Student student = (Student)reporter;

                int academicID;
                if (int.TryParse(hfSchoolCode.Value, out academicID))
                {
                    student.AcademicID = academicID;
                }
                else
                {
                    student.AcademicID = null;
                }

                reporter = student;
            }
            else if (reporter is FacultyMember)
            {
                FacultyMember facultyMember = (FacultyMember)reporter;

                if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID))
                {
                    facultyMember.InstitutionID = institutionID;
                }
                else
                {
                    facultyMember.InstitutionID = null;
                }

                reporter = facultyMember;
            }
            else if (reporter is Other)
            {
                Other other = (Other)reporter;

                other.Description = txtDescription.Text.ToNull();

                reporter = other;
            }

            reporter.ContactName = txtContactName.Text.ToNull();

            var phone = txtContactPhone.Text.ToNull();
            if (!string.IsNullOrEmpty(phone))
            {
                if (phone.StartsWith("69"))
                {
                    reporter.ContactPhone = null;
                    reporter.ContactMobilePhone = phone;
                }
                else
                {
                    reporter.ContactPhone = phone;
                    reporter.ContactMobilePhone = null;
                }
            }
            else
            {
                reporter.ContactPhone = null;
                reporter.ContactMobilePhone = null;
            } 

            reporter.ContactEmail = txtContactEmail.Text.ToNull();
        }

        public void SetReporter(Reporter reporter)
        {
            if (reporter is InternshipProvider)
            {
                InternshipProvider provider = (InternshipProvider)reporter;

                lblReporterType.Text = enReporterType.InternshipProvider.GetLabel();

                txtProviderName.Text = provider.Name;
                txtProviderTradeName.Text = provider.TradeName;
                txtProviderAFM.Text = provider.AFM;

                tbProviderDetails.Visible = true;
            }
            else if (reporter is InternshipOffice)
            {
                InternshipOffice office = (InternshipOffice)reporter;

                lblReporterType.Text = enReporterType.InternshipOffice.GetLabel();

                if (office.InstitutionID.HasValue)
                {
                    ddlInstitution.SelectedValue = office.InstitutionID.Value.ToString();
                }

                tbInstitutionDetails.Visible = true;
            }
            else if (reporter is Student)
            {
                Student student = (Student)reporter;

                lblReporterType.Text = enReporterType.Student.GetLabel();

                if (student.AcademicID.HasValue)
                {
                    hfSchoolCode.Value = student.AcademicID.Value.ToString();

                    var academic = CacheManager.Academics.Get(student.AcademicID.Value);
                    txtInstitutionName.Text = academic.Institution;
                    txtSchoolName.Text = academic.School;
                    txtDepartmentName.Text = academic.Department;
                }

                tbAcademicDetails.Visible = true;
            }
            else if (reporter is FacultyMember)
            {
                FacultyMember facultyMember = (FacultyMember)reporter;

                lblReporterType.Text = enReporterType.FacultyMember.GetLabel();

                if (facultyMember.InstitutionID.HasValue)
                {
                    ddlInstitution.SelectedValue = facultyMember.InstitutionID.Value.ToString();
                }

                tbInstitutionDetails.Visible = true;
            }
            else if (reporter is Other)
            {
                Other other = (Other)reporter;

                lblReporterType.Text = enReporterType.Other.GetLabel();
                txtDescription.Text = other.Description;

                tbOtherDetails.Visible = true;
            }

            txtContactName.Text = reporter.ContactName;
            txtContactPhone.Text = reporter.ContactMobilePhone ?? reporter.ContactPhone;
            txtContactEmail.Text = reporter.ContactEmail;
        }

        protected void cvAFM_ServerValidate(object sender, ServerValidateEventArgs e) {
            e.IsValid = BusinessHelper.CheckAFM(e.Value);
        }
    }
}