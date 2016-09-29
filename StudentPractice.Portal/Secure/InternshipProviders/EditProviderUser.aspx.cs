using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using Imis.Domain;
using StudentPractice.Portal.Controls;
using System.Web.Security;
using StudentPractice.Utils;
using System.Data;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.InternshipProviders
{
    public partial class EditProviderUser : BaseEntityPortalPage<InternshipProvider>
    {
        #region [ Databind Methods ]

        private InternshipProvider ProviderUser;

        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int providerUserID;
            if (int.TryParse(Request.QueryString["pID"], out providerUserID) && providerUserID > 0)
            {
                ProviderUser = new InternshipProviderRepository(UnitOfWork).Load(providerUserID);

                if (ProviderUser == null)
                    Response.Redirect("ProviderUsers.aspx");
            }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucProviderUserInput.Entity = ProviderUser;
                ucProviderUserInput.Bind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string oldEmail = ProviderUser.Email;
            
            ucProviderUserInput.Fill(ProviderUser);

            string newEmail = ucProviderUserInput.Email;

            if (oldEmail != newEmail)
            {
                if (!string.IsNullOrEmpty(Membership.GetUserNameByEmail(newEmail)))
                {
                    lblValidationErrors.Text = Resources.ProviderUser.EmailExists;
                    return;
                }
                else
                {
                    ProviderUser.Email = newEmail;
                }
            }

            if (string.IsNullOrEmpty(ProviderUser.AFM))
                ProviderUser.AFM = Entity.AFM;

            if (string.IsNullOrEmpty(ProviderUser.DOY))
                ProviderUser.DOY = Entity.DOY;

            UnitOfWork.Commit();

            MembershipUser mu = Membership.GetUser(ProviderUser.UserName);
            if (oldEmail != newEmail)
            {
                mu.Email = newEmail;
                Membership.UpdateUser(mu);
            }

            Response.Redirect("ProviderUsers.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProviderUsers.aspx");
        }
    }
}