<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncidentReportView.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportView" %>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία αναφοράς
        </th>
    </tr>
    <tr>
        <th style="width: 30%">
            Κωδικός Αναφοράς:
        </th>
        <td>
            <asp:Literal ID="ltIncidentID" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Κατάσταση:
        </th>
        <td>
            <asp:Literal ID="ltReportStatus" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Πηγή Αναφοράς Συμβάντος:
        </th>
        <td>
            <asp:Literal ID="ltReporterType" runat="server" />
        </td>
    </tr>
    <asp:PlaceHolder ID="phStudent" runat="server" Visible="false">
        <tr>
            <th style="width: 30%">
                Α.Φ.Μ.:
            </th>
            <td>
                <asp:Literal ID="ltStudentAFM" runat="server" />
            </td>
        </tr>
        <tr>
            <th style="width: 30%">
                Όνομα:
            </th>
            <td>
                <asp:Literal ID="ltStudentFirstName" runat="server" />
            </td>
        </tr>
        <tr>
            <th style="width: 30%">
                Επώνυμο:
            </th>
            <td>
                <asp:Literal ID="ltStudentLastName" runat="server" />
            </td>
        </tr>
    </asp:PlaceHolder>
    <tr>
        <th style="width: 30%">
            Είδος Συμβάντος:
        </th>
        <td>
            <asp:Literal ID="ltIncidentType" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Υποβολή Από:
        </th>
        <td>
            <asp:Literal ID="ltCreatedBy" runat="server" />
        </td>
    </tr>
    <tr id="trUpdatedBy" runat="server">
        <th style="width: 30%">
            Τροποποίηση Από:
        </th>
        <td>
            <asp:Literal ID="ltUpdatedBy" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Ον/μο ατόμου επικοινωνίας:
        </th>
        <td>
            <asp:Literal ID="ltReporterName" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Τηλέφωνο ατόμου επικοινωνίας:
        </th>
        <td>
            <asp:Literal ID="ltReporterPhone" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            E-mail ατόμου επικοινωνίας:
        </th>
        <td>
            <asp:Literal ID="ltReporterEmail" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">
            Πλήρες κείμενο αναφοράς:
        </th>
        <td>
            <asp:TextBox ID="ltReportText" runat="server" TextMode="MultiLine" Rows="4" Width="100%" />
        </td>
    </tr>
</table>
