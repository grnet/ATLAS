using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls
{
    public partial class PreAssignmentView : BaseEntityUserControl<InternshipPosition>
    {
        public override void Bind() {
            if (Entity == null)
                return;

            lblPreAssignedAt.Text = Entity.PreAssignedAt.HasValue ? Entity.PreAssignedAt.Value.ToString("dd/MM/yyyy") : string.Empty;            
        }

        protected void Page_Load(object sender, EventArgs e) {

        }
    }
}