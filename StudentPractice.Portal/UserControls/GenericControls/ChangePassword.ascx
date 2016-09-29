<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.ChangePassword" %>

<table style="width: 100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            <asp:Literal runat="server" Text="<%$ Resources:ChangePassword, Header %>" />
        </th>
    </tr>
    <tr id="trOldPassword" runat="server" visible="true">
        <th>
            <label for="txtOldPassword">
                <asp:Literal runat="server" Text="<%$ Resources:ChangePassword, OldPassword %>" />
            </label>
        </th>
        <td>
            <asp:TextBox ID="txtOldPassword" ClientIDMode="Static" runat="server" TextMode="Password" Width="89%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />

            <imis:CapsWarning ID="CapsWarning2" runat="server" TextBoxControlId="txtOldPassword"
                CssClass="capsLockWarning" Text="<%$ Resources:GlobalProvider, CapsWarning %>">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvOldPassword" Display="Dynamic" runat="server"
                ControlToValidate="txtOldPassword" ErrorMessage="<%$ Resources:ChangePassword, OldPasswordRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ChangePassword, OldPasswordRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <label for="txtNewPassword">
                <asp:Literal runat="server" Text="<%$ Resources:ChangePassword, NewPassword %>" />
            </label>
        </th>
        <td>
            <asp:TextBox ID="txtNewPassword" ClientIDMode="Static" runat="server" TextMode="Password" Width="89%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />

            <imis:CapsWarning ID="CapsWarning1" runat="server" TextBoxControlId="txtNewPassword"
                CssClass="capsLockWarning" Text="<%$ Resources:GlobalProvider, CapsWarning %>">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvNewPassword" Display="Dynamic" runat="server" ControlToValidate="txtNewPassword"
                ErrorMessage="<%$ Resources:ChangePassword, NewPasswordRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ChangePassword, NewPasswordRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtNewPassword"
                Display="Dynamic" ValidationExpression="^(.){7,}$" ErrorMessage="<%$ Resources:ChangePassword, NewPasswordRule %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ChangePassword, NewPasswordRule %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>
            <label for="txtNewPasswordConfirmation">
                <asp:Literal runat="server" Text="<%$ Resources:ChangePassword, ConfirmPassword %>" />
            </label>
        </th>
        <td>
            <asp:TextBox ID="txtNewPasswordConfirmation" ClientIDMode="Static" runat="server" TextMode="Password" Width="89%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" />

            <imis:CapsWarning ID="CapsWarning3" runat="server" TextBoxControlId="txtNewPasswordConfirmation"
                CssClass="capsLockWarning" Text="<%$ Resources:GlobalProvider, CapsWarning %>">
            </imis:CapsWarning>

            <asp:RequiredFieldValidator ID="rfvNewPasswordConfirmation" Display="Dynamic" runat="server" ControlToValidate="txtNewPasswordConfirmation"
                ErrorMessage="<%$ Resources:ChangePassword, ConfirmPasswordRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ChangePassword, ConfirmPasswordRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvNewPasswordConfirmation" ControlToCompare="txtNewPassword" ControlToValidate="txtNewPasswordConfirmation"
                runat="server" Display="Dynamic" Operator="Equal" Type="String" ValueToCompare="Text" ErrorMessage="<%$ Resources:ChangePassword, PasswordMatch %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ChangePassword, PasswordMatch %>" />
            </asp:CompareValidator>
        </td>
    </tr>
</table>
