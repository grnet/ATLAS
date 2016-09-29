<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncidentReportPostsView.ascx.cs"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportPostsView" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Μηνύματα που έχουν ανταλλαγεί
        </th>
    </tr>
</table>
<asp:Repeater ID="rptIncidentReportPosts" runat="server">
    <HeaderTemplate>
        <div class="postContainer">
    </HeaderTemplate>
    <ItemTemplate>    
        <div class="postInfo">
            <a name='<%# Eval("ID") %>'></a>Δημιουργήθηκε από το χρήστη [<%# Eval("CreatedBy") %>]
            στις
            <%# ((DateTime)Eval("CreatedAt")).ToString("dd/MM/yyyy HH:mm") %> - <%# ((enCallType)Eval("CallType")).GetLabel() %> κλήση
        </div>
        <div class="postText">
            <%# Eval("PostText") %>
        </div>
    </ItemTemplate>    
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>
