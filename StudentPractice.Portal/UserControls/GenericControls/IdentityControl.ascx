<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IdentityControl.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.IdentityControl" %>

<table id="tbIdentificationInfo" width="100%" class="dv" style="border-top: hidden; margin-top: 0px;" runat="server">
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderIdType %>" />:
        </th>
        <td>
            <asp:RadioButtonList ID="rblIdType" runat="server" CssClass="SelfPublisher" RepeatLayout="Flow"
                RepeatDirection="Horizontal" OnInit="rblIdType_Init">
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Label ID="lblIdNumber" runat="server" Style="font-size: 11px" Text="<%$ Resources:ProviderInput, ProviderIdNumber %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtIdNumber" runat="server" MaxLength="100" Width="60%" />

            <asp:CustomValidator Display="Dynamic" runat="server" ID="cvNumber" ErrorMessage="<%$ Resources:ProviderInput, ProviderIdNumberValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderIdNumberValidation %>" />
            </asp:CustomValidator>
        </td>
    </tr>
    <tr class="idIssuer">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderIdIssuer %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtIdIssuer" runat="server" MaxLength="100" Width="60%" />

            <asp:CustomValidator runat="server" Display="Dynamic" ID="cvIssuer" ErrorMessage="<%$ Resources:ProviderInput, ProviderIdIssuerValidation %>">
                <img src="~/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderIdIssuerValidation %>" />
            </asp:CustomValidator>
        </td>
    </tr>
    <tr class="idIssueDate">
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ProviderIdIssueDate %>" />:
        </th>
        <td>
            <asp:TextBox ID="txtIdIssueDate" runat="server" MaxLength="100" Width="20%" />

            <asp:HyperLink ID="lnkSelectDate" runat="server" NavigateUrl="#">
                <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
            </asp:HyperLink>

            <act:CalendarExtender ID="ceSelectDate" runat="server" PopupButtonID="lnkSelectDate" TargetControlID="txtIdIssueDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:CustomValidator Display="Dynamic" runat="server" ID="cuvIssueDate" ErrorMessage="<%$ Resources:ProviderInput, ProviderIdIssueDateValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderIdIssueDateValidation %>" />
            </asp:CustomValidator>

            <asp:CompareValidator ID="cvIssueDate" runat="server" Display="Dynamic" Type="Date"
                Operator="DataTypeCheck" ControlToValidate="txtIdIssueDate" ErrorMessage="<%$ Resources:ProviderInput, ProviderIdIssueDateInvalid %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderIdIssueDateInvalid %>" />
            </asp:CompareValidator>

            <asp:CustomValidator runat="server" ID="cvMaxDate" OnServerValidate="cvMaxDate_ServerValidate"
                Display="Dynamic" ErrorMessage="<%$ Resources:ProviderInput, ProviderIdIssueDateValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ProviderIdIssueDateCustom %>" />
            </asp:CustomValidator>
        </td>
    </tr>
</table>
