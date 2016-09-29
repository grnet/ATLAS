<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderRegisterUserInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipProviderControls.InputControls.ProviderRegisterUserInput" %>

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
        spanAvailability.innerHTML = "<span style='color: #ccc;'><%# GetLiteral("Checking") %></span>";
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
                spanAvailability.innerHTML = "<span style='color: Red;'><%# GetLiteral("UsernameInUse") %></span>";
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
                spanEmailAvailability.innerHTML = "<span style='color: Red;'><%# GetLiteral("EmailInUse") %></span>";
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
    spanEmailAvailability.innerHTML = "<span style='color: #ccc;'><%# GetLiteral("Checking") %></span>";
    window.setTimeout("StudentPractice.Portal.PortalServices.Services.EmailExists('" + email + "', onSucceeded, onFailure);", 750);
}

function emailCheckerValidate(s, e) {
    e.IsValid = isEmailUnique;
}

function showTipsy(element) {
    $(element).tipsy('show');
}

//]]> 
</script>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, UserDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, Username %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtUsername" ClientIDMode="Static" runat="server" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)"
                onblur="usernameChecker(this);" title="<%$ Resources:RegistrationInput, UsernameTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" Display="Dynamic"
                ErrorMessage="<%$ Resources:RegistrationInput, UsernameValidation %>" CssClass="validation" EnableClientScript="true">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, UsernameValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername"
                CssClass="validation" Display="Dynamic" ValidationExpression="^([A-Za-z0-9_\-\.]){5,}$"
                ErrorMessage="<%$ Resources:RegistrationInput, UsernameInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, UsernameInvalid %>" />          
            </asp:RegularExpressionValidator>

            <asp:CustomValidator ID="cvUserName" runat="server" ClientValidationFunction="usernameCheckerValidate"
                ControlToValidate="txtUsername" ErrorMessage="<%$ Resources:RegistrationInput, UsernameInUse %>"
                ValidateEmptyText="false" Display="Dynamic">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, UsernameInUse %>" />
            </asp:CustomValidator>

            <span id="spanAvailability"></span>
        </td>
    </tr>
    <tr id="trPassword1" runat="server">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, Password %>" />:
            <asp:Image runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png" tip="<%$ Resources:RegistrationInput, PasswordInfo %>" />
        </th>
        <td>
            <asp:TextBox ID="txtPassword1" ClientIDMode="Static" runat="server" TextMode="Password"
                Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" title="<%$ Resources:RegistrationInput, PasswordTooltip %>" />

            <imis:CapsWarning ID="CapsWarning2" runat="server" TextBoxControlId="txtPassword1"
                CssClass="capsLockWarning" Text="<%$ Resources:RegistrationInput, PasswordCapsWarning %>">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvPassword1" runat="server" ControlToValidate="txtPassword1"
                Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, PasswordValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, PasswordValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revPassword1" runat="server" ControlToValidate="txtPassword1"
                Display="Dynamic" ValidationExpression="^(.){6,}$" ErrorMessage="<%$ Resources:RegistrationInput, PasswordInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, PasswordInvalid %>" />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
    <tr id="trPassword2" runat="server">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, PasswordConfirm %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)"
                ClientIDMode="Static" />

            <imis:CapsWarning ID="CapsWarning1" runat="server" TextBoxControlId="txtPassword2"
                CssClass="capsLockWarning" Text="<%$ Resources:RegistrationInput, PasswordCapsWarning %>">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtPassword2"
                Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, PasswordConfirmValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, PasswordConfirmValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvPassword2" ControlToCompare="txtPassword1" ControlToValidate="txtPassword2"
                runat="server" Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, PasswordConfirmMatch %>"
                Operator="Equal" Type="String" ValueToCompare="Text">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, PasswordConfirmMatch %>" />
            </asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, Email %>" />:
            <asp:Image runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png" tip="<%$ Resources:RegistrationInput, EmailInfo %>" />
        </th>
        <td>
            <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server" Width="60%" onblur="emailChecker(this);" title="<%$ Resources:RegistrationInput, EmailTooltip %>" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, EmailValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, EmailValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:RegistrationInput, EmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, EmailInvalid %>" />
            </asp:RegularExpressionValidator>

            <asp:CustomValidator ID="cvEmail" runat="server" ClientValidationFunction="emailCheckerValidate" ControlToValidate="txtEmail"
                ErrorMessage="<%$ Resources:RegistrationInput, EmailInUse %>" ValidateEmptyText="false" Display="Dynamic">
               <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, EmailInUse %>" />
            </asp:CustomValidator>

            <span id="spanEmailAvailability"></span>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, EmailConfirmation %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtEmailConfirmation" runat="server" Width="60%" ClientIDMode="Static" />

            <asp:RequiredFieldValidator ID="rfvEmailConfirmation" runat="server" ControlToValidate="txtEmailConfirmation"
                Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, EmailConfirmationValidation %>">
                 <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, EmailConfirmationValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvEmailConfirmation" ControlToCompare="txtEmail" ControlToValidate="txtEmailConfirmation"
                runat="server" Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, EmailConfirmationMatch %>"
                Operator="Equal" Type="String" ValueToCompare="Text">
                 <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, EmailConfirmationMatch %>" />
            </asp:CompareValidator>

        </td>
    </tr>
</table>
