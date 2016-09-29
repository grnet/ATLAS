using System;
using System.Web.UI;
using System.ComponentModel;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public partial class FlashMessage : System.Web.UI.UserControl, ITextControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AutoDisplay();
        }

        private void AutoDisplay()
        {
            if (!string.IsNullOrEmpty(SessionKey) && Session[SessionKey] != null)
            {
                Text = Convert.ToString(Session[SessionKey]);
                Session.Remove(SessionKey);
            }
            else
                Text = "";
        }

        public string Text
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; this.Visible = Text.Length > 0; }
        }

        public string CssClass
        {
            get { return lblMessage.CssClass; }
            set { lblMessage.CssClass = value; }
        }

        const string DefaultSessionKey = "flash";
        private string sessionKey = DefaultSessionKey;

        [DefaultValue(DefaultSessionKey)]
        public string SessionKey
        {
            get { return sessionKey; }
            set { sessionKey = value; }
        }

        public void AddMessageEndRedirect(string message, string urlToRedirect, bool endResponse)
        {
            Session[SessionKey] = message;
            Response.Redirect(urlToRedirect, endResponse);
        }
    }
}