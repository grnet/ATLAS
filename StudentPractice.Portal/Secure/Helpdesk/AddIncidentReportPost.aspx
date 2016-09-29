<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.AddIncidentReportPost"
    CodeBehind="AddIncidentReportPost.aspx.cs" Title="Προβολή Συμβάντος" %>

<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportView.ascx" TagName="IncidentReportView"
    TagPrefix="my" %>
<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportPostsView.ascx" TagName="IncidentReportPostsView"
    TagPrefix="my" %>
<%@ Register Src="~/Secure/Helpdesk/UserControls/IncidentReportPostInput.ascx" TagName="IncidentReportPostInput"
    TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <asp:PlaceHolder ID="phAddPost" runat="server">
        <div style="padding-left: 10px">
            <asp:LinkButton ID="lnkUnlockReport" runat="server" Text="Ξεκλείδωμα Συμβάντος" CssClass="icon-btn bg-unlock"
                OnClick="btnUnlockReport_Click" OnClientClick="return confirm('Μετά το ξεκλείδωμα, το συμβάν θα τεθεί σε κατάσταση «Εκκρεμεί» και θα επιτρέπεται η επεξεργασία του και η προσθήκη απαντήσεων. Είστε σίγουροι ότι θέλετε να συνεχίσετε;')" />
        </div>
        <div style="margin: 10px;">
            <my:IncidentReportView ID="ucIncidentReportView" runat="server" />
            <br />
            <my:IncidentReportPostsView ID="ucIncidentReportPostsView" runat="server" />
            <br />
            <my:IncidentReportPostInput ID="ucIncidentReportPostInput" runat="server" />
        </div>
        <table id="tbActions" runat="server" style="width: 100%">
            <tr>
                <td colspan="2" style="text-align: left; padding-left: 7px;">
                    <asp:LinkButton ID="btnSubmit" runat="server" Text="Προσθήκη" CssClass="icon-btn bg-accept"
                        OnClick="btnSubmit_Click" OnClientClick="return ASPxClientEdit.ValidateGroup();" />
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
        <asp:ObjectDataSource ID="odsIncidentReportPosts" runat="server" TypeName="StudentPractice.Portal.DataSources.IncidentReportPosts"
            SelectMethod="FindByIncidentReportID">
            <SelectParameters>
                <asp:QueryStringParameter Name="incidentReportID" Type="Int32" QueryStringField="id"
                    DefaultValue="-1" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phReportLocked" runat="server">
        <div class="reminder">
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
