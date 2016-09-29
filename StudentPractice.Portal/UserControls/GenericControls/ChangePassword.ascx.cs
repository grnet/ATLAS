using System;
using System.ComponentModel;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class ChangePassword : System.Web.UI.UserControl
    {
        [DefaultValue(true)]
        public bool RequestOldPassword
        {
            get { return trOldPassword.Visible; }
            set { trOldPassword.Visible = value; }
        }

        public string NewPassword
        {
            get { return txtNewPassword.Text; }
        }

        public string OldPassword
        {
            get { return txtOldPassword.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string ValidationGroup
        {
            get
            {
                return rfvNewPassword.ValidationGroup;
            }
            set
            {
                rfvNewPassword.ValidationGroup = value;
                rfvNewPasswordConfirmation.ValidationGroup = value;
                revPassword.ValidationGroup = value;
                rfvOldPassword.ValidationGroup = value;
                cvNewPasswordConfirmation.ValidationGroup = value;
            }
        }

        public void ClearInput()
        {
            txtOldPassword.Text = "";
            txtNewPassword.Text = "";
            txtNewPasswordConfirmation.Text = "";
        }
    }
}