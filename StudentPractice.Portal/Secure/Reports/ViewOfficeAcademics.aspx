<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Reports.ViewOfficeAcademics"
    CodeBehind="ViewOfficeAcademics.aspx.cs" Title="Προβολή Σχολών/Τμημάτων Γραφείου Πρακτικής" %>

<%@ Register Src="~/UserControls/InternshipOfficeControls/ViewControls/OfficeAcademicsGridView.ascx"
    TagName="OfficeAcademicsGridView" TagPrefix="my" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <my:OfficeAcademicsGridView ID="ucOfficeAcademicsGridView" runat="server" />
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
