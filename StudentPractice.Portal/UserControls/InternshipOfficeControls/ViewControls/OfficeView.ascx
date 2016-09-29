<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipOfficeControls.ViewControls.OfficeView" %>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Γραφείου
        </th>
    </tr>
    <tr>
        <th style="width: 180px">
            Είδος Γραφείου
        </th>
        <td>
            <asp:Label ID="lblOfficeType" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Ίδρυμα
        </th>
        <td>
            <asp:Label ID="lblInstitution" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="rowAcademic" runat="server" visible="false">
        <th style="width: 180px">
            Τμήμα:
        </th>
        <td id="cellSingleAcademic" runat="server" visible="false">
            <asp:Label ID="lblAcademic" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
        <td id="cellMultipleAcademic" runat="server" visible="false">
            <asp:Literal ID="litMultipleAcademic" runat="server" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Όνομα Υπευθύνου
        </th>
        <td>
            <asp:Label ID="lblContactName" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>    
    <tr>
        <th style="width: 180px">
            Τηλέφωνο Υπευθύνου
        </th>
        <td>
            <asp:Label ID="lblContactPhone" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Email Υπευθύνου
        </th>
        <td>
            <asp:Label ID="lblEmailPhone" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
</table>
