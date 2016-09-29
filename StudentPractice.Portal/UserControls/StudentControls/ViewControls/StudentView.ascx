<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudentView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.StudentControls.ViewControls.StudentView" %>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; <asp:Literal runat="server" Text="<%$ Resources:Student, StudentDetails %>" />
        </th>
    </tr>
    <tr id="trGreekName" runat="server">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:Student, GreekName %>" />
        </th>
        <td>
            <asp:Label ID="lblGreekName" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Label ID="lblLatinNameLabel" runat="server" Text="<%$ Resources:Student, LatinName %>" Font-Size="11px" />            
        </th>
        <td>
            <asp:Label ID="lblLatinName" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:Student, Institution %>" />
        </th>
        <td>
            <asp:Label ID="lblInstitution" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:Student, School %>" />
        </th>
        <td>
            <asp:Label ID="lblSchool" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:Student, Department %>" />
        </th>
        <td>
            <asp:Label ID="lblDepartment" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:Student, StudentNumber %>" />
        </th>
        <td>
            <asp:Label ID="lblStudentNumber" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
</table>
