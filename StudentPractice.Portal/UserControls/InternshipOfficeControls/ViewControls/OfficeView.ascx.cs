using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.UserControls.InternshipOfficeControls.ViewControls
{
    public partial class OfficeView : BaseEntityUserControl<InternshipOffice>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            lblOfficeType.Text = Entity.OfficeType.GetLabel();
            lblInstitution.Text = Entity.InstitutionID.HasValue ? CacheManager.Institutions.Get(Entity.InstitutionID.Value).Name : string.Empty;
            if (Entity.OfficeType == enOfficeType.Departmental)
            {
                rowAcademic.Visible = true;
                cellSingleAcademic.Visible = true;
                lblAcademic.Text = Entity.Academics.FirstOrDefault().Department;
            }
            else if (Entity.OfficeType == enOfficeType.MultipleDepartmental)
            {
                rowAcademic.Visible = true;
                cellMultipleAcademic.Visible = true;
                litMultipleAcademic.Text = string.Format("<a href='javascript:void(0)' onclick='popUp.show(\"ViewOfficeAcademics.aspx?oID={0}\",\"Προβολή Σχολών/Τμημάτων\")'><img src='/_img/iconInformation.png' width='16px' alt='Τμήματα' /></a>", Entity.ID);
            }
            lblContactName.Text = Entity.ContactName;
            lblContactPhone.Text = string.Format("Σταθερό: {0}<br />Κινητό: {1}", Entity.ContactPhone, Entity.ContactMobilePhone);
            lblEmailPhone.Text = Entity.ContactEmail;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}