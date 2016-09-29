<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.CompletePosition"
    CodeBehind="CompletePosition.aspx.cs" Title="Ολοκλήρωση Θέσης Πρακτικής Άσκησης" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/InputControls/CompletionInput.ascx" TagName="CompletionInput" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <my:CompletionInput ID="ucCompletionInput" runat="server" ValidationGroup="vgCompletionInput" />
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" ValidationGroup="vgCompletionInput" />
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
