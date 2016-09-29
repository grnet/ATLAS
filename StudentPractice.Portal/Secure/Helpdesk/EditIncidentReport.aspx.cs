using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditIncidentReport : BaseEntityPortalPage<IncidentReport>
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
                //if (Entity.IsLocked && Entity.LastLockBy != User.Identity.Name && DateTime.Now < Entity.LastLockAt.Value.AddMinutes(30))
                //{
                //    phEditReport.Visible = false;
                //    lblReportLocked.Text = string.Format("Δεν μπορείτε να επεξεργαστείτε το συμβάν, γιατί κλειδώθηκε για επεξεργασία από το χρήστη {0} στις {1:dd/MM/yyyy HH:mm}<br/><br/>Σε περίπτωση που ο χρήστης το επεξεργάζεται το ακόμα ή τελείωσε την επεξεργασία, αλλά το συμβάν δεν ξεκλειδώθηκε (π.χ. γιατί έκλεισε τον browser απότομα), το συμβάν θα ξεκλειδωθεί αυτόματα στις {2:dd/MM/yyyy HH:mm} και θα έχετε δικαίωμα να το επεξεργαστείτε", Entity.LastLockBy, Entity.LastLockAt.Value, Entity.LastLockAt.Value.AddMinutes(30));
                //}
                //else
                //{
                    phReportLocked.Visible = false;
                    ucIncidentReportInput.SetIncidentReport(Entity, false);
                //}
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    if (!Entity.IsLocked || DateTime.Now > Entity.LastLockAt.Value.AddMinutes(30))
            //    {
            //        Entity.IsLocked = true;
            //        Entity.LastLockBy = User.Identity.Name;
            //        Entity.LastLockAt = DateTime.Now;
            //        UnitOfWork.Commit();
            //    }
            //}
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucIncidentReportInput.FillIncidentReport(Entity);
            Entity.IsLocked = false;

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Entity.IsLocked && Entity.LastLockBy == User.Identity.Name && DateTime.Now < Entity.LastLockAt.Value.AddMinutes(30))
            {
                Entity.IsLocked = false;
                UnitOfWork.Commit();
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
