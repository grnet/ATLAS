
<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.InternshipOffices.OfficeAcademicsAuthorization"
    CodeBehind="OfficeAcademicsAuthorization.aspx.cs" Title="Επιλογή Τμημάτων" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <table width="100%" class="dv">
        <tr>
            <th class="header">
                &raquo; Πρόσβαση στα παρακάτω τμήματα του Γραφείου Πρακτικής
            </th>
        </tr>
        <tr>
            <td>
                <asp:CheckBoxList ID="chbxlOfficeAcademics" runat="server" RepeatDirection="Vertical"
                    Font-Bold="true" Font-Size="11px" OnInit="chbxlOfficeAcademics_Init" ClientIDMode="Static">
                </asp:CheckBoxList>
            </td>
        </tr>
    </table>
    <div style="padding: 10px 0px 0px 0px">
        <asp:LinkButton ID="btnSubmit" runat="server" Text="Αποθήκευση" CssClass="icon-btn bg-save"
            OnClick="btnSubmit_Click" ValidationGroup="vgEvaluation" />
        <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
            CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
    </div>
    <div style="padding: 10px 0px 0px 0px">
        <asp:Label ID="lblErrors" runat="server" Font-Bold="true" ForeColor="Red" />
    </div>
</asp:Content>
