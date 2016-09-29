<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderUserInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipProviderControls.InputControls.ProviderUserInput" %>

<%@ Register TagPrefix="my" TagName="AddressInfoInput" Src="~/UserControls/GenericControls/AddressInfoInput.ascx" %>

<script type="text/javascript">
    var altName;
    var altPhone;
    var altMobilePhone;
    var altEmail;
    $(function () {
        //Cache the objects for extra speed 
        altName = $('#txtAlternateContactName');
        altPhone = $('#txtAlternateContactPhone');
        altMobilePhone = $('#txtAlternateContactMobilePhone');
        altEmail = $('#txtAlternateContactEmail');
    });
    function validateAlternativeGroup(s, e) {
        if (altName.val() == '' && altPhone.val() == '' && altMobilePhone.val() == '' && altEmail.val() == '') {
            e.IsValid = true;
        }
        else {
            if ($('#' + s.controltovalidate).val() != '') {
                e.IsValid = true;
            }
            else {
                e.IsValid = false;
            }
        }
    }
</script>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, UserDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <th>
            <label for="txtUsername">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, Username %>" />:</label>
        </th>
        <td>
            <asp:TextBox ID="txtUsername" ClientIDMode="Static" runat="server" Width="30%" title="<%$ Resources:RegistrationInput, UsernameTooltip %>"
                onkeyup="Imis.Lib.NoGreekCharacters(this,false)" onblur="RemoveTags(this);" />

            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, UsernameValidation %>" CssClass="validation">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, UsernameValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername"
                CssClass="validation" Display="Dynamic" ValidationExpression="^([A-Za-z0-9_\-\.]){5,}$"
                ErrorMessage="<%$ Resources:RegistrationInput, UsernameInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, UsernameInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="trPassword1" runat="server">
        <th>
            <label for="txtPassword1">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, Password %>" />:</label>
        </th>
        <td>
            <asp:TextBox ID="txtPassword1" ClientIDMode="Static" runat="server" TextMode="Password" title="<%$ Resources:RegistrationInput, Password %>"
                Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" onblur="RemoveTags(this);" />

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
            <label for="txtPassword2">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, PasswordConfirm %>" />:</label>
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
            <label for="txtEmail">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, Email %>" /></label>
        </th>
        <td>
            <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server" Width="60%" title="<%$ Resources:RegistrationInput, Email %>" onblur="RemoveTags(this);" />

            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" ErrorMessage="<%$ Resources:RegistrationInput, EmailValidation %>"><asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, EmailValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:RegistrationInput, EmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:RegistrationInput, EmailInvalid %>" />
            </asp:RegularExpressionValidator>
    </tr>
    <tr id="trEmailConfirmation" runat="server">
        <th>
            <label for="txtEmailConfirmation">
                <asp:Literal runat="server" Text="<%$ Resources:RegistrationInput, EmailConfirmation %>" />:</label>
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
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderTypeHeader %>" />:
        </th>
        <td>
            <asp:DropDownList ID="ddlProviderType" runat="server" ClientIDMode="Static" OnInit="ddlProviderType_Init" Width="61%" />

            <asp:RequiredFieldValidator ID="rfvProviderType" Display="Dynamic" runat="server" ControlToValidate="ddlProviderType"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderTypeValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderTypeValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, PrimaryActivityHeader %>" />:
        </th>
        <td>
            <asp:DropDownList ID="ddlPrimaryActivity" runat="server" ClientIDMode="Static" OnInit="ddlPrimaryActivity_Init" Width="61%" />

            <asp:RequiredFieldValidator ID="rfvPrimaryActivity" Display="Dynamic" runat="server"
                ControlToValidate="ddlPrimaryActivity" ErrorMessage="<%$ Resources:ProviderInput, PrimaryActivityValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, PrimaryActivityValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, Name %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtName" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, NameTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvName" Display="Dynamic" runat="server" ControlToValidate="txtName"
                ErrorMessage="<%$ Resources:ProviderInput, NameValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, NameValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, TradeName %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtTradeName" runat="server" MaxLength="500" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, TradeNameTooltip %>" />
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AFM %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtAFM" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, AFMTooltip %>" />

            <asp:CustomValidator ID="cvAFM" runat="server" ControlToValidate="txtAFM" ErrorMessage="<%$ Resources:ProviderInput, AFMInvalid %>"
                ValidateEmptyText="false" Display="Dynamic" ClientValidationFunction="Imis.Lib.CheckAfm"
                OnServerValidate="cvAFM_ServerValidate">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AFMInvalid %>" />
            </asp:CustomValidator>
        </td>
    </tr>
    <tr id="trDOY" runat="server">
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, DOY %>" />:
        </td>
        <td>
            <asp:DropDownList ID="ddlDOY" runat="server" ClientIDMode="Static" OnInit="ddlDOY_Init" Width="61%" />
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderPhone %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtProviderPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, ProviderPhoneTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvProviderPhone" Display="Dynamic" runat="server" ControlToValidate="txtProviderPhone"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderPhoneValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderPhoneValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revProviderPhone" runat="server" ControlToValidate="txtProviderPhone"
                Display="Dynamic" ValidationExpression="^(2[0-9]{9})|(800[0-9]{7})|(801[0-9]{7})|([0-9]{5})|([0-9]{4})$" ErrorMessage="<%$ Resources:ProviderInput, ProviderPhoneInvalid %>">
                <img id="imgRevProviderPhone" src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderPhoneInvalid %>" />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderFax %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtProviderFax" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, ProviderFaxTooltip %>" />

            <asp:RegularExpressionValidator ID="revProviderFax" runat="server" ControlToValidate="txtProviderFax"
                Display="Dynamic" ValidationExpression="^2[0-9]{9}$" ErrorMessage="<%$ Resources:ProviderInput, ProviderFaxValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderFaxValidation %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderEmail %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtProviderEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, ProviderEmailTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvProviderEmail" Display="Dynamic" runat="server" ControlToValidate="txtProviderEmail"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderEmailValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderEmailValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revProviderEmail" runat="server" ControlToValidate="txtProviderEmail"
                Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderEmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderEmailInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderURL %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtProviderURL" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, ProviderURLTooltip %>" />

            <asp:RegularExpressionValidator ID="revProviderURL" runat="server" ControlToValidate="txtProviderURL"
                Display="Dynamic" ValidationExpression="^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderUrlInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderUrlInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderNumberOfEmployees %>" />:
            <asp:Image runat="server" CssClass="tooltip" ImageUrl="~/_img/iconHelp.png" tip="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesAnalytical %>" />
        </th>
        <td>
            <asp:TextBox ID="txtProviderNumberOfEmployees" runat="server" Columns="20" Width="20%" title="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvProviderNumberOfEmployees" runat="server" ControlToValidate="txtProviderNumberOfEmployees"
                Display="Dynamic" ErrorMessage="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revProviderNumberOfEmployees" runat="server"
                ControlToValidate="txtProviderNumberOfEmployees" Display="Dynamic" ValidationExpression="^\d{1,6}$"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesInvalid %>" />
            </asp:RegularExpressionValidator>

            <asp:CustomValidator ID="cvProviderNumberOfEmployees" runat="server" ControlToValidate="txtProviderNumberOfEmployees"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesZero %>"
                ValidateEmptyText="false" Display="Dynamic" OnServerValidate="cvProviderNumberOfEmployees_ServerValidate">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesZero %>" />
            </asp:CustomValidator>
        </td>
    </tr>
