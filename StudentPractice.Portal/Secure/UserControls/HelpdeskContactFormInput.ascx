<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpdeskContactFormInput.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.UserControls.HelpdeskContactFormInput" %>

<table width="100%" class="dv">
    <tr>
        <th style="width: 100px">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Name %>" />
        </th>
        <td>
            <asp:TextBox ID="txtReporterName" runat="server" Width="88%" onkeyup="Imis.Lib.ToUpper"></asp:TextBox>

            <asp:RequiredFieldValidator ID="rfvReporterName" runat="server" ControlToValidate="txtReporterName"
                ErrorMessage="<%$ Resources:HelpdeskContact, Form_NameRequired %>" Display="Dynamic">
                 <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_NameRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 100px">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Phone %>" />
        </th>
        <td>
            <asp:TextBox ID="txtReporterPhone" runat="server" MaxLength="10" Width="88%"></asp:TextBox>

            <asp:RequiredFieldValidator ID="rfvReporterPhone" runat="server" ControlToValidate="txtReporterPhone"
                Display="Dynamic" ErrorMessage="<%$ Resources:HelpdeskContact, Form_PhoneRequired %>">
                 <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_PhoneRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revReporterPhone" runat="server" ControlToValidate="txtReporterPhone" Display="Dynamic" ValidationExpression="^(2[0-9]{9})|(69[0-9]{8})$"                 
                ErrorMessage="Το πεδίο 'Τηλέφωνο' πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό.">
                <img src="/_img/error.gif" class="errortip" id="revReporterPhoneTip" runat="server" 
                     title="Το πεδίο 'Τηλέφωνο' πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό." />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
    <tr>
        <th style="width: 100px">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Email %>" />
        </th>
        <td>
            <asp:TextBox ID="txtReporterEmail" runat="server" Width="88%"></asp:TextBox>

            <asp:RequiredFieldValidator ID="rfvReporterEmail" runat="server" ControlToValidate="txtReporterEmail"
                Display="Dynamic" ErrorMessage="<%$ Resources:HelpdeskContact, Form_EmailRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_EmailRequired %>" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revReporterEmail" runat="server" ControlToValidate="txtReporterEmail"
                Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ErrorMessage="<%$ Resources:HelpdeskContact, Form_EmailRegex %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_EmailRegex %>" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 100px">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Reporter %>" />
        </th>
        <td align="left">
            <asp:DropDownList ID="ddlReporterType" runat="server" OnInit="ddlReporterType_Init" Width="88%" />

            <asp:RequiredFieldValidator ID="rfvReporterType" runat="server" ControlToValidate="ddlReporterType"
                Display="Dynamic" ErrorMessage="<%$ Resources:HelpdeskContact, Form_ReporterRequired %>">
                 <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_ReporterRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 100px">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_ReportType %>" />
        </th>
        <td>
            <asp:DropDownList ID="ddlIncidentType" runat="server" Width="88%" />

            <act:CascadingDropDown ID="cddIncidentType" runat="server" TargetControlID="ddlIncidentType"
                ParentControlID="ddlReporterType" Category="IncidentTypes" PromptText="<%$ Resources:HelpdeskContact, Form_ReportTypePrompt %>"
                ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetIncidentTypes"
                LoadingText="<%$ Resources:HelpdeskContact, Form_ReportTypeLoading %>">
            </act:CascadingDropDown>

            <asp:RequiredFieldValidator ID="rfvIncidentType" runat="server" ControlToValidate="ddlIncidentType"
                Display="Dynamic" ErrorMessage="<%$ Resources:HelpdeskContact, Form_ReportTypeRequired %>">
                 <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_ReportTypeRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 100px">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, Form_Text %>" />
        </th>
        <td>
            <asp:TextBox ID="txtReportText" runat="server" TextMode="MultiLine" Height="100px" Width="88%"></asp:TextBox>

            <asp:RequiredFieldValidator ID="rvfReportText" runat="server" ControlToValidate="txtReportText"
                Display="Dynamic" ErrorMessage="<%$ Resources:HelpdeskContact, Form_TextRequired %>">
                <img src="/_img/error.gif" class="errortip" runat="server" title="<%$ Resources:HelpdeskContact, Form_TextRequired %>" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
