using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using StudentPractice.Mails;

namespace StudentPractice.Portal.Secure.Admin
{
    public partial class SendCustomMassEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            var rIDs = txtSenders.Text.Split(',').Select(x => int.Parse(x)).ToList();
            var subject = txtSubject.Text;
            var body = txtBody.Text;

            var students = new StudentRepository().LoadMany(rIDs).ToList();

            var batches = students.Split(50);

            foreach (var batch in batches)
            {
                using (var uow = UnitOfWorkFactory.Create())
                {
                    foreach (var item in batch)
                    {
                        if (!string.IsNullOrEmpty(item.ContactEmail))
                        {
                            var email = MailSender.SendCustomMessageToReporter(item.ID, item.ContactEmail, subject, body);
                            uow.MarkAsNew(email);
                        }
                        else if (!string.IsNullOrEmpty(item.Email))
                        {
                            var email = MailSender.SendCustomMessageToReporter(item.ID, item.Email, subject, body);
                            uow.MarkAsNew(email);
                        }
                    }
                    uow.Commit();
                }
            }
        }
    }
}