using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxClasses;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class IncidentTypeManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlSubSystem_Init(object sender, EventArgs e)
        {
            ddlSubSystem.Items.Add(new ListItem("-- αδιάφορο --", "-1"));

            foreach (var item in CacheManager.SubSystems.GetItems())
            {
                ddlSubSystem.Items.Add(new ListItem(item.Name, item.ID.ToString()));
            }
        }


        protected void cmdRefresh_Click(object sender, EventArgs e)
        {
            gvIncidentTypes.DataBind();
        }

        protected void ddlSubSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int subSystemID;
            int.TryParse(ddlSubSystem.SelectedValue, out subSystemID);

            if (subSystemID > 0)
            {
                lnkCreateIncidentType.Visible = true;
                lnkCreateIncidentType.Attributes.Add("onclick", string.Format("popUp.show('CreateIncidentType.aspx?sID={0}','Δημιουργία Τύπου Συμβάντος', cmdRefresh);", subSystemID));
            }

            gvIncidentTypes.DataBind();
        }

        protected void odsIncidentTypes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<IncidentType> criteria = new Criteria<IncidentType>();

            criteria.Include(x => x.SubSystem)
                .Include(x => x.ReporterIncidentTypes)
                .Include(x => x.IncidentReports);

            int subSystemID = 0;
            if (int.TryParse(ddlSubSystem.SelectedValue, out subSystemID) && subSystemID > 0)
            {
                criteria.Expression = criteria.Expression.Where(x => x.SubSystem.ID, subSystemID);
            }

            e.InputParameters["criteria"] = criteria;
        }

        public void gvIncidentTypes_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            if (e.CommandArgs.CommandName == "DeleteIncidentType")
            {
                int itID = Convert.ToInt32(e.CommandArgs.CommandArgument);

                IUnitOfWork unitOfWork = UnitOfWorkFactory.Create();

                IncidentType it = new IncidentTypeRepository(unitOfWork).Load(itID, x => x.ReporterIncidentTypes);

                for (int i = it.ReporterIncidentTypes.Count - 1; i >= 0; i--)
                {
                    unitOfWork.MarkAsDeleted(it.ReporterIncidentTypes.ToList()[i]);
                }

                unitOfWork.MarkAsDeleted(it);
                unitOfWork.Commit();

                CacheManager.Refresh();

                gvIncidentTypes.DataBind();
            }
        }

        protected string GetReporterTypes(IncidentType incidentType)
        {
            if (incidentType == null)
                return string.Empty;

            var reporterTypes = incidentType.ReporterIncidentTypes.Select(x => x.ReporterType.GetLabel()).ToArray();

            return string.Join(", ", reporterTypes);
        }

        protected int GetIncidentCount(IncidentType incidentType)
        {
            if (incidentType == null)
                return 0;

            return incidentType.IncidentReports.Count();
        }
    }
}