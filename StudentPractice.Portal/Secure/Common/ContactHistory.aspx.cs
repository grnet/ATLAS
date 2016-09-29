using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Common
{
    public partial class ContactHistory : BaseEntityPortalPage<IncidentReport>
    {
        protected override void Fill()
        {
            int incidentReportID;
            if (int.TryParse(Request.QueryString["iID"], out incidentReportID) && incidentReportID > 0)
            {
                Entity = new IncidentReportRepository(UnitOfWork).Load(incidentReportID, x => x.IncidentReportPosts);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucHelpdeskContactHistory.Entity = Entity;
                ucHelpdeskContactHistory.Bind();
            }
        }
    }
}