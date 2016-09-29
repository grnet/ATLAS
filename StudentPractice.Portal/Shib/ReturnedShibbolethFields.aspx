<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true"
    CodeBehind="ReturnedShibbolethFields.aspx.cs" Inherits="StudentPractice.Portal.Shib.ReturnedShibbolethFields" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div class="reminder">
        <asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Red" Text="Ο ΜΗΧΑΝΙΣΜΟΣ ΤΟΥ SHIBBOLETH ΔΕΝ ΕΙΝΑΙ ΣΩΣΤΑ ΡΥΘΜΙΣΜΕΝΟΣ" />
    </div>
    <br />
    <table width="100%" class="dv">
        <tr>
            <th colspan="2" class="header">
                &raquo; Παράδειγμα πεδίων που περιμένουμε να λάβουμε
            </th>
        </tr>
        <tr>
            <th style="width: 250px">
                Πλήρες Ον/μο:<br />
                (Πεδίο: HTTP_SHIBCN)
            </th>
            <td>
                <asp:Label runat="server" Font-Bold="true" ForeColor="Blue" Text="ΔΗΜΗΤΡΙΟΣ ΠΑΠΑΝΙΚΟΛΑΟΥ" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Όνομα:<br />
                (Πεδίο: HTTP_SHIBGIVENNAME)
            </th>
            <td>
                <asp:Label runat="server" Font-Bold="true" ForeColor="Blue" Text="ΔΗΜΗΤΡΙΟΣ" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Επώνυμο:
                <br />
                (Πεδίο: HTTP_SHIBSN)
            </th>
            <td>
                <asp:Label runat="server" Font-Bold="true" ForeColor="Blue" Text="ΠΑΠΑΝΙΚΟΛΑΟΥ" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Αναγνωριστικό Ιδρύματος:
                <br />
                (Πεδίο: HTTP_SHIBHOMEORGANIZATION)
            </th>
            <td>
                <asp:Label runat="server" Font-Bold="true" ForeColor="Blue" Text="upatras.gr" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="sub-description">
                    Όπου ισχύουν τα εξής για το παρακάτω παράδειγμα<br />
                    <br />
                    <span style="font-size: 11px; font-weight: bold;">upatras.gr:</span> Πρέπει να ταυτίζεται
                    με το πεδίο που επιστρέφεται από το HTTP_SHIBHOMEORGANIZATION<br />
                    <span style="font-size: 11px; font-weight: bold;">267:</span> Ο Κωδικός ΥΠΕΠΘ του
                    Τμήματος<br />
                    <span style="font-size: 11px; font-weight: bold;">10125:</span> Ο Αριθμός Μητρώου
                    του φοιτητή<br />
                    <br />
                    <span style="font-size: 11px; font-weight: bold;">Θέλει προσοχή στα κεφαλαία-μικρά στο
                        κομμάτι urn:mace:terena.org:schac:personalUniqueCode:gr</span>
                </div>
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Μοναδικό Αναγνωριστικό:<br />
                (Πεδίο: HTTP_SHIBPERSONALUNIQUECODE)
            </th>
            <td>
                <asp:Label runat="server" Font-Bold="true" ForeColor="Blue" Text="urn:mace:terena.org:schac:personalUniqueCode:gr:upatras.gr:267:10125" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Username:<br />
                (Πεδίο: HTTP_SHIBEPPN)
            </th>
            <td>
                <asp:Label runat="server" Font-Bold="true" ForeColor="Blue" Text="chem10125@upatras.gr" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table width="100%" class="dv">
        <tr>
            <th colspan="2" class="header">
                &raquo; Πεδία που ελήφθησαν από το Ίδρυμα
            </th>
        </tr>
        <tr>
            <th style="width: 250px">
                Πλήρες Ον/μο:
            </th>
            <td>
                <asp:Label ID="lblFullName" runat="server" Font-Bold="true" ForeColor="Blue" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Όνομα:
            </th>
            <td>
                <asp:Label ID="lblFirstName" runat="server" Font-Bold="true" ForeColor="Blue" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Επώνυμο:
            </th>
            <td>
                <asp:Label ID="lblLastName" runat="server" Font-Bold="true" ForeColor="Blue" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Αναγνωριστικό Ιδρύματος:
            </th>
            <td>
                <asp:Label ID="lblHomeOrganization" runat="server" Font-Bold="true" ForeColor="Blue"
                    Text="upatras.gr" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Μοναδικό Αναγνωριστικό:
            </th>
            <td>
                <asp:Label ID="lblPersonalUniqueCode" runat="server" Font-Bold="true" ForeColor="Blue" />
            </td>
        </tr>
        <tr>
            <th style="width: 250px">
                Username:
            </th>
            <td>
                <asp:Label ID="lblUserName" runat="server" Font-Bold="true" ForeColor="Blue" />
            </td>
        </tr>
    </table>
</asp:Content>
