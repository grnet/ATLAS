using System;
using System.Web.Security;
using System.Web.UI;

using StudentPractice.BusinessModel;
using StudentPractice.Portal.Controls;
using System.Web;
using StudentPractice.Mails;
using System.Web.Services;
using Imis.Domain;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class ProviderDetails : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name, x => x.MasterAccount);
            Entity.SaveToCurrentContext();
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            lblUserName.Text = Entity.UserName;
            txtEmail.Text = Entity.Email;

            if (Entity.IsEmailVerified)
            {
                btnSendEmailVerificationCode.Visible = false;
            }

            if (!Page.IsPostBack)
                ViewState["EntityID"] = Entity.ID;

            if (Entity.IsMasterAccount)
            {
                ucProviderInput.Entity = Entity;

                if (!Page.IsPostBack)
                    ucProviderInput.Bind();

                divAccountVerified.Visible = Entity.VerificationStatus == enVerificationStatus.Verified;

                if (Entity.VerificationStatus == enVerificationStatus.CannotBeVerified)
                    btnUpdateProvider.Visible = false;

                mvProvider.SetActiveView(vMasterAccount);
            }
            else
            {
                ucProviderUserInput.Entity = Entity;
                ucProviderUserInput.Bind();

                InternshipProvider masterProvider = Entity.MasterAccount as InternshipProvider;
                lblMasterAccountDetails.Text = string.Format(Resources.ProviderDetails.UpdateNotMaster, masterProvider.Name, masterProvider.ContactPhone, masterProvider.ContactEmail);
                mvProvider.SetActiveView(vProviderUser);
            }

            base.OnLoad(e);
        }

        #region [ Button Handlers ]

        protected void btnUpdateProvider_Click(object sender, EventArgs e)
        {
            if (Entity.ID != (int)ViewState["EntityID"])
                Response.Redirect("~/Default.aspx");

            if (!Page.IsValid)
                return;

            ucProviderInput.Fill(Entity);

            if (Entity.ProviderType != enProviderType.PublicCarrier && new InternshipProviderRepository(UnitOfWork).IsAfmVerified(Entity.ID, Entity.AFM))
            {
                lblErrors.Visible = true;
                lblErrors.Text = Resources.ProviderDetails.UpdateFail;
                return;
            }

            UnitOfWork.Commit();

            ucProviderInput.Bind();
            fm.Text = Resources.ProviderDetails.UpdateSuccess;
        }

        protected void btnSendEmailVerificationCode_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            Uri baseURI;
            if (Config.IsSSL)
            {
                baseURI = new Uri("https://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }
            else
            {
                baseURI = new Uri("http://" + HttpContext.Current.Request.Url.Authority + "/Common/");
            }

            Uri uri = new Uri(baseURI, "VerifyEmail.aspx?id=" + Entity.EmailVerificationCode);

            var email = MailSender.SendEmailVerification(Entity.ID, Entity.Email, Entity.ContactName, uri, LanguageService.GetUserLanguage());
            UnitOfWork.MarkAsNew(email);
            UnitOfWork.Commit();
        }

        #endregion

    }
}
