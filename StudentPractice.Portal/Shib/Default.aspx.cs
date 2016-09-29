using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentPractice.BusinessModel;
using System.Drawing;
using Imis.Domain;
using System.Web.Security;
using StudentPractice.Portal;
using StudentPractice.Utils;

namespace StudentPractice.Portal.Shib
{
    public partial class Default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                Response.Redirect("~/Secure/Students/Default.aspx");

            if (!Config.StudentLoginAllowed)
            {
                mvShibbolethLogin.SetActiveView(vNotAllowed);
                return;
            }

            if (!Page.IsPostBack)
            {
                //gather all shibboleth server variables into a single object
                ShibDetails shibDetails = ShibDetails.Create(Request.ServerVariables);

                try
                {
                    //Αν ο φοιτητής ανήκει σε Τμήμα που έχει αλλάξει Ίδρυμα και προσπαθήσει να συνδεθεί με το παλιό Τμήμα
                    if ((shibDetails.AcademicID == "271" && shibDetails.HomeOrganization == "uwg.edu.gr")
                            || (shibDetails.AcademicID == "346" && shibDetails.HomeOrganization == "uwg.edu.gr")
                            || (shibDetails.AcademicID == "368" && shibDetails.HomeOrganization == "uwg.edu.gr")
                            || (shibDetails.AcademicID == "369" && shibDetails.HomeOrganization == "ucg.gr")
                            || (shibDetails.AcademicID == "479" && shibDetails.HomeOrganization == "teihal.gr")
                            || (shibDetails.AcademicID == "499" && shibDetails.HomeOrganization == "teihal.gr")
                        //|| (shibDetails.AcademicID == "544" && shibDetails.HomeOrganization == "teimes.gr")
                            || (shibDetails.AcademicID == "549" && shibDetails.HomeOrganization == "teithe.gr")
                        //|| (shibDetails.AcademicID == "555" && shibDetails.HomeOrganization == "teimes.gr")
                        //|| (shibDetails.AcademicID == "557" && shibDetails.HomeOrganization == "teimes.gr")
                            || (shibDetails.AcademicID == "577" && shibDetails.HomeOrganization == "teihal.gr")
                            || (shibDetails.AcademicID == "597" && shibDetails.HomeOrganization == "teihal.gr")
                            || (shibDetails.AcademicID == "703" && shibDetails.HomeOrganization == "teithe.gr")
                            || (shibDetails.AcademicID == "722" && shibDetails.HomeOrganization == "teihal.gr")
                        //|| (shibDetails.AcademicID == "736" && shibDetails.HomeOrganization == "teimes.gr")
                            || (shibDetails.AcademicID == "737" && shibDetails.HomeOrganization == "teihal.gr")
                            || (shibDetails.AcademicID == "740" && shibDetails.HomeOrganization == "teihal.gr"))
                    {
                        lblError.Text = "Με βάση τα στοιχεία που στάλθηκαν από τον Κατάλογο Χρηστών του Ιδρύματός σας εντοπίστηκε ότι ανήκετε σε Τμήμα το οποίο έχει μεταφερθεί σε άλλο Ίδρυμα.<br/><br/>Παρακαλείσθε να επικοινωνήσετε με τη μηχανοργάνωση ή το Κέντρο Δικτύων του Ιδρύματος στο οποίο έχετε ενταχθεί ώστε να σας δοθούν τα νέα στοιχεία εισόδου.<br/><br/>Για περισσότερες πληροφορίες μπορείτε να επικοινωνήσετε με το Γραφείο Αρωγής Χρηστών.";
                        mvShibbolethLogin.SetActiveView(vError);
                        return;
                    }

                    //Αν ο φοιτητής ανήκει στο ΤΕΙ ΙΟΝΙΩΝ ΝΗΣΩΝ κόψε προσωρινά την πρόσβαση
                    //var academic = CacheManager.Academics.Get(int.Parse(shibDetails.AcademicID));
                    //if (academic.InstitutionID == 32)
                    //{
                    //    lblError.Text = "Λόγω εργασιών συντήρησης και αναβάθμισης της υπηρεσίας Καταλόγου του Ιδρύματός σας, δεν επιτρέπεται προσωρινά η πρόσβασή σας στην εφαρμογή της Ακαδημαϊκής Ταυτότητας.<br/><br/>Ζητούμε συγγνώμη για την αναστάτωση.";
                    //    mvShibbolethLogin.SetActiveView(vError);
                    //    return;
                    //}

                    if (string.IsNullOrWhiteSpace(shibDetails.StudentCode) || shibDetails.StudentCode.ToNull() == "-")
                    {
                        lblError.Text = "Το Ίδρυμά σας δεν έχει ολοκληρώσει ακόμη τις απαραίτητες ενέργειες για την είσοδό σας στο σύστημα (επιστράφηκε κενή τιμή στον Αρ. Μητρώου σας)<br/><br/>Παρακαλούμε επικοινωνήστε με το Κέντρο Δικτύων του Ιδρύματός σας για να λυθεί το πρόβλημα.";
                        mvShibbolethLogin.SetActiveView(vError);
                        return;
                    }

                    string username = shibDetails.Username;
                    int academicID = int.Parse(shibDetails.AcademicID);
                    string studentNumber = shibDetails.StudentCode;

                    using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                    {
                        StudentRepository studentRepository = new StudentRepository(uow);

                        //Εάν δεν υπάρχει Shibbolized χρήστης με το συγκεκριμένο username
                        if (!studentRepository.ShibbolizedStudentExists(academicID, studentNumber))
                        {
                            //Αναζήτη στο Πάσο αν υπάρχει για επιβεβαίωση στοιχείων και με τα στοιχεία του Πάσο αναζήτηση για τα Previous Academics και ενημέρωση
                            var student = SearchStudentCard(academicID, studentNumber, uow);

                            //Αν βρεθεί τότε ανανεώνω τα στοιχεία και τον κάνω Login
                            if (student != null)
                            {
                                AuthenticationService.LoginReporter(student);
                                Response.Redirect("~/Secure/Students/Default.aspx");
                            }

                            //Αν βρεθεί τότε ανανεώνω τα στοιχεία και τον κάνω Login
                            else
                            {
                                //Στοιχεία από Κατάλογο Χρηστών Ιδρύματος
                                var academicForAccount = CacheManager.Academics.Get(int.Parse(shibDetails.AcademicID));

                                lblFirstNameForAccount.Text = shibDetails.FirstName;
                                lblLastNameForAccount.Text = shibDetails.LastName;
                                lblInstitutionForAccount.Text = academicForAccount.Institution;
                                lblSchoolForAccount.Text = academicForAccount.School ?? "-";
                                lblDepartmentForAccount.Text = academicForAccount.Department ?? "-";
                                lblStudentNumberForAccount.Text = shibDetails.StudentCode;
                            }
                        }

                        else
                        {
                            var students = studentRepository.FindShibbolizedStudentsByAcademicIDNameAndStudentNumber(academicID, shibDetails.FirstName.ToNull(), shibDetails.LastName.ToNull(), studentNumber);

                            //Εάν υπάρχει μόνο ένας φοιτητής με το συγκεκριμένο username και Ον/μο τον κάνουμε Login
                            if (students.Count == 1)
                            {
                                //Αναζήτη στο Πάσο αν υπάρχει για ενημέρωση των στοιχείων του
                                try
                                {
                                    students[0].UpdateInfo(uow);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.LogError(ex, this, "Failed to update from paso");
                                }

                                AuthenticationService.LoginReporter(students[0]);
                                Response.Redirect("~/Secure/Students/Default.aspx");
                            }
                            else
                            {
                                if (!Config.StudentLoginAllowed)
                                    mvShibbolethLogin.SetActiveView(vNotAllowed);

                                var noExactMatchStudents = studentRepository.FindShibbolizedStudentsByAcademicIDAndStudentNumber(academicID, studentNumber);

                                //Αν υπάρχει φοιτητής με το συγκεκριμένο username, αλλά διαφορετικό Ον/μο του εμφανίζουμε μήνυμα λάθους και τον προτρέπουμε να απευθυνθεί στο Γραφείο Αρωγής
                                if (noExactMatchStudents.Count == 1)
                                {
                                    //Στοιχεία από Κατάλογο Χρηστών Ιδρύματος
                                    var academicFromLDAP = CacheManager.Academics.Get(academicID);

                                    lblFirstNameFromLDAP.Text = shibDetails.FirstName;
                                    lblLastNameFromLDAP.Text = shibDetails.LastName;
                                    lblStudentNumberFromLDAP.Text = shibDetails.StudentCode;

                                    //Στοιχεία από λογαριασμό χρήστη                            
                                    var student = noExactMatchStudents[0];

                                    lblFirstName.Text = student.OriginalFirstName;
                                    lblLastName.Text = student.OriginalLastName;
                                    lblStudentNumber.Text = student.StudentNumber;

                                    if (student.OriginalFirstName != shibDetails.FirstName)
                                        lblFirstNameFromLDAP.ForeColor = Color.Red;

                                    if (student.OriginalLastName != shibDetails.LastName)
                                        lblLastNameFromLDAP.ForeColor = Color.Red;

                                    if (student.StudentNumber != shibDetails.StudentCode)
                                        lblStudentNumberFromLDAP.ForeColor = Color.Red;

                                    mvShibbolethLogin.SetActiveView(vOneStudentExists);
                                }
                                //Αν υπάρχουν παραπάνω από ένας φοιτητές με το ίδιο username,εμφανίζουμε μήνυμα λάθους
                                else
                                {
                                    mvShibbolethLogin.SetActiveView(vMoreThanOneStudentsExist);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    if (string.IsNullOrEmpty(shibDetails.FirstName))
                    {
                        mvShibbolethLogin.SetActiveView(vError);
                        lblError.Text = "Η πρόσβασή σας στο Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ δεν είναι εφικτή προσωρινά λόγω τεχνικών προβλημάτων στη διαδικασία ταυτοποίησης των στοιχείων σας από πλευράς του Ιδρύματός σας.<br/><br/>Μπορείτε να επικοινωνήσετε με το Γραφείο Αρωγής Χρηστών στο 215 215 7860 για να αναφέρετε το πρόβλημα.";
                    }
                }
            }

            //The redirect is happening from Shib ISAPI Extension
            //string url = "http://wayf.grnet.gr/";        
            base.OnLoad(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                mvRegister.SetActiveView(vRegister);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            ShibDetails shibDetails = ShibDetails.Create(Request.ServerVariables);
            int academicID = int.Parse(shibDetails.AcademicID);
            string studentNumber = shibDetails.StudentCode;

            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    StudentRepository sRep = new StudentRepository(uow);
                    var student = sRep.FindShibbolizedStudentByAcademicIDAndStudentNumber(academicID, studentNumber);

                    //Αντιγράφουμε τα στοιχεία που επέστρεψε ο Κατάλογος Χρηστών του Ιδρύματος στο λογαριασμό του χρήστη
                    sRep.UpdateShibDetails(student, shibDetails);

                    //Κάνουμε login το χρήστη
                    AuthenticationService.LoginReporter(student);
                }
                Response.Redirect("~/Secure/Students/Default.aspx");
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))//This happends due to Redirect so nothing to worry about
                    LogHelper.LogError(ex, this, string.Format("Error while updating user with LDAP Details --> FirstName: {0} --> LastName: {1} --> EDETCode: {2} --> StudentNumber: {3} --> Username: {4} --> Affiliation: {5}", shibDetails.FirstName, shibDetails.LastName, shibDetails.AcademicID, shibDetails.StudentCode, shibDetails.Username, shibDetails.Affiliation));
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            ShibDetails shibDetails = ShibDetails.Create(Request.ServerVariables);

            int academicID;
            if (!int.TryParse(shibDetails.AcademicID, out academicID) || CacheManager.Academics.Get(int.Parse(shibDetails.AcademicID)) == null)
            {
                lblErrors.Text = "Η πρόσβασή σας στο Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ δεν είναι εφικτή, γιατί επιστράφηκε λάθος τιμή στον Κωδικό της Σχολής από τον Κατάλογο Χρηστών του Ιδρύματός σας.<br/><br/>Παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής Χρηστών στο 215 215 7860 για να αναφέρετε το πρόβλημα μαζί με τη Σχολή στην οποία προσπαθείτε να συνδεθείτε και τον Αριθμό Μητρώου σας.";
                return;
            }

            if (!CacheManager.Academics.Get(int.Parse(shibDetails.AcademicID)).IsActive)
            {
                lblErrors.Text = "Με βάση τα στοιχεία που στάλθηκαν από τον Κατάλογο Χρηστών του Ιδρύματός σας εντοπίστηκε ότι ανήκετε σε Τμήμα το οποίο έχει καταργηθεί/συγχωνευτεί.<br/><br/>Εφόσον έχετε ενταχθεί σε κάποιο ενεργό Τμήμα, παρακαλείσθε να επικοινωνήσετε με τη μηχανοργάνωση ή το Κέντρο Δικτύων του Ιδρύματός σας για να γίνει η σχετική διόρθωση στα στοιχεία σας.<br/><br/>Για περισσότερες πληροφορίες, παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής Χρηστών.<br/><br/>";
                return;
            }

            if (new StudentRepository().FindByUsernameFromLDAP(shibDetails.Username, int.Parse(shibDetails.AcademicID), shibDetails.StudentCode) != null)
            {
                lblErrors.Text = "Δεν είναι δυνατή η σύνδεσή σας στην εφαρμογή, καθώς το Ίδρυμά σας έχει προχωρήσει σε αλλαγή του Αριθμού Μητρώου σας.<br/><br/>Για περισσότερες πληροφορίες παρακαλείσθε να επικοινωνήσετε με το Γραφείο Αρωγής Χρηστών.<br/><br/>";
                return;
            }

            using (var ctx = UnitOfWorkFactory.Create() as DBEntities)
            {
                if (ctx.Connection.State != ConnectionState.Open)
                    ctx.Connection.Open();
                using (var ts = ctx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        StudentRepository studentRepository = new StudentRepository(ctx);
                        var student = studentRepository.CreateOrGetStudent(shibDetails);

                        var provider = Roles.Provider as StudentPracticeRoleProvider;
                        provider.AddUsersToRoles(new[] { student.UserName }, new[] { RoleNames.Student }, ctx);
                        ts.Commit();

                        if (student.AcademicID.HasValue && !string.IsNullOrEmpty(student.StudentNumber))
                            ServiceWorker.UpdateStudentInfoFromStudentCard(student.ID, async: false, useQueue: true);

                        AuthenticationService.LoginReporter(student);
                        Response.Redirect("~/Secure/Students/Default.aspx", true);
                    }
                    catch (Exception ex)
                    {

                        if (!(ex is ThreadAbortException))//This happends due to Redirect sso nothing to worry about
                        {
                            ts.Rollback();
                            LogHelper.LogError(ex, this, string.Format("Error while creating user with LDAP Details --> FirstName: {0} --> LastName: {1} --> EDETCode: {2} --> StudentNumber: {3} --> Username: {4} --> Affiliation: {5}", shibDetails.FirstName, shibDetails.LastName, shibDetails.AcademicID, shibDetails.StudentCode, shibDetails.Username, shibDetails.Affiliation));
                        }
                    }

                }
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        private Student SearchStudentCard(int academicID, string studentNumber, IUnitOfWork uow)
        {
            var response = StudentCardNumberService.GetStudentInfo(new AcademicIDCardRequest()
            {
                AcademicID = academicID,
                StudentNumber = studentNumber,
                ServiceCallerID = 1
            });

            if (response != null && response.Success)
            {
                var studentRepository = new StudentRepository(uow);
                bool foundByPrevious = false;

                var student = studentRepository.FindActiveByStudentNumberAndAcademicID(response.StudentNumber, response.AcademicID);
                if (student == null && !string.IsNullOrEmpty(response.PreviousStudentNumber) && response.PreviousAcademicID.HasValue)
                {
                    foundByPrevious = true;
                    student = studentRepository.FindActiveByStudentNumberAndAcademicID(response.PreviousStudentNumber, response.PreviousAcademicID.Value);
                }

                if (student != null)
                {
                    if (foundByPrevious)
                    {
                        student.AcademicID = response.AcademicID;
                        student.StudentNumber = response.StudentNumber;
                        BusinessHelper.UpdateStudentGeneralInfo(student, response);
                    }

                    BusinessHelper.UpdateStudentAcademicInfo(student, response);
                    uow.Commit();

                    return student;
                }
                return null;
            }

            return null;
        }
    }
}