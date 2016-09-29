using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class ViewOfficeAcademics : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            int officeID;
            if (int.TryParse(Request.QueryString["oID"], out officeID) && officeID > 0)
            {
                Entity = new InternshipOfficeRepository(UnitOfWork).Load(officeID, x => x.Academics);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            ucOfficeAcademicsGridView.DataSource = Entity.Academics;
            ucOfficeAcademicsGridView.DataBind();

            base.OnLoad(e);
        }
    }
}
