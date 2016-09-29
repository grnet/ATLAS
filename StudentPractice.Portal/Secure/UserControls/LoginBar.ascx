<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginBar.ascx.cs" Inherits="Ebooks.Portal.Secure.UserControls.LoginBar" %>

<asp:LoginView ID="loginView" runat="server">
    <AnonymousTemplate>
        Δεν έχετε συνδεθεί.
        <asp:LoginStatus ID="loginStatus" runat="server" />
    </AnonymousTemplate>
    <LoggedInTemplate>
        Έχετε συνδεθεί ως <a runat="server" href="~/Secure/Common/UserAccount.aspx"><asp:LoginName ID="loginName" runat="server" /></a>&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="lnkLogout" runat="server" CssClass="icon-btn bg-logout" Text="Αποσύνδεση" OnClick="lnkLogout_Click" />
    </LoggedInTemplate>
</asp:LoginView>
