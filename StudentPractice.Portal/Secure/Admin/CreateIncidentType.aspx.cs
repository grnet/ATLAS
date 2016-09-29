using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class CreateIncidentType : BaseEntityPortalPage<IncidentType>
    {
        protected override void Fill()
        {
            SubSystem s = new SubSystemRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["sID"]), x => x.SubSystemReporterTypes);

            Entity = new IncidentType();
            Entity.SubSystem = s;
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

            UnitOfWork.MarkAsNew(Entity);
            UnitOfWork.Commit();

            CacheManager.Refresh();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}