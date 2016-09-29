using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using StudentPractice.Utils;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditProvider : BaseEntityPortalPage<InternshipProvider>
    {
        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["pID"]));
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ucProviderInput.Entity = Entity;
                ucProviderInput.Bind();
            }
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            ucProviderInput.Fill(Entity);

            if (Entity.ProviderType != enProviderType.PublicCarrier && new InternshipProviderRepository(UnitOfWork).IsAfmVerified(Entity.ID, Entity.AFM))
            {
                lblErrors.Visible = true;
                return;
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
