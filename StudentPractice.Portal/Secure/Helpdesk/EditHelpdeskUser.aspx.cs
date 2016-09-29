using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using System.Web.Security;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class EditHelpdeskUser : BaseEntityPortalPage<HelpdeskUser>
    {
        protected override void Fill()
        {
            Entity = new HelpdeskUserRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["sID"]));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucHelpdeskUserInput.Entity = Entity;
                ucHelpdeskUserInput.Bind();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string oldEmail = Entity.Email;

            ucHelpdeskUserInput.Fill(Entity);

            string newEmail = Entity.Email;

            if (oldEmail != newEmail && !string.IsNullOrEmpty(Membership.GetUserNameByEmail(newEmail)))
            {
                lblValidationErrors.Text = "Το E-mail χρησιμοποιείται ήδη από κάποιο άλλο χρήστη του Πληροφοριακού Συστήματος. Παρακαλούμε επιλέξτε κάποιο άλλο.";
                return;
            }

            UnitOfWork.Commit();

            MembershipUser mu = Membership.GetUser(Entity.UserName);
            if (oldEmail != newEmail)
            {
                mu.Email = newEmail;
                Membership.UpdateUser(mu);
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
