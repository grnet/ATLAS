<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddressInfoInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.AddressInfoInput" %>

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
            <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server" OnInit="ddlCountry_Init" Width="61%" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />

            <asp:RequiredFieldValidator ID="rfvCountry" Display="Dynamic" runat="server" ControlToValidate="ddlCountry"
                ErrorMessage="<%$ Resources:ProviderInput, CountryValidation %>">
                <img id="Img1" src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, CountryValidation %>" />
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

            <asp:RegularExpressionValidator ID="revZipCode" runat="server" ControlToValidate="txtZipCode"
                Display="Dynamic" ValidationExpression="^\d{5}$" ErrorMessage="<%$ Resources:ProviderInput, ZipCodeRegex %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:ProviderInput, ZipCodeRegex %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>

    <asp:MultiView ID="mvCountry" runat="server" ActiveViewIndex="0" ClientIDMode="Static">
        <asp:View runat="server" ID="vGreekCity" ClientIDMode="Static">
            <tr>
                <th>
                    <asp:Literal ID="litPrefecture" runat="server" />
                </th>
                <td>
                    <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="61%" OnInit="ddlPrefecture_Init" DataTextField="Name" DataValueField="ID" />

                    <asp:RequiredFieldValidator ID="rfvPrefecture" Display="Dynamic" runat="server" ControlToValidate="ddlPrefecture" ErrorMessage="<%$ Resources:PositionGroupInput, PrefectureGrRequired %>">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, PrefectureGrRequired %>" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th id="cityLit" runat="server">
                    <asp:Literal ID="litCity" runat="server" />
                </th>
                <td>
                    <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="61%" DataTextField="Name" DataValueField="ID" />

                    <asp:RequiredFieldValidator ID="rfvCity" Display="Dynamic" runat="server" ControlToValidate="ddlCity" ErrorMessage="<%$ Resources:PositionGroupInput, Kali_CityGrRequired %>">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, Kali_CityGrRequired %>" />
                    </asp:RequiredFieldValidator>

                    <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                        ParentControlID="ddlPrefecture" Category="Cities" ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
                    </act:CascadingDropDown>
                </td>
            </tr>
        </asp:View>

        <asp:View runat="server" ID="vForeignCity" ClientIDMode="Static">
            <tr>
                <th>
                    <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, City %>" />
                </th>
                <td>
                    <asp:TextBox ID="txtCityText" runat="server" ClientIDMode="Static" Width="60%" title="<%$ Resources:PositionInput, ForeignCity %>" />

                    <asp:RequiredFieldValidator ID="rfvCityText" Display="Dynamic" runat="server" ControlToValidate="txtCityText" ErrorMessage="<%$ Resources:PositionGroupInput, CityRequired %>">
                        <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:PositionGroupInput, CityRequired %>" />
                    </asp:RequiredFieldValidator>

                </td>
            </tr>
        </asp:View>
    </asp:MultiView>
</table>
