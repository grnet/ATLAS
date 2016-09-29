<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProviderView.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipProviderControls.ViewControls.ProviderInput" %>

<script type="text/javascript">
    $(function () {
        var altContactTable = $('#tbAlternateContact');
        var altName = $('#lblAlternateContactName');
        var altPhone = $('#lblAlternateContactPhone');
        var altMobilePhone = $('#lblAlternateContactMobilePhone');
        var altEmail = $('#lblAlternateContactEmail');

        if (altName.val() != '' && altPhone.val() != '' && altMobilePhone.val() != '' && altEmail.val() != '') {
            altContactTable.show();
        }
        else {
            altContactTable.hide();
        }
    });
</script>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Φορέα Υποδοχής Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th>Είδος φορέα:
        </th>
        <td>
            <asp:Label ID="lblProviderType" runat="server" Width="60%"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Πεδίο δραστηριότητας:
        </th>
        <td>
            <asp:Label ID="lblPrimaryActivity" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Επωνυμία:
        </th>
        <td>
            <asp:Label ID="lblName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Διακριτικός Τίτλος:
        </th>
        <td>
            <asp:Label ID="lblTradeName" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Α.Φ.Μ.:
        </th>
        <td>
            <asp:Label ID="lblAFM" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Δ.Ο.Υ.:
        </th>
        <td>
            <asp:Label ID="lblDOY" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (σταθερό):
        </th>
        <td>
            <asp:Label ID="lblProviderPhone" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Fax:
        </th>
        <td>
            <asp:Label ID="lblProviderFax" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>E-mail:
        </th>
        <td>
            <asp:Label ID="lblProviderEmail" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Ιστοσελίδα:
        </th>
        <td>
            <asp:HyperLink ID="hlkProviderURL" Target="_blank" runat="server"></asp:HyperLink>
        </td>
    </tr>
    <tr>
        <th>Αριθμός απασχολούμενων:
        </th>
        <td>
            <asp:Label ID="lblProviderNumberOfEmployees" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<br />


<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Διεύθυνσης Έδρας Φορέα Υποδοχής Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th>Οδός - Αριθμός:
        </th>
        <td>
            <asp:Label ID="lblAddress" runat="server" Width="61%"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τ.Κ.:
        </th>
        <td>
            <asp:Label ID="lblZipCode" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Χώρα:
        </th>
        <td>
            <asp:Label ID="lblCountry" runat="server"></asp:Label>
        </td>
    </tr>
    <tr ID="trPrefecture" runat="server">
        <th>
            <asp:Literal ID="ltrPrefecture" runat="server"></asp:Literal>
        </th>
        <td>
            <asp:Label ID="lblPrefecture" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>
            <asp:Literal ID="ltrCity" runat="server"></asp:Literal>
        </th>
        <td>
            <asp:Label ID="lblCity" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<br />


<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Νομίμου Εκπροσώπου του Φορέα Υποδοχής Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th>Ονοματεπώνυμο:
        </th>
        <td>
            <asp:Label ID="lblLegalPersonName" runat="server" Width="60%"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο:
        </th>
        <td>
            <asp:Label ID="lblLegalPersonPhone" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>E-mail:
        </th>
        <td>
            <asp:Label ID="lblLegalPersonEmail" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="idType" runat="server">
        <th>Τύπος Εγγράφου Πιστοποίησης:
        </th>
        <td>
            <asp:Label ID="lblIdType" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="idNumber" runat="server">
        <th>
            <asp:Literal ID="ltrIdNumber" runat="server"></asp:Literal>
        </th>
        <td>
            <asp:Label ID="lblIdNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="idIssuer" runat="server">
        <th>Αρχή Έκδοσης:
        </th>
        <td>
            <asp:Label ID="lblIdIssuer" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="idIssueDate" runat="server">
        <th>Ημ/νία Έκδοσης:
        </th>
        <td>
            <asp:Label ID="lblIdIssueDate" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<br />


<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Υπευθύνου του Φορέα για το πρόγραμμα Άτλας
        </th>
    </tr>
    <tr>
        <th>Ονοματεπώνυμο:
        </th>
        <td>
            <asp:Label ID="lblContactName" runat="server" Width="60%"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (σταθερό):
        </th>
        <td>
            <asp:Label ID="lblContactPhone" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (κινητό):
        </th>
        <td>
            <asp:Label ID="lblContactMobilePhone" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>E-mail:
        </th>
        <td>
            <asp:Label ID="lblContactEmail" runat="server"></asp:Label>
        </td>
    </tr>
</table>
<br />


<table id="tbAlternateContact" width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Αναπληρωτή Υπευθύνου του Φορέα για το πρόγραμμα Άτλας
        </th>
    </tr>
    <tr>
        <th>Ονοματεπώνυμο:
        </th>
        <td>
            <asp:Label ID="lblAlternateContactName" runat="server" Width="60%" ClientIDMode="Static"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (σταθερό):
        </th>
        <td>
            <asp:Label ID="lblAlternateContactPhone" runat="server" Width="60%" ClientIDMode="Static"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (κινητό):
        </th>
        <td>
            <asp:Label ID="lblAlternateContactMobilePhone" runat="server" Width="60%" ClientIDMode="Static"></asp:Label>
        </td>
    </tr>
    <tr>
        <th>E-mail:
        </th>
        <td>
            <asp:Label ID="lblAlternateContactEmail" runat="server" Width="60%" ClientIDMode="Static"></asp:Label>
        </td>
    </tr>
</table>
<br />
