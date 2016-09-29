<%@ Page Language="C#" MasterPageFile="~/PopUpPublic.Master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.EditProvider" CodeBehind="EditProvider.aspx.cs"
    Title="Αλλαγή Στοιχείων Φορέα Υποδοχής" %>

<%@ Register Src="~/UserControls/InternshipProviderControls/InputControls/ProviderInput.ascx"
    TagName="ProviderInput" TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:Label ID="lblErrors" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"
            Text="Η τροποποίηση δεν μπορεί να ολοκληρωθεί, γιατί υπάρχει ήδη άλλος πιστοποιημένος λογαριασμός για το ίδιο Α.Φ.Μ." />
        <my:ProviderInput ID="ucProviderInput" runat="server" HelpDeskEditMode="true" />
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Αποθήκευση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" OnClientClick="return ValidatePage('vgEditProvider');" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
