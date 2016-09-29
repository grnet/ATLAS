<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.EditIncidentReport"
    CodeBehind="EditIncidentReport.aspx.cs" Title="Επεξεργασία Συμβάντος" %>

<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportInput.ascx" TagName="IncidentReportInput"
    TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:PlaceHolder ID="phEditReport" runat="server">
        <div style="margin: 10px;">
            <my:IncidentReportInput ID="ucIncidentReportInput" runat="server" />
        </div>
        <table style="width: 100%">
            <tr>
                <td colspan="2" style="text-align: left; padding-left: 7px;">
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                        OnClick="btnSubmit_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                        CausesValidation="false" OnClick="btnCancel_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phReportLocked" runat="server">
        <div class="reminder" style="text-align: left">
            <asp:Label ID="lblReportLocked" runat="server" />
        </div>
        <table style="width: 100%">
            <tr>
                <td colspan="2" style="text-align: left; padding-left: 7px;">
                    <asp:LinkButton ID="btnClose" runat="server" Text="Κλείσιμο" CssClass="icon-btn bg-cancel"
                        CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
