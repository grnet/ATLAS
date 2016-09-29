<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpdeskContactHistory.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.UserControls.HelpdeskContactHistory" %>
<%@ Import Namespace="StudentPractice.BusinessModel" %>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, History_Header %>" />
        </th>
    </tr>
    <tr>
        <th>
            <asp:TextBox ID="txtReportText" runat="server" TextMode="MultiLine" Rows="10" Width="100%" Enabled="false" />
        </th>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo;
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, History_MessagesSent %>" />
        </th>
    </tr>
</table>
<asp:Repeater ID="rptIncidentReportPosts" runat="server">
    <HeaderTemplate>
        <div class="postContainer">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="postInfo">
            <asp:Literal runat="server" Text="<%$ Resources:HelpdeskContact, AnswerDatetime %>" />:
            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm") %>
        </div>
        <div class="postText">
            <%# Eval("PostText") %>
        </div>
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>
