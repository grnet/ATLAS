<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Admin.EditSubSystem"
    CodeBehind="EditSubSystem.aspx.cs" Title="Επεξεργασία Υποσυστήματος" %>

<%@ Register Src="~/Secure/Admin/UserControls/SubSystemInput.ascx" TagName="SubSystemInput"
    TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <my:SubSystemInput ID="ucSubSystemInput" runat="server" />
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" OnClientClick="return ASPxClientEdit.ValidateGroup();" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel" CausesValidation="false"
                    OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
            </td>
        </tr>
    </table>
</asp:Content>