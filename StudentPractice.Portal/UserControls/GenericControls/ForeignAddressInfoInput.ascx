<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForeignAddressInfoInput.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.GenericControls.ForeignAddressInfoInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ForeignAddressDetailsHeader %>" />
        </th>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, Country %>" />:</th>
        <td>
            <asp:DropDownList ID="ddlCountry" runat="server" OnInit="ddlCountry_Init" Width="61%" />

            <asp:RequiredFieldValidator ID="rfvCountry" Display="Dynamic" runat="server" ControlToValidate="ddlCountry"
                ErrorMessage="<%$ Resources:ProviderInput, CountryValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, CountryValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, Address %>" />:</th>
        <td>
            <asp:TextBox ID="txtAddress" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" />

            <asp:RequiredFieldValidator ID="rfvAddress" Display="Dynamic" runat="server" ControlToValidate="txtAddress"
                ErrorMessage="<%$ Resources:ProviderInput, AddressValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, AddressValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, ZipCode %>" />:</th>
        <td>
            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="50" ClientIDMode="Static" Width="60%" />

            <asp:RequiredFieldValidator ID="rfvZipCode" Display="Dynamic" runat="server" ControlToValidate="txtZipCode"
                ErrorMessage="<%$ Resources:ProviderInput, ZipCodeValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ZipCodeValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal runat="server" Text="<%$ Resources:ProviderInput, City %>" />:</th>
        <td>
            <asp:TextBox ID="txtCityText" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" />

            <asp:RequiredFieldValidator ID="rfvCityText" Display="Dynamic" runat="server" ControlToValidate="txtCityText"
                ErrorMessage="<%$ Resources:ProviderInput, CityValidation %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, CityValidation %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
