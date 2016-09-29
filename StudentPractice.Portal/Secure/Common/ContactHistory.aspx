<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ContactHistory.aspx.cs" Inherits="StudentPractice.Portal.Secure.Common.ContactHistory" %>

<%@ Register Src="~/Secure/UserControls/HelpdeskContactHistory.ascx" TagName="HelpdeskContactHistory" TagPrefix="my" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <my:HelpdeskContactHistory ID="ucHelpdeskContactHistory" runat="server" />
</asp:Content>
