using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.Controls;
using System.Web.Security;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditIncidentReportHandler : BaseEntityPortalPage<IncidentReport>
    {
        protected override void Fill()
        {
            Entity = new IncidentReportRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["irID"]), x => x.Reporter, y => y.IncidentType);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucIncidentReportHandlerInput.SetIncidentReportHandler(Entity);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucIncidentReportHandlerInput.FillIncidentReportHandler(Entity);

            if (Entity.HandlerType == enHandlerType.Helpdesk)
            {
                Entity.IncidentReportPosts.Load();

                List<IncidentReportPost> incidentReportPosts = Entity.IncidentReportPosts.ToList();

                foreach (IncidentReportPost post in incidentReportPosts)
                {
                    if (Roles.IsUserInRole(post.CreatedBy, RoleNames.Supervisor))
                    {
                        lblValidationErrors.Text = "Δεν μπορεί να αλλάξει ο χειριστής του Συμβάντος σε Γραφείο Αρωγής, γιατί έχει δοθεί τουλάχιστον μία απάντηση από την Ομάδα Παρακολούθησης";
                        return;
                    }
                }
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
