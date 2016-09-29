<%@ Control Language="C#" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Admin.UserControls.SubSystemInput"
    CodeBehind="SubSystemInput.ascx.cs" %>

<script type="text/javascript">
    function validateRole(s, e) {
        if (s.GetSelectedItem() == null) {
            e.isValid = false;
            e.errorText = 'Το πεδίο είναι υποχρεωτικό';
        }
    }

    function validateReporterType(s, e) {
        if (s.GetSelectedItem() == null) {
            e.isValid = false;
            e.errorText = 'Πρέπει να επιλέξετε τουλάχιστον ένα τύπο αναφέροντα';
        }
    }
</script>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Υποσυστήματος
        </th>
    </tr>
    <tr>
        <th style="width: 70px">
            Όνομα:
        </th>
        <td>
            <dx:ASPxTextBox ID="txtName" runat="server" Width="460px">
                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Το πεδίο είναι υποχρεωτικό'"
                    ErrorDisplayMode="ImageWithTooltip" Display="Static" />
            </dx:ASPxTextBox>
        </td>
    </tr>
    <tr>
        <th style="width: 70px">
            Ρόλος:
        </th>
        <td>
            <dx:ASPxComboBox ID="cbxRole" runat="server" OnInit="cbxRole_Init" Width="460px">
                <ClientSideEvents Validation="validateRole" />
                <ValidationSettings Display="Static" ErrorDisplayMode="ImageWithTooltip" />
            </dx:ASPxComboBox>
        </td>
    </tr>
    <tr>
        <th style="width: 70px">
            Κατηγορίες Αναφερόντων:
        </th>
        <td>
            <dx:ASPxListBox ID="cbxReporterType" runat="server" SelectionMode="CheckColumn"
                OnInit="cbxReporterType_Init" Width="460px" Style="border: none;">
                <ClientSideEvents Validation="validateReporterType" />
                <ValidationSettings Display="Static" ErrorDisplayMode="ImageWithTooltip" />
            </dx:ASPxListBox>
        </td>
    </tr>
</table>
