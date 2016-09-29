<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImplementationInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls.ImplementationInput" %>

<script type="text/javascript">
    var cbxIsNameLatin;
    var greekFirstName;
    var greekLastName;
    var latinFirstName;
    var latinLastName;

    $(function () {
        cbxIsNameLatin = $('#chbxIsNameLatin');
        greekFirstName = $('#txtGreekFirstName');
        greekLastName = $('#txtGreekLastName');
        latinFirstName = $('#txtLatinFirstName');
        latinLastName = $('#txtLatinLastName');
        showGreekName(true);

        Imis.Lib.ToElUpperForNames(greekFirstName[0]);
        Imis.Lib.ToElUpperForNames(greekLastName[0]);
        Imis.Lib.ToEnUpperForNames(latinFirstName[0]);
        Imis.Lib.ToEnUpperForNames(latinLastName[0]);
    });

    function validateGreekName(s, e) {
        if (cbxIsNameLatin.attr('checked')) {
            e.IsValid = true;
        }
        else {
            if ($('#' + s.controltovalidate).val() != '') {
                e.IsValid = true;
            }
            else {
                e.IsValid = false;
            }
        }
    }

    function showGreekName(isFirstLoad) {
        var copyChecked = cbxIsNameLatin.attr('checked');
        var regExp = new RegExp('^[A-Z]+$');

        if (copyChecked) {
            $('#tbGreekName').hide();
        }
        else {
            $('#tbGreekName').show();

            if (cbxIsNameLatin.attr('disabled'))
                return;
            if (isFirstLoad)
                return;

            if (greekFirstName.val().match(regExp)) {
                greekFirstName.val('');
            }

            if (greekLastName.val().match(regExp)) {
                greekLastName.val('');
            }
        }
    }
</script>
<asp:PlaceHolder ID="phFieldChecking" runat="server" Visible="false">
    <asp:Label ID="lblFieldChecking" runat="server" Text="Ελέγξτε ότι τα στοιχεία του Ον/μου είναι σωστά και κάνετε τις απαραίτητες διορθώσεις όπου κρίνετε απαραίτητο"
        Font-Bold="true" ForeColor="Blue" />
    <br /><br />
</asp:PlaceHolder>
<table id="tbNameFromShibboleth" runat="server" width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Ονοματεπώνυμο Φοιτητή από Κατάλογο Ιδρύματος
        </th>
    </tr>
    <tr>
        <td colspan="2">
            <div class="sub-description">
                Όπως ακριβώς επιστράφηκε από τον Κατάλογο Χρηστών του Ιδρύματος
            </div>
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Ονοματεπώνυμο:
        </th>
        <td>
            <asp:Label ID="lblFullNameFromLDAP" runat="server" Font-Bold="true" ForeColor="Blue" />
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Ονοματεπώνυμο Φοιτητή με Ελληνικούς Χαρακτήρες
        </th>
    </tr>
    <tr>
        <td colspan="2">
            <div class="sub-description">
                Αν το Ον/μο περιέχει <span style="font-weight: bold; text-decoration: underline;
                    font-size: 11px">μόνο</span> λατινικούς χαρακτήρες, κάντε click στο κουτάκι
                "Ον/μο μόνο στα λατινικά"
            </div>
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Ον/μο <span style="font-weight: bold; text-decoration: underline; font-size: 11px">μόνο</span>
            στα λατινικά:
        </th>
        <td>
            <asp:CheckBox ID="chbxIsNameLatin" runat="server" onclick="showGreekName()" ClientIDMode="Static" />
        </td>
    </tr>
