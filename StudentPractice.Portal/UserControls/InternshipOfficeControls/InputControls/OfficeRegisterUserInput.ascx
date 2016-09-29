<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeRegisterUserInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipOfficeControls.InputControls.OfficeRegisterUserInput" %>

<script type="text/javascript">
    //<![CDATA[
    var usernameCheckerTimer = null;
    var spanAvailability;
    var isUsernameUnique = true;
    var usernameToCheck;
    var emailCheckerTimer = null;
    var spanEmailAvailability;
    var isEmailUnique = true;
    var emailToCheck;

    Sys.Application.add_load(function() {
        spanAvailability = $get("spanAvailability");
        spanEmailAvailability = $get("spanEmailAvailability");
        // onpaste attribute is not defined in XHTML 1.0 or in any other HTML specification. 
        // so in order to prevent w3c validation to fail we use this script
        document.getElementById("txtPassword2").onpaste = function () { return false };
        document.getElementById("txtEmailConfirmation").onpaste = function () { return false };
    });

    function usernameChecker(usernameCtrl) {        
        var regx = /\s/g;        
        usernameCtrl.value = usernameCtrl.value.replace(regx,'');        
        
        var username = usernameCtrl.value;        
        if (usernameToCheck == username)
            return;
        else
            usernameToCheck = username;
        
        if(usernameCheckerTimer!=null)
            window.clearTimeout(usernameCheckerTimer);
        
        spanAvailability.innerHTML = "";
        
        if (username.length >= 3)
            usernameCheckerTimer = window.setTimeout("checkUsernameUsage('" + username + "');", 100);
    }

    function checkUsernameUsage(username) {
        spanAvailability.innerHTML = "<span style='color: #ccc;'>γίνεται έλεγχος...</span>";
        window.setTimeout("StudentPractice.Portal.PortalServices.Services.UsernameExists('" + username + "', onSucceeded, onFailure);", 750);
    }

    function onFailure(result, ctx, methodName) {
        if (methodName == "UsernameExists")
            spanAvailability.innerHTML = "";
        else if (methodName == "EmailExists")
            spanEmailAvailability.innerHTML = "";
    }

    function onSucceeded(result, userContext, methodName) {
        if (methodName == "UsernameExists") {
            if (result == false) {
                spanAvailability.innerHTML = '';                
                isUsernameUnique = true;
            }
            else {
                isUsernameUnique = false;
                $get('<%= txtUsername.ClientID %>').focus();
                spanAvailability.innerHTML = '';
                spanAvailability.innerHTML = "<span style='color: Red;'>Το Όνομα Χρήστη χρησιμοποιείται</span>";
            }
            <%= cvUserName.ClientID %>.isvalid = isUsernameUnique;
            <%= cvUserName.ClientID %>.evaluationfunction(<%= cvUserName.ClientID %>);
            ValidatorUpdateDisplay(<%= cvUserName.ClientID %>);
        }
        else if (methodName == "EmailExists") {
            if (result == false) {
                spanEmailAvailability.innerHTML = '';                
                isEmailUnique = true;
            }
            else {
                isEmailUnique = false;
                $get('<%= txtEmail.ClientID %>').focus();
                spanEmailAvailability.innerHTML = '';
                spanEmailAvailability.innerHTML = "<span style='color: Red;'>Το Email χρησιμοποιείται</span>";
            }
            <%= cvEmail.ClientID %>.isvalid = isEmailUnique;
            <%= cvEmail.ClientID %>.evaluationfunction(<%= cvEmail.ClientID %>);
            ValidatorUpdateDisplay(<%= cvEmail.ClientID %>);
        }
}

function usernameCheckerValidate(s, e) {
    e.IsValid = isUsernameUnique;
}

function emailChecker(emailCtrl) {        
    if( (<%=revEmail.ClientID %>.isvalid) == true)
    {
        var email = emailCtrl.value;        
        if (emailToCheck == email)
            return;
        else
            emailToCheck = email;
        if(emailCheckerTimer!=null)
            window.clearTimeout(emailCheckerTimer);
        spanEmailAvailability.innerHTML = '';
        emailCheckerTimer = window.setTimeout("checkemailUsage('" + email + "');", 100);
    }
}

function checkemailUsage(email) {
    spanEmailAvailability.innerHTML = "<span style='color: #ccc;'>γίνεται έλεγχος...</span>";
    window.setTimeout("StudentPractice.Portal.PortalServices.Services.EmailExists('" + email + "', onSucceeded, onFailure);", 750);
}

