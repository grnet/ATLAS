using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class StatisticsByOffice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gveStatisticsByOffice.FileName = String.Format("StatisticsByOffice_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveStatisticsByOffice.WriteXlsxToResponse(true);
        }
    }
}