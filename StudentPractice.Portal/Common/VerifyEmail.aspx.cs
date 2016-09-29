using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;

namespace StudentPractice.Portal.Common
{
    public partial class VerifyEmail : BaseEntityPortalPage<Reporter>
    {
        protected override void Fill()
        {
            Entity = new ReporterRepository(UnitOfWork).FindByEmailVerificationCode(Request.QueryString["id"]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                lblMessage.Text = Resources.CommonPages.VerifyEmail_InvalideUrl;
            }
            else
            {
                if (Entity == null)
                    lblMessage.Text = Resources.CommonPages.VerifyEmail_NoUser;

                else if (Entity.IsEmailVerified)
                    lblMessage.Text = Resources.CommonPages.VerifyEmail_AlreadyVerified;

                else
                {
                    Entity.IsEmailVerified = true;
                    Entity.EmailVerificationDate = DateTime.Now;

                    UnitOfWork.Commit();

                    lblMessage.Text = Resources.CommonPages.VerifyEmail_Verified;
                }
            }
        }
    }
}