function emailCheckerValidate(s, e) {
    e.IsValid = isEmailUnique;
}
//]]> 
</script>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Λογαριασμού Χρήστη
        </th>
    </tr>
    <tr>
        <th>Όνομα Χρήστη:
        </th>
        <td>
            <asp:TextBox ID="txtUsername" ClientIDMode="Static" runat="server" Width="30%" title="<%$ Resources:RegistrationInput, Username %>"
                onkeyup="Imis.Lib.NoGreekCharacters(this,false)" onblur="usernameChecker(this);" />

            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Όνομα Χρήστη' είναι υποχρεωτικό" CssClass="validation">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Όνομα Χρήστη' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername"
                CssClass="validation" Display="Dynamic" ValidationExpression="^([A-Za-z0-9_\-\.]){5,}$"
                ErrorMessage="Το 'Όνομα Χρήστη' πρέπει να αποτελείται από τουλάχιστον 5 χαρακτήρες. Επιτρέπονται μόνο λατινικοί χαρακτήρες, αριθμητικοί (π.χ. 1,2) ή ειδικοί (π.χ. _,-,.)">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το 'Όνομα Χρήστη' πρέπει να αποτελείται από τουλάχιστον 5 χαρακτήρες. Επιτρέπονται μόνο λατινικοί χαρακτήρες, αριθμητικοί (π.χ. 1,2) ή ειδικοί (π.χ. _,-,.)" />
            </asp:RegularExpressionValidator>

            <asp:CustomValidator ID="cvUserName" runat="server" ClientValidationFunction="usernameCheckerValidate"
                ControlToValidate="txtUsername" ErrorMessage="Το Όνομα Χρήστη χρησιμοποιείται"
                ValidateEmptyText="false" Display="Dynamic">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το 'Όνομα Χρήστη' χρησιμοποιείται" />
            </asp:CustomValidator>

            <span id="spanAvailability"></span>
        </td>
    </tr>
    <tr id="trPassword1" runat="server">
        <th>Κωδικός Πρόσβασης:
            <asp:Image runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png" tip="<%$ Resources:RegistrationInput, PasswordInfo %>" />
        </th>
        <td>
            <asp:TextBox ID="txtPassword1" ClientIDMode="Static" runat="server" TextMode="Password"
                Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" title="<%$ Resources:RegistrationInput, Password %>" />

            <imis:CapsWarning ID="CapsWarning2" runat="server" TextBoxControlId="txtPassword1"
                CssClass="capsLockWarning" Text="Προσοχή: το πλήκτρο Caps Lock είναι πατημένο">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvPassword1" runat="server" ControlToValidate="txtPassword1"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Κωδικός Πρόσβασης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Κωδικός Πρόσβασης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revPassword1" runat="server" ControlToValidate="txtPassword1"
                Display="Dynamic" ValidationExpression="^(.){6,}$" ErrorMessage="Ο Κωδικός Πρόσβασης πρέπει να αποτελείται από τουλάχιστον 6 χαρακτήρες">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Ο Κωδικός Πρόσβασης πρέπει να αποτελείται από τουλάχιστον 6 χαρακτήρες" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="trPassword2" runat="server">
        <th>Επιβεβαίωση Κωδικού:
        </th>
        <td>
            <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)"
                ClientIDMode="Static" />

            <imis:CapsWarning ID="CapsWarning1" runat="server" TextBoxControlId="txtPassword2"
                CssClass="capsLockWarning" Text="Προσοχή: το πλήκτρο Caps Lock είναι πατημένο">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtPassword2"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Επιβεβαίωση Κωδικού Πρόσβασης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Επιβεβαίωση Κωδικού Πρόσβασης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvPassword2" ControlToCompare="txtPassword1" ControlToValidate="txtPassword2"
                runat="server" Display="Dynamic" ErrorMessage="Ο Κωδικός Πρόσβασης και η Επιβεβαίωση Κωδικού Πρόσβασης πρέπει να ταιριάζουν"
                Operator="Equal" Type="String" ValueToCompare="Text">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Ο Κωδικός Πρόσβασης και η Επιβεβαίωση Κωδικού Πρόσβασης πρέπει να ταιριάζουν" />
            </asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div class="sub-description">
                <span style="font-size: 1em; font-weight: bold">Προσοχή:</span> Στο e-mail που θα
                δηλώσετε, θα σας σταλεί το e-mail ενεργοποίησης του λογαριασμού σας. Βεβαιωθείτε
                ότι το πληκτρολογήσατε σωστά.
            </div>
        </td>
    </tr>
    <tr>
        <th>E-mail:
        </th>
        <td>
            <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server" Width="60%" onblur="emailChecker(this);" title="<%$ Resources:RegistrationInput, Email %>" />

            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" ErrorMessage="Το πεδίο 'E-mail' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'E-mail' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το E-mail δεν είναι έγκυρο">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το E-mail δεν είναι έγκυρο" />
            </asp:RegularExpressionValidator>

            <asp:CustomValidator ID="cvEmail" runat="server" ClientValidationFunction="emailCheckerValidate"
                ControlToValidate="txtEmail" ErrorMessage="Το Email χρησιμοποιείται" ValidateEmptyText="false" Display="Dynamic">
               <img src="/_img/error.gif" class="errortip" runat="server" title="Το Email χρησιμοποιείται" />
            </asp:CustomValidator>

            <span id="spanEmailAvailability"></span>
        </td>
    </tr>
    <tr>
        <th>Επιβεβαίωση E-mail:
        </th>
        <td>
            <asp:TextBox ID="txtEmailConfirmation" runat="server" Width="60%" ClientIDMode="Static" />

            <asp:RequiredFieldValidator ID="rfvEmailConfirmation" runat="server" ControlToValidate="txtEmailConfirmation"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Επιβεβαίωση E-mail' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Επιβεβαίωση E-mail' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvEmailConfirmation" ControlToCompare="txtEmail" ControlToValidate="txtEmailConfirmation"
                runat="server" Display="Dynamic" ErrorMessage="Τα πεδία 'E-mail' και 'Επιβεβαίωση E-mail' πρέπει να ταιριάζουν"
                Operator="Equal" Type="String" ValueToCompare="Text">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Τα πεδία 'E-mail' και 'Επιβεβαίωση E-mail' πρέπει να ταιριάζουν" />
            </asp:CompareValidator>
        </td>
    </tr>
</table>
