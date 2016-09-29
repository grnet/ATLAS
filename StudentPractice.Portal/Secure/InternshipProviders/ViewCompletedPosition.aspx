<%@ Page Title="" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ViewCompletedPosition.aspx.cs" Inherits="StudentPractice.Portal.Secure.InternshipProviders.ViewCompletedPosition" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionGroupView.ascx" TagName="PositionGroupView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PreAssignmentView.ascx" TagName="PreAssignmentView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/ImplementationView.ascx" TagName="ImplementationView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/StudentControls/ViewControls/StudentView.ascx" TagName="StudentView" %>
<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/CompletionView.ascx" TagName="CompletionView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <div id="divCompletion" runat="server" style="margin: 10px;" visible="false">
        <my:CompletionView ID="ucCompletionView" runat="server" />
    </div>
   
     <div id="divImplementation" runat="server" style="margin: 10px;" visible="false">
        <my:ImplementationView ID="ucImplementationView" runat="server" />
    </div>
   
     <div id="divStudent" runat="server" style="margin: 10px;" visible="false">
        <my:StudentView ID="ucStudentView" runat="server" />
    </div>
    
    <div id="divPreAssignment" runat="server" style="margin: 10px;" visible="false">
        <my:PreAssignmentView ID="ucPreAssignmentView" runat="server" />
    </div>
   
     <div style="margin: 10px;">
        <my:PositionGroupView ID="ucPositionGroupView" runat="server" />
    </div>

    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:Evaluation, Close %>" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
