<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.ViewPositionDetails"
    CodeBehind="ViewPositionDetails.aspx.cs" Title="Προβολή Θέσης Πρακτικής Άσκησης" %>

<%@ Register Src="~/UserControls/InternshipPositionControls/ViewControls/PositionView.ascx"
    TagName="PositionView" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipPositionControls/ViewControls/PreAssignmentView.ascx"
    TagName="PreAssignmentView" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipPositionControls/ViewControls/ImplementationView.ascx"
    TagName="ImplementationView" TagPrefix="my" %>
<%@ Register Src="~/UserControls/StudentControls/ViewControls/StudentView.ascx" TagName="StudentView"
    TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipPositionControls/ViewControls/CompletionView.ascx"
    TagName="CompletionView" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipOfficeControls/ViewControls/OfficeView.ascx"
    TagName="OfficeView" TagPrefix="my" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">

        <div id="divCompletion" runat="server" style="margin: 10px;" visible="false">
            <my:CompletionView ID="ucCompletionView" runat="server" />
        </div>
        <div id="divImplementation" runat="server" style="margin: 10px;" visible="false">
            <my:ImplementationView ID="ucImplementationView" runat="server" />
        </div>
        <div id="divStudent" runat="server" style="margin: 10px;" visible="false">
            <my:StudentView ID="ucStudentView" runat="server" />
        </div>
        <div id="divOffice" runat="server" style="margin: 10px;" visible="false">
            <my:OfficeView ID="ucOfficeView" runat="server" />
        </div>
        <div id="divPreAssignment" runat="server" style="margin: 10px;" visible="false">
            <my:PreAssignmentView ID="ucPreAssignmentView" runat="server" />
        </div>
        <div style="margin: 10px;">
            <my:PositionView ID="ucPositionView" runat="server" />
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
