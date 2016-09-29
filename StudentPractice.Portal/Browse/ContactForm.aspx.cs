using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Browse
{
    public partial class ContactForm : BaseEntityPortalPage<Reporter>
    {
        #region [ Control Inits ]

        protected void ddlReporterType_Init(object sender, EventArgs e)
        {
            ddlReporterType.Items.Add(new ListEditItem(Resources.HelpdeskContact.Form_ReporterPrompt, null));

            ddlReporterType.Items.Add(new ListEditItem(enReporterType.InternshipProvider.GetLabel(), ((int)enReporterType.InternshipProvider).ToString()));
            ddlReporterType.Items.Add(new ListEditItem(enReporterType.InternshipOffice.GetLabel(), ((int)enReporterType.InternshipOffice).ToString()));
            ddlReporterType.Items.Add(new ListEditItem(enReporterType.Student.GetLabel(), ((int)enReporterType.Student).ToString()));
            ddlReporterType.Items.Add(new ListEditItem(enReporterType.Other.GetLabel(), ((int)enReporterType.Other).ToString()));
        }

        protected void ddlIncidentType_Callback(object source, CallbackEventArgsBase e)
        {
            int reporterType;
            if (int.TryParse(e.Parameter, out reporterType) && reporterType > 0)
            {
                ddlIncidentType.FillIncidentTypes(reporterType);
            }
        }

        protected void ddlInstitution_Init(object sender, EventArgs e)
        {
            ddlInstitution.Items.Add(new ListEditItem(Resources.HelpdeskContact.Form_InstitutionPrompt, null));

            foreach (var item in CacheManager.GetOrderedInstitutions())
            {
                ddlInstitution.Items.Add(new ListEditItem(item.Name, item.ID));
            }
        }

        protected void ddlDepartment_Callback(object source, CallbackEventArgsBase e)
        {
            ddlDepartment.FillDepartments(e.Parameter);
        }

        protected void cbpAcademicDetails_Callback(object source, CallbackEventArgsBase e)
        {
            int reporterType;
            if (int.TryParse(e.Parameter, out reporterType) && reporterType > 0)
            {
                switch ((enReporterType)reporterType)
                {
                    case enReporterType.Student:
                        phAcademicDetails.Visible = true;
                        break;
                    case enReporterType.InternshipOffice:
                        phAcademicDetails.Visible = true;
                        trDepartment.Visible = false;
                        break;
                    default:
                        phAcademicDetails.Visible = false;
                        break;
                }

                cbpAcademicDetails.DataBind();
            }
        }

        #endregion

        #region [ Button Handlers ]

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            IncidentReport ir = new IncidentReport();
            Reporter reporter;

            int reporterType = ddlReporterType.GetSelectedInteger().Value;

            switch ((enReporterType)reporterType)
            {
                case enReporterType.InternshipProvider:
                    reporter = new InternshipProvider();
                    break;
                case enReporterType.InternshipOffice:
                    reporter = new InternshipOffice();
                    ((InternshipOffice)reporter).InstitutionID = ddlInstitution.GetSelectedInteger().Value;
                    break;
                case enReporterType.Student:
                    reporter = new Student();
                    ((Student)reporter).AcademicID = ddlDepartment.GetSelectedInteger().Value;
                    break;
                default:
                    reporter = new Other();
                    break;
            }

            reporter.DeclarationType = enReporterDeclarationType.FromPortal;
            reporter.Language = LanguageService.GetUserLanguage();
            reporter.UsernameFromLDAP = Guid.NewGuid().ToString();

            reporter.ContactName = txtContactName.GetText();

            var phone = txtContactPhone.GetText();
            if (phone.StartsWith("69"))
            {
                reporter.ContactMobilePhone = phone;
            }
            else
            {
                reporter.ContactPhone = phone;
            }
            reporter.ContactEmail = txtContactEmail.GetText();

            reporter.CreatedBy = "online";

            ir.SubmissionType = enReportSubmissionType.Portal;

            ir.Reporter = reporter;

            ir.SubSystemID = StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID;
            ir.IncidentTypeID = ddlIncidentType.GetSelectedInteger().Value;

            ir.CreatedBy = "online";

            ir.ReporterName = txtContactName.GetText();
            ir.ReporterPhone = txtContactPhone.GetText();
            ir.ReporterEmail = txtContactEmail.GetText();

            ir.ReportText = txtReportText.Text;
            ir.CallType = enCallType.Incoming;
            ir.ReportStatus = enReportStatus.Pending;

            UnitOfWork.MarkAsNew(ir);
            UnitOfWork.Commit();

            string emailGiven = txtContactEmail.GetText();

            var email = MailSender.SendIncidentReportSubmitConfirmation(reporter.ID, emailGiven, txtReportText.GetText(), LanguageService.GetUserLanguage());
            UnitOfWork.MarkAsNew(email);
            UnitOfWork.Commit();

            ltSentEmailConfirmation.Text = string.Format(Resources.HelpdeskContact.SentIncidentReportConfirmation, emailGiven);

            mvContact.SetActiveView(vComplete);
        }

        #endregion
    }
}


























