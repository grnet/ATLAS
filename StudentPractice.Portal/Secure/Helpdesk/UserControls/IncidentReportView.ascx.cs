using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class IncidentReportView : System.Web.UI.UserControl
    {
        protected IncidentReport CurrentIncidentReport { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetIncidentReport(IncidentReport incidentReport)
        {
            CurrentIncidentReport = incidentReport;

            DataBind();
        }

        public override void DataBind()
        {
            ltIncidentID.Text = CurrentIncidentReport.ID.ToString();
            ltReporterType.Text = CurrentIncidentReport.Reporter.GetLabel();
            if (CurrentIncidentReport.Reporter is Student)
            {
                Student student = (Student)CurrentIncidentReport.Reporter;

                ltStudentAFM.Text = student.StudentNumber;
                ltStudentLastName.Text = student.OriginalLastName;
                ltStudentLastName.Text = student.OriginalLastName;
            }

            ltIncidentType.Text = CurrentIncidentReport.IncidentType.Name;

            ltCreatedBy.Text = string.Format("{0}, {1:dd/MM/yyyy HH:mm}", CurrentIncidentReport.CreatedBy, CurrentIncidentReport.CreatedAt);

            if (CurrentIncidentReport.UpdatedBy != null)
            {
                trUpdatedBy.Visible = true;
                ltUpdatedBy.Text = string.Format("{0}, {1:dd/MM/yyyy HH:mm}", CurrentIncidentReport.UpdatedBy, CurrentIncidentReport.UpdatedAt);
            }
            else
            {
                trUpdatedBy.Visible = false;
            }

            ltReporterName.Text = CurrentIncidentReport.ReporterName;
            ltReporterPhone.Text = CurrentIncidentReport.ReporterPhone;
            ltReporterEmail.Text = CurrentIncidentReport.ReporterEmail;

            ltReportText.Text = CurrentIncidentReport.ReportText;
            ltReportStatus.Text = CurrentIncidentReport.ReportStatus.GetLabel();
        }
    }
}