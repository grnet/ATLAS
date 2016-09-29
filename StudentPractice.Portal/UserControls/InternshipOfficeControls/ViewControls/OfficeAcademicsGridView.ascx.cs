using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.UserControls.InternshipOfficeControls.ViewControls
{
    public partial class OfficeAcademicsGridView : System.Web.UI.UserControl
    {
        public object DataSource {
            get { return gvAcademics.DataSource; }
            set {
                gvAcademics.DataSource = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {

        }
    }
}