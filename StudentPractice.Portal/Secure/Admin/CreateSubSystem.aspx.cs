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
    public partial class CreateSubSystem : BaseEntityPortalPage<SubSystem>
    {
        protected override void Fill()
        {
            Entity = new SubSystem();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucSubSystemInput.FillSubSystem(Entity);

            UnitOfWork.MarkAsNew(Entity);
            UnitOfWork.Commit();

            CacheManager.Refresh();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}