using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using Microsoft.Data.Extensions;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditOffice : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["sID"]), x => x.Academics);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Entity.CanViewAllAcademics.HasValue && !Entity.CanViewAllAcademics.Value)
            {
                ucOfficeAcademicsGridView.DataSource = Entity.Academics;
                ucOfficeAcademicsGridView.DataBind();
            }
            else if (Entity.CanViewAllAcademics.HasValue && Entity.CanViewAllAcademics.Value)
            {
                ucOfficeAcademicsGridView.DataSource = new List<Academic>();
                ucOfficeAcademicsGridView.DataBind();
            }
            else
            {
                ucOfficeAcademicsGridView.Visible = false;
            }

            ucOfficeInput.Entity = Entity;
            if (!Page.IsPostBack)
                ucOfficeInput.Bind();

            base.OnLoad(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucOfficeInput.Fill(Entity);

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}