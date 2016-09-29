using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;

namespace StudentPractice.Portal.Secure
{
    public partial class SecurePages : System.Web.UI.MasterPage
    {
        protected InternshipOffice Office { get; set; }
        protected InternshipProvider Provider { get; set; }
        protected Student Student { get; set; }
        protected int? MasterCountryID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Roles.IsUserInRole(RoleNames.MasterOffice) || Roles.IsUserInRole(RoleNames.OfficeUser))
            {
                Office = Context.LoadOffice() ?? new InternshipOfficeRepository().FindByUsername(Page.User.Identity.Name);
                if (Office != null)
                {
                    Office.SaveReporterIDToCurrentContext();
                    if (Office.MustChangePassword)
                        Response.Redirect("~/Common/ChangePassword.aspx");

                    var institution = CacheManager.Institutions.Get(Office.InstitutionID.Value);
                    loginBar.SetUserDetails(string.Format("Ίδρυμα: <strong style='font-weight:bold;'>{0}</strong>", institution.Name));
                }
                SetAlerts(Office);
            }

            else if (Roles.IsUserInRole(RoleNames.MasterProvider) || Roles.IsUserInRole(RoleNames.ProviderUser))
            {
                var pRep = new InternshipProviderRepository();
                Provider = Context.LoadProvider() ?? pRep.FindByUsername(Page.User.Identity.Name, x => x.MasterAccount);

                if (Provider != null)
                {
                    Provider.SaveReporterIDToCurrentContext();

                    if (!Provider.IsMasterAccount)
                        MasterCountryID = Context.LoadMasterCountryID() ?? pRep.Load(Provider.MasterAccountID.Value).CountryID;

                    if (IsForeign(Provider.CountryID) || IsForeign(MasterCountryID))
                        ucLanguageBar.Visible = true;

                    if (Provider.MustChangePassword)
                        Response.Redirect("~/Common/ChangePassword.aspx");

                    loginBar.SetUserDetails(string.Format("{0}: <strong style='font-weight:bold;'>{1}</strong>", Resources.ProviderInput.Name, Provider.Name));
                }
                SetAlerts(Provider);
            }

            else if (Roles.IsUserInRole(RoleNames.Student))
            {
                loginBar.ChangePasswordButton.Visible = false;
                loginBar.LogoutButton.Attributes["onclick"] = "alert('ΠΡΟΣΟΧΗ!\\n\\nΓια να ολοκληρωθεί η αποσύνδεσή σας από την υπηρεσία ΑΤΛΑΣ θα πρέπει να κλείσετε το φυλλομετρητή (browser) σας.');";
                Student = Context.LoadStudent() ?? new StudentRepository().FindByUsername(Page.User.Identity.Name);
                if (Student != null)
                {
                    Student.SaveReporterIDToCurrentContext();
                    if (Student.MustChangePassword)
                        Response.Redirect("~/Common/ChangePassword.aspx");

                    loginBar.ChangePasswordButton.Visible = false;
                    loginBar.SetUserDetails(string.Format("Όνομα Χρήστη: <strong style='font-weight:bold;'>{0} {1}</strong>", Student.GreekFirstName ?? Student.OriginalFirstName, Student.GreekLastName ?? Student.OriginalLastName));
                }
                SetAlerts(Student);
            }

