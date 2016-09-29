<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Common.ContactForm"
    CodeBehind="ContactForm.aspx.cs" Title="Ερώτημα προς Γραφείο Αρωγής" %>

<%@ Register Src="~/Secure/UserControls/HelpdeskContactFormInput.ascx" TagName="HelpdeskContactFormInput" TagPrefix="my" %>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <asp:ValidationSummary ID="vdSummary" runat="server" ValidationGroup="vgContact" />

    <div style="margin: 10px;">
        <my:HelpdeskContactFormInput ID="ucHelpdeskContactFormInput" runat="server" ValidationGroup="vgContact" />
    </div>

    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" ValidationGroup="vgContact" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" Text="<%$ Resources:GlobalProvider, Global_Send %>" />
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn bg-cancel" CausesValidation="false"
                    OnClientClick="window.parent.popUp.hide();" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
            </td>
        </tr>
    </table>
</asp:Content>
