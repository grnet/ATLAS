using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.Secure.Helpdesk.UserControls
{
    public partial class IncidentReportPostsView : System.Web.UI.UserControl
    {
        public object DataSource
        {
            get { return rptIncidentReportPosts.DataSource; }
            set
            {
                rptIncidentReportPosts.DataSource = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}