</table>
<table id="tbGreekName" width="100%" class="dv" style="border-top: hidden">
    <tr>
        <th style="width: 180px">
            Όνομα:
        </th>
        <td>
            <asp:TextBox ID="txtGreekFirstName" runat="server" CssClass="target t1" MaxLength="40"
                Width="45%" ClientIDMode="Static" onpaste="return false" />
            <asp:CustomValidator ID="cvGreekFirstName" runat="server" ClientValidationFunction="validateGreekName"
                ControlToValidate="txtGreekFirstName" Display="Dynamic" OnServerValidate="cvGreekName_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="Το πεδίο 'Όνομα (με ελληνικούς χαρακτήρες)' είναι υποχρεωτικό">
                Το πεδίο είναι υποχρεωτικό
            </asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revGreekFirstName" runat="server" ControlToValidate="txtGreekFirstName"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Όνομα (με ελληνικούς χαρακτήρες)' πρέπει να περιέχει τουλάχιστον 2 χαρακτήρες"
                ValidationExpression="^([\S\s]{2,40})$">
                Πρέπει να εισάγετε τουλάχιστον 2 χαρακτήρες
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Επώνυμο:
        </th>
        <td>
            <asp:TextBox ID="txtGreekLastName" runat="server" CssClass="target t2" MaxLength="40"
                Width="45%" ClientIDMode="Static" onpaste="return false" />
            <asp:CustomValidator ID="cvGreekLastName" runat="server" ClientValidationFunction="validateGreekName"
                ControlToValidate="txtGreekLastName" Display="Dynamic" OnServerValidate="cvGreekName_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="Το πεδίο 'Επώνυμο (με ελληνικούς χαρακτήρες)' είναι υποχρεωτικό">
                Το πεδίο είναι υποχρεωτικό
            </asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revGreekLastName" runat="server" ControlToValidate="txtGreekLastName"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Επώνυμο (με ελληνικούς χαρακτήρες)' πρέπει να περιέχει τουλάχιστον 2 χαρακτήρες"
                ValidationExpression="^([\S\s]{2,40})$">
                Πρέπει να εισάγετε τουλάχιστον 2 χαρακτήρες
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Ονοματεπώνυμο Φοιτητή με Λατινικούς Χαρακτήρες
        </th>
    </tr>
    <tr>
        <td colspan="2">
            <div class="sub-description">
                Για το σωστό τρόπο αναγραφής του Ον/μου με λατινικούς χαρακτήρες διαβάστε <a target="_blank"
                    style="text-decoration: underline; font-size: 11px" href="http://www.passport.gov.gr/elot-743.html">
                    πληροφορίες για την μεταγραφή χαρακτήρων κατά ΕΛΟΤ 743</a>
            </div>
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Όνομα:
        </th>
        <td>
            <asp:TextBox ID="txtLatinFirstName" runat="server" CssClass="source t1" MaxLength="40"
                Width="45%" ClientIDMode="Static" onpaste="return false" />
            <asp:RequiredFieldValidator ID="rfvLatinFirstName" Display="Dynamic" runat="server"
                ControlToValidate="txtLatinFirstName" ErrorMessage="Το πεδίο 'Όνομα (με λατινικούς χαρακτήρες)' είναι υποχρεωτικό">
                Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revLatinFirstName" runat="server" ControlToValidate="txtLatinFirstName"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Όνομα (με λατινικούς χαρακτήρες)' πρέπει να περιέχει τουλάχιστον 2 χαρακτήρες"
                ValidationExpression="^([\S\s]{2,40})$">
                Πρέπει να εισάγετε τουλάχιστον 2 χαρακτήρες
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            Επώνυμο:
        </th>
        <td>
            <asp:TextBox ID="txtLatinLastName" runat="server" CssClass="source t2" MaxLength="40"
                Width="45%" ClientIDMode="Static" onpaste="return false" />
            <asp:RequiredFieldValidator ID="rfvLatinLastName" Display="Dynamic" runat="server"
                ControlToValidate="txtLatinLastName" ErrorMessage="Το πεδίο 'Επώνυμο (με λατινικούς χαρακτήρες)' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="revLatinLastName" runat="server" ControlToValidate="txtLatinLastName"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Επώνυμο (με λατινικούς χαρακτήρες)' πρέπει να περιέχει τουλάχιστον 2 χαρακτήρες"
                ValidationExpression="^([\S\s]{2,40})$">Πρέπει να εισάγετε τουλάχιστον 2 χαρακτήρες</asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Εκτέλεσης Πρακτικής Άσκησης
        </th>
    </tr>
    <tr id="rowPreAssignedAcademic" runat="server" visible="false">
        <th style="width: 160px">Τμήμα:
        </th>
        <td>
            <asp:DropDownList ID="ddlPreAssignAcademics" runat="server" OnInit="ddlPreAssignAcademics_Init" />
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal ID="ltrStartDate" runat="server" Text="Προβλεπόμενη ημ/νία έναρξης:" />
        </th>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" Width="15%" />
            <asp:HyperLink ID="lnkSelectStartDate" runat="server" NavigateUrl="#">
                    <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
                </asp:HyperLink>

            <act:CalendarExtender ID="ceSelectStartDate" runat="server" PopupButtonID="lnkSelectStartDate" TargetControlID="txtStartDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Προβλεπόμενη ημ/νία έναρξης' είναι υποχρεωτικό">
                Το πεδίο είναι υποχρεωτικό
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvStartDate" runat="server" Display="Dynamic" Type="Date"
                Operator="DataTypeCheck" ControlToValidate="txtStartDate" ErrorMessage="Το πεδίο 'Προβλεπόμενη ημ/νία έναρξης' πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)">
                    Η ημ/νία πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)
            </asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 180px">
            <asp:Literal ID="ltrEndDate" runat="server" Text="Προβλεπόμενη ημ/νία λήξης:" />
        </th>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" Width="15%" />
            <asp:HyperLink ID="lnkSelectEndDate" runat="server" NavigateUrl="#">
                    <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
                </asp:HyperLink>

            <act:CalendarExtender ID="ceSelectEndDate" runat="server" PopupButtonID="lnkSelectEndDate" TargetControlID="txtEndDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                Display="Dynamic" ErrorMessage="Το πεδίο 'Προβλεπόμενη ημ/νία λήξης' είναι υποχρεωτικό">Το πεδίο είναι υποχρεωτικό
            </asp:RequiredFieldValidator>

            <asp:CompareValidator ID="cvEndDate" runat="server" Display="Dynamic" Type="Date"
                Operator="DataTypeCheck" ControlToValidate="txtEndDate" ErrorMessage="Το πεδίο 'Προβλεπόμενη ημ/νία λήξης' πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)">
                    Η ημ/νία πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)
            </asp:CompareValidator>

            <asp:CustomValidator ID="cvMinEndDate" runat="server" ControlToValidate="txtEndDate"
                Display="Dynamic" OnServerValidate="cvMinEndDate_ServerValidate" ValidateEmptyText="true"
                ErrorMessage="Η 'Προβλεπόμενη ημ/νία λήξης' πρέπει να είναι μεταγενέστερη της προβλεπόμενης ημ/νίας έναρξης">
               Πρέπει να εισάγετε ημ/νία μεταγενέστερη της προβλεπόμενης ημ/νίας έναρξης
            </asp:CustomValidator>

            <asp:CustomValidator ID="cvMaxEndDate" runat="server" ControlToValidate="txtEndDate"
                Display="Dynamic" OnServerValidate="cvMaxEndDate_ServerValidate" ValidateEmptyText="true"
                ErrorMessage="Η 'Hμ/νία λήξης' πρέπει να είναι προγενέστερη της σημερινής">
               Πρέπει να εισάγετε ημ/νία προγενέστερη της σημερινής
            </asp:CustomValidator>
        </td>
    </tr>
    <%--<tr>
        <th style="width: 180px">
            <asp:Literal ID="ltrFundingType" runat="server" Text="Τρόπος Χρηματοδότησης:" />
        </th>
        <td>
            <asp:DropDownList ID="ddlFundingType" runat="server" ClientIDMode="Static" OnInit="ddlFundingType_Init" Width="61%" />
        </td>
    </tr>--%>
    <tr id="rowCompletionComments" visible="false" runat="server">
        <th style="width: 160px">Παρατηρήσεις ολοκλήρωσης:
        </th>
        <td>
            <asp:TextBox ID="txtCompletionComments" runat="server" TextMode="MultiLine" Rows="9"
                Width="100%" />
        </td>
    </tr>
</table>
