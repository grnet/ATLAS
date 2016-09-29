using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.UserControls.HelpdeskControls.InputControls
{
    public partial class HelpdeskUserInput : BaseEntityUserControl<HelpdeskUser>
    {
        public override HelpdeskUser Fill(HelpdeskUser entity)
        {
            if (entity == null)
                entity = new HelpdeskUser();

            if (entity.Email != txtEmail.Text.ToNull())
                entity.Email = entity.ContactEmail = txtEmail.Text.ToNull();

            if (entity.ContactName != txtContactName.Text.ToNull())
                entity.ContactName = txtContactName.Text.ToNull();

            if (entity.ContactMobilePhone != txtContactMobilePhone.Text.ToNull())
                entity.ContactMobilePhone = txtContactMobilePhone.Text.ToNull();

            return entity;
        }

        public override void Bind()
        {
            if (Entity == null)
                return;

            txtUsername.Text = Entity.UserName;
            txtEmail.Text = Entity.Email;
            txtContactName.Text = Entity.ContactName;
            txtContactMobilePhone.Text = Entity.ContactMobilePhone;
        }

        public bool EditMode
        {
            get
            {
                return !txtUsername.Enabled;
            }
            set
            {
                txtUsername.Enabled =
                trPassword1.Visible =
                trPassword2.Visible = !value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Attributes["onblur"] = "RemoveTags(this)";
            txtPassword1.Attributes["onblur"] = "RemoveTags(this)";
            txtPassword2.Attributes["onblur"] = "RemoveTags(this)";
            txtEmail.Attributes["onblur"] = "RemoveTags(this)";
            txtContactName.Attributes["onkeyup"] = "Imis.Lib.ToUpperForNames(this)";
        }

        public string ValidationGroup
        {
            get
            {
                return rfvUsername.ValidationGroup;
            }
            set
            {
                foreach (var validator in this.RecursiveOfType<BaseValidator>())
                    validator.ValidationGroup = value;
            }
        }

        public string Username { get { return txtUsername.Text.Trim(); } }
        public string Password { get { return txtPassword1.Text.Trim(); } }
        public string Email { get { return txtEmail.Text.Trim(); } }
    }
}