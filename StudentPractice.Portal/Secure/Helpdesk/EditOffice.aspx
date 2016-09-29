<%@ Page Language="C#" MasterPageFile="~/PopUpPublic.Master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.EditOffice" CodeBehind="EditOffice.aspx.cs"
    Title="Αλλαγή Στοιχείων Γραφείου Πρακτικής Άσκησης" %>

<%@ Register Src="~/UserControls/InternshipOfficeControls/InputControls/OfficeInput.ascx"
    TagName="OfficeInput" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipOfficeControls/ViewControls/OfficeAcademicsGridView.ascx"
    TagName="OfficeAcademicsGridView" TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:Label ID="lblErrors" runat="server" Visible="false" Font-Bold="true" ForeColor="Red" />
        <my:OfficeInput ID="ucOfficeInput" runat="server" ValidationGroup="vg" />
        <br />
        <my:OfficeAcademicsGridView ID="ucOfficeAcademicsGridView" runat="server" />
    </div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vg" />
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Αποθήκευση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" OnClientClick="return ValidatePage('vgEditOffice');" ValidationGroup="vg" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
