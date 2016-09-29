<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.AddHelpdeskUser"
    CodeBehind="AddHelpdeskUser.aspx.cs" Title="Δημιουργία Χρήστη" %>

<%@ Register Src="~/UserControls/HelpdeskControls/InputControls/HelpdeskUserInput.ascx" TagName="HelpdeskUserInput"
    TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="vgHelpdeskUser" />
        <my:HelpdeskUserInput ID="ucHelpdeskUserInput" runat="server" ValidationGroup="vgHelpdeskUser" />
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" ValidationGroup="vgHelpdeskUser" />
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
