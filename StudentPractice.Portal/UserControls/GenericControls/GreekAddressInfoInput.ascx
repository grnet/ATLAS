<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GreekAddressInfoInput.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.GenericControls.GreekAddressInfoInput" %>

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
            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" ClientIDMode="Static" Width="20%" />

            <asp:RequiredFieldValidator ID="rfvZipCode" Display="Dynamic" runat="server" ControlToValidate="txtZipCode"
                ErrorMessage="Το πεδίο 'Τ.Κ.' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τ.Κ.' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revZipCode" runat="server" ControlToValidate="txtZipCode"
                Display="Dynamic" ValidationExpression="^\d{5}$" ErrorMessage="Ο Τ.Κ. πρέπει να αποτελείται από ακριβώς 5 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Ο Τ.Κ. πρέπει να αποτελείται από ακριβώς 5 ψηφία" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>Περιφερειακή Ενότητα:</th>
        <td>
            <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="61%"
                OnInit="ddlPrefecture_Init" DataTextField="Name" DataValueField="ID" />

            <asp:RequiredFieldValidator ID="rfvPrefecture" Display="Dynamic" runat="server" ControlToValidate="ddlPrefecture"
                ErrorMessage="Το πεδίο 'Περιφερειακή Ενότητα' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Περιφερειακή Ενότητα' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Καλλικρατικός Δήμος:</th>
        <td>
            <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="61%" DataTextField="Name" DataValueField="ID" />

            <asp:RequiredFieldValidator ID="rfvCity" Display="Dynamic" runat="server" ControlToValidate="ddlCity"
                ErrorMessage="Το πεδίο 'Καλλικρατικός Δήμος' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Καλλικρατικός Δήμος' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- επιλέξτε καλλικρατικό δήμο --"
                ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
            </act:CascadingDropDown>
        </td>
    </tr>
</table>
