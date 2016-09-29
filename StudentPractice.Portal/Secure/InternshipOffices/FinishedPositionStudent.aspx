<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.FinishedPositionStudent" 
    Title="Επιλογή Φοιτητή" CodeBehind="FinishedPositionStudent.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipOfficeControls/InputControls/AssignStudent.ascx" TagName="AssignStudent" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeHeaderTitle" runat="server">
    <asp:PlaceHolder ID="phSteps" runat="server">
        <ul id="mainNav" runat="server" clientidmode="Static" class="fourStep">
            <li class="done">
                <a id="btnPositionProviders" runat="server" title="Επιλογή Φορέα Υποδοχής">
                    <em>Βήμα 1</em>
                    <span>Επιλογή Φορέα Υποδοχής</span>
                </a>
            </li>

            <li class="done">
                <a id="btnPositionDetails" runat="server" title="Εισαγωγή Γενικών Στοιχείων">
                    <em>Βήμα 2</em>
                    <span>Εισαγωγή Γενικών Στοιχείων</span>
                </a>
            </li>

            <li class="lastDone">
                <a id="btnPositionPhysicalObject" runat="server" title="Προσθήκη Αντικειμένου Θέσης">
                    <em>Βήμα 3</em>
                    <span>Προσθήκη Αντικειμένου Θέσης</span>
                </a>
            </li>

            <li class="current preview">
                <a title="Επιλογή Φοιτητή">
                    <em>Βήμα 4</em>
                    <span>Επιλογή Φοιτητή</span>
                </a>
            </li>
        </ul>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <br />
    <my:AssignStudent ID="ucAssignStudent" runat="server" ValidationGroup="vgPosition" AllowInactiveAcademics="true" ShowCompletePositionButton="true" OnPositionCompleted="ucAssignStudent_PositionCompleted"
        AllowAllOfficeAcademics="true" OnStudentAssignmentComplete="ucAssignStudent_StudentAssignmentComplete" OnStudentAssignmentCanceled="ucAssignStudent_StudentAssignmentCanceled" />

    <br />
    <div style="clear: both; text-align: left">
        <asp:LinkButton ID="btnSubmit" runat="server" Text="Καταχώριση Θέσης" CssClass="icon-btn bg-accept" OnClick="btnSubmit_Click" />
        <asp:LinkButton ID="btnBack" runat="server" Text="Προηγούμενο Βήμα" OnClick="btnBack_Click" CssClass="icon-btn bg-cancel" CausesValidation="false" />
    </div>

    <br />
    <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
</asp:Content>
