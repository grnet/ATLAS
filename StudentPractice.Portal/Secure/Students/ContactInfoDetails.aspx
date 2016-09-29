<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" CodeBehind="ContactInfoDetails.aspx.cs"
    Inherits="StudentPractice.Portal.Secure.Students.ContactInfoDetails" Title="Στοιχεία Επικοινωνίας" %>

<%@ Register TagPrefix="my" Src="~/UserControls/StudentControls/InputControls/StudentInput.ascx" TagName="StudentInput" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="padding: 0px 0px 0px 10px; text-align: left;">
        <asp:ValidationSummary ID="vdSummary" runat="server" CssClass="validation-summary" />
    </div>
    <my:StudentInput ID="ucStudentInput" runat="server" ValidationGroup="vgStudentInput" />
    <br />
    <table style="width: 920px" class="dv">
        <tr>
            <th colspan="2" class="header">
                &raquo; Στοιχεία Επικοινωνίας
            </th>
        </tr>
        <tr>
            <td colspan="2">
                <div class="sub-description">
                    Είναι σημαντικό να δηλώσετε τα παρακάτω στοιχεία επικοινωνίας για να λαμβάνετε ενημερώσεις
                    σχετικά με την εκπόνηση της Πρακτικής σας Άσκησης
                </div>
            </td>
        </tr>
        <tr>
            <th style="width: 180px">
                E-mail:
            </th>
            <td style="width: 780px">
                <asp:TextBox ID="txtEmail" runat="server" Width="45%" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="vgStudentInput" Display="Dynamic" ErrorMessage="Το πεδίο 'E-mail' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                    ValidationGroup="vgStudentInput" runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                    ErrorMessage="Το E-mail δεν είναι έγκυρο">Το E-mail δεν είναι έγκυρο</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th style="width: 180px">
                Επιβεβαίωση E-mail:
            </th>
            <td style="width: 780px">
                <asp:TextBox ID="txtEmailConfirmation" runat="server" Width="45%" onpaste="return false" />
                <asp:RequiredFieldValidator ID="rfvEmailConfirmation" runat="server" ControlToValidate="txtEmailConfirmation"
                    ValidationGroup="vgStudentInput" Display="Dynamic" ErrorMessage="Το πεδίο 'Επιβεβαίωση E-mail' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvEmailConfirmation" ControlToCompare="txtEmail" ControlToValidate="txtEmailConfirmation"
                    ValidationGroup="vgStudentInput" runat="server" Display="Dynamic" ErrorMessage="Τα πεδία 'E-mail' και 'Επιβεβαίωση E-mail' πρέπει να ταιριάζουν"
                    Operator="Equal" Type="String" ValueToCompare="Text">Πρέπει να ταιριάζει με το πεδίο 'E-mail'</asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th style="width: 180px">
                Κινητό:
            </th>
            <td style="width: 780px">
                <asp:TextBox CssClass="source t3" ID="txtMobilePhone" runat="server" MaxLength="10"
                    Width="45%" />
                <asp:RequiredFieldValidator ID="rfvMobilePhone" Display="Dynamic" runat="server"
                    ControlToValidate="txtMobilePhone" ValidationGroup="vgStudentInput" ErrorMessage="Το πεδίο 'Κινητό' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revMobilePhone" runat="server" ControlToValidate="txtMobilePhone"
                    ValidationGroup="vgStudentInput" Display="Dynamic" ValidationExpression="^69[0-9]{8}$"
                    ErrorMessage="Το πεδίο 'Κινητό' πρέπει να ξεκινάει από 69 και να αποτελείται από 10 ψηφία">Πρέπει να ξεκινάει από 69 και να αποτελείται από 10 ψηφία</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th style="width: 180px">
                Επιβεβαίωση Κινητού:
            </th>
            <td style="width: 780px">
                <asp:TextBox ID="txtMobilePhoneConfirmation" runat="server" Width="45%" MaxLength="10"
                    onpaste="return false" />
                <asp:RequiredFieldValidator ID="rfvMobilePhoneConfirmation" runat="server" ControlToValidate="txtMobilePhoneConfirmation"
                    ValidationGroup="vgStudentInput" Display="Dynamic" ErrorMessage="Το πεδίο 'Επιβεβαίωση Κινητού' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvMobilePhoneConfirmation" runat="server" ControlToCompare="txtMobilePhone"
                    ControlToValidate="txtMobilePhoneConfirmation" ValidationGroup="vgStudentInput"
                    Display="Dynamic" ErrorMessage="Τα πεδία 'Κινητό' και 'Επιβεβαίωση Κινητού' πρέπει να ταιριάζουν"
                    Operator="Equal" Type="String" ValueToCompare="Text">Πρέπει να ταιριάζει με το πεδίο 'Κινητό'</asp:CompareValidator>
            </td>
        </tr>
    </table>
    <br />
    <div style="clear: both; text-align: left;">
        <asp:LinkButton ID="btnSave" runat="server" CssClass="icon-btn bg-save" OnClick="btnSave_Click" ValidationGroup="vgStudentInput"
            OnClientClick="javascript:return ValidatePage('vdApplication');" Text="Αποθήκευση" />
    </div>
    <br />
    <br />
    <div class="information">
        <em>Παρατηρήσεις</em>
        <br />

        Θα σας σταλεί e-mail επιβεβαίωσης στη διεύθυνση που θα δηλώσετε. Παρακαλούμε να
        επιλέξτε τον υπερσύνδεσμο που περιέχει το e-mail αυτό προκειμένου να επιβεβαιωθεί
        η εγκυρότητα του e-mail σας.
        <br />
        <br />

        <em>Σημαντική Σημείωση</em>
        <br />
        Υπάρχει περίπτωση ο e-mail client που χρησιμοποιείτε να έχει χαρακτηρίσει το e-mail
        επιβεβαίωσης ως "Ανεπιθύμητη Αλληλογραφία". Παρακαλούμε ελέγξτε το φάκελο "Ανεπιθύμητη
        Αλληλογραφία", "Junk E-mail" κ.λ.π.
        <br />
        <br />

        Επίσης, υπάρχει περίπτωση ο e-mail client που χρησιμοποιείτε να έχει διαγράψει αυτόματα
        το e-mail επιβεβαίωσης.
        <br />
        <br />

        <em>Σε κάθε περίπτωση</em> μπορείτε να συνεχίσετε με τη χρήση της εφαρμογής ακόμα
        και αν δεν έχετε λάβει το e-mail επιβεβαίωσης.
    </div>
</asp:Content>
