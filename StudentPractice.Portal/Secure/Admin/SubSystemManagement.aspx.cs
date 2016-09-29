using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using DevExpress.Web.ASPxGridView;

namespace StudentPractice.Portal.Secure.Admin {
    public partial class SubSystemManagement : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void cmdRefresh_Click(object sender, EventArgs e) {
            gvSubSystems.DataBind();
        }

        protected void odsSubSystems_Selecting(object sender, ObjectDataSourceSelectingEventArgs e) {
            Criteria<SubSystem> criteria = new Criteria<SubSystem>();

            criteria.Include(x => x.SubSystemReporterTypes)
                    .Include(x => x.IncidentReports);

            e.InputParameters["criteria"] = criteria;
        }

        public void gvSubSystems_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "DeleteSubSystem") {
                int sID = Convert.ToInt32(e.CommandArgs.CommandArgument);

                IUnitOfWork unitOfWork = UnitOfWorkFactory.Create();

                SubSystem s = new SubSystemRepository(unitOfWork).Load(sID, x => x.SubSystemReporterTypes);

                for (int i = s.SubSystemReporterTypes.Count - 1; i >= 0; i--) {
                    unitOfWork.MarkAsDeleted(s.SubSystemReporterTypes.ToList()[i]);
                }

                unitOfWork.MarkAsDeleted(s);
                unitOfWork.Commit();

                CacheManager.Refresh();

                gvSubSystems.DataBind();
            }
        }

        protected string GetReporterTypes(SubSystem subSystem) {
            if (subSystem == null)
                return string.Empty;

            var reporterTypes = subSystem.SubSystemReporterTypes.Select(x => x.ReporterType.GetLabel()).ToArray();

            return string.Join(", ", reporterTypes);
        }

        protected int GetIncidentCount(SubSystem subSystem) {
            if (subSystem == null)
                return 0;

            return subSystem.IncidentReports.Count();
        }
    }
}
