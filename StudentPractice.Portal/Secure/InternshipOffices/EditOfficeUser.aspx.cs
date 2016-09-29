using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Web.Security;

namespace StudentPractice.Portal.Secure.InternshipOffices
{
    public partial class EditOfficeUser : BaseEntityPortalPage<InternshipOffice>
    {
        private InternshipOffice OfficeUser;

        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).FindByUsername(Page.User.Identity.Name);
            Entity.SaveToCurrentContext();

            int officeUserID;
            if (int.TryParse(Request.QueryString["sID"], out officeUserID) && officeUserID > 0) {
                OfficeUser = new InternshipOfficeRepository(UnitOfWork).Load(officeUserID);

                if (OfficeUser == null)
                    Response.Redirect("OfficeUsers.aspx");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucOfficeUserInput.Entity = OfficeUser;
                ucOfficeUserInput.Bind();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string oldEmail = OfficeUser.Email;

            ucOfficeUserInput.Fill(OfficeUser);

            string newEmail = OfficeUser.Email;

            if (oldEmail != newEmail && !string.IsNullOrEmpty(Membership.GetUserNameByEmail(newEmail)))
            {
                lblValidationErrors.Text = "Το E-mail χρησιμοποιείται ήδη από κάποιο άλλο χρήστη του Πληροφοριακού Συστήματος. Παρακαλούμε επιλέξτε κάποιο άλλο.";
                return;
            }

            UnitOfWork.Commit();

            MembershipUser mu = Membership.GetUser(OfficeUser.UserName);
            if (oldEmail != newEmail)
            {
                mu.Email = newEmail;
                Membership.UpdateUser(mu);
            }

            Response.Redirect("OfficeUsers.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e) {
            Response.Redirect("OfficeUsers.aspx");
        }
    }
}
