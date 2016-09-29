using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.UserControls.InternshipProviderControls.InputControls
{
    public partial class ProviderRegisterUserInput : BaseEntityUserControl<InternshipProvider>
    {
        public string Username { get { return txtUsername.Text; } }
        public string Password { get { return txtPassword1.Text; } }
        public string Email { get { return txtEmail.Text; } }

        public override InternshipProvider Fill(InternshipProvider entity)
        {
            if (entity == null)
            {
                entity = new InternshipProvider();
                entity.UsernameFromLDAP = Guid.NewGuid().ToString();
            }

            if (entity.Email != txtEmail.Text.ToNull())
            {
                MembershipUser mu = Membership.GetUser(entity.UserName);
                mu.Email = txtEmail.Text.ToNull();
                Membership.UpdateUser(mu);

                entity.Email = txtEmail.Text.ToNull();
            }

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            txtUsername.Text = Entity.UserName;
            txtEmail.Text = Entity.Email;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtPassword1.Attributes["onblur"] = "RemoveTags(this)";
            txtPassword2.Attributes["onblur"] = "RemoveTags(this)";
        }

        protected string GetLiteral(string key)
        {
            return Resources.RegistrationInput.ResourceManager.GetString(key);
        }

        public string CreateUser()
        {
            try
            {
                MembershipCreateStatus status;
                MembershipUser mu;

                mu = Membership.CreateUser(txtUsername.Text.Trim(), txtPassword1.Text, txtEmail.Text, null, null, true, out status);

                if (mu == null)
                    throw new MembershipCreateUserException(status);

                _CreatedUser = mu;
                return mu.UserName;
            }
            catch (MembershipCreateUserException)
            {
                throw;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return !txtUsername.Enabled;
            }
            set
            {
                txtUsername.Enabled =
                rfvUsername.Visible =
                revUsername.Visible =
                cvUserName.Visible =
                trPassword1.Visible =
                trPassword2.Visible = !value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                return rfvEmail.ValidationGroup;
            }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        MembershipUser _CreatedUser = null;
        public string ProviderUserKey
        {
            get
            {
                if (_CreatedUser == null)
                    throw new InvalidOperationException("No MembershipUser was found. Please check CreateUser() or SetUser().");
                return _CreatedUser.ProviderUserKey.ToString();
            }
        }
    }
}