</table>
        <my:AddressInfoInput ID="ucAddressInfoInput" runat="server" />
   
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ContactPersonDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ContactName %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtContactName" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, ContactNameTooltip %>" />
            <asp:RequiredFieldValidator ID="rfvContactName" Display="Dynamic" runat="server" ControlToValidate="txtContactName"
                ErrorMessage="<%$ Resources:ProviderInput, ContactNameValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactNameValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ContactPhone %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, ContactPhoneTooltip %>" />
            <asp:RequiredFieldValidator ID="rfvContactPhone" Display="Dynamic" runat="server" ControlToValidate="txtContactPhone"
                ErrorMessage="<%$ Resources:ProviderInput, ContactPhoneValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactPhoneValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactPhone" runat="server" ControlToValidate="txtContactPhone"
                Display="Dynamic" ValidationExpression="^2[0-9]{9}$" ErrorMessage="<%$ Resources:ProviderInput, ContactPhoneInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactPhoneInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ContactMobilePhone %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtContactMobilePhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, ContactMobilePhoneTooltip %>" />
            <asp:RequiredFieldValidator ID="rfvContactMobilePhone" Display="Dynamic" runat="server"
                ControlToValidate="txtContactMobilePhone" ErrorMessage="<%$ Resources:ProviderInput, ContactMobilePhoneValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactMobilePhoneValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactMobilePhone" runat="server" ControlToValidate="txtContactMobilePhone"
                Display="Dynamic" ValidationExpression="^69[0-9]{8}$" ErrorMessage="<%$ Resources:ProviderInput, ContactMobilePhoneInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactMobilePhoneInvalid %>" />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ContactEmail %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtContactEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, ContactEmailTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvContactEmail" Display="Dynamic" runat="server"
                ControlToValidate="txtContactEmail" ErrorMessage="<%$ Resources:ProviderInput, ContactEmailValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactEmailValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactEmail" runat="server" ControlToValidate="txtContactEmail"
                ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                Display="Dynamic" ErrorMessage="<%$ Resources:ProviderInput, ContactEmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactEmailInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AlternateContactPersonDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AlternateContactName %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactName" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, AlternateContactNameTooltip %>" />

            <asp:CustomValidator ID="cvAlternateContactName" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactName" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="<%$ Resources:ProviderInput, AlternateContactNameValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactNameValidation %>" />
            </asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AlternateContactPhone %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, AlternateContactPhoneTooltip %>" />

            <asp:CustomValidator ID="cvAlternateContactPhone" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactPhone" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="<%$ Resources:ProviderInput, AlternateContactPhoneValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactPhoneValidation %>" />
            </asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revAlternateContactPhone" runat="server" ControlToValidate="txtAlternateContactPhone"
                Display="Dynamic" ValidationExpression="^2[0-9]{9}$" ErrorMessage="<%$ Resources:ProviderInput, AlternateContactPhoneInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactPhoneInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AlternateContactMobilePhone %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactMobilePhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, AlternateContactMobilePhoneTooltip %>" />

            <asp:CustomValidator ID="cvAlternateContactMobilePhone" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactMobilePhone" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="<%$ Resources:ProviderInput, AlternateContactMobilePhoneValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactMobilePhoneValidation %>" />
            </asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revAlternateContactMobilePhone" runat="server"
                ControlToValidate="txtAlternateContactMobilePhone" Display="Dynamic" ValidationExpression="^69[0-9]{8}$"
                ErrorMessage="<%$ Resources:ProviderInput, AlternateContactMobilePhoneInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactMobilePhoneInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AlternateContactEmail %>" />:
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, AlternateContactEmailTooltip %>" />

            <asp:CustomValidator ID="cvAlternateContactEmail" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactEmail" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="<%$ Resources:ProviderInput, AlternateContactEmailValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactEmailValidation %>" />
            </asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revAlternateContactEmail" runat="server" ControlToValidate="txtAlternateContactEmail"
                Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:ProviderInput, AlternateContactEmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactEmailInvalid %>" />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
</table>
