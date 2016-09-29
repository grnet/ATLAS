using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.Portal.Controls;
using StudentPractice.BusinessModel;
using StudentPractice.Mails;
using StudentPractice.Utils;

namespace StudentPractice.Portal.Secure.Helpdesk
{
    public partial class VerifyProvider : BaseEntityPortalPage<InternshipProvider>
    {
        protected override void Fill()
        {
            Entity = new InternshipProviderRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["pID"]));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                if (Entity.IsMasterAccount)
                {
                    ucProviderView.Entity = Entity;
                    ucProviderView.Bind();

                    mvProvider.SetActiveView(vMasterAccount);
                }
                else
                {
                    ucProviderUserView.Entity = Entity;
                    ucProviderUserView.Bind();

                    mvProvider.SetActiveView(vProviderUser);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnVerify.Visible = false;
            btnUnVerify.Visible = false;
            btnReject.Visible = false;
            btnRestore.Visible = false;

            if (Entity.CertificationNumber != null)
            {
                if (Entity.VerificationStatus == enVerificationStatus.NotVerified && new InternshipProviderRepository(UnitOfWork).IsAfmVerified(Entity.ID, Entity.AFM))
                {
                    lblErrors.Visible = true;
                    lblErrors.Text = "Υπάρχει ήδη ένας πιστοποιημένος Φορέας με το ίδιο Α.Φ.Μ.<br/>Είστε σίγουροι ότι θέλετε να προχωρήσετε σε πιστοποίηση του λογαριασμού;";
                }

                btnUnVerify.Visible = Entity.VerificationStatus == enVerificationStatus.Verified;
                btnVerify.Visible = Entity.VerificationStatus == enVerificationStatus.NotVerified;
                btnReject.Visible = Entity.VerificationStatus == enVerificationStatus.NotVerified;
                btnRestore.Visible = Entity.VerificationStatus == enVerificationStatus.CannotBeVerified;
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (!IsValid)
                return;

            if (Entity.ProviderType != enProviderType.PublicCarrier && new InternshipProviderRepository(UnitOfWork).IsAfmVerified(Entity.ID, Entity.AFM))
            {
                lblErrors.Visible = true;
                lblErrors.Text = "Ο λογαριασμός δεν μπορεί να πιστοποιηθεί, γιατί υπάρχει ήδη άλλος πιστοποιημένος λογαριασμός (μη Δημόσιου Φορέα) με το ίδιο Α.Φ.Μ.";
                return;
            }

            try
            {
                Entity.VerificationStatus = enVerificationStatus.Verified;
                Entity.VerificationDate = DateTime.Now;

                var verificationLog1 = new VerificationLog();
                verificationLog1.ReporterID = Entity.ID;
                verificationLog1.OldVerificationStatus = enVerificationStatus.NotVerified;
                verificationLog1.NewVerificationStatus = enVerificationStatus.Verified;
                verificationLog1.CreatedAt = DateTime.Now;
                verificationLog1.CreatedBy = Page.User.Identity.Name;
                UnitOfWork.MarkAsNew(verificationLog1);

                HashSet<string> emails = new HashSet<string>();

                emails.Add(Entity.LegalPersonEmail);
                emails.Add(Entity.ContactEmail);

                if (!string.IsNullOrEmpty(Entity.AlternateContactEmail))
                {
                    emails.Add(Entity.AlternateContactEmail);
                }

                foreach (string email in emails)
                {
                    var sentEmail = MailSender.SendProviderVerification(Entity.ID, email, Entity.UserName, Entity.Language.GetValueOrDefault());
                    UnitOfWork.MarkAsNew(sentEmail);
                }

                if (Entity.ProviderType != enProviderType.PublicCarrier)
                {
                    IList<InternshipProvider> providers = new InternshipProviderRepository(UnitOfWork).FindProvidersByVerificationStatus(Entity.AFM, enVerificationStatus.NotVerified);

                    foreach (InternshipProvider provider in providers)
                    {
                        if (provider.ID != Entity.ID)
                        {
                            provider.VerificationStatus = enVerificationStatus.CannotBeVerified;

                            var verificationLog2 = new VerificationLog();
                            verificationLog2.ReporterID = provider.ID;
                            verificationLog2.OldVerificationStatus = enVerificationStatus.NotVerified;
                            verificationLog2.NewVerificationStatus = enVerificationStatus.CannotBeVerified;
                            verificationLog2.CreatedAt = DateTime.Now;
                            verificationLog2.CreatedBy = Page.User.Identity.Name;
                            UnitOfWork.MarkAsNew(verificationLog2);
                        }
                    }
                }

                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, this, string.Format("Προέκυψε σφάλμα στην πιστοποίηση του Φορέα με ID {0}", Entity.ID));
                Entity.VerificationStatus = enVerificationStatus.NotVerified;
                UnitOfWork.Commit();
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void btnUnVerify_Click(object sender, EventArgs e)
        {
            Entity.VerificationStatus = enVerificationStatus.NotVerified;
            Entity.VerificationDate = null;

            VerificationLog verificationLog = new VerificationLog();
            verificationLog.ReporterID = Entity.ID;
            verificationLog.OldVerificationStatus = enVerificationStatus.Verified;
            verificationLog.NewVerificationStatus = enVerificationStatus.NotVerified;
            verificationLog.CreatedAt = DateTime.Now;
            verificationLog.CreatedBy = Page.User.Identity.Name;
            UnitOfWork.MarkAsNew(verificationLog);
            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            Entity.VerificationStatus = enVerificationStatus.CannotBeVerified;

            VerificationLog verificationLog = new VerificationLog();
            verificationLog.ReporterID = Entity.ID;
            verificationLog.OldVerificationStatus = enVerificationStatus.NotVerified;
            verificationLog.NewVerificationStatus = enVerificationStatus.CannotBeVerified;
            verificationLog.CreatedAt = DateTime.Now;
            verificationLog.CreatedBy = Page.User.Identity.Name;

            UnitOfWork.MarkAsNew(verificationLog);

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void btnRestore_Click(object sender, EventArgs e)
        {
            Entity.VerificationStatus = enVerificationStatus.NotVerified;

            VerificationLog verificationLog = new VerificationLog();
            verificationLog.ReporterID = Entity.ID;
            verificationLog.OldVerificationStatus = enVerificationStatus.CannotBeVerified;
            verificationLog.NewVerificationStatus = enVerificationStatus.NotVerified;
            verificationLog.CreatedAt = DateTime.Now;
            verificationLog.CreatedBy = Page.User.Identity.Name;

            UnitOfWork.MarkAsNew(verificationLog);

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }
    }
}
