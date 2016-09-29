<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImplementationView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls.ImplementationView" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, ImplementationHeader %>" />
        </th>
    </tr>
    <tr id="trPositionStatus" runat="server">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PositionStatus %>" />
        </th>
        <td>
            <asp:Label ID="lblPositionStatus" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PositionAssignedAt %>" />
        </th>
        <td>
            <asp:Label ID="lblAssignedAt" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="rowPreAssignedAcademic" runat="server" visible="false">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Department %>" />
        </th>
        <td>
             <asp:Label ID="lblPreAssignAcademic" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PredictedStartDate %>" />
        </th>
        <td>
            <asp:Label ID="lblImplementationStartDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, PredictedEndDate %>" />
        </th>
        <td>
            <asp:Label ID="lblImplementationEndDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <%--<tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, FundingType %>" />
        </th>
        <td>
            <asp:Label ID="lblFundingType" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>--%>
</table>
