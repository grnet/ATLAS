<%@ Page Title="Ενημέρωση Cache" Language="C#"MasterPageFile="~/Secure/BackOffice.master"AutoEventWireup="true"
    CodeBehind="RefreshCache.aspx.cs" Inherits="StudentPractice.Portal.Secure.Admin.RefreshCache" %>

<%@ Register Src="~/UserControls/GenericControls//FlashMessage.ascx" TagName="FlashMessage" TagPrefix="my" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <my:FlashMessage ID="fm" runat="server" CssClass="fade" />
    <asp:Button ID="btnRefreshCache" runat="server" Text="Refresh Cache" CssClass="btn" OnClick="btnRefreshCache_Click" />
</asp:Content>
