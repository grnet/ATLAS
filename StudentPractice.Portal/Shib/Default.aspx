<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="StudentPractice.Portal.Shib.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpHelpBar" runat="server">
    <div id="primary-menu">
        <ul>
            <li>
                <a href="/Default.aspx" class="home">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, HomePage %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href="http://atlas.grnet.gr/FAQ.aspx" class="faq">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, FAQ %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href='<%= string.Format("{0}/RedirectFromPortal.ashx?id=3&language=0", ConfigurationManager.AppSettings["StudentPracticeMainUrl"]) %>' class="contact">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, Contact %>" />
                </a>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <script type="text/javascript">
//<![CDATA[
        var terms, termsRead, btnSubmit, registerHref;
        $(function () {
            btnSubmit = $('#btnSubmit');
            registerHref = btnSubmit.attr('href');
            terms = $('#terms');
            if (terms.length > 0) {
                terms.scroll(function () {
                    acceptTerms();
                });

                acceptTerms();
            }
        });

        function acceptTerms() {
            if (!termsRead) {
                termsRead = terms[0].scrollHeight <= (terms[0].offsetHeight + terms[0].scrollTop);
            }

            btnSubmit.unbind('click');
            if (termsRead) {
                btnSubmit.removeClass('mask');
                btnSubmit.addClass('icon-btn');
                btnSubmit.attr('href', registerHref);
            }
            else {
                btnSubmit.addClass('mask');
                btnSubmit.removeClass('icon-btn');
                btnSubmit.attr('href', '#');
                btnSubmit.click(function () { return false; });
            }
        }
