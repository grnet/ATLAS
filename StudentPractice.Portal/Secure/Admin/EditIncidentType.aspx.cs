using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class EditIncidentType : BaseEntityPortalPage<IncidentType>
    {
        protected override void Fill()
        {
            Entity = new IncidentTypeRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["itID"]), x => x.SubSystem, y => y.SubSystem.SubSystemReporterTypes, z => z.ReporterIncidentTypes);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucIncidentTypeInput.SetIncidentType(Entity);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucIncidentTypeInput.FillIncidentType(Entity);

            UnitOfWork.Commit();

            CacheManager.Refresh();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
