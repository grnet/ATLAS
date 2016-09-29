<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.FinishedPositionProvider"
    Title="Επιλογή Φορέα Υποδοχής" CodeBehind="FinishedPositionProvider.aspx.cs" %>

<%@ Register TagPrefix="my" Src="~/UserControls/FinishedInternshipPositionControls/InputControls/InternshipPositionProviderSelect.ascx" TagName="InternshipPositionProviderSelect" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeHeaderTitle" runat="server">
    <asp:PlaceHolder ID="phSteps" runat="server">
        <ul id="mainNav" runat="server" clientidmode="Static" class="fourStep">
            <li class="current">
                <a title="Επιλογή Φορέα Υποδοχής">
                    <em>Βήμα 1</em>
                    <span>Επιλογή Φορέα Υποδοχής</span>
                </a>
            </li>

            <li>
                <a title="Εισαγωγή Γενικών Στοιχείων">
                    <em>Βήμα 2</em>
                    <span>Εισαγωγή Γενικών Στοιχείων</span>
                </a>
            </li>

            <li>
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
    <my:InternshipPositionProviderSelect ID="ucInternshipPositionProviderSelect" runat="server" ValidationGroup="vgPosition" />

    <div style="clear: both; text-align: left; margin-top: 15px; margin-bottom: 15px;">
        <asp:LinkButton ID="btnSubmit" runat="server" Text="Αποθήκευση &amp; Συνέχεια" CssClass="icon-btn bg-accept"
            OnClick="btnSubmit_Click" />
        <asp:LinkButton ID="btnBack" runat="server" Text="Ακύρωση" OnClick="btnBack_Click"
            CssClass="icon-btn bg-cancel" CausesValidation="false" />
    </div>

    <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
</asp:Content>
