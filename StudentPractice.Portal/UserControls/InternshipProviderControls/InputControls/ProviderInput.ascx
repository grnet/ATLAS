<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderInput.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipProviderControls.InputControls.ProviderInput" %>

<%@ Register TagName="GreekAddressInfoInput" TagPrefix="my" Src="~/UserControls/GenericControls/GreekAddressInfoInput.ascx" %>
<%@ Register TagName="CyprusAddressInfoInput" TagPrefix="my" Src="~/UserControls/GenericControls/CyprusAddressInfoInput.ascx" %>
<%@ Register TagName="ForeignAddressInfoInput" TagPrefix="my" Src="~/UserControls/GenericControls/ForeignAddressInfoInput.ascx" %>
<%@ Register TagName="IdentityControl" TagPrefix="my" Src="~/UserControls/GenericControls/IdentityControl.ascx" %>

<script type="text/javascript">
    var altContactTable;
    var altName;
    var altPhone;
    var altMobilePhone;
    var altEmail;
    var btnAlternateContact;

    $(function () {
        //Cache the objects for extra speed 
        altContactTable = $('#tbAlternateContact');
        altName = $('#txtAlternateContactName');
        altPhone = $('#txtAlternateContactPhone');
        altMobilePhone = $('#txtAlternateContactMobilePhone');
        altEmail = $('#txtAlternateContactEmail');
        btnAlternateContact = $('#btnAlternateContact');

        if (altName.val() != '' && altPhone.val() != '' && altMobilePhone.val() != '' && altEmail.val() != '') {
            altContactTable.show();
            btnAlternateContact.hide();
        }
        else {
            altContactTable.hide();
            btnAlternateContact.show();
        }
    });

    function checkNumberOfEmployees(val, e) {
        if (!e || !e.Value || e.Value == null)
            return;
        e.IsValid = (e.Value > 0);
    }


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
            
            <asp:RequiredFieldValidator ID="rfvName" Display="Dynamic" runat="server" ControlToValidate="txtName" ErrorMessage="<%$ Resources:ProviderInput, NameValidation %>">
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
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AFM %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtAFM" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, AFMTooltip %>" />
            
            <asp:RequiredFieldValidator ID="rfvAFM" Display="Dynamic" runat="server" ControlToValidate="txtAFM" ErrorMessage="<%$ Resources:ProviderInput, AFMValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AFMValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:CustomValidator ID="cvAFM" runat="server" ControlToValidate="txtAFM" ErrorMessage="<%$ Resources:ProviderInput, AFMInvalid %>"
                ValidateEmptyText="false" Display="Dynamic" ClientValidationFunction="Imis.Lib.CheckAfm" OnServerValidate="cvAFM_ServerValidate">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AFMInvalid %>" />
            </asp:CustomValidator>
        </td>
    </tr>
    <tr id="trDOY" runat="server">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, DOY %>" />:
        </th>
        <td>
            <asp:DropDownList ID="ddlDOY" runat="server" ClientIDMode="Static" OnInit="ddlDOY_Init" Width="61%" />
            
            <asp:RequiredFieldValidator ID="rfvDOY" Display="Dynamic" runat="server" ControlToValidate="ddlDOY"
                ErrorMessage="<%$ Resources:ProviderInput, DOYValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, DOYValidation %>" />
            </asp:RequiredFieldValidator>
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
                <img src="/_img/error.gif" id="revProviderPhoneTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderPhoneInvalid %>" />
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
                <img src="/_img/error.gif" id="revProviderFaxTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderFaxValidation %>" />
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
                Display="Dynamic" ValidationExpression="^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|center|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$"
                ErrorMessage="<%$ Resources:ProviderInput, ProviderUrlInvalid %>">
                <img id="Img1" src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderUrlInvalid %>" />
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
                ErrorMessage="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesZero %>" ClientValidationFunction="checkNumberOfEmployees"
                ValidateEmptyText="false" Display="Dynamic" OnServerValidate="cvProviderNumberOfEmployees_ServerValidate">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderNumberOfEmployeesZero %>" />
            </asp:CustomValidator>
        </td>
    </tr>
