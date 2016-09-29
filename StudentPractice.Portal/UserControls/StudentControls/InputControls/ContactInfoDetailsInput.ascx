<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactInfoDetailsInput.ascx.cs"
    Inherits="StudentPractice.Portal.UserControls.StudentControls.InputControls.ContactInfoDetailsInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Επικοινωνίας
        </th>
    </tr>
    <tr>
        <th style="width: 20%">E-mail:</th>
        <td>
            <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" Width="50%" />

            <asp:Label ID="lblEmailVerified" runat="server" ForeColor="Blue" />

            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                Display="Dynamic" ErrorMessage="Το πεδίο 'E-mail' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'E-mail' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revEmail" Display="Dynamic" ControlToValidate="txtEmail"
                runat="server" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το E-mail δεν είναι έγκυρο">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το E-mail δεν είναι έγκυρο" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 20%">Κινητό:</th>
        <td>
            <asp:TextBox ID="txtMobilePhone" runat="server" Columns="30" MaxLength="10" Width="20%" />

            <asp:Label ID="lblMobilePhoneVerified" runat="server" ForeColor="Blue" Visible="false" />

            <asp:RequiredFieldValidator ID="rfvMobilePhone" Display="Dynamic" runat="server"
                ControlToValidate="txtMobilePhone" ErrorMessage="Το πεδίο 'Κινητό' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Κινητό' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revMobilePhone" runat="server" Display="Dynamic"
                ControlToValidate="txtMobilePhone" ValidationExpression="^69[0-9]{8}$" ErrorMessage="Το πεδίο 'Κινητό' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Κινητό' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
