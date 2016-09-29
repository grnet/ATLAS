using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Web.Security;

namespace StudentPractice.Portal.Secure.Helpdesk {
    public partial class EditAcademic : BaseEntityPortalPage<Academic> {
        protected override void Fill() {
            Entity = new AcademicRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["aID"]));
        }

        protected override void OnPreRender(EventArgs e) {
            if (!Page.IsPostBack) {
                lblInstitution.Text = Entity.Institution;
                lblSchool.Text = Entity.School ?? "-";
                lblDepartment.Text = Entity.Department;
                lblPreAssignedPositions.Text = Entity.PreAssignedPositions.ToString();
                lblMaxAllowedPreAssignedPositions.Text = Entity.MaxAllowedPreAssignedPositions.ToString();
            }

            base.OnPreRender(e);
        }


        protected void Page_Load(object sender, EventArgs e) {
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e) {
            if (!Page.IsValid)
                return;

            int maxAllowedPreAssignedPositions;
            if (int.TryParse(txtNewMaxAllowedPreAssignedPositions.Text.ToNull(), out maxAllowedPreAssignedPositions) && maxAllowedPreAssignedPositions > 0) {
                if (Entity.MaxAllowedPreAssignedPositions != maxAllowedPreAssignedPositions)
                    Entity.MaxAllowedPreAssignedPositions = maxAllowedPreAssignedPositions;
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
