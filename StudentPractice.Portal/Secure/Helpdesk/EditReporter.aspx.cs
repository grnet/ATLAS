using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditReporter : BaseEntityPortalPage<Reporter>
    {
        private Reporter _currentReporter = null;
        protected Reporter CurrentReporter
        {
            get
            {
                if (_currentReporter != null)
                    return _currentReporter;

                try
                {
                    _currentReporter = new ReporterRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["rID"]));
                }
                catch (FormatException)
                {
                }

                return _currentReporter;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucReporterInput.SetReporter(CurrentReporter);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            ucReporterInput.FillReporter(CurrentReporter);

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
