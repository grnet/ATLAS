using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Students
{
    public partial class ViewPositionCert : BaseEntityPortalPage<InternshipPosition>
    {
        private int? _pID = null;
        protected int PositionID
        {
            get
            {
                if (!_pID.HasValue)
                {
                    int pID = 0;
                    int.TryParse(Request.QueryString["pID"], out pID);
                    _pID = pID;
                }
                return _pID.Value;
            }
        }

        protected Student CurrentStudent { get; set; }

        protected override void Fill()
        {
            CurrentStudent = new StudentRepository(UnitOfWork).FindByUsername(User.Identity.Name);
            Entity = new InternshipPositionRepository(UnitOfWork).FindOneByStudentID(PositionID, CurrentStudent.ID, x => x.AssignedToStudent, x => x.InternshipPositionGroup.Provider);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity != null)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var provider = Entity.InternshipPositionGroup.Provider;
            var academic = CacheManager.Academics.Get(CurrentStudent.AcademicID.GetValueOrDefault());

            litContactName.Text = CurrentStudent.ContactName;
            litDepartment.Text = academic.Department;
            litInstitution.Text = academic.Institution;
            litStudentCode.Text = CurrentStudent.StudentNumber;

            litStartDate.Text = Entity.ImplementationStartDate.GetValueOrDefault().ToShortDateString();
            litEndDate.Text = Entity.ImplementationEndDate.GetValueOrDefault().ToShortDateString();

            litProviderName.Text = string.IsNullOrEmpty(provider.TradeName) ? provider.Name : string.Format("{0} / {1}", provider.Name, provider.TradeName);
            litProviderAFM.Text = provider.AFM;
            litProviderContactName.Text = provider.ContactName;

            litPositionTitle.Text = Entity.InternshipPositionGroup.Title;
            litPositionID.Text = Entity.ID.ToString();
        }
    }
}