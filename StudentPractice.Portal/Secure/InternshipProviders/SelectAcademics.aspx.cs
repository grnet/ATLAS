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
using System.Text;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class SelectAcademics : BaseEntityPortalPage<InternshipPositionGroup>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["gID"], out groupID) && groupID > 0)
            {
                Entity = new InternshipPositionGroupRepository(UnitOfWork).Load(groupID, x => x.Academics);

                if (Entity == null)
                    ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

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

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            var selectedIds = GetSelectedId(gvAcademics);

            var academics = new List<Academic>();
            if (selectedIds.Count() != 0)
                academics.AddRange(new AcademicRepository(UnitOfWork).LoadMany(selectedIds));

            if (academics.Count != 0 && academics.Any(x => !string.IsNullOrEmpty(x.PositionRules)))
            {
                var builder = new StringBuilder();

                foreach (var item in academics)
                {
                    if (string.IsNullOrEmpty(item.PositionRules))
                        continue;

                    builder.Append("<h3>");
                    builder.Append(item.Institution + " - " + item.Department);
                    builder.Append("</h3>");
                    builder.Append("<pre class='rulesPreviewArea'>");
                    builder.Append(item.PositionRules);
                    builder.Append("</pre>");
                }

                rulesArea.InnerHtml = builder.ToString();
                ViewState["SelectedAcademicIDs"] = selectedIds;
                mv.SetActiveView(vAccept);
            }
            else
            {
                UpdateAcademics(selectedIds);
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = (List<int>)ViewState["SelectedAcademicIDs"];

            UpdateAcademics(selectedIds);

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        private void UpdateAcademics(IEnumerable<int> selectedIds)
        {
            Entity.Academics.Clear();

            foreach (var id in selectedIds)
            {
                var academic = new AcademicRepository(UnitOfWork).Load(id);

                Entity.Academics.Add(academic);
            }

            if (Entity.Academics.Count == 0)
                Entity.IsVisibleToAllAcademics = true;
            else
                Entity.IsVisibleToAllAcademics = false;


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
