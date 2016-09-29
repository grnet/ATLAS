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
using System.Threading;
using StudentPractice.BusinessModel;
using StudentPractice.Mails;
using System.Web.Profile;
using StudentPractice.Portal.Controls;
using Imis.Domain;

namespace StudentPractice.Portal.Common
{
    public partial class ForgotPassword : BaseEntityPortalPage<object>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblInfo.Text = string.Empty;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                var users = Membership.FindUsersByEmail(txtEmail.Text.ToNull()).OfType<MembershipUser>();

                if (users.Count() == 0)
                {
                    lblInfo.Text = Resources.CommonPages.ForgotPassword_EmailNotFound;
                    return;
                }
                else if (users.Count() > 1)
                {
                    lblInfo.Text = Resources.CommonPages.ForgotPassword_MoreEmail;
                    return;
                }
                else
                {
                    var user = users.Single();
                    if (user.IsLockedOut)
                    {
                        user.UnlockUser();
                    }

                    string oldPassword = user.ResetPassword();
                    string newPassword = Guid.NewGuid().ToString().Substring(0, 8);

                    user.ChangePassword(oldPassword, newPassword);
                    Membership.UpdateUser(user);

                    using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                    {
                        var reporter = new ReporterRepository(uow).FindByEmail(user.Email);
                        
                        reporter.MustChangePassword = true;

                        AuthenticationService.InvalidateCookie(user.UserName, false);

                        var email = MailSender.SendForgotPassword(reporter.ID, user.Email, user.UserName, newPassword, LanguageService.GetUserLanguage());

                        uow.MarkAsNew(email);
                        uow.Commit();
                    }

                    lblInfo.Text = Resources.CommonPages.ForgotPassword_Success;
                }
            }
            catch (ThreadAbortException)
            {
                //swallow like a fish
            }
            catch (Exception)
            {
                lblInfo.Text = Resources.CommonPages.ForgotPassword_Error;
            }
        }
    }
}
