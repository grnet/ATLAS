<%@ Page Language="C#" MasterPageFile="~/PopUpPublic.Master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.VerifyProvider" CodeBehind="VerifyProvider.aspx.cs"
    Title="Πιστοποίηση Φορέα Υποδοχής" %>

<%@ Register Src="~/UserControls/InternshipProviderControls/ViewControls/ProviderView.ascx"
    TagName="ProviderView" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipProviderControls/ViewControls/ProviderUserView.ascx"
    TagName="ProviderUserView" TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:MultiView ID="mvProvider" runat="server">
            <asp:View ID="vMasterAccount" runat="server">
                <asp:Label ID="lblErrors" runat="server" Visible="false" Font-Bold="true" ForeColor="Blue" />
                <my:ProviderView ID="ucProviderView" runat="server" />
            </asp:View>
            <asp:View ID="vProviderUser" runat="server">
                <my:ProviderUserView ID="ucProviderUserView" runat="server" />
            </asp:View>
        </asp:MultiView>
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnVerify" runat="server" Text="Πιστοποίηση" CssClass="icon-btn bg-accept"
                    OnClick="btnVerify_Click" OnClientClick="return confirm('Θέλετε σίγουρα να πιστοποιήσετε το φορέα;')" />
                <asp:LinkButton ID="btnUnVerify" runat="server" Text="Από-Πιστοποίηση" CssClass="icon-btn bg-reject"
                    OnClick="btnUnVerify_Click" OnClientClick="return confirm('Θέλετε σίγουρα να από-πιστοποιήσετε το φορέα;')" />
                <asp:LinkButton ID="btnReject" runat="server" Text="Απόρριψη" CssClass="icon-btn bg-reject"
                    OnClick="btnReject_Click" OnClientClick="return confirm('Θέλετε σίγουρα να απορρίψετε το γραφείο;')" />
                <asp:LinkButton ID="btnRestore" runat="server" Text="Επαναφορά" CssClass="icon-btn bg-restore"
                    OnClick="btnRestore_Click" OnClientClick="return confirm('Θέλετε σίγουρα να επαναφέρετε το φορέα;')" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
