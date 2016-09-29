<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpdeskUserInput.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.HelpdeskControls.InputControls.HelpdeskUserInput" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Λογαριασμού Χρήστη
        </th>
    </tr>
    <tr>
        <th style="width: 30%">
            Όνομα Χρήστη:
        </th>
        <td>
            <asp:TextBox ID="txtUsername" runat="server" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />
            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Όνομα Χρήστη' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername"
                Display="Dynamic" ValidationExpression="^([A-Za-z0-9_\-\.]){5,}$" ErrorMessage="Το Όνομα Χρήστη πρέπει να αποτελείται από τουλάχιστον 5 χαρακτήρες. Επιτρέπονται μόνο λατινικοί χαρακτήρες, αριθμητικοί (π.χ. 1,2) ή ειδικοί (π.χ. _,-,.)"><img src="/_img/error.gif" title="Το όνομα χρήστη δεν είναι έγκυρο" /></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="trPassword1" runat="server">
        <th style="width: 30%">
            Κωδικός Πρόσβασης:            
        </th>
        <td>
            <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />
            <imis:CapsWarning ID="CapsWarning2" runat="server" TextBoxControlId="txtPassword1"
                CssClass="capsLockWarning" Text="Προσοχή: το πλήκτρο Caps Lock είναι πατημένο"></imis:CapsWarning>
            <asp:RequiredFieldValidator ID="rfvPassword1" runat="server" ControlToValidate="txtPassword1"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Κωδικός Πρόσβασης' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revPassword1" runat="server" ControlToValidate="txtPassword1"
                Display="Dynamic" ValidationExpression="^(.){6,}$" ErrorMessage="Ο Κωδικός Πρόσβασης πρέπει να αποτελείται από τουλάχιστον 6 χαρακτήρες"><img src="/_img/error.gif" title="Ο κωδικός πρόσβασης δεν είναι έγκυρος" /></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="trPassword2" runat="server">
        <th style="width: 30%">
            Επιβεβαίωση Κωδικού:            
        </th>
        <td>
            <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />
            <imis:CapsWarning ID="CapsWarning1" runat="server" TextBoxControlId="txtPassword2"
                CssClass="capsLockWarning" Text="Προσοχή: το πλήκτρο Caps Lock είναι πατημένο"></imis:CapsWarning>
            <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtPassword2"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Επιβεβαίωση Κωδικού Πρόσβασης' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="cvPassword2" ControlToCompare="txtPassword1" ControlToValidate="txtPassword2"
                runat="server" Display="Dynamic" ErrorMessage="Ο Κωδικός Πρόσβασης και η Επιβεβαίωση Κωδικού Πρόσβασης πρέπει να ταιριάζουν"
                Operator="Equal" Type="String" ValueToCompare="Text"><img src="/_img/error.gif" title="Ο Κωδικός Πρόσβασης και η Επιβεβαίωση Κωδικού Πρόσβασης πρέπει να ταιριάζουν" /></asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            E-mail:            
        </th>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" Width="90%" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" ErrorMessage="Το πεδίο 'E-mail' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το E-mail δεν είναι έγκυρο"><img src="/_img/error.gif" title="Το πεδίο δεν είναι έγκυρο" /></asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Χρήστη
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Ονοματεπώνυμο:            
        </td>
        <td>
            <asp:TextBox ID="txtContactName" runat="server" MaxLength="100" Width="90%" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Τηλέφωνο (κινητό):
        </td>
        <td>
            <asp:TextBox ID="txtContactMobilePhone" runat="server" MaxLength="10" Width="20%" />
            <asp:RegularExpressionValidator ID="revContactMobilePhone" runat="server" Display="Dynamic"
                ControlToValidate="txtContactMobilePhone" ValidationExpression="^69[0-9]{8}$"
                ErrorMessage="Το πεδίο 'Τηλέφωνο (κινητό)' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία"><img src="/_img/error.gif" title="Μη έγκυρος αριθμός τηλεφώνου" /></asp:RegularExpressionValidator>
        </td>
    </tr>
</table>