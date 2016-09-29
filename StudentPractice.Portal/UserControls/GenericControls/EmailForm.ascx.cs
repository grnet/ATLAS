using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Web.Security;
using System.ComponentModel;
using StudentPractice.Mails;
using StudentPractice.Utils;

namespace StudentPractice.Portal.UserControls.GenericControls
{
    public class EmailFormSendingEventArgs : CancelEventArgs
    {
        public EmailFormSendingEventArgs(bool sendToLoggedInUser)
        {
            EmailRecepients = new List<string>();
            SendToLoggedInUser = sendToLoggedInUser;
        }
        public List<string> EmailRecepients { get; set; }
        public bool SendToLoggedInUser { get; private set; }
        public string InfoMessage { get; set; }
    }

    public class EmailFormSentEventArgs : EventArgs
    {
        public EmailFormSentEventArgs() { }
    }

    public partial class EmailForm : System.Web.UI.UserControl
    {
        #region [ Events ]



        #endregion
        public event EventHandler<EmailFormSendingEventArgs> EmailSending;

        public event EventHandler<EmailFormSentEventArgs> EmailSent;

        protected override void OnPreRender(EventArgs e)
        {
            if (emailArea.Style["display"] == "block")
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "__EmailFormExpanded", "var __EmailFormExpanded = true;", true);
            }
            base.OnPreRender(e);
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            var args = new EmailFormSendingEventArgs(cbxTestSend.Checked);

            if (EmailSending != null)
                EmailSending(this, args);

            if (args.Cancel)
            {
                emailArea.Style["display"] = "block";
                return;
            }
            if (cbxTestSend.Checked)
            {
                var user = Membership.GetUser();
                if (user != null)
                {
                    SendEmail(txtSubject.Text, txtBody.Text, new List<string>() { user.Email });
                }
            }
            else
                SendEmail(txtSubject.Text, txtBody.Text, args.EmailRecepients);

            lblEmailFormMessage.Text = args.InfoMessage;

            var sentArgs = new EmailFormSentEventArgs();
            if (EmailSent != null)
                EmailSent(this, sentArgs);
        }


        private void SendEmail(string subject, string body, IEnumerable<string> recepients)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                foreach (var r in recepients)
                {
                    try
                    {
                        MailSender.SendCustomMessage(r, subject, body);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogError<EmailForm>(ex);
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(txtAdditionalEmail.Text))
                {
                    MailSender.SendCustomMessage(txtAdditionalEmail.Text, subject, body);
                }
            });
        }

        public string EmailBody { get { return txtBody.Text.ToNull(); } }
    }
}