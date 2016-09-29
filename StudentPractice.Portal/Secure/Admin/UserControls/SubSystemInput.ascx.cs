using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

using System.Web.Security;
using StudentPractice.Portal.Controls;
using DevExpress.Web.ASPxEditors;

namespace StudentPractice.Portal.Secure.Admin.UserControls
{
    public partial class SubSystemInput : BaseUserControl<BaseEntityPortalPage<SubSystem>>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cbxRole_Init(object sender, EventArgs e)
        {
            cbxRole.Items.Clear();
            cbxRole.Text = "-- αδιάφορο --";

            string[] roles = Roles.GetAllRoles();
            var subSystems = CacheManager.SubSystems.GetItems();

            foreach (string role in roles)
            {
                if (!subSystems.Any(x => x.Role == role))
                {
                    cbxRole.Items.Add(role, role);
                }
            }
        }

        protected void cbxReporterType_Init(object sender, EventArgs e)
        {
            cbxReporterType.Items.Clear();

            foreach (enReporterType item in Enum.GetValues(typeof(enReporterType)))
            {
                cbxReporterType.Items.Add(item.GetLabel(), ((int)item).ToString());
            }
        }

        public void FillSubSystem(SubSystem subSystem)
        {
            if (subSystem.Name != txtName.Text.ToNull())
            {
                subSystem.Name = txtName.Text.ToNull();
            }

            if (subSystem.Role != cbxRole.Value.ToString())
            {
                subSystem.Role = cbxRole.Value.ToString();
            }

            foreach (enReporterType reporterType in Enum.GetValues(typeof(enReporterType)))
            {
                if (!subSystem.SubSystemReporterTypes.Any(x => x.ReporterType == reporterType) &&
                    cbxReporterType.Items.FindByValue(((int)reporterType).ToString()).Selected)
                {
                    SubSystemReporterType srt = new SubSystemReporterType();

                    srt.ReporterType = reporterType;

                    subSystem.SubSystemReporterTypes.Add(srt);
                }
                else if (subSystem.SubSystemReporterTypes.Any(x => x.ReporterType == reporterType) &&
                    !cbxReporterType.Items.FindByValue(((int)reporterType).ToString()).Selected)
                {
                    Page.UnitOfWork.MarkAsDeleted(subSystem.SubSystemReporterTypes.Where(x => x.ReporterType == reporterType).FirstOrDefault());
                }
            }
        }

        public void SetSubSystem(SubSystem subSystem)
        {
            txtName.Text = subSystem.Name;

            cbxRole.Items.Add(subSystem.Role, subSystem.Role);
            cbxRole.SelectedItem = cbxRole.Items.FindByValue(subSystem.Role);

            var reporterTypes = subSystem.SubSystemReporterTypes.Select(x => x.ReporterType);

            foreach (ListEditItem item in cbxReporterType.Items)
            {
                item.Selected = reporterTypes.Contains((enReporterType)Convert.ToInt32(item.Value));
            }
        }
    }
}