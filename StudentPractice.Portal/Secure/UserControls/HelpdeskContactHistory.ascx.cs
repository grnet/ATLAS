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
    public partial class HelpdeskContactHistory : BaseEntityUserControl<IncidentReport>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            txtReportText.Text = Entity.ReportText;

            rptIncidentReportPosts.DataSource = Entity.IncidentReportPosts.Where(x => x.LastDispatchID.HasValue).OrderByDescending(x => x.CreatedAt);
            rptIncidentReportPosts.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}