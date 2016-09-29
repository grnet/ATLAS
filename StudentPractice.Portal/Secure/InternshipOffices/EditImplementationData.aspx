<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.EditImplementationData"
    CodeBehind="EditImplementationData.aspx.cs" Title="Αλλαγή Στοιχείων Εκτέλεσης Πρακτικής Άσκησης" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/InputControls/ImplementationInput.ascx" TagName="ImplementationInput" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="vgImplementationInput" />
        <my:ImplementationInput ID="ucImplementationInput" runat="server" ValidationGroup="vgImplementationInput" />
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" ValidationGroup="vgImplementationInput" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
            </td>
        </tr>
    </table>
</asp:Content>