//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using StudentPractice.BusinessModel;
//using StudentPractice.Mails;
//using StudentPractice.Portal.Controls;
//using DevExpress.Web.ASPxEditors;
//using DevExpress.Web.ASPxClasses;

//namespace StudentPractice.Portal.Browse
//{
//    public partial class ContactForm : BaseEntityPortalPage<Reporter>
//    {
//        #region [ Control Inits ]

//        protected void ddlReporterType_Init(object sender, EventArgs e)
//        {
//            ddlReporterType.Items.Add(new ListEditItem("-- επιλέξτε σε ποιά κατηγορία χρηστών ανήκετε --", ""));

//            ddlReporterType.Items.Add(new ListEditItem(enReporterType.InternshipProvider.GetLabel(), ((int)enReporterType.InternshipProvider).ToString()));
//            ddlReporterType.Items.Add(new ListEditItem(enReporterType.InternshipOffice.GetLabel(), ((int)enReporterType.InternshipOffice).ToString()));
//            ddlReporterType.Items.Add(new ListEditItem(enReporterType.Student.GetLabel(), ((int)enReporterType.Student).ToString()));
//            ddlReporterType.Items.Add(new ListEditItem(enReporterType.Other.GetLabel(), ((int)enReporterType.Other).ToString()));
//        }

//        protected void ddlIncidentType_Callback(object source, CallbackEventArgsBase e)
//        {
//            int reporterType;
//            if (int.TryParse(e.Parameter, out reporterType) && reporterType > 0)
//            {
//                ddlIncidentType.FillIncidentTypes(reporterType);
//            }
//        }

//        //protected void ddlInstitution_Init(object sender, EventArgs e)
//        //{
//        //    ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));
//        //    ddlInstitution.Items.AddRange(CacheManager.Institutions.GetItems()
//        //        .OrderBy(x => x.Name)
//        //        .Select(x => new ListItem(x.Name, x.ID.ToString()))
//        //        .ToArray());
//        //}

//        #endregion

//        #region [ Page Rendering ]

//        protected override void Render(System.Web.UI.HtmlTextWriter writer)
//        {
//            IList<IncidentType> incidentTypes = CacheManager.IncidentTypes.GetItems();

//            Page.ClientScript.RegisterForEventValidation(ddlIncidentType.UniqueID, string.Empty);

//            foreach (IncidentType incidentType in incidentTypes)
//            {
//                Page.ClientScript.RegisterForEventValidation(ddlIncidentType.UniqueID, incidentType.ID.ToString());
//            }

//            base.Render(writer);
//        }

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (!Page.IsPostBack)
//            {
//                int reporterID = Convert.ToInt32(!String.IsNullOrEmpty(Request.QueryString["rID"]) ? Convert.ToInt32(Request.QueryString["rID"]) : 0);
//                int reporterType = Convert.ToInt32(!String.IsNullOrEmpty(Request.QueryString["source"]) ? Convert.ToInt32(Request.QueryString["source"]) : 0);
//                int incidentType = Convert.ToInt32(!String.IsNullOrEmpty(Request.QueryString["type"]) ? Convert.ToInt32(Request.QueryString["type"]) : 0);

//                if (reporterID > 0)
//                {
//                    Reporter reporter = new ReporterRepository(UnitOfWork).Load(reporterID);

//                    txtReporterEmail.Text = reporter.ContactEmail;
//                }