</table>
<br />
<asp:MultiView ID="mvAddress" runat="server" ActiveViewIndex="0">
    <asp:View ID="vGreekAddress" runat="server">
        <my:GreekAddressInfoInput ID="ucGreekAddressInfoInput" runat="server" />
    </asp:View>
    <asp:View ID="vCyprusAddress" runat="server">
        <my:CyprusAddressInfoInput ID="ucCyprusAddressInfoInput" runat="server" />
    </asp:View>
    <asp:View ID="vForeignAddress" runat="server">
        <my:ForeignAddressInfoInput ID="ucForeignAddressInfoInput" runat="server" />
    </asp:View>
</asp:MultiView>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, LegalPersonDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, LegalPersonName %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtLegalPersonName" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, LegalPersonNameTooltip %>" />
            
            <asp:RequiredFieldValidator ID="rfvLegalPersonName" Display="Dynamic" runat="server"
                ControlToValidate="txtLegalPersonName" ErrorMessage="<%$ Resources:ProviderInput, LegalPersonNameValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, LegalPersonNameValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, LegalPersonPhone %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtLegalPersonPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:ProviderInput, LegalPersonPhoneTooltip %>" />
            
            <asp:RequiredFieldValidator ID="rfvLegalPersonPhone" Display="Dynamic" runat="server"
                ControlToValidate="txtLegalPersonPhone" ErrorMessage="<%$ Resources:ProviderInput, LegalPersonPhoneValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, LegalPersonPhoneValidation %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revLegalPersonPhone" runat="server" ControlToValidate="txtLegalPersonPhone"
                Display="Dynamic" ValidationExpression="^(2[0-9]{9})|(69[0-9]{8})$" ErrorMessage="<%$ Resources:ProviderInput, LegalPersonPhoneInvalid %>">
                <img src="/_img/error.gif" id="revLegalPersonPhoneTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, LegalPersonPhoneInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, LegalPersonEmail %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtLegalPersonEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, LegalPersonEmailTooltip %>" />

            <asp:RequiredFieldValidator ID="rfvLegalPersonEmail" Display="Dynamic" runat="server"
                ControlToValidate="txtLegalPersonEmail" ErrorMessage="<%$ Resources:ProviderInput, LegalPersonEmailValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, LegalPersonEmailValidation %>" />
            </asp:RequiredFieldValidator>
            
            <asp:RegularExpressionValidator ID="revLegalPersonEmail" runat="server" ControlToValidate="txtLegalPersonEmail"
                Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:ProviderInput, LegalPersonEmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, LegalPersonEmailInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<my:IdentityControl ID="idLegalPerson" runat="server" />

<br />
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
                <img src="/_img/error.gif" id="revContactPhoneTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactPhoneInvalid %>" />
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
                <img src="/_img/error.gif" id="revContactMobilePhoneTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactMobilePhoneInvalid %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ContactEmail %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtContactEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:ProviderInput, ContactEmailTooltip %>" />
            
            <asp:RequiredFieldValidator ID="rfvContactEmail" Display="Dynamic" runat="server" ControlToValidate="txtContactEmail" ErrorMessage="<%$ Resources:ProviderInput, ContactEmailValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactEmailValidation %>" />
            </asp:RequiredFieldValidator>
           
            <asp:RegularExpressionValidator ID="revContactEmail" runat="server" ControlToValidate="txtContactEmail"
                Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="<%$ Resources:ProviderInput, ContactEmailInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ContactEmailInvalid %>" />
            </asp:RegularExpressionValidator>            
        </td>
    </tr>
</table>
<br />
<a id="btnAlternateContact" runat="server" clientidmode="Static" href="javascript:void(0)" onclick="altContactTable.show(); btnAlternateContact.hide();"
    style="font-weight: bold; text-decoration: underline; color: Blue">
    <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, AddAlternateContactPerson %>" /></a>
<table id="tbAlternateContact" width="100%" class="dv">
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
                <img src="/_img/error.gif" id="revAlternateContactPhoneTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactPhoneInvalid %>" />
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
                <img src="/_img/error.gif" id="revAlternateContactMobilePhoneTip" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AlternateContactMobilePhoneInvalid %>" />
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
