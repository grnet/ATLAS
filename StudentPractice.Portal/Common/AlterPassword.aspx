<%@ Page Language="C#" MasterPageFile="~/PopUpPublic.Master" AutoEventWireup="true" CodeBehind="AlterPassword.aspx.cs"
    Inherits="StudentPractice.Portal.Common.AlterPassword" Title="Αλλαγή Κωδικού Πρόσβασης" %>

<%@ Register Src="~/UserControls/GenericControls/ChangePassword.ascx" TagName="ChangePassword" TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:ValidationSummary ID="vdChangePassword" runat="server" ValidationGroup="vgChangePassword" />
    <div id="divErrors">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" Visible="false"
            Text="Ο παλιός κωδικός πρόσβασης δεν είναι σωστός." />
    </div>
    <div style="margin: 10px 0px">
        <my:ChangePassword ID="changePassword" runat="server" ValidationGroup="vgChangePassword" />
        <asp:LinkButton ID="btnSubmit" runat="server" CssClass="icon-btn" Text="Ενημέρωση" OnClick="btnSubmit_Click" ValidationGroup="vgChangePassword" OnClientClick="clearErrors()" />
        <asp:LinkButton ID="btnCancel" runat="server" CssClass="icon-btn" Text="Ακύρωση" CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
    </div>
</asp:Content>
