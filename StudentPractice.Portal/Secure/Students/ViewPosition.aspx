<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Students.ViewPosition"
    CodeBehind="ViewPosition.aspx.cs" Title="Προβολή Θέσης Πρακτικής Άσκησης" %>

<%@ Register TagPrefix="my" Src="~/UserControls/InternshipPositionControls/ViewControls/PositionGroupView.ascx" TagName="PositionGroupView" %>

<asp:Content ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMain" runat="server">
    
    <div style="margin: 10px;">
        <my:PositionGroupView ID="ucPositionGroupView" runat="server" HideAcademics="true" />
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
