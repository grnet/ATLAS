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
    public partial class AddIncidentReportPost : BaseEntityPortalPage<IncidentReport>
    {
        protected override void Fill()
        {
            Entity = new IncidentReportRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["id"]), x => x.Reporter, y => y.IncidentType);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Entity.ReportStatus != enReportStatus.Closed && (!Entity.IsLocked || DateTime.Now > Entity.LastLockAt.Value.AddMinutes(30)))
                {
                    Entity.IsLocked = true;
                    Entity.LastLockBy = User.Identity.Name;
                    Entity.LastLockAt = DateTime.Now;
                    UnitOfWork.Commit();
                }
                DataBind();
            }
        }

        public override void DataBind()
        {
            if (Entity != null)
            {
                if (Entity.IsLocked && Entity.LastLockBy != User.Identity.Name && DateTime.Now < Entity.LastLockAt.Value.AddMinutes(30))
                {
                    phAddPost.Visible = false;
                    phReportLocked.Visible = true;
                    lblReportLocked.Text = string.Format("Δεν μπορείτε να επεξεργαστείτε το συμβάν, γιατί κλειδώθηκε για επεξεργασία από το χρήστη {0} στις {1:dd/MM/yyyy HH:mm}<br/><br/>Σε περίπτωση που ο χρήστης το επεξεργάζεται το ακόμα ή τελείωσε την επεξεργασία, αλλά το συμβάν δεν ξεκλειδώθηκε (π.χ. γιατί έκλεισε τον browser απότομα), το συμβάν θα ξεκλειδωθεί αυτόματα στις {2:dd/MM/yyyy HH:mm} και θα έχετε δικαίωμα να το επεξεργαστείτε", Entity.LastLockBy, Entity.LastLockAt.Value, Entity.LastLockAt.Value.AddMinutes(30));
                }
                else
                {
                    phReportLocked.Visible = false;
                    phAddPost.Visible = true;

                    ucIncidentReportView.SetIncidentReport(Entity);
                    ucIncidentReportView.DataBind();

                    ucIncidentReportPostsView.DataSource = odsIncidentReportPosts;
                    ucIncidentReportPostsView.DataBind();

                    if (Entity.ReportStatus == enReportStatus.Closed)
                    {
                        lnkUnlockReport.Visible = true;

                        ucIncidentReportPostInput.Visible = false;
                        tbActions.Visible = false;
                    }
                    else
                    {
                        lnkUnlockReport.Visible = false;

                        ucIncidentReportPostInput.SetIncidentReportPost(Entity);
                        ucIncidentReportPostInput.DataBind();

                        ucIncidentReportPostInput.Visible = true;
                        tbActions.Visible = true;
                    }
                }
            }
        }

        protected void btnUnlockReport_Click(object sender, EventArgs e)
        {
            if (Entity.IsLocked && Entity.LastLockBy != User.Identity.Name && DateTime.Now < Entity.LastLockAt.Value.AddMinutes(30))
            {
                phAddPost.Visible = false;
                phReportLocked.Visible = true;
                lblReportLocked.Text = string.Format("Δεν μπορείτε να επεξεργαστείτε το συμβάν, γιατί κλειδώθηκε για επεξεργασία από το χρήστη {0} στις {1:dd/MM/yyyy HH:mm}<br/><br/>Σε περίπτωση που ο χρήστης το επεξεργάζεται το ακόμα ή τελείωσε την επεξεργασία, αλλά το συμβάν δεν ξεκλειδώθηκε (π.χ. γιατί έκλεισε τον browser απότομα), το συμβάν θα ξεκλειδωθεί αυτόματα στις {2:dd/MM/yyyy HH:mm} και θα έχετε δικαίωμα να το επεξεργαστείτε", Entity.LastLockBy, Entity.LastLockAt.Value, Entity.LastLockAt.Value.AddMinutes(30));
            }
            else
            {
                Entity.ReportStatus = enReportStatus.Pending;
                Entity.IsLocked = true;
                Entity.LastLockBy = User.Identity.Name;
                Entity.LastLockAt = DateTime.Now;
                UnitOfWork.Commit();
                DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucIncidentReportPostInput.FillIncidentReportPost(Entity);
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
