<%@ Page Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Admin.EditAdminUser"
    CodeBehind="EditAdminUser.aspx.cs" Title="Επεξεργασία Χρήστη" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div style="margin: 10px;">
        <asp:ValidationSummary ID="vsSummary" runat="server" ValidationGroup="vgUser" />
        <table width="100%" class="dv">
            <tr>
                <th colspan="2" class="header">
                    &raquo; Στοιχεία Λογαριασμού Χρήστη
                </th>
            </tr>
            <tr>
                <th style="width: 30%">
                    Όνομα Χρήστη:
                </th>
                <td>
                    <asp:TextBox ID="txtUsername" runat="server" Width="30%" onkeyup="Imis.Lib.NoGreekCharacters(this,false)" Enabled="false" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                        Display="Dynamic" ErrorMessage="Το πεδίο 'Όνομα Χρήστη' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername"
                        Display="Dynamic" ValidationExpression="^([A-Za-z0-9_\-\.]){4,}$" ErrorMessage="Το Όνομα Χρήστη πρέπει να αποτελείται από τουλάχιστον 4 χαρακτήρες. Επιτρέπονται μόνο λατινικοί χαρακτήρες, αριθμητικοί (π.χ. 1,2) ή ειδικοί (π.χ. _,-,.)"><img src="/_img/error.gif" title="Το όνομα χρήστη δεν είναι έγκυρο" /></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    E-mail:
                </th>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" Width="90%" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        Display="Dynamic" ErrorMessage="Το πεδίο 'E-mail' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                        runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                        ErrorMessage="Το E-mail δεν είναι έγκυρο"><img src="/_img/error.gif" title="Το πεδίο δεν είναι έγκυρο" /></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" class="dv">
            <tr>
                <th colspan="2" class="header">
                    &raquo; Στοιχεία Χρήστη
                </th>
            </tr>
            <tr>
                <td class="notRequired" style="width: 30%">
                    Ονοματεπώνυμο:
                </td>
                <td>
                    <asp:TextBox ID="txtContactName" runat="server" MaxLength="100" Width="90%" />
                </td>
            </tr>
            <tr>
                <td class="notRequired" style="width: 30%">
                    Τηλέφωνο (κινητό):
                </td>
                <td>
                    <asp:TextBox ID="txtContactMobilePhone" runat="server" MaxLength="10" Width="20%" />
                    <asp:RegularExpressionValidator ID="revContactMobilePhone" runat="server" Display="Dynamic"
                        ControlToValidate="txtContactMobilePhone" ValidationExpression="^69[0-9]{8}$"
                        ErrorMessage="Το πεδίο 'Τηλέφωνο (κινητό)' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία"><img src="/_img/error.gif" title="Μη έγκυρος αριθμός τηλεφώνου" /></asp:RegularExpressionValidator>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" class="dv">
            <tr>
                <th colspan="2" class="header">
                    &raquo; Master Account Για
                </th>
            </tr>
            <tr>
                <th style="width: 30%">
                    Γραφείο Αρωγής:
                </th>
                <td>
                    <asp:CheckBox ID="chbxHelpdesk" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    Επιτελικές Αναφορές (με στοιχεία φοιτητών):
                </th>
                <td>
                    <asp:CheckBox ID="chbxSuperReports" runat="server" />
                </td>
            </tr>
            <tr>
                <th style="width: 30%">
                    Επιτελικές Αναφορές (χωρίς στοιχεία φοιτητών):
                </th>
                <td>
                    <asp:CheckBox ID="chbxReports" runat="server" />
                    <%--<asp:CustomValidator ID="cvAuthorization" runat="server" ErrorMessage="Ο χρήστης πρέπει να έχει πρόσβαση σε τουλάχιστον μία κατηγορία φοιτητών"
                        Display="Dynamic" OnServerValidate="cvAuthorization_ServerValidate">
                <img src="/_img/error.gif" title="Ο χρήστης πρέπει να έχει πρόσβαση σε τουλάχιστον μία κατηγορία" />
                    </asp:CustomValidator>--%>
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
