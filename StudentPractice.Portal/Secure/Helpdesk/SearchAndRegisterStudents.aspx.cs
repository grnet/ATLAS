using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class SearchAndRegisterStudents : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(User.Identity.Name);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucSearchAndRegisterStudent.UnitOfWork = UnitOfWork;
            //ucSearchAndRegisterStudent.CurrentUserID = Entity.ID;
            ucSearchAndRegisterStudent.Bind();
        }
    }
}