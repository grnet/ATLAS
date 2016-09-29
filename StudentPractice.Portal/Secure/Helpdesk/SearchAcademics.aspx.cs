using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure.Helpdesk {
    public partial class SearchAcademics : BaseEntityPortalPage<object> {
        protected void ddlInstitution_Init(object sender, EventArgs e) {
            ddlInstitution.Items.Add(new ListItem("-- αδιάφορο --", ""));

            foreach (var item in CacheManager.Institutions.GetItems()) {
                ddlInstitution.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer) {
            Page.ClientScript.RegisterForEventValidation(ddlDepartment.UniqueID, string.Empty);

            IList<Academic> academics = CacheManager.Academics.GetItems();
            foreach (Academic academic in academics) {
                Page.ClientScript.RegisterForEventValidation(ddlDepartment.UniqueID, academic.ID.ToString());
            }
            base.Render(writer);
        }

        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e) {
            gvAcademics.DataBind();
        }

        public void btnSearch_Click(object sender, EventArgs e) {
            gvAcademics.PageIndex = 0;
            gvAcademics.DataBind();
        }

        protected string GetAcademicRulesLink(Academic academic)
        {
            if (academic == null || string.IsNullOrEmpty(academic.PositionRules))
                return string.Empty;

            return string.Format("<a href='javascript:void(0);' class='btn-academicRules' data-aid='{0}'><img src='/_img/iconInformation.png' alt='Περιγραφή Πρακτικής Άσκησης Τμημάτων' /></a>", academic.ID);
        }

        protected void odsAcademics_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
            Criteria<Academic> criteria = new Criteria<Academic>();

            int institutionID;
            if (int.TryParse(ddlInstitution.SelectedItem.Value, out institutionID) && institutionID > 0) {
                criteria.Expression = criteria.Expression.Where(x => x.InstitutionID, institutionID);
            }

            int academicID;
            if (int.TryParse(ddlDepartment.SelectedItem.Value, out academicID) && academicID > 0) {
                criteria.Expression = criteria.Expression.Where(x => x.ID, academicID);
            }

            int maxAllowedPreAssignedPositions;
            if (int.TryParse(txtMaxAllowedPreAssignedPositions.Text.ToNull(), out maxAllowedPreAssignedPositions) && maxAllowedPreAssignedPositions >= 0) {
                criteria.Expression = criteria.Expression.Where(x => x.MaxAllowedPreAssignedPositions, maxAllowedPreAssignedPositions);
            }

            int preAssignedPositions;
            if (int.TryParse(txtPreAssignedPositions.Text.ToNull(), out preAssignedPositions) && preAssignedPositions >= 0) {
                criteria.Expression = criteria.Expression.Where(x => x.PreAssignedPositions, preAssignedPositions);
            }

            e.InputParameters["criteria"] = criteria;
        }
    }
}