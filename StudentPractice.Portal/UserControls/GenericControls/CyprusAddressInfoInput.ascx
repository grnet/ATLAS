<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CyprusAddressInfoInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.CyprusAddressInfoInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Διεύθυνσης Έδρας Φορέα Υποδοχής Πρακτικής Άσκησης</th>
    </tr>
    <tr>
        <th>Χώρα:</th>
        <td>
            <asp:DropDownList ID="ddlCountry" runat="server" OnInit="ddlCountry_Init" Width="61%" Enabled="false" />

            <asp:RequiredFieldValidator ID="rfvCountry" Display="Dynamic" runat="server" ControlToValidate="ddlCountry"
                ErrorMessage="Το πεδίο 'Χώρα' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Χώρα' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Οδός - Αριθμός:</th>
        <td>
            <asp:TextBox ID="txtAddress" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" />

            <asp:RequiredFieldValidator ID="rfvAddress" Display="Dynamic" runat="server" ControlToValidate="txtAddress"
                ErrorMessage="Το πεδίο 'Οδός - Αριθμός' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Οδός - Αριθμός' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Τ.Κ.:</th>
        <td>
            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="50" ClientIDMode="Static" Width="60%" />

            <asp:RequiredFieldValidator ID="rfvZipCode" Display="Dynamic" runat="server" ControlToValidate="txtZipCode"
                ErrorMessage="Το πεδίο 'Τ.Κ.' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τ.Κ.' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Επαρχία:</th>
        <td>
            <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="61%"
                OnInit="ddlPrefecture_Init" DataTextField="Name" DataValueField="ID" />

            <asp:RequiredFieldValidator ID="rfvPrefecture" Display="Dynamic" runat="server" ControlToValidate="ddlPrefecture"
                ErrorMessage="Το πεδίο 'Επαρχία' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Επαρχία' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Δήμος:</th>
        <td>
            <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="61%" DataTextField="Name" DataValueField="ID" />

            <asp:RequiredFieldValidator ID="rfvCity" Display="Dynamic" runat="server" ControlToValidate="ddlCity"
                ErrorMessage="Το πεδίο 'Δήμος' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Δήμος' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- επιλέξτε δήμο --"
                ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
            </act:CascadingDropDown>
        </td>
    </tr>
</table>
