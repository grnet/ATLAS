using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Common
{
    public partial class ContactForm : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucHelpdeskContactFormInput.SetContactForm(Entity);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            IncidentReport ir = new IncidentReport();

            ucHelpdeskContactFormInput.FillContactForm(ir);

            ir.Reporter = Entity;

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}