//                if (reporterType > 0)
//                {
//                    ddlReporterType.SelectedValue = reporterType.ToString();
//                    ddlReporterType.Enabled = false;
//                }

//                if (incidentType > 0)
//                {
//                    ddlIncidentType.SelectedValue = incidentType.ToString();
//                    ddlIncidentType.Enabled = false;
//                }
//            }
//        }

//        #endregion

//        #region [ Validation ]

//        protected void cvValidateInstitution_ServerValidate(object source, ServerValidateEventArgs args)
//        {
//            if (ddlReporterType.SelectedItem.Value == enReporterType.InternshipOffice.ToString("D") ||
//                ddlReporterType.SelectedItem.Value == enReporterType.FacultyMember.ToString("D"))
//            {
//                args.IsValid = !string.IsNullOrEmpty(ddlInstitution.SelectedValue);
//            }
//        }

//        protected void cvValidateSchool_ServerValidate(object source, ServerValidateEventArgs args)
//        {
//            if (ddlReporterType.SelectedItem.Value == enReporterType.Student.ToString("D"))
//            {
//                args.IsValid = !string.IsNullOrWhiteSpace(txtInstitutionName.Text);
//            }
//        }

//        #endregion

//        #region [ Button Handlers ]

//        protected void lnkSend_Click(object sender, EventArgs e)
//        {
//            IncidentReport ir = new IncidentReport();
//            Reporter reporter;

//            int reporterType = Convert.ToInt32(ddlReporterType.SelectedValue);

//            switch ((enReporterType)reporterType)
//            {
//                case enReporterType.InternshipProvider:
//                    reporter = new InternshipProvider();
//                    break;
//                case enReporterType.InternshipOffice:
//                    reporter = new InternshipOffice();
//                    ((InternshipOffice)reporter).InstitutionID = int.Parse(ddlInstitution.SelectedItem.Value);
//                    break;
//                case enReporterType.Student:
//                    reporter = new Student();
//                    ((Student)reporter).AcademicID = int.Parse(hfSchoolCode.Value);
//                    break;
//                case enReporterType.FacultyMember:
//                    reporter = new FacultyMember();
//                    ((FacultyMember)reporter).InstitutionID = int.Parse(ddlInstitution.SelectedItem.Value);
//                    break;
//                case enReporterType.Other:
//                    reporter = new Other();
//                    break;
//                default:
//                    reporter = new Other();
//                    break;
//            }

//            reporter.DeclarationType = enReporterDeclarationType.FromPortal;
//            reporter.UsernameFromLDAP = Guid.NewGuid().ToString();

//            reporter.ContactName = txtReporterName.Text.ToNull();

//            var phone = txtReporterPhone.Text.ToNull();
//            if (phone.StartsWith("69"))
//            {
//                reporter.ContactMobilePhone = phone;
//            }
//            else
//            {
//                reporter.ContactPhone = phone;
//            }
//            reporter.ContactEmail = txtReporterEmail.Text.ToNull();

//            reporter.CreatedBy = "online";

//            ir.SubmissionType = enReportSubmissionType.Portal;

//            ir.Reporter = reporter;

//            ir.SubSystemID = StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID;
//            ir.IncidentTypeID = Convert.ToInt32(ddlIncidentType.SelectedValue);

//            ir.CreatedBy = "online";

//            ir.ReporterName = txtReporterName.Text.ToNull();
//            ir.ReporterPhone = txtReporterPhone.Text.ToNull();
//            ir.ReporterEmail = txtReporterEmail.Text.ToNull();

//            ir.ReportText = txtReportText.Text;
//            ir.CallType = enCallType.Incoming;
//            ir.ReportStatus = enReportStatus.Pending;

//            UnitOfWork.MarkAsNew(ir);
//            UnitOfWork.Commit();

//            string emailGiven = txtReporterEmail.Text.ToNull();

//            var email = MailSender.SendIncidentReportSubmitConfirmation(reporter.ID, emailGiven, txtReportText.Text, LanguageService.GetUserLanguage());
//            UnitOfWork.MarkAsNew(email);
//            UnitOfWork.Commit();

//            ltEmailGiven.Text = emailGiven;
//            mvContact.SetActiveView(vComplete);
//        }

//        #endregion
//    }
//}
