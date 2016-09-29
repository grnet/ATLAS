<%@ Page Title="Αναζήτηση Φοιτητών" Language="C#" MasterPageFile="~/Secure/BackOffice.master" AutoEventWireup="true" CodeBehind="SearchAndRegisterStudents.aspx.cs" Inherits="StudentPractice.Portal.Secure.Helpdesk.SearchAndRegisterStudents" %>

<%@ Register TagPrefix="my" TagName="SearchAndRegisterStudent" Src="~/UserControls/GenericControls/SearchAndRegisterStudent.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <my:SearchAndRegisterStudent ID="ucSearchAndRegisterStudent" runat="server" AllowStudentAssignment="false" 
        ShowCancelButtons="false" ConfirmStudentRegistration="true" AllowAllInstitutions="true" PageSize="20" IsHelpdesk="true" />
</asp:Content>
