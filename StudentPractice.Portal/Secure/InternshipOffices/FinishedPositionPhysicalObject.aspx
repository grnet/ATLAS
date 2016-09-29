<%@ Page  Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.FinishedPositionPhysicalObject"
    Title="Προσθήκη Αντικειμένου Θέσης" CodeBehind="FinishedPositionPhysicalObject.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/InputControls/PhysicalObjectsInput.ascx" TagName="PhysicalObjectsInput" %>

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

            <li class="lastDone">
                <a id="btnPositionDetails" runat="server" title="Εισαγωγή Γενικών Στοιχείων">
                    <em>Βήμα 2</em>
                    <span>Εισαγωγή Γενικών Στοιχείων</span>
                </a>
            </li>

            <li class="current">
                <a title="Προσθήκη Αντικειμένου Θέσης">
                    <em>Βήμα 3</em>
                    <span>Προσθήκη Αντικειμένου Θέσης</span>
                </a>
            </li>

            <li class="preview">
                <a title="Επιλογή Φοιτητή">
                    <em>Βήμα 4</em>
                    <span>Επιλογή Φοιτητή</span>
                </a>
            </li>
        </ul>
    </asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <my:PhysicalObjectsInput ID="ucPhysicalObjectsInput" runat="server"
        OnComplete="ucPhysicalObjectsInput_Complete" OnCancel="ucPhysicalObjectsInput_Cancel" />
</asp:Content>
