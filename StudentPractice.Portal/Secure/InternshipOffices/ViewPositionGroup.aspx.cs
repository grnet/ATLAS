using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class ViewPositionGroup : BaseEntityPortalPage<InternshipPositionGroup>
    {
        InternshipOffice CurrentOffice;

        protected override void Fill()
        {
            int positionID;
            if (int.TryParse(Request.QueryString["pID"], out positionID) && positionID > 0)
            {
                Entity = new InternshipPositionGroupRepository(UnitOfWork).Load(positionID, x => x.PhysicalObjects, x => x.Academics);
            }

            CurrentOffice = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.Academics);
        }

        protected override void OnLoad(EventArgs e)
        {
            ucPositionGroupView.Entity = Entity;

            if (CurrentOffice.CanViewAllAcademics.GetValueOrDefault())
            {
                ucPositionGroupView.UserAssociatedAcademics = CacheManager.Academics.GetItems().Where(x => x.InstitutionID == CurrentOffice.InstitutionID.Value && x.IsActive).ToList();
            }
            else
            {
                ucPositionGroupView.UserAssociatedAcademics = CurrentOffice.Academics.ToList();
            }

            ucPositionGroupView.Bind();

            base.OnLoad(e);
        }
    }
}
