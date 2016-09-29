<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.EditIncidentReportHandler"
    CodeBehind="EditIncidentReportHandler.aspx.cs" Title="Επεξεργασία Χειριστή Συμβάντος" %>

<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportHandlerInput.ascx" TagName="IncidentReportHandlerInput"
    TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <my:IncidentReportHandlerInput ID="ucIncidentReportHandlerInput" runat="server" />
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" />
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