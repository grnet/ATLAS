<%@ Page Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true" CodeBehind="PageNotFound.aspx.cs"
    Inherits="StudentPractice.Portal.Common.PageNotFound" Title="Η σελίδα που ζητήσατε δεν βρέθηκε." %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <asp:Literal runat="server" Text="<%$ Resources:CommonPages, PageNotFound_Header %>" />
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
    <p>
        <asp:Literal runat="server" Text="<%$ Resources:CommonPages, PageNotFound_Message1 %>" />
        <a href="<%$ Resources:PrimaryMenu, DefaultUrl %>" runat="server">
            <asp:Literal runat="server" Text="<%$ Resources:CommonPages, PageNotFound_Message2 %>" />
        </a>
    </p>
</asp:Content>
