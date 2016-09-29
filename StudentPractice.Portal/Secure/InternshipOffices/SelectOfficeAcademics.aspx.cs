using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using Imis.Domain;
using System.Drawing;
using DevExpress.Web.ASPxGridView;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class SelectOfficeAcademics : BaseEntityPortalPage<InternshipOffice>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
            Entity.SaveToCurrentContext();
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            gvAcademics.DataSource = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == Entity.InstitutionID.Value && x.IsActive).ToList();
            gvAcademics.DataBind();
        }

        protected void gvAcademics_Databound(object sender, EventArgs e)
        {
            if (!Entity.CanViewAllAcademics.GetValueOrDefault())
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
                Entity.OfficeType = enOfficeType.Institutional;
                if (Entity.InstitutionID.HasValue)
                {
                    var academics = new AcademicRepository(UnitOfWork).FindByInstitutionID(Entity.InstitutionID.Value);
                    foreach (var item in academics)
                        Entity.Academics.Add(item);
                }
            }
            else if (Entity.Academics.Count == CacheManager.Academics.GetItems().Count(x => x.InstitutionID == Entity.InstitutionID.Value && x.IsActive))
            {
                Entity.CanViewAllAcademics = true;
                Entity.OfficeType = enOfficeType.Institutional;
            }
            else if (Entity.Academics.Count == 1)
            {
                Entity.CanViewAllAcademics = false;
                Entity.OfficeType = enOfficeType.Departmental;
            }
            else
            {
                Entity.CanViewAllAcademics = false;
                Entity.OfficeType = enOfficeType.MultipleDepartmental;
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
