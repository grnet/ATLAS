using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.ComponentModel;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Common
{
    public partial class ChangePassword : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var pRep = new InternshipProviderRepository();
            var provider = Context.LoadProvider() ?? pRep.FindByUsername(Page.User.Identity.Name, x => x.MasterAccount);
            if (provider != null)
            {
                provider.SaveReporterIDToCurrentContext();

                int? masterCountryID = null;
                if (!provider.IsMasterAccount)
                    masterCountryID = Context.LoadMasterCountryID() ?? pRep.Load(provider.MasterAccountID.Value).CountryID;

                if (IsForeign(provider.CountryID) || IsForeign(masterCountryID))
                    ucLanguageBar.Visible = true;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (Entity.ReporterType == enReporterType.Student)
            {
                var student = (Student)Entity;

                Entity.MustChangePassword = false;
                UnitOfWork.Commit();

                lblInfo.Text = Resources.CommonPages.ChangePassword_Student;
                return;
            }

            var user = Membership.GetUser(Page.User.Identity.Name);

            if (user.ChangePassword(txtOldPassword.Text.ToNull(), txtPassword1.Text.ToNull()))
            {
                Entity.MustChangePassword = false;
                UnitOfWork.Commit();

                AuthenticationService.InvalidateCookie(user.UserName, true);
                AuthenticationService.LoginReporter(Entity);

                mvChangePassword.SetActiveView(vPasswordChanged);
            }
            else
            {
                lblInfo.Text = Resources.CommonPages.ChangePassword_WrongOld;
            }
        }
        
        private bool IsForeign(int? countryID)
        {
            if (!countryID.HasValue)
                return false;

            return (countryID.Value != StudentPracticeConstants.GreeceCountryID && countryID.Value != StudentPracticeConstants.CyprusCountryID);
        }
    }
}
