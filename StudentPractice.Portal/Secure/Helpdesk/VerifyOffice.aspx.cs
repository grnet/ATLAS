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
    public partial class VerifyOffice : BaseEntityPortalPage<InternshipOffice>
    {
        protected override void Fill()
        {
            Entity = new InternshipOfficeRepository(UnitOfWork).Load(Convert.ToInt32(Request.QueryString["sID"]), x => x.Academics);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                ucOfficeInput.Entity = Entity;
                ucOfficeInput.Bind();
                ucOfficeInput.ReadOnly = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Entity.CanViewAllAcademics.HasValue && !Entity.CanViewAllAcademics.Value)
            {
                ucOfficeAcademicsGridView.DataSource = Entity.Academics;
                ucOfficeAcademicsGridView.DataBind();
            }
            else if (Entity.CanViewAllAcademics.HasValue && Entity.CanViewAllAcademics.Value)
            {
                ucOfficeAcademicsGridView.DataSource = new List<Academic>();
                ucOfficeAcademicsGridView.DataBind();
            }
            else
            {
                ucOfficeAcademicsGridView.Visible = false;
            }

            btnVerify.Visible = false;
            btnUnVerify.Visible = false;
            btnVerifyWithExistingAccount.Visible = false;
            btnReject.Visible = false;
            btnRestore.Visible = false;
            phVerificationComments.Visible = false;

            var officeRep = new InternshipOfficeRepository(UnitOfWork);

            if (Entity.CertificationNumber != null)
            {
                switch (Entity.VerificationStatus)
                {
                    case enVerificationStatus.NotVerified:
                        if (officeRep.VerifiedOfficeExists(Entity.ID, Entity.OfficeType, Entity.InstitutionID.Value, Entity.Academics.Select(x => x.ID).ToList()))
                        {
                            lblErrors.Visible = true;
                            lblErrors.Text = string.Format("Ο λογαριασμός δεν μπορεί να πιστοποιηθεί, γιατί υπάρχει ήδη πιστοποιημένος {0} λογαριασμός.", Entity.OfficeType == enOfficeType.Institutional ? "Ιδρυματικός" : "Τμηματικός ή Πολυ-Τμηματικός");
                        }
                        else
                        {
                            if (Entity.OfficeType == enOfficeType.Institutional)
                            {
                                if (officeRep.DepartmentalVerifiedOfficeExists(Entity.InstitutionID.Value))
                                {
                                    lblErrors.Visible = true;
                                    lblErrors.Text = "Υπάρχει ήδη πιστοποιημένος Τμηματικός ή Πολυ-Τμηματικός λογαριασμός για το Ιδρυματικό ΓΠΑ που θέλετε να πιστοποιήσετε. Βεβαιωθείτε ότι εισάγατε σωστά τα σχόλια πιστοποίησης που θα βρείτε στο κάτω μέρος αυτής της οθόνης.";

                                    phVerificationComments.Visible = true;
                                    lblCanHandleDepartmentalStudents.Text = "Θέλετε να μπορεί να διαχειρίζεται και το Ιδρυματικό τους φοιτητές που χειρίζονται τα αντίστοιχα Τμηματικά?";
                                    lblTransferStudentsToOtherAccount.Text = "Θέλετε να μεταφερθούν οι φοιτητές από τα ήδη υπάρχοντα Tμηματικά στον Ιδρυματικό λογαριασμό?";

                                    btnVerifyWithExistingAccount.Visible = true;
                                }
                                else
                                {
                                    btnVerify.Visible = true;
                                }
                            }
                            else
                            {
                                if (officeRep.InstitutionalVerifiedOfficeExists(Entity.InstitutionID.Value))
                                {
                                    lblErrors.Visible = true;
                                    lblErrors.Text = string.Format("Υπάρχει ήδη πιστοποιημένος Ιδρυματικός λογαριασμός για το {0} ΓΠΑ που θέλετε να πιστοποιήσετε. Βεβαιωθείτε ότι εισάγατε σωστά τα σχόλια πιστοποίησης που θα βρείτε στο κάτω μέρος αυτής της οθόνης.", Entity.OfficeType == enOfficeType.Departmental ? "Τμηματικό" : (Entity.OfficeType == enOfficeType.MultipleDepartmental ? "Πολυ-Τμηματικό" : ""));

                                    phVerificationComments.Visible = true;
                                    lblCanHandleDepartmentalStudents.Text = "Θέλετε να συνεχίσει να διαχειρίζεται και το Ιδρυματικό τους φοιτητές του Τμήματός/Τμημάτων αυτού του ΓΠΑ?";
                                    lblTransferStudentsToOtherAccount.Text = "Οι μέχρι σήμερα εξυπηρετούμενοι από το Ιδρυματικό φοιτητές, να μεταφερθούν στο Tμηματικό αυτό ΓΠΑ?";

                                    btnVerifyWithExistingAccount.Visible = true;
                                }
                                else
                                {
                                    btnVerify.Visible = true;
                                }
                            }
                        }

                        btnReject.Visible = true;

                        break;
                    case enVerificationStatus.Verified:
                        if (Entity.CanHandleDepartmentalStudents.HasValue)
                        {
                            phVerificationComments.Visible = true;

                            if (Entity.OfficeType == enOfficeType.Institutional)
                            {
                                phVerificationComments.Visible = true;
                                lblCanHandleDepartmentalStudents.Text = "Θέλετε να μπορεί να διαχειρίζεται και το Ιδρυματικό τους φοιτητές που χειρίζονται τα αντίστοιχα Τμηματικά?";
                                lblTransferStudentsToOtherAccount.Text = "Θέλετε να μεταφερθούν οι φοιτητές από τα ήδη υπάρχοντα Tμηματικά στον Ιδρυματικό λογαριασμό?";
                            }
                            else
                            {
                                phVerificationComments.Visible = true;
                                lblCanHandleDepartmentalStudents.Text = "Θέλετε να συνεχίσει να διαχειρίζεται και το Ιδρυματικό τους φοιτητές του Τμήματός/Τμημάτων αυτού του ΓΠΑ?";
                                lblTransferStudentsToOtherAccount.Text = "Οι μέχρι σήμερα εξυπηρετούμενοι από το Ιδρυματικό φοιτητές, να μεταφερθούν στο Tμηματικό αυτό ΓΠΑ?";
                            }


                            ddlCanHandleDepartmentalStudents.SelectedValue = Entity.CanHandleDepartmentalStudents.Value ? "1" : "2";
                            ddlCanHandleDepartmentalStudents.Enabled = false;

                            ddlTransferStudentsToOtherAccount.SelectedValue = Entity.TransferStudentsToOtherAccount.Value ? "1" : "2";
                            ddlTransferStudentsToOtherAccount.Enabled = false;
                        }

                        btnUnVerify.Visible = true;
                        break;
                    case enVerificationStatus.CannotBeVerified:
                        btnRestore.Visible = true;
                        break;
                }
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (!IsValid)
                return;

            try
            {
                Entity.VerificationStatus = enVerificationStatus.Verified;
                Entity.VerificationDate = DateTime.Now;

                if (phVerificationComments.Visible)
                {
                    int canHandleDepartmentalStudents;
                    if (int.TryParse(ddlCanHandleDepartmentalStudents.SelectedValue, out canHandleDepartmentalStudents) && canHandleDepartmentalStudents > 0)
                    {
                        Entity.CanHandleDepartmentalStudents = canHandleDepartmentalStudents == 1;
                    }

                    int transferStudentsToOtherAccount;
                    if (int.TryParse(ddlTransferStudentsToOtherAccount.SelectedValue, out transferStudentsToOtherAccount) && transferStudentsToOtherAccount > 0)
                    {
                        Entity.TransferStudentsToOtherAccount = transferStudentsToOtherAccount == 1;
                    }
                }

                var verificationLog1 = new VerificationLog();
                verificationLog1.ReporterID = Entity.ID;
                verificationLog1.OldVerificationStatus = enVerificationStatus.NotVerified;
                verificationLog1.NewVerificationStatus = enVerificationStatus.Verified;
                verificationLog1.CreatedAt = DateTime.Now;
                verificationLog1.CreatedBy = Page.User.Identity.Name;
                UnitOfWork.MarkAsNew(verificationLog1);

                HashSet<string> emails = new HashSet<string>();
                emails.Add(Entity.ContactEmail);

                if (!string.IsNullOrEmpty(Entity.AlternateContactEmail))
                {
                    emails.Add(Entity.AlternateContactEmail);
                }

                foreach (string email in emails)
                {
                    var sentEmail = MailSender.SendOfficeVerification(Entity.ID, email, Entity.UserName);
                    UnitOfWork.MarkAsNew(sentEmail);
                }

                UnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, this, string.Format("Προέκυψε σφάλμα στην πιστοποίηση του Γραφείου με ID {0}", Entity.ID));
                Entity.VerificationStatus = enVerificationStatus.NotVerified;
                UnitOfWork.Commit();
            }

            ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
        }

        protected void btnUnVerify_Click(object sender, EventArgs e)
        {
            Entity.VerificationStatus = enVerificationStatus.NotVerified;
            Entity.VerificationDate = null;
            Entity.CanHandleDepartmentalStudents = null;
            Entity.TransferStudentsToOtherAccount = null;

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
