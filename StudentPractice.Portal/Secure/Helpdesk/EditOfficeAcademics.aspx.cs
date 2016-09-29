using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using DevExpress.Web.ASPxGridView;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditOfficeAcademics : BaseEntityPortalPage<InternshipOffice>
    {
        private bool _isSuperHelpdesk;

        protected override void Fill()
        {
            int oID = -1;
            int.TryParse(Request.QueryString["oID"], out oID);
            Entity = new InternshipOfficeRepository(UnitOfWork).Load(oID, x => x.Academics);
            _isSuperHelpdesk = System.Web.Security.Roles.IsUserInRole(RoleNames.SuperHelpdesk);
        }

        protected override void OnInit(EventArgs e)
        {
            if (Entity != null && Entity.InstitutionID.HasValue)
                gvAcademics.DataSource = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == Entity.InstitutionID.Value).ToList();
            else
                gvAcademics.DataSource = new List<Academic>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!_isSuperHelpdesk)
                {
                    mv.SetActiveView(vCannotEdit);
                    lblErrors.Text = "Δεν έχετε το δικαίωμα να επεξεργαστείτε τις σχολές ενός ΓΠΑ.";
                }
                else if (Entity == null)
                {
                    mv.SetActiveView(vCannotEdit);
                    lblErrors.Text = "Δεν βρέθηκε το ΓΠΑ.";
                }
                else if (Entity.OfficeType != enOfficeType.Institutional)
                {
                    mv.SetActiveView(vCannotEdit);
                    lblErrors.Text = "Δεν μπορείτε να επεξεργαστείτε τις σχολές ενός μη-ιδρυματικού ΓΠΑ.";
                }
                else
                {
                    mv.SetActiveView(vGrid);
                    gvAcademics.DataBind();
                }
            }
        }

        protected void gvAcademics_Databound(object sender, EventArgs e)
        {
            for (int i = 0; i < gvAcademics.VisibleRowCount; i++)
            {
                int id = Convert.ToInt32(gvAcademics.GetRowValues(i, new[] { "ID" }));
                if (Entity.Academics.Any(x => x.ID == id))
                {
                    gvAcademics.Selection.SelectRow(i);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = new List<int>();
            selectedIds.AddRange(GetSelectedId(gvAcademics));

            Entity.Academics.Clear();

            foreach (var id in selectedIds)
            {
                var academic = new AcademicRepository(UnitOfWork).Load(id);

                Entity.Academics.Add(academic);
            }

            if (Entity.Academics.Count == 0)
            {
                Entity.CanViewAllAcademics = true;
                if (Entity.InstitutionID.HasValue)
                {
                    var academics = new AcademicRepository(UnitOfWork).FindByInstitutionID(Entity.InstitutionID.Value);
                    foreach (var item in academics)
                        Entity.Academics.Add(item);
                }
            }
            else if (Entity.Academics.Count == CacheManager.Academics.GetItems().Count(x => x.InstitutionID == Entity.InstitutionID.Value))
            {
                Entity.CanViewAllAcademics = true;
            }
            else
            {
                Entity.CanViewAllAcademics = false;
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        private List<int> GetSelectedId(ASPxGridView grid)
        {
            return grid.GetSelectedFieldValues("ID").OfType<int>().ToList();
        }
    }
}