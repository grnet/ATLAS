<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ViewAssignedPositionDetails.aspx.cs" Inherits="StudentPractice.Portal.Secure.Students.ViewAssignedPositionDetails" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionView.ascx" TagName="PositionView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/ImplementationView.ascx" TagName="ImplementationView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/CompletionView.ascx" TagName="CompletionView" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <div id="divCompletion" runat="server" style="margin: 10px;" visible="false">
            <my:CompletionView ID="ucCompletionView" runat="server" />
        </div>
        <div id="divImplementation" runat="server" style="margin: 10px;" visible="false">
            <my:ImplementationView id="ucImplementationView" runat="server" />
        </div>
        <div style="margin: 10px;">
            <my:PositionView id="ucPositionView" runat="server" HideAcademics="true" />
        </div>
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnCancel" runat="server" Text="Κλείσιμο" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
