using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.CacheManagerExtensions;

namespace StudentPractice.Portal.UserControls.StudentControls.ViewControls
{
    public partial class StudentView : BaseEntityUserControl<Student>
    {
        public override void Bind()
        {
            if (Entity == null)
                return;

            //Στοιχεία Φοιτητή

            if (Entity.IsNameLatin.GetValueOrDefault()) {
                trGreekName.Visible = false;
                lblLatinNameLabel.Text = "Ον/μο:";
            }
            else {
                lblGreekName.Text = string.Format("{0} {1}", Entity.GreekFirstName, Entity.GreekLastName);
            }
            
            lblLatinName.Text = string.Format("{0} {1}", Entity.LatinFirstName, Entity.LatinLastName);

            var academic = CacheManager.Academics.Get(Entity.AcademicID.Value);
            lblInstitution.Text = academic.Institution;
            lblSchool.Text = academic.School ?? "-";
            lblDepartment.Text = academic.Department;

            lblStudentNumber.Text = Entity.StudentNumber;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}