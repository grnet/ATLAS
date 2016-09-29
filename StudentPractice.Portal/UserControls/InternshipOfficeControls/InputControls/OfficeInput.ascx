<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfficeInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipOfficeControls.InputControls.OfficeInput" %>

<script type="text/javascript">
    var altName;
    var altPhone;
    var altMobilePhone;
    var altEmail;
    var rbtlCertifierType;
    $(function () {
        //Cache the objects for extra speed 
        altName = $('#txtAlternateContactName');
        altPhone = $('#txtAlternateContactPhone');
        altMobilePhone = $('#txtAlternateContactMobilePhone');
        altEmail = $('#txtAlternateContactEmail');
        rbtlCertifierType = $('#rbtlCertifierType input');
        rbtlCertifierType.click(updateCertifierDisplay);
        rbtlCertifierType.next().click(updateCertifierDisplay);
        updateCertifierDisplay();
    });
    function validateAlternativeGroup(s, e) {
        if (altName.val() == '' && altPhone.val() == '' && altMobilePhone.val() == '' && altEmail.val() == '') {
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

    function updateCertifierDisplay() {
        $('#trCertifierName').hide();
        rbtlCertifierType.each(function (index) {
            var btn = $(rbtlCertifierType[index]);

            if (btn.attr('checked') == "checked") {
                $('#trCertifierName').show();

                if ($(rbtlCertifierType[index]).val() == '<%= StudentPractice.BusinessModel.enCertifierType.AEIPresident.ToString("D") %>') {
                    $('#sCertifierName').html('Ονοματεπώνυμο Πρύτανη:');
                }
                else if ($(rbtlCertifierType[index]).val() == '<%= StudentPractice.BusinessModel.enCertifierType.TEIPresident.ToString("D") %>') {
                    $('#sCertifierName').html('Ονοματεπώνυμο Προέδρου Ιδρύματος:');
                }
                else if ($(rbtlCertifierType[index]).val() == '<%= StudentPractice.BusinessModel.enCertifierType.DepartmentPresident.ToString("D") %>') {
                    $('#sCertifierName').html('Ονοματεπώνυμο Προέδρου Τμήματος:');
                }
                else if ($(rbtlCertifierType[index]).val() == '<%= StudentPractice.BusinessModel.enCertifierType.DepartmentChief.ToString("D") %>') {
                    $('#sCertifierName').html('Ονοματεπώνυμο Προϊσταμένου Τμήματος:');
                }
    }
        })
}
</script>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Υπευθύνου για το Γραφείο Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th>Ίδρυμα:
        </th>
        <td colspan="3">
            <asp:DropDownList ID="ddlInstitution" runat="server" Width="61%" OnInit="ddlInstitution_Init" />

            <asp:RequiredFieldValidator ID="rfvInstitution" Display="Dynamic" runat="server" ControlToValidate="ddlInstitution"
                ErrorMessage="Το πεδίο 'Ίδρυμα' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Ίδρυμα' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr id="trDepartmentInfo" runat="server">
        <td colspan="2">
            <div class="sub-description">
                <b>Σημείωση:</b> Αν επιλέξετε ότι εκπροσωπείτε συγκεκριμένα Τμήματα, το Γραφείο
                Πρακτικής που θα δημιουργηθεί θα έχει πρόσβαση μόνο σε θέσεις Πρακτικής Άσκησης
                και Φοιτητές των Τμημάτων αυτών.
            </div>
        </td>
    </tr>
    <tr id="trOfficeType" runat="server">
        <th>Το Γραφείο Πρακτικής εκπροσωπεί:
        </th>
        <td colspan="3">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rbtlOfficeType" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal"
                            OnInit="rbtlOfficeType_Init">
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvOfficeType" Display="Dynamic" runat="server" ControlToValidate="rbtlOfficeType"
                            ErrorMessage="Το πεδίο 'Τι εκπροσωπεί το Γραφείο Πρακτικής' είναι υποχρεωτικό">
                            <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τι εκπροσωπεί το Γραφείο Πρακτικής' είναι υποχρεωτικό" />
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <th>Ονοματεπώνυμο:
        </th>
        <td>
            <asp:TextBox ID="txtContactName" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" title="<%$ Resources:OfficeInput, ContactName %>" />

            <asp:RequiredFieldValidator ID="rfvContactName" Display="Dynamic" runat="server" ControlToValidate="txtContactName"
                ErrorMessage="Το πεδίο 'Ονοματεπώνυμο Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Ονοματεπώνυμο Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (σταθερό):
        </th>
        <td>
            <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:OfficeInput, ContactPhone %>" />

            <asp:RequiredFieldValidator ID="rfvContactPhone" Display="Dynamic" runat="server" ControlToValidate="txtContactPhone"
                ErrorMessage="Το πεδίο 'Τηλέφωνο (σταθερό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (σταθερό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactPhone" runat="server" ControlToValidate="txtContactPhone"
                Display="Dynamic" ValidationExpression="^2[0-9]{9}$" ErrorMessage="Το πεδίο 'Τηλέφωνο (σταθερό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 2 και να αποτελείται από ακριβώς 10 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (σταθερό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 2 και να αποτελείται από ακριβώς 10 ψηφία" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>Τηλέφωνο (κινητό):
        </th>
        <td>
            <asp:TextBox ID="txtContactMobilePhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:OfficeInput, ContactMobilePhone %>" />

            <asp:RequiredFieldValidator ID="rfvContactMobilePhone" Display="Dynamic" runat="server"
                ControlToValidate="txtContactMobilePhone" ErrorMessage="Το πεδίο 'Τηλέφωνο (κινητό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (κινητό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactMobilePhone" runat="server" ControlToValidate="txtContactMobilePhone"
                Display="Dynamic" ValidationExpression="^69[0-9]{8}$" ErrorMessage="Το πεδίο 'Τηλέφωνο (κινητό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (κινητό) του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th>E-mail:
        </th>
        <td>
            <asp:TextBox ID="txtContactEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:OfficeInput, ContactEmail %>" />

            <asp:RequiredFieldValidator ID="rfvContactEmail" Display="Dynamic" runat="server" ControlToValidate="txtContactEmail"
                ErrorMessage="Το πεδίο 'E-mail Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'E-mail Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revContactEmail" runat="server" ControlToValidate="txtContactEmail"
                Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το E-mail του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης δεν είναι έγκυρο">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το E-mail του Υπευθύνου για το Γραφείο Πρακτικής Άσκησης δεν είναι έγκυρο" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Πιστοποιούσα Αρχή
        </th>
    </tr>
    <tr id="trCertifierInfo" runat="server">
        <td colspan="2">
            <div class="sub-description">
                <b>Προσοχή:</b> Αν εκπροσωπείτε <b>μόνο ένα</b> Τμήμα, μπορείτε να επιλέξετε ως
                πιστοποιούσα αρχή <b>Πρόεδρο Τμήματος</b> (Πανεπιστήμιο) ή <b>Προϊστάμενο Τμήματος</b>
                (ΤΕΙ). Σε αντίθετη περίπτωση θα πρέπει να επιλέξετε <b>Πρύτανη</b> (Πανεπιστήμιο)
                ή <b>Πρόεδρο Ιδρύματος</b> (ΤΕΙ).
            </div>
        </td>
    </tr>
    <tr>
        <th>Είδος Πιστοποιούσας Αρχής:
        </th>
        <td>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rbtlCertifierType" runat="server" ClientIDMode="Static" CssClass="FormatRadioButtonList"
                            RepeatDirection="Horizontal" OnInit="rbtlCertifierType_Init">
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvCertifierType" Display="Dynamic" runat="server" ControlToValidate="rbtlCertifierType"
                            ErrorMessage="Το πεδίο 'Είδος Πιστοποιούσας Αρχής' είναι υποχρεωτικό">
                            <img id="Img1" src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Είδος Πιστοποιούσας Αρχής' είναι υποχρεωτικό" />
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>



        </td>
    </tr>
    <tr id="trCertifierName">
        <th>
            <span id="sCertifierName" style="font-size: 11px">Ονοματεπώνυμο:</span>
        </th>
        <td>
            <asp:TextBox ID="txtCertifierName" runat="server" MaxLength="500" ClientIDMode="Static"
                Width="60%" CssClass="source t1" />

            <asp:RequiredFieldValidator ID="rfvCertifierName" Display="Dynamic" runat="server" ControlToValidate="txtCertifierName"
                ErrorMessage="Το πεδίο 'Ονοματεπώνυμο Πιστοποιούσας Αρχής' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Ονοματεπώνυμο Πιστοποιούσας Αρχής' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Διεύθυνσης Γραφείου Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th>
            <label for="txtAddress">
                Οδός - Αριθμός:</label>
        </th>
        <td>
            <asp:TextBox ID="txtAddress" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" />

            <asp:RequiredFieldValidator ID="rfvAddress" Display="Dynamic" runat="server" ControlToValidate="txtAddress" ErrorMessage="Το πεδίο 'Οδός - Αριθμός' είναι υποχρεωτικό">
               <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Οδός - Αριθμός' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <label for="txtZipCode">
                Τ.Κ.:</label>
        </th>
        <td>
            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" ClientIDMode="Static" Width="20%" />

            <asp:RequiredFieldValidator ID="rfvZipCode" Display="Dynamic" runat="server" ControlToValidate="txtZipCode" ErrorMessage="Το πεδίο 'Τ.Κ.' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τ.Κ.' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <asp:RegularExpressionValidator ID="revZipCode" runat="server" ControlToValidate="txtZipCode" Display="Dynamic" ValidationExpression="^\d{5}$"
                ErrorMessage="Ο Τ.Κ. πρέπει να αποτελείται από ακριβώς 5 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Ο Τ.Κ. πρέπει να αποτελείται από ακριβώς 5 ψηφία" />
            </asp:RegularExpressionValidator>

        </td>
    </tr>
    <tr>
        <th>
            <label for="ddlPrefecture">
                Περιφερειακή ενότητα:</label>
        </th>
        <td>
            <asp:DropDownList ID="ddlPrefecture" runat="server" ClientIDMode="Static" Width="61%"
                OnInit="ddlPrefecture_Init" DataTextField="Name" DataValueField="ID" />

            <asp:RequiredFieldValidator ID="rfvPrefecture" Display="Dynamic" runat="server" ControlToValidate="ddlPrefecture"
                ErrorMessage="Το πεδίο 'Νομός' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Νομός' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th>
            <label for="ddlCity">
                Καλλικρατικός Δήμος:</label>
        </th>
        <td>
            <asp:DropDownList ID="ddlCity" runat="server" ClientIDMode="Static" Width="61%" DataTextField="Name"
                DataValueField="ID" />

            <asp:RequiredFieldValidator ID="rfvCity" Display="Dynamic" runat="server" ControlToValidate="ddlCity"
                ErrorMessage="Το πεδίο 'Πόλη' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Πόλη' είναι υποχρεωτικό" />
            </asp:RequiredFieldValidator>

            <act:CascadingDropDown ID="cddCity" runat="server" TargetControlID="ddlCity"
                ParentControlID="ddlPrefecture" Category="Cities" PromptText="-- επιλέξτε καλλικρατικό δήμο --"
                ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetCities" LoadingText="Παρακαλω περιμένετε">
            </act:CascadingDropDown>
        </td>
    </tr>
</table>
<br />
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <td class="notRequired">
            <label for="txtAlternateContactName">
                Ονοματεπώνυμο:</label>
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactName" runat="server" MaxLength="100" ClientIDMode="Static" Width="60%" title="<%$ Resources:OfficeInput, AlternateContactName %>" />

            <asp:CustomValidator ID="cvAlternateContactName" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactName" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="Το πεδίο 'Ονοματεπώνυμο Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Ονοματεπώνυμο Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:CustomValidator>

        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <label for="txtAlternateContactPhone">
                Τηλέφωνο (σταθερό):</label>
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactPhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:OfficeInput, AlternateContactPhone %>" />

            <asp:CustomValidator ID="cvAlternateContactPhone" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactPhone" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="Το πεδίο 'Τηλέφωνο (σταθερό) του Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (σταθερό) του Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revAlternateContactPhone" runat="server" ControlToValidate="txtAlternateContactPhone"
                Display="Dynamic" ValidationExpression="^2[0-9]{9}$" ErrorMessage="Το πεδίο 'Τηλέφωνο (σταθερό) του Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 2 και να αποτελείται από ακριβώς 10 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (σταθερό) του Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 2 και να αποτελείται από ακριβώς 10 ψηφία" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <label for="txtAlternateContactMobilePhone">
                Τηλέφωνο (κινητό):</label>
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactMobilePhone" runat="server" MaxLength="10" ClientIDMode="Static" Width="20%" title="<%$ Resources:OfficeInput, AlternateContactMobilePhone %>" />

            <asp:CustomValidator ID="cvAlternateContactMobilePhone" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactMobilePhone" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="Το πεδίο 'Τηλέφωνο (κινητό) του Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (κινητό) του Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revAlternateContactMobilePhone" runat="server"
                ControlToValidate="txtAlternateContactMobilePhone" Display="Dynamic" ValidationExpression="^69[0-9]{8}$"
                ErrorMessage="Το πεδίο 'Τηλέφωνο (κινητό) του Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Τηλέφωνο (κινητό) του Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' πρέπει να ξεκινάει από 69 και να αποτελείται από ακριβώς 10 ψηφία" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired">
            <label for="txtAlternateContactEmail">
                E-mail:</label>
        </td>
        <td>
            <asp:TextBox ID="txtAlternateContactEmail" runat="server" MaxLength="256" ClientIDMode="Static" Width="60%" title="<%$ Resources:OfficeInput, AlternateContactEmail %>" />

            <asp:CustomValidator ID="cvAlternateContactEmail" runat="server" ClientValidationFunction="validateAlternativeGroup"
                ControlToValidate="txtAlternateContactEmail" Display="Dynamic" OnServerValidate="cvAlternativeGroup_ServerValidate"
                ValidateEmptyText="true" ErrorMessage="Το πεδίο 'E-mail του Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης' είναι υποχρεωτικό">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'E-mail του Αναπληρωτή Υπευθύνου Γραφείου Πρακτικής Άσκησης' είναι υποχρεωτικό" />
            </asp:CustomValidator>

            <asp:RegularExpressionValidator ID="revAlternateContactEmail" runat="server" ControlToValidate="txtAlternateContactEmail"
                Display="Dynamic" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το πεδίο 'E-mail του Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' δεν είναι έγκυρο">
                <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'E-mail του Αναπληρωτή Υπευθύνου για το Γραφείο Πρακτικής Άσκησης' δεν είναι έγκυρο" />
            </asp:RegularExpressionValidator>
        </td>
    </tr>
</table>
