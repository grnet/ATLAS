<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.EditAcademic"
    CodeBehind="EditAcademic.aspx.cs" Title="Επεξεργασία Τμήματος" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="vgUser" />
        <table width="100%" class="dv">
            <tr>
                <th colspan="2" class="header">
                    &raquo; Στοιχεία Τμήματος
                </th>
            </tr>
            <tr>
                <th style="width: 30%">
                    Ίδρυμα:
                </th>
                <td>
                    <asp:Label ID="lblInstitution" runat="server" Font-Bold="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    Σχολή:
                </th>
                <td>
                    <asp:Label ID="lblSchool" runat="server" Font-Bold="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    Τμήμα:
                </th>
                <td>
                    <asp:Label ID="lblDepartment" runat="server" Font-Bold="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    Αριθμός Προδεσμεύσεων:
                </th>
                <td>
                    <asp:Label ID="lblPreAssignedPositions" runat="server" Font-Bold="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                   Μέγιστος Αριθμός Προδεσμεύσεων:
                </th>
                <td>
                    <asp:Label ID="lblMaxAllowedPreAssignedPositions" runat="server" Font-Bold="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    Νέος Μέγιστος Αριθμός Προδεσμεύσεων:
                </th>
                <td>
                    <asp:TextBox ID="txtNewMaxAllowedPreAssignedPositions" runat="server" Width="10%" />
                    <asp:RequiredFieldValidator ID="rfvNewMaxAllowedPreAssignedPositions" runat="server"
                        ControlToValidate="txtNewMaxAllowedPreAssignedPositions" Display="Dynamic" ErrorMessage="Το πεδίο 'Μέγιστος Αριθμός Προδεσμεύσεων' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvNewMaxAllowedPreAssignedPositions" runat="server" Type="Integer" MinimumValue="1"
                        MaximumValue="999" ControlToValidate="txtNewMaxAllowedPreAssignedPositions" ErrorMessage="Το πεδίο 'Μέγιστος Αριθμός Προδεσμεύσεων' πρέπει να έχει τιμή μεταξύ 1 και 999"
                        ValidateEmptyText="false" Display="Dynamic" OnServerValidate="rvNewMaxAllowedPreAssignedPositions_ServerValidate">
                        Το πεδίο πρέπει να έχει τιμή μεταξύ 1 και 999</asp:RangeValidator>
                </td>
            </tr>
        </table>
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="Ενημέρωση" CssClass="icon-btn bg-accept"
                    OnClick="btnSubmit_Click" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblValidationErrors" runat="server" CssClass="error" />
            </td>
        </tr>
    </table>
</asp:Content>
