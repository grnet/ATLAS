using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Drawing;

namespace StudentPractice.Portal.Secure.Reports {
    public partial class InternshipOfficeCounters : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            
        }

        protected void btnExport_Click(object sender, EventArgs e) {
            gveOfficeCounters.FileName = String.Format("InternshipOfficeCounters_{0}", DateTime.Now.ToString("yyyyMMdd"));
            gveOfficeCounters.WriteXlsxToResponse(true);
        }
    }
}