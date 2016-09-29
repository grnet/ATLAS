<%@ Page Language="C#" MasterPageFile="~/Secure/SecurePages.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipProviders.AddProviderUser"
    CodeBehind="AddProviderUser.aspx.cs" Title="Δημιουργία Χρήστη" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipProviderControls/InputControls/ProviderUserInput.ascx" TagName="ProviderUserInput" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
    <link href="/_css/wizard.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">

    <div style="margin: 10px;">
        <asp:ValidationSummary ID="vsSummary" runat="server" CssClass="validation-summary" ValidationGroup="vgProviderUser"
            HeaderText="<%$ Resources:ProviderUser, ValidationProviderUser %>" />
        <my:ProviderUserInput ID="ucProviderUserInput" runat="server" ValidationGroup="vgProviderUser" />
    </div>

    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:GlobalProvider, Global_Save %>" CssClass="icon-btn bg-accept"
                    OnClientClick="javascript:return ValidatePage('vgProviderUser');" OnClick="btnSubmit_Click" ValidationGroup="vgProviderUser" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:GlobalProvider, Global_Cancel %>" CssClass="icon-btn bg-cancel"
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
