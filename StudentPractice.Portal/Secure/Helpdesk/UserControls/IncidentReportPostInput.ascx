<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncidentReportPostInput.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportPostInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Προσθήκη νέου μηνύματος
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Κατάσταση συμβάντος:
        </td>
        <td>
            <asp:DropDownList ID="ddlReportStatus" runat="server" OnInit="ddlReportStatus_Init"
                Width="460px" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Τύπος Κλήσης:
        </th>
        <td>
            <asp:DropDownList ID="ddlCallType" runat="server" OnInit="ddlCallType_Init" Width="460px" />
            <asp:RequiredFieldValidator ID="rfvCallType" Display="Dynamic" runat="server"
                ControlToValidate="ddlCallType" ErrorMessage="Το πεδίο 'Τύπος Κλήσης' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Κείμενο μηνύματος:
        </th>
        <td>
            <asp:TextBox ID="txtPostText" runat="server" TextMode="MultiLine" Rows="4" Width="460px" />
            <asp:RequiredFieldValidator ID="rfvPostText" Display="Dynamic" runat="server"
                ControlToValidate="txtPostText" ErrorMessage="Το πεδίο 'Κείμενο μηνύματος' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
