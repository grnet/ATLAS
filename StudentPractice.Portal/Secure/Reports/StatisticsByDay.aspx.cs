using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.Secure.Reports
{
    public partial class StatisticsByDay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvStudents.SettingsPager.PageSize = int.MaxValue;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvePositions.FileName = String.Format("StatisticsByDay_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gvePositions.WriteXlsxToResponse(true);
        }
    }
}