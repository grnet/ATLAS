<%@ Control Language="C#" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportHandlerInput"
    CodeBehind="IncidentReportHandlerInput.ascx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Χειρισμού Συμβάντος
        </th>
    </tr>
    <tr>
        <th style="width: 30%">
            Κατηγορία Χειριστή Συμβάντος:
        </th>
        <td>
            <asp:DropDownList ID="ddlHandlerType" runat="server" OnInit="ddlHandlerType_Init" Width="460px" />
            <asp:RequiredFieldValidator ID="rfvHandlerType" Display="Dynamic" runat="server"
                ControlToValidate="ddlHandlerType" ErrorMessage="Το πεδίο 'Κατηγορία Χειριστή Συμβάντος' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Επικοινωνία με Χειριστή Συμβάντος:
        </th>
        <td>
            <asp:DropDownList ID="ddlHandlerStatus" runat="server" OnInit="ddlHandlerStatus_Init"
                Width="460px" />
            <asp:RequiredFieldValidator ID="rfvHandlerStatus" Display="Dynamic" runat="server"
                ControlToValidate="ddlHandlerStatus" ErrorMessage="Το πεδίο 'Επικοινωνία με Χειριστή Συμβάντος' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>