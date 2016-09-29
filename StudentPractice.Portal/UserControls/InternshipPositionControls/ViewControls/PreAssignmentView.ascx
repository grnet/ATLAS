<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PreAssignmentView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls.PreAssignmentView" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PreAssignmentHeader %>" />
        </th>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PreAssignedAt %>" />
        </th>
        <td>
            <asp:Label ID="lblPreAssignedAt" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>   
</table>
