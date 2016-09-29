<%@ Page Language="C#" MasterPageFile="~/PopUpPublic.Master" AutoEventWireup="true"
    Inherits="StudentPractice.Portal.Secure.Helpdesk.VerifyOffice" CodeBehind="VerifyOffice.aspx.cs"
    Title="Πιστοποίηση Γραφείου" %>

<%@ Register Src="~/UserControls/InternshipOfficeControls/InputControls/OfficeInput.ascx"
    TagName="OfficeInput" TagPrefix="my" %>
<%@ Register Src="~/UserControls/InternshipOfficeControls/ViewControls/OfficeAcademicsGridView.ascx"
    TagName="OfficeAcademicsGridView" TagPrefix="my" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:Label ID="lblErrors" runat="server" Visible="false" Font-Bold="true" ForeColor="Blue" />
        <my:OfficeInput ID="ucOfficeInput" runat="server" />
        <br />
        <my:OfficeAcademicsGridView ID="ucOfficeAcademicsGridView" runat="server" />
        <asp:PlaceHolder ID="phVerificationComments" runat="server">
            <br />
            <table width="100%" class="dv">
                <tr>
                    <th colspan="2" class="header">
                        &raquo; Σχόλια Πιστοποίησης
                    </th>
                </tr>
                <tr>
                    <th style="width: 67%">
                        <asp:Label ID="lblCanHandleDepartmentalStudents" runat="server" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlCanHandleDepartmentalStudents" runat="server" Width="50%">
                            <asp:ListItem Text="-- επιλέξτε --" Value="" Selected="True" />
                            <asp:ListItem Text="Ναι" Value="1" />
                            <asp:ListItem Text="Οχι" Value="2" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCanHandleDepartmentalStudents" runat="server"
                            ControlToValidate="ddlCanHandleDepartmentalStudents" Display="Dynamic" ValidationGroup="vgExistingAccount"
                            ErrorMessage="Το πεδίο είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <th style="width: 67%">
                        <asp:Label ID="lblTransferStudentsToOtherAccount" runat="server" />
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlTransferStudentsToOtherAccount" runat="server" Width="50%">
                            <asp:ListItem Text="-- επιλέξτε --" Value="" Selected="True" />
                            <asp:ListItem Text="Ναι" Value="1" />
                            <asp:ListItem Text="Οχι" Value="2" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvTransferStudentsToOtherAccount" runat="server"
                            ControlToValidate="ddlTransferStudentsToOtherAccount" Display="Dynamic" ValidationGroup="vgExistingAccount"
                            ErrorMessage="Το πεδίο είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
    </div>
    <table style="width: 100%">
        <tr>
            <td colspan="2" style="text-align: left; padding-left: 7px;">
                <asp:LinkButton ID="btnVerify" runat="server" Text="Πιστοποίηση" CssClass="icon-btn bg-accept"
                    OnClick="btnVerify_Click" OnClientClick="return confirm('Θέλετε σίγουρα να πιστοποιήσετε το Γραφείο Πρακτικής Άσκησης;')" />
                <asp:LinkButton ID="btnVerifyWithExistingAccount" runat="server" Text="Πιστοποίηση"
                    ValidationGroup="vgExistingAccount" CssClass="icon-btn bg-accept" OnClick="btnVerify_Click"
                    OnClientClick="return confirm('Θέλετε σίγουρα να πιστοποιήσετε το Γραφείο Πρακτικής Άσκησης;')" />
                <asp:LinkButton ID="btnUnVerify" runat="server" Text="Από-Πιστοποίηση" CssClass="icon-btn bg-reject"
                    OnClick="btnUnVerify_Click" OnClientClick="return confirm('Θέλετε σίγουρα να από-πιστοποιήσετε το Γραφείο Πρακτικής Άσκησης;')" />
                <asp:LinkButton ID="btnReject" runat="server" Text="Απόρριψη" CssClass="icon-btn bg-reject"
                    OnClick="btnReject_Click" OnClientClick="return confirm('Θέλετε σίγουρα να απορρίψετε το Γραφείο Πρακτικής Άσκησης;')" />
                <asp:LinkButton ID="btnRestore" runat="server" Text="Επαναφορά" CssClass="icon-btn bg-restore"
                    OnClick="btnRestore_Click" OnClientClick="return confirm('Θέλετε σίγουρα να επαναφέρετε το Γραφείο Πρακτικής Άσκησης;')" />
                <asp:LinkButton ID="btnCancel" runat="server" Text="Ακύρωση" CssClass="icon-btn bg-cancel"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
