using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

using StudentPractice.Portal.Controls;
using DevExpress.Web.ASPxEditors;

namespace StudentPractice.Portal.Secure.Admin.UserControls
{
    public partial class IncidentTypeInput : BaseUserControl<BaseEntityPortalPage<IncidentType>>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cbxReporterType_Init(object sender, EventArgs e)
        {
            cbxReporterType.Items.Clear();

            var reporterTypes = Page.Entity.SubSystem.SubSystemReporterTypes.Select(x => x.ReporterType);

            foreach (enReporterType item in reporterTypes)
            {
                cbxReporterType.Items.Add(item.GetLabel(), ((int)item).ToString());
            }
        }

        public void FillIncidentType(IncidentType incidentType)
        {
            if (incidentType.NameInGreek != txtNameInGreek.Text.ToNull())
                incidentType.NameInGreek = txtNameInGreek.Text.ToNull();

            if (incidentType.NameInLatin != txtNameInLatin.Text.ToNull())
                incidentType.NameInLatin = txtNameInLatin.Text.ToNull();

            foreach (enReporterType reporterType in Enum.GetValues(typeof(enReporterType)))
            {
                if (!incidentType.ReporterIncidentTypes.Any(x => x.ReporterType == reporterType) &&
                    cbxReporterType.Items.FindByValue(((int)reporterType).ToString()).Selected)
                {
                    ReporterIncidentType rIt = new ReporterIncidentType();

                    rIt.ReporterType = reporterType;

                    incidentType.ReporterIncidentTypes.Add(rIt);
                }
                else if (incidentType.ReporterIncidentTypes.Any(x => x.ReporterType == reporterType) &&
                    !cbxReporterType.Items.FindByValue(((int)reporterType).ToString()).Selected)
                {
                    Page.UnitOfWork.MarkAsDeleted(incidentType.ReporterIncidentTypes.Where(x => x.ReporterType == reporterType).FirstOrDefault());
                }
            }
        }

        public void SetIncidentType(IncidentType incidentType)
        {
            lblSubSystem.Text = incidentType.SubSystem.Name;
            txtNameInGreek.Text = incidentType.NameInGreek;
            txtNameInLatin.Text = incidentType.NameInLatin;

            var reporterTypes = incidentType.ReporterIncidentTypes.Select(x => x.ReporterType);

            foreach (ListEditItem item in cbxReporterType.Items)
            {
                item.Selected = reporterTypes.Contains((enReporterType)Convert.ToInt32(item.Value));
            }
        }
    }
}