            else
            {
                alertsArea.Visible = false;
            }
        }

        protected void Portal_ResolveScriptReference(object sender, ScriptReferenceEventArgs e)
        {
            if (e.Script.Name == string.Empty)
            {
                e.Script.Path += "?v=" + Global.VersionNumber;
            }
        }

        protected string GetTitle(SiteMapNode node)
        {
            return (LanguageService.GetUserLanguage() == enLanguage.English && !string.IsNullOrEmpty(node["titleEN"]))
                ? node["titleEN"]
                : node.Title;
        }

        private void SetAlerts(InternshipOffice office)
        {
            StringBuilder sb = new StringBuilder();
            IFormatProvider prov = new CultureInfo("el-GR");

            if (office.CanViewAllAcademics.HasValue)
            {
                if (!office.IsEmailVerified)
                {
                    sb.AppendFormat("<p>Δεν έχετε ακόμη πιστοποιήσει το e-mail που έχετε δηλώσει ({0}). ", office.Email);
                    sb.Append(@"Για οδηγίες πατήστε <a href='javascript:void(0)' onclick='window.open(""../../EmailVerificationInfo.aspx"",""colourExplanation"",""toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=700, height=350""); return false;' target='_blank'>εδώ</a></p>");

                    if (office.VerificationStatus == enVerificationStatus.NotVerified && office.IsMasterAccount)
                    {
                        sb.Append("<p>Θα πρέπει να πιστοποιήσετε το e-mail σας για να μπορέσετε να εκτυπώσετε τη Βεβαίωση Συμμετοχής και να χρησιμοποιήσετε την εφαρμογή.</p>");
                    }
                    ltAlerts.Text = sb.ToString();
                }
                else
                {
                    switch (office.VerificationStatus)
                    {
                        case enVerificationStatus.NotVerified:
                            sb.Append("<p>Δεν έχετε ακόμη πιστοποιήσει το λογαριασμό σας. Θα πρέπει να εκτυπώσετε τη Βεβαίωση Συμμετοχής από την καρτέλα <a href='Default.aspx'>Κεντρική Σελίδα</a> και να την αποστείλετε με ΦΑΞ στο Γραφείο Αρωγής για να προχωρήσει στην πιστοποίησή του.</p>");
                            break;
                        case enVerificationStatus.Verified:
                            alertsArea.Attributes.Add("class", "message");
                            sb.Append("<p>Ο λογαριασμός σας έχει πιστοποιηθεί και μπορείτε να εκτελέσετε όλες τις λειτουργίες που παρέχονται από το Πληροφοριακό Σύστημα. Βίντεο με οδηγίες μπορείτε να δείτε στην Κεντρική Σελίδα. ");
                            sb.Append("Αναλυτικές οδηγίες υπάρχουν <a href='http://atlas.grnet.gr/Files/Man_GPA_App.pdf' target='_blank'>εδώ</a></p>");
                            break;
                        case enVerificationStatus.CannotBeVerified:
                            break;
                    }
                }
                ltAlerts.Text = sb.ToString();
            }
            alertsArea.Visible = !string.IsNullOrWhiteSpace(ltAlerts.Text);

        }

        private void SetAlerts(InternshipProvider provider)
        {
            StringBuilder sb = new StringBuilder();
            IFormatProvider prov = new CultureInfo("el-GR");

            if (!provider.IsEmailVerified)
            {
                sb.AppendFormat("<p>{0} ({1}). {2} ",
                    Resources.ProviderLiterals.UnverifiedEmail1, provider.Email, Resources.ProviderLiterals.UnverifiedEmail2);
                sb.AppendFormat(@"<a href='javascript:void(0)' onclick='window.open(""../../EmailVerificationInfo.aspx"",""colourExplanation"",""toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=700, height=350""); return false;' target='_blank'>{0}</a><br/>", Resources.ProviderLiterals.UnverifiedEmail3);

                if (provider.VerificationStatus == enVerificationStatus.NotVerified && provider.IsMasterAccount)
                    sb.AppendFormat("{0}.</p>", Resources.ProviderLiterals.UnverifiedEmail4);
                ltAlerts.Text = sb.ToString();
            }
            else
            {
                switch (provider.VerificationStatus)
                {
                    case enVerificationStatus.NotVerified:
                        sb.AppendFormat("<p>{0} <a href='Default.aspx'>{1}</a> {2}.</li>",
                            Resources.ProviderLiterals.UnverifiedUser1, Resources.ProviderLiterals.UnverifiedUser2, Resources.ProviderLiterals.UnverifiedUser3);
                        break;
                    case enVerificationStatus.Verified:
                        alertsArea.Attributes.Add("class", "message");
                        sb.AppendFormat("<p>{0} {1} <a href='{2}' target='_blank'>{3}</a>.</p>",
                            Resources.ProviderLiterals.VerifiedUser1, Resources.ProviderLiterals.VerifiedUser2, Resources.ProviderLiterals.VerifiedUserUrl, Resources.ProviderLiterals.VerifiedUser3);
                        break;
                    case enVerificationStatus.CannotBeVerified:
                        sb.AppendFormat("<p>{0}<br/>{1}.</p>", Resources.ProviderLiterals.CannotBeVerifiedUser1, Resources.ProviderLiterals.CannotBeVerifiedUser2);
                        break;
                }
                ltAlerts.Text = sb.ToString();
            }
            alertsArea.Visible = !string.IsNullOrWhiteSpace(ltAlerts.Text);
        }

        private void SetAlerts(Student student)
        {
            if (!student.IsContactInfoCompleted)
                alertsArea.Visible = false;

            else if (!student.IsActive)
                ltAlerts.Text = "<p>Ο λογαριασμός σας έχει απενεργοποιηθεί, καθώς βρέθηκε ήδη εγγεγραμμένος χρήστης με τα ίδια στοιχεία.<br/><br/>Για περισσότερες πληροφορίες μπορείτε να επικοινωνήσετε με το Γραφείο Αρωγής Χρηστών.</p>";

            else if (!student.IsEmailVerified)
            {
                StringBuilder sb = new StringBuilder();
                IFormatProvider prov = new CultureInfo("el-GR");

                sb.AppendFormat("<p>Δεν έχετε ακόμη πιστοποιήσει το e-mail που έχετε δηλώσει ({0}). ", student.ContactEmail);
                sb.Append(@"Για οδηγίες πατήστε <a href='javascript:void(0)' onclick='window.open(""../../EmailVerificationInfo.aspx"",""colourExplanation"",""toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=700, height=350""); return false;' target='_blank'>εδώ</a>. ");
                sb.Append("Είναι σημαντικό να πιστοποιήσετε το e-mail σας, για να λαμβάνετε ενημερώσεις σχετικά με τη διαδικασία εκπόνησης της Πρακτικής σας Άσκησης. ");
                sb.Append("Σε κάθε περίπτωση, μπορείτε να συνεχίσετε με τη χρήση της εφαρμογής ακόμα και αν δεν έχετε λάβει το e-mail επιβεβαίωσης.</p>");

                ltAlerts.Text = sb.ToString();
            }
            else
                alertsArea.Visible = false;

        }

        private bool IsForeign(int? countryID)
        {
            if (!countryID.HasValue)
                return false;

            return (countryID.Value != StudentPracticeConstants.GreeceCountryID && countryID.Value != StudentPracticeConstants.CyprusCountryID);
        }
        protected bool IsNodeVisible(object node)
        {
            var smNode = node as SiteMapNode;
            if (smNode != null)
            {
                //if (Provider != null && smNode.Url == "/Secure/InternshipProviders/Evaluation.aspx" && (IsForeign(Provider.CountryID) || IsForeign(MasterCountryID)))
                //    return false;
            }

            return true;
        }

    }
}