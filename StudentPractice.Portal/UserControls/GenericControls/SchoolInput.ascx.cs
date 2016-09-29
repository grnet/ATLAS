using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class SchoolInput : System.Web.UI.UserControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "jsInit", string.Format("hd.init('{0}','{1}','{2}','{3}');", hfSchoolCode.ClientID, txtInstitutionName.ClientID, txtSchoolName.ClientID, txtDepartmentName.ClientID), true);

            txtInstitutionName.Attributes.Add("readonly", "readonly");
            txtSchoolName.Attributes.Add("readonly", "readonly");
            txtDepartmentName.Attributes.Add("readonly", "readonly");

            //txtInstitutionName.Attributes.Add("onfocus", "popUp.show('/Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");
            //txtSchoolName.Attributes.Add("onfocus", "popUp.show('/Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");
            //txtDepartmentName.Attributes.Add("onfocus", "popUp.show('/Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');");


            base.OnPreRender(e);
        }

        public void Bind(Academic academic)
        {
            txtInstitutionName.Text = academic.Institution;
            txtSchoolName.Text = academic.School;
            txtDepartmentName.Text = academic.Department;
            hfSchoolCode.Value = academic.ID.ToString();
        }

        public int GetAcademicID()
        {
            return int.Parse(hfSchoolCode.Value);
        }

        #region [ UI Region ]

        public bool ReadOnly
        {
            set
            {
                foreach (WebControl c in Controls.OfType<WebControl>())
                    c.Enabled = !value;

                phSelectAcademic.Visible = !value;
            }
        }

        #endregion
    }
}