<%@ Page Language="C#" MasterPageFile="~/Portal.master" AutoEventWireup="true"
    CodeBehind="AccessDenied.aspx.cs" Inherits="StudentPractice.Portal.Common.AccessDenied"
    Title="Απαγορεύεται η πρόσβαση" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Απαγορεύεται η πρόσβαση
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cpHelpBar" runat="server">
    <div id="primary-menu">
        <ul>
            <li>
                <a runat="server" href="<%$ Resources:PrimaryMenu, DefaultUrl %>" class="home">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, HomePage %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href="http://atlas.grnet.gr/FAQ.aspx" class="faq">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, FAQ %>" />
                </a>
            </li>
            <li>
                <a target="_blank" href='<%= string.Format("{0}/RedirectFromPortal.ashx?id=3&language=0", ConfigurationManager.AppSettings["StudentPracticeMainUrl"]) %>' class="contact">
                    <asp:Literal runat="server" Text="<%$ Resources:PrimaryMenu, Contact %>" />
                </a>
            </li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <h2>
        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, AccessDenied_Header %>" />
    </h2>

    <p>
        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, AccessDenied_Message %>" />
    </p>

    <asp:LoginView ID="LoginView1" runat="server">
        <LoggedInTemplate>
            <p>
                <asp:Literal runat="server" Text="<%$ Resources:CommonPages, LoggedAs %>" />
                <asp:LoginName ID="LoginName1" runat="server" />
                <asp:LoginStatus ID="LoginStatus1" runat="server" LogoutAction="Redirect" LogoutPageUrl="<%$ Resources:PrimaryMenu, DefaultUrl %>"
                    OnLoggingOut="LoginStatus1_OnLoggingOut" OnLoggedOut="LoginStatus1_LoggedOut" />
            </p>
        </LoggedInTemplate>
    </asp:LoginView>
</asp:Content>
