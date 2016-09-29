using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class OfficeAcademicsAuthorization : BaseEntityPortalPage<InternshipOffice>
    {

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["oID"]), x => x.Academics);
        }

        protected void chbxlOfficeAcademics_Init(object sender, EventArgs e)
        {
            var academics = new List<Academic>();
            var masterOffice = new InternshipOfficeRepository(UnitOfWork).Load(Entity.MasterAccountID.Value, x => x.Academics);          

            if (masterOffice.CanViewAllAcademics.GetValueOrDefault())
            {
                academics = CacheManager.Academics.GetItems()
                    .Where(x => x.InstitutionID == masterOffice.InstitutionID.Value)
                    .OrderBy(x => x.Department)
                    .ToList();
            }
            else
            {
                academics = masterOffice.Academics
                    .OrderBy(x => x.Department)
                    .ToList();
            }

            foreach (var academic in academics)
            {
                ListItem listItem = new ListItem(academic.Department, academic.ID.ToString("D"));
                if (Entity.CanViewAllAcademics.GetValueOrDefault() || Entity.Academics.Any(x => x.ID == academic.ID))
                    listItem.Selected = true;
                else
                    listItem.Selected = false;

                chbxlOfficeAcademics.Items.Add(listItem);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var noAcademicSelected = true;

            foreach (ListItem item in chbxlOfficeAcademics.Items)
            {
                if (item.Selected)
                {
                    noAcademicSelected = false;
                }
            }

            if (noAcademicSelected)
            {
                lblErrors.Text = "Πρέπει να επιλέξετε τουλάχιστον ένα Τμήμα";
                return;
            }

            var masterOffice = Entity.MasterAccount as InternshipOffice;
            List<Academic> academics;

            if (masterOffice.CanViewAllAcademics.GetValueOrDefault())
            {
                academics = new AcademicRepository(UnitOfWork).FindByInstitutionID(masterOffice.InstitutionID.Value);
            }
            else
            {
                academics = masterOffice.Academics.ToList();
            }

            Entity.Academics.Clear();

            UnitOfWork.Commit();

            foreach (ListItem item in chbxlOfficeAcademics.Items)
            {
                if (item.Selected)
                {
                    var academic = academics.Where(x => x.ID == int.Parse(item.Value)).FirstOrDefault();

                    Entity.Academics.Add(academic);
                }
            }

            if (Entity.Academics.Count == 1)
            {
                Entity.OfficeType = enOfficeType.Departmental;
                Entity.CanViewAllAcademics = false;
            }
            else
            {
                if (masterOffice.OfficeType == enOfficeType.Institutional && Entity.Academics.Count() ==
                    CacheManager.Academics.GetItems().Count(x => x.InstitutionID == Entity.InstitutionID.Value && x.IsActive))
                {
                    Entity.OfficeType = enOfficeType.Institutional;
                    Entity.CanViewAllAcademics = true;
                    //Entity.Academics.Clear();
                }
                else
                {
                    Entity.OfficeType = enOfficeType.MultipleDepartmental;
                    Entity.CanViewAllAcademics = false;
                }
            }

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}