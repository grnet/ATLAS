<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompletionView.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.ViewControls.CompletionView" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, CompletionHeader %>" />
        </th>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, CompletionVerdict %>" />
        </th>
        <td>
            <asp:Label ID="lblCompletionVerdict" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
     <tr id="trPreAssignedAcademic" runat="server" visible="false">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, Department %>" />
        </th>
        <td>
             <asp:Label ID="lblPreAssignAcademic" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trImplementationStartDate" runat="server" visible="false">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, CompletionStartDate %>" />
        </th>
        <td>
            <asp:Label ID="lblImplementationStartDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="trImplementationEndDate" runat="server" visible="false">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, CompletionEndDate %>" />
        </th>
        <td>
            <asp:Label ID="lblImplementationEndDate" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
    <%--<tr id="trFundingType" runat="server" visible="false">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, FundingType %>" />
        </th>
        <td>
            <asp:Label ID="lblFundingType" runat="server" Width="90%" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>--%>
    <tr id="trCompletionComments" runat="server" visible="false">
        <th style="width: 180px">
            <asp:Literal runat="server" Text="<%$ Resources:PositionGroupInput, CompletionComments %>" />
        </th>
        <td>
            <asp:TextBox ID="txtCompletionComments" runat="server" Enabled="false" TextMode="MultiLine" Rows="9"
                Width="100%" />
        </td>
    </tr>    
</table>
