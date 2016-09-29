<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master"
    AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.AssignPosition"
    Title="Αντιστοίχιση Θέσης Πρακτικής Άσκησης" CodeBehind="AssignPosition.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipOfficeControls/InputControls/AssignStudent.ascx" TagName="AssignStudent" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <my:AssignStudent ID="ucAssignStudent" runat="server" AllowAllOfficeAcademics="false" AllowInactiveAcademics="true" ShowCompletePositionButton="false"
        OnStudentAssignmentComplete="ucAssignStudent_StudentAssignmentComplete" OnStudentAssignmentCanceled="ucAssignStudent_StudentAssignmentCanceled" />
</asp:Content>
