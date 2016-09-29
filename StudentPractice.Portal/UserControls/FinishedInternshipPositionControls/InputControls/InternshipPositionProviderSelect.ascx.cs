using DevExpress.Web.ASPxGridView;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentPractice.Portal.UserControls.FinishedInternshipPositionControls.InputControls
{
    public partial class InternshipPositionProviderSelect : BaseEntityUserControl<InternshipPositionGroup>
    {
        public override InternshipPositionGroup Fill(InternshipPositionGroup entity)
        {
            if (entity == null)
                entity = new InternshipPositionGroup();

            entity.ProviderID = GetSelectedId(gvProviders);

            return entity;
        }

        protected void gvProviders_Databound(object sender, EventArgs e)
        {
            for (int i = 0; i < gvProviders.VisibleRowCount; i++)
            {
                int id = Convert.ToInt32(gvProviders.GetRowValues(i, new[] { "ID" }));
                if (Entity != null && Entity.ProviderID == id)
                {
                    gvProviders.Selection.SelectRow(i);
                }
            }
        }

        public override void Bind()
        {
            if (Entity == null)
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                txtProviderID.Text = Entity.ProviderID.ToString();
                gvProviders.DataBind();
            }

        }

        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvProviders.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e)
        {
            gvProviders.PageIndex = 0;
            gvProviders.DataBind();
        }

        private int GetSelectedId(ASPxGridView grid)
        {
            return grid.GetSelectedFieldValues("ID").OfType<int>().FirstOrDefault();
        }

        protected void odsProviders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<InternshipProvider> criteria = new Criteria<InternshipProvider>();

            criteria.Expression = criteria.Expression.Where(x => x.DeclarationType, enReporterDeclarationType.FromRegistration);
            criteria.Expression = criteria.Expression.Where(x => x.VerificationStatus, enVerificationStatus.Verified);

            int providerID;
            if (int.TryParse(txtProviderID.Text.ToNull(), out providerID) && providerID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.ID, providerID);
            }

            if (!string.IsNullOrEmpty(txtProviderAFM.Text))
            {
                criteria.Expression = criteria.Expression.Where(x => x.AFM, txtProviderAFM.Text.ToNull());
            }

            if (!string.IsNullOrEmpty(txtProviderName.Text))
            {
                var or1CreationExpression = Imis.Domain.EF.Search.Criteria<InternshipProvider>.Empty;
                var or2CreationExpression = Imis.Domain.EF.Search.Criteria<InternshipProvider>.Empty;
                or1CreationExpression = or1CreationExpression.Where(x => x.Name, txtProviderName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                or2CreationExpression = or2CreationExpression.Where(x => x.TradeName, txtProviderName.Text.ToNull(), Imis.Domain.EF.Search.enCriteriaOperator.Like);
                or1CreationExpression = or1CreationExpression.Or(or2CreationExpression);
                criteria.Expression = criteria.Expression.And(or1CreationExpression);
            }


            gvProviders.Visible = !string.IsNullOrEmpty(txtProviderID.Text.ToNull()) || !string.IsNullOrEmpty(txtProviderAFM.Text.ToNull()) || !string.IsNullOrEmpty(txtProviderName.Text.ToNull());
            lblNoSearchCriteria.Visible = string.IsNullOrEmpty(txtProviderID.Text.ToNull()) && string.IsNullOrEmpty(txtProviderAFM.Text.ToNull()) && string.IsNullOrEmpty(txtProviderName.Text.ToNull()); ;

            e.InputParameters["criteria"] = criteria;
        }
    }
}