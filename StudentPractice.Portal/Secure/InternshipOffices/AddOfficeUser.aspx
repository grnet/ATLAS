<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.AddOfficeUser"
    CodeBehind="AddOfficeUser.aspx.cs" Title="Δημιουργία Χρήστη" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipOfficeControls/InputControls/OfficeUserInput.ascx" TagName="OfficeUserInput" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <div style="margin: 10px;">
        <asp:ValidationSummary ID="vsSummary" runat="server" CssClass="validation-summary"
            ValidationGroup="vgOfficeUser" HeaderText="Υπάρχει σφάλμα ή έλλειψη συμπλήρωσης ενός από τα πεδία της φόρμας. Παρακαλώ κάντε τις απαραίτητες διορθώσεις." />
        <my:OfficeUserInput ID="ucOfficeUserInput" runat="server" ValidationGroup="vgOfficeUser" />
    </div>

    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Αποθήκευση" CssClass="icon-btn bg-accept"
                    OnClientClick="javascript:return ValidatePage('vgOfficeUser');" OnClick="btnSubmit_Click" ValidationGroup="vgOfficeUser" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    OnClick="btnCancel_Click" CausesValidation="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
            </td>
        </tr>
    </table>
</asp:Content>
