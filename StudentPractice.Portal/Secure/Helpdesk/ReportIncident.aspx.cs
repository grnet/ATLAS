using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

using System.Web.Security;
using System.Web.Profile;
using Imis.Domain;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ReportIncident : BaseEntityPortalPage<IncidentReport>
    {
        private Reporter _currentReporter = null;
        protected Reporter CurrentReporter
        {
            get
            {
                if (_currentReporter != null)
                    return _currentReporter;

                try
                {
                    _currentReporter = new ReporterRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["rID"]));
                }
                catch (FormatException)
                {
                }

                return _currentReporter;
            }
        }

        protected override void Fill()
        {
            Entity = new IncidentReport();

            if (CurrentReporter != null)
            {
                Entity.Reporter = CurrentReporter;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucIncidentReportInput.SetIncidentReport(Entity, true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucIncidentReportInput.FillIncidentReport(Entity);
            Entity.SubSystemID = StudentPracticeConstants.DEFAULT_SUBSYSTEM_ID;
            Entity.SubmissionType = enReportSubmissionType.Helpdesk;

            if (Entity.IsNew)
            {
                UnitOfWork.MarkAsNew(Entity);
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
