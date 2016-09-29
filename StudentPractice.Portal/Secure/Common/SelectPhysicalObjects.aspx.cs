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
using System.Threading;

namespace StudentPractice.Portal.Secure.Common
{
    public partial class SelectPhysicalObjects : BaseEntityPortalPage<InternshipPositionGroup>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                Entity = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.PhysicalObjects);

                if (Entity == null)
                    ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        protected void gvPhysicalObjects_Databound(object sender, EventArgs e)
        {
            for (int i = 0; i < gvPhysicalObjects.VisibleRowCount; i++)
            {
                int id = Convert.ToInt32(gvPhysicalObjects.GetRowValues(i, new[] { "ID" }));
                if (Entity.PhysicalObjects.Any(x => x.ID == id))
                {
                    gvPhysicalObjects.Selection.SelectRow(i);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = new List<int>();
            selectedIds.AddRange(GetSelectedId(gvPhysicalObjects));

            Entity.PhysicalObjects.Clear();

            foreach (var id in selectedIds)
            {
                var physicalObject = new PhysicalObjectRepository(UnitOfWork).Load(id);

                Entity.PhysicalObjects.Add(physicalObject);
            }

            Entity.UpdatedAt = DateTime.Now;
            Entity.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

            InternshipPositionGroupLog log = new InternshipPositionGroupLog();
            log.InternshipPositionGroup = Entity;
            log.OldStatus = Entity.PositionGroupStatus;
            log.NewStatus = Entity.PositionGroupStatus;
            log.CreatedAt = DateTime.Now;
            log.CreatedAtDateOnly = DateTime.Now.Date;
            log.CreatedBy = Thread.CurrentPrincipal.Identity.Name;
            log.UpdatedAt = DateTime.Now;
            log.UpdatedAtDateOnly = DateTime.Now.Date;
            log.UpdatedBy = Thread.CurrentPrincipal.Identity.Name;

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