//]]>
    </script>
    <asp:MultiView ID="mvShibbolethLogin" runat="server" ActiveViewIndex="0">
        <asp:View ID="vNoStudentExists" runat="server">
            <asp:MultiView ID="mvRegister" runat="server" ActiveViewIndex="0">
                <asp:View ID="vTermsAndConditions" runat="server">
                    <div id="terms">
                        <h2>
                            Όροι και Προϋποθέσεις Συμμετοχής στο
                            <br />
                            Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ
                        </h2>
                        <ol>
                            <li>Κάθε συμμετέχων φοιτητής οφείλει να διαβάσει προσεκτικά τους παρόντες όρους και
                                προϋποθέσεις συμμετοχής πριν από την συμμετοχή του στο πρόγραμμα ΑΤΛΑΣ. Η συμμετοχή
                                του στο πρόγραμμα συνεπάγεται την αυτόματη αποδοχή των παρόντων όρων συμμετοχής.</li>
                            <li>Ο συμμετέχων φοιτητής στο πρόγραμμα ΑΤΛΑΣ αποδέχεται ότι τα στοιχεία που δηλώνει
                                στο πρόγραμμα είναι αληθή και επίκαιρα και ότι κάθε δήλωσή του στο πρόγραμμα επέχει
                                θέση Υπεύθυνης Δήλωσης κατά την έννοια και με τις συνέπειες του ν.1599/86, όπως
                                ισχύει.</li>
                            <li>Ο συμμετέχων φοιτητής στο πρόγραμμα «Άτλας» δηλώνει και αποδέχεται ρητά και χωρίς
                                καμία επιφύλαξη, ότι κάθε δραστηριότητά του στο Πληροφοριακό Σύστημα του Έργου θα
                                υπακούει στους παρόντες όρους και προϋποθέσεις και στην εκάστοτε ισχύουσα νομοθεσία.</li>
                            <li>Η συμμετοχή του φοιτητή στο πρόγραμμα ΑΤΛΑΣ σε καμία περίπτωση δεν συνεπάγεται την
                                αντιστοίχισή του σε κάποια θέση Πρακτικής Άσκησης. Την απόλυτη ευθύνη αντιστοίχισης
                                φοιτητών και θέσεων Πρακτικής Άσκησης έχουν τα Ακαδημαϊκά Ιδρύματα.</li>
                            <li>Η συλλογή και επεξεργασία των δεδομένων προσωπικού χαρακτήρα του συμμετέχοντα φοιτητή
                                υπόκειται στους όρους του παρόντος, στις σχετικές διατάξεις του ν. 2472/97, όπως
                                ισχύει, καθώς και στους ισχύοντες νόμους του εθνικού, ευρωπαϊκού και διεθνούς δικαίου
                                για την προστασία του ατόμου από την επεξεργασία δεδομένων προσωπικού χαρακτήρα.
                                Τα δεδομένα συλλέγονται και χρησιμοποιούνται για τους σκοπούς της συγκεκριμένης
                                υπηρεσίας και για την επικοινωνία με τους συμμετέχοντες φοιτητές και την ενημέρωση
                                τους από την ΕΔΕΤ Α.Ε.. Με την παροχή της συγκατάθεσης στο παρόν, ο φοιτητής παρέχει
                                και τη συγκατάθεσή του για τη συλλογή και επεξεργασία των δεδομένων προσωπικού χαρακτήρα
                                που τον αφορούν κατά τα οριζόμενα στο ν. 2472/97. Η ΕΔΕΤ Α.Ε. τηρεί το απόρρητο
                                των δεδομένων προσωπικού χαρακτήρα που συλλέγει και επεξεργάζεται για λογαριασμό
                                του Υπουργείου Παιδείας, Θρησκευμάτων, Πολιτισμού και Αθλητισμού και δεν διαβιβάζει
                                τα δεδομένα σε τρίτους με εξαίρεση τα προβλεπόμενα στο παρόν. H ΕΔΕΤ Α.Ε. λαμβάνει
                                τα κατάλληλα οργανωτικά και τεχνικά μέτρα για την ασφάλεια των δεδομένων και την
                                προστασία τους από τυχαία ή αθέμιτη καταστροφή, τυχαία απώλεια, αλλοίωση, απαγορευμένη
                                διάδοση ή πρόσβαση και κάθε άλλη μορφή αθέμιτης επεξεργασίας. Ο συμμετέχων φοιτητής
                                έχει το δικαίωμα ενημέρωσης σχετικά με τα προσωπικά δεδομένα που η ΕΔΕΤ Α.Ε. τηρεί.
                                Για πληροφορίες σχετικά με τα τηρούμενα προσωπικά στοιχεία και τη χρήση τους ή για
                                τη διόρθωση τους, ο φοιτητής μπορεί να απευθυνθεί (ηλεκτρονικά) στη διεύθυνση atlas.grnet.gr/Contact.aspx.
                            </li>
                            <li>Η ΕΔΕΤ Α.Ε. δεν φέρει καμία ευθύνη έναντι των συμμετεχόντων φοιτητών ή έναντι κάθε
                                τρίτου (νομικού ή φυσικού προσώπου) για τυχόν συμπεριφορά των χρηστών διαφορετική
                                από αυτή για την οποία προσφέρεται το πρόγραμμα ΑΤΛΑΣ. Οι χρήστες φέρουν την αποκλειστική
                                ευθύνη πρόσβασης στο πρόγραμμα και είναι οι μόνοι και οι αποκλειστικοί υπεύθυνοι
                                να αποκαταστήσουν πλήρως κάθε ζημία που θα υποστεί η ΕΔΕΤ Α.Ε. , οι συνεργάτες της
                                ή / και οποιοδήποτε τρίτο πρόσωπο εξαιτίας οποιασδήποτε αμφισβήτησης / διαφοράς
                                / διαμάχης που τυχόν προκύψει και θα οφείλεται στη μη συμμόρφωση των χρηστών ή εξουσιοδοτούμενων
                                από αυτούς προσώπων με την ισχύουσα νομοθεσία και με τους όρους του παρόντος. Κάθε
                                χρήστης υποχρεούται να αναδέχεται οποιαδήποτε αξίωση προβληθεί κατά της Ε∆ΕΤ Α.Ε.
                                και να απαλλάξει την Ε∆ΕΤ Α.Ε. και τους διευθυντές, υπαλλήλους, εργαζομένους και
                                αντιπροσώπους της από κάθε ευθύνη για αποζημίωση, έξοδα (συμπεριλαμβανομένων και
                                των ευλόγων δικαστικών εξόδων), δικαστικές αποφάσεις και άλλες δαπάνες ή απαιτήσεις
                                τρίτων που τυχόν προέλθουν από παραβίαση της ισχύουσας νομοθεσίας από τους συμμετέχοντες
                                στο πρόγραμμα ΑΤΛΑΣ. Η ΕΔΕΤ Α.Ε. δεν εμπλέκεται σε καμία περίπτωση και με κανένα
                                τρόπο σε οιαδήποτε διένεξη τυχόν προκύψει μεταξύ τρίτων, φυσικών ή νομικών προσώπων
                                που συμμετέχουν στο πρόγραμμα ΑΤΛΑΣ .</li>
                            <li>Η ΕΔΕΤ Α.Ε. διατηρεί το δικαίωμα τροποποίησης των παρόντων όρων και προϋποθέσεων
                                κατόπιν ενημέρωσης των συμμετεχόντων φοιτητών μέσω της παρούσας ιστοσελίδας. Για
                                τον λόγο αυτό κάθε συμμετέχων φοιτητής οφείλει να επισκέπτεται τακτικά την ιστοσελίδα
                                του έργου και να ελέγχει τους όρους και τις προϋποθέσεις συμμετοχής.</li>
                        </ol>
                    </div>
                    <div id="declaration">
                        <p>
                            <em>Δηλώνω υπεύθυνα ότι:</em></p>
                        <p>
                            Έχω διαβάσει και αποδέχομαι τους <em>Όρους και Προϋποθέσεις</em> συμμετοχής στο
                            &quot;Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ&quot; (κάντε
                            scroll για να τους διαβάσετε)</p>
                    </div>
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Συνέχεια Εγγραφής" CssClass="icon-btn bg-accept"
                        ClientIDMode="Static" OnClick="btnSubmit_Click" />
                </asp:View>
                <asp:View ID="vRegister" runat="server">
                    <div class="information">
                        <p>
                            <em>Εντοπίστηκε ότι είναι η πρώτη φορά που συνδέεστε στο Σύστημα Κεντρικής Υποστήριξης
                                της Πρακτικής Άσκησης Φοιτητών ΑΕΙ, κάνοντας login από τον Κατάλογο Χρηστών του
                                Ιδρύματός σας.</em>
                        </p>
                        <p>
                            <em>Από τον Κατάλογο Χρηστών του Ιδρύματός σας στάλθηκαν στο Πληροφοριακό Σύστημα Κεντρικής
                                Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ τα ακόλουθα στοιχεία, τα οποία θα
                                καταχωριστούν αυτόματα στο λογαριασμό σας.</em>
                        </p>
                        <p>
                            <em><span style="text-decoration: underline">Μην ανησυχείτε</span> αν ο Κατάλογος Χρηστών
                                επέστρεψε στο πεδίο του Ον/μου σας τόσο ελληνικούς όσο και λατινικούς χαρακτήρες.</em>
                        </p>
                        <table class="dv">
                            <tr>
                                <th>
                                    Όνομα:
                                </th>
                                <td>
                                    <asp:Label ID="lblFirstNameForAccount" runat="server" Font-Bold="true" ForeColor="Blue" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Επώνυμο:
                                </th>
                                <td>
                                    <asp:Label ID="lblLastNameForAccount" runat="server" Font-Bold="true" ForeColor="Blue" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Ίδρυμα:
                                </th>
                                <td>
                                    <asp:Label ID="lblInstitutionForAccount" runat="server" Font-Bold="true" ForeColor="Blue" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Σχολή:
                                </th>
                                <td>
                                    <asp:Label ID="lblSchoolForAccount" runat="server" Font-Bold="true" ForeColor="Blue" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Τμήμα:
                                </th>
                                <td>
                                    <asp:Label ID="lblDepartmentForAccount" runat="server" Font-Bold="true" ForeColor="Blue" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    Αρ. Μητρώου:
                                </th>
                                <td>
                                    <asp:Label ID="lblStudentNumberForAccount" runat="server" Font-Bold="true" ForeColor="Blue" />
                                </td>
                            </tr>
                        </table>
                        <p>
                            <em>Εάν τα στοιχεία αυτά ΔΕΝ είναι ακριβή, πατήστε ΑΚΥΡΩΣΗ και επικοινωνήστε με το Κέντρο
                                Δικτύων του Ιδρύματός σας ζητώντας να γίνει η σχετική διόρθωση στον Κατάλογο Χρηστών
                                του Ιδρύματός σας.</em></p>
                        <p>
                            <em>Εάν τα στοιχεία αυτά είναι ακριβή, πατήστε ΣΥΝΕΧΕΙΑ για να συνεχίσετε τη διαδικασία
                                εγγραφής σας στο Πληροφοριακό Σύστημα</em></p>
                        <script type="text/javascript">
                        //<![CDATA[
                            $(function () {
                                $('#btnRegister').click(function () {
                                    $(this).hide();
                                });
                            })
                        //]]>
                        </script>
                        <div style="clear: both; text-align: center; margin-top: 1em;">
                            <asp:LinkButton ID="btnRegister" runat="server" Text="Συνέχεια" CssClass="icon-btn bg-accept"
                                ClientIDMode="Static" OnClick="btnRegister_Click" />
                            <asp:LinkButton ID="btnCancel1" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                                OnClick="btnCancel_Click" />
                        </div>
                        <br />
                        <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
                    </div>
                </asp:View>
            </asp:MultiView>
        </asp:View>
        <asp:View ID="vOneStudentExists" runat="server">
            <div class="info">
                <p>
                    <em>Στο Πληροφοριακό Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ
                        είχαν καταχωριστεί τα εξής στοιχεία:</em></p>
                <table class="dv" width="100%">
                    <tr>
                        <th style="width: 130px">
                            Όνομα:
                        </th>
                        <td>
                            <asp:Label ID="lblFirstName" runat="server" Font-Bold="true" ForeColor="Blue" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 130px">
                            Επώνυμο:
                        </th>
                        <td>
                            <asp:Label ID="lblLastName" runat="server" Font-Bold="true" ForeColor="Blue" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Αρ. Μητρώου:
                        </th>
                        <td>
                            <asp:Label ID="lblStudentNumber" runat="server" Font-Bold="true" ForeColor="Blue" />
                        </td>
                    </tr>
                </table>
                <p>
                    <em>Από τον Κατάλογο Χρηστών του Ιδρύματός σας στάλθηκαν στο Πληροφοριακό Σύστημα Κεντρικής
                        Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ τα ακόλουθα στοιχεία, τα οποία θα
                        καταχωριστούν στο λογαριασμό σας χωρίς να έχετε τη δυνατότητα να τα αλλάξετε (σημειώνονται
                        με <span style="font-size: 12px; font-weight: bold; color: Red">κόκκινο χρώμα</span>
                        τα στοιχεία που διαφέρουν)</em></p>
                <table class="dv" width="100%">
                    <tr>
                        <th style="width: 130px">
                            Όνομα:
                        </th>
                        <td>
                            <asp:Label ID="lblFirstNameFromLDAP" runat="server" Font-Bold="true" ForeColor="Blue" />
                        </td>
                    </tr>
                    <tr>
                        <th style="width: 130px">
                            Επώνυμο:
                        </th>
                        <td>
                            <asp:Label ID="lblLastNameFromLDAP" runat="server" Font-Bold="true" ForeColor="Blue" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Αρ. Μητρώου:
                        </th>
                        <td>
                            <asp:Label ID="lblStudentNumberFromLDAP" runat="server" Font-Bold="true" ForeColor="Blue" />
                        </td>
                    </tr>
                </table>
                <p>
                    <em>Εάν τα στοιχεία αυτά ΔΕΝ είναι ακριβή, πατήστε ΑΚΥΡΩΣΗ και επικοινωνήσετε άμεσα
                        με το Γραφείο Αρωγής της δράσης ή το Κέντρο Δικτύων του Ιδρύματός σας ζητώντας να
                        γίνει η σχετική διόρθωση στον Κατάλογο Χρηστών του Ιδρύματός σας.</em></p>
                <p>
                    <em>Εάν τα στοιχεία αυτά είναι ακριβή, πατήστε ΣΥΝΕΧΕΙΑ για να ολοκληρωθεί η είσοδός
                        σας στο Πληροφοριακό Σύστημα</em></p>
                <div style="clear: both; text-align: center; margin-top: 1em;">
                    <asp:LinkButton ID="btnLogin" runat="server" Text="Συνέχεια" CssClass="icon-btn bg-accept"
                        OnClick="btnLogin_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                        OnClick="btnCancel_Click" />
                </div>
            </div>
        </asp:View>
        <asp:View ID="vMoreThanOneStudentsExist" runat="server">
            <div class="information">
                <p>
                    <em><span style="color: #f00">Βρέθηκαν παραπάνω από ένας φοιτητές με τον ίδιο αριθμό
                        μητρώου στη σχολή που ανήκετε.</span></em></p>
                <p>
                    <em><span style="color: #f00">Παρακαλούμε επικοινωνήστε με το Γραφείο Αρωγής της δράσης
                        για να αναφέρετε το πρόβλημα.</span></em></p>
            </div>
        </asp:View>
        <asp:View ID="vError" runat="server">
            <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" />
        </asp:View>
        <asp:View ID="vNotAllowed" runat="server">
            <div class="reminder" style="font-weight: bold; color: Red">
                <%= StudentPractice.Portal.Config.StudentLoginMessage %>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
