<%@ Control Language="C#" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.IncidentReportInput"
    CodeBehind="IncidentReportInput.ascx.cs" %>

<%@ Import Namespace="StudentPractice.BusinessModel" %>

<script type="text/javascript">
    $(function() {
        showDetails('#<%= ddlReporterType.ClientID %>');
    });

    var checkOtherDetails = false;

    function showDetails(elem) {
        var reporterType = $(elem).val();
        
        var providerDetails = $('#tbProviderDetails');
        var otherDetails = $('#tbOtherDetails');
        var institutionDetails = $('#tbInstitutionDetails');
        var academicDetails = $('#tbAcademicDetails');

        providerDetails.hide();
        otherDetails.hide();
        institutionDetails.hide();
        academicDetails.hide();

        if (reporterType == <%= (int)enReporterType.InternshipProvider %>) {
            providerDetails.show();
            
        $get('<%= txtDescription.ClientID %>').value = '';
    }
    else if (reporterType == <%= (int)enReporterType.InternshipOffice %>) {
        institutionDetails.show();
            
        $get('<%= txtDescription.ClientID %>').value = '';
    }
    else if (reporterType == <%= (int)enReporterType.Student %>) {
        academicDetails.show();
            
        $get('<%= txtDescription.ClientID %>').value = '';
    }
    else if (reporterType == <%= (int)enReporterType.FacultyMember %>) {
        institutionDetails.show();
            
        $get('<%= txtDescription.ClientID %>').value = '';
    }
    else if (reporterType == <%= (int)enReporterType.Other %>) {
        otherDetails.show();
    }
    else {
            $get('<%= txtDescription.ClientID %>').value = '';
    }
    }
</script>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Γενικά Στοιχεία
        </th>
    </tr>
    <tr>
        <th style="width: 30%">Κατηγορία αναφέροντος:
        </th>
        <td>
            <asp:DropDownList ID="ddlReporterType" runat="server" OnInit="ddlReporterType_Init"
                onchange="showDetails(this)" Width="460px" />
            <asp:RequiredFieldValidator ID="rfvReporterType" Display="Dynamic" runat="server"
                ControlToValidate="ddlReporterType" ErrorMessage="Το πεδίο 'Κατηγορία αναφέροντος' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Είδος συμβάντος:
        </th>
        <td>
            <asp:DropDownList ID="ddlIncidentType" runat="server" Width="460px" DataTextField="Name"
                DataValueField="ID" />
            <act:CascadingDropDown ID="cddIncidentType" runat="server" TargetControlID="ddlIncidentType"
                ParentControlID="ddlReporterType" Category="IncidentTypes" PromptText="-- επιλέξτε πηγή αναφοράς --"
                ServicePath="~/PortalServices/Services.asmx" ServiceMethod="GetIncidentTypes"
                LoadingText="Παρακαλω περιμένετε">
            </act:CascadingDropDown>
            <asp:RequiredFieldValidator ID="rfvIncidentType" Display="Dynamic" runat="server"
                ControlToValidate="ddlIncidentType" ErrorMessage="Το πεδίο 'Είδος συμβάντος' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Τύπος Κλήσης:
        </th>
        <td>
            <asp:DropDownList ID="ddlCallType" runat="server" OnInit="ddlCallType_Init" Width="460px" />
            <asp:RequiredFieldValidator ID="rfvCallType" Display="Dynamic" runat="server" ControlToValidate="ddlCallType"
                ErrorMessage="Το πεδίο 'Τύπος Κλήσης' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
<table id="tbProviderDetails" runat="server" clientidmode="Static" class="dv" width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Φορέα Υποδοχής Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Επωνυμία:
        </td>
        <td>
            <asp:TextBox ID="txtProviderName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Διακριτικός Τίτλος:
        </td>
        <td>
            <asp:TextBox ID="txtProviderTradeName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Α.Φ.Μ.:
        </td>
        <td>
            <asp:TextBox ID="txtProviderAFM" runat="server" Width="460px" />
            <asp:CustomValidator ID="cvProviderAFM" runat="server" ControlToValidate="txtProviderAFM"
                ErrorMessage="Το πεδίο 'Α.Φ.Μ.' δεν είναι έγκυρο" ValidateEmptyText="false" Display="Dynamic"
                ClientValidationFunction="Imis.Lib.CheckAfm" OnServerValidate="cvAFM_ServerValidate"><img src="/_img/error.gif" title="Το πεδίο 'Α.Φ.Μ.' δεν είναι έγκυρο" /></asp:CustomValidator>
        </td>
    </tr>
</table>
<table id="tbOfficeDetails" runat="server" clientidmode="Static" visible="false"
    class="dv" width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Γραφείου Πρακτικής Άσκησης
        </th>
    </tr>
</table>
<table id="tbStudentDetails" runat="server" clientidmode="Static" visible="false"
    class="dv" width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Φοιτητή
        </th>
    </tr>
</table>
<table id="tbFacultyMemberDetails" runat="server" clientidmode="Static" visible="false"
    class="dv" width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Διδακτικού Προσωπικού / Επόπτη
        </th>
    </tr>
</table>
<table id="tbOtherDetails" runat="server" clientidmode="Static" class="dv" width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Χρήστη
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Λοιπά Στοιχεία:
        </td>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" Width="460px" />
        </td>
    </tr>
</table>
<table id="tbInstitutionDetails" runat="server" clientidmode="Static" class="dv"
    style="border-top: hidden" width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Ιδρύματος
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Ίδρυμα:
        </td>
        <td>
            <asp:DropDownList ID="ddlInstitution" runat="server" Width="460px" OnInit="ddlInstitution_Init" />
        </td>
    </tr>
</table>
<table id="tbAcademicDetails" runat="server" clientidmode="Static" class="dv" style="border-top: hidden"
    width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Τμήματος
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Ίδρυμα:
        </td>
        <td>
            <asp:TextBox ID="txtInstitutionName" runat="server" Width="460px" />
            <a href="javascript:void(0);" id="lnkSelectSchool" onclick="popUp.show('../Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');">
                <img id="Img1" runat="server" align="absmiddle" src="~/_img/iconSelectSchool.png"
                    alt="Επιλογή Σχολής" /></a> <a href="javascript:void(0);" id="lnkRemoveSchoolSelection"
                        onclick="return hd.removeSchoolSelection();" style="display: none;">
                        <img id="Img2" runat="server" align="absmiddle" src="~/_img/iconRemoveSchool.png"
                            alt="Αφαίρεση Σχολής" /></a>
            <asp:HiddenField ID="hfSchoolCode" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Σχολή:
        </td>
        <td>
            <asp:TextBox ID="txtSchoolName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Τμήμα:
        </td>
        <td>
            <asp:TextBox ID="txtDepartmentName" runat="server" Width="460px" />
        </td>
    </tr>
</table>
<table id="tbRegisteredProviderDetails" runat="server" visible="false" class="dv"
    width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Φορέα Υποδοχής Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th style="width: 30%">Επωνυμία:
        </th>
        <td>
            <asp:Label ID="lblProviderName" runat="server" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Διακριτικός Τίτλος:
        </th>
        <td>
            <asp:Label ID="lblProviderTradeName" runat="server" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Α.Φ.Μ.:
        </th>
        <td>
            <asp:Label ID="lblProviderAFM" runat="server" ForeColor="Blue" />
        </td>
    </tr>
</table>
<table id="tbRegisteredStudentDetails" runat="server" visible="false" class="dv"
    width="100%">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Φοιτητή
        </th>
    </tr>
    <tr>
        <th style="width: 30%">Όν/μο:
        </th>
        <td>
            <asp:Label ID="lblStudentName" runat="server" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Αρ. Μητρώου:
        </th>
        <td>
            <asp:Label ID="lblStudentNumber" runat="server" ForeColor="Blue" />
        </td>
    </tr>
</table>
<table id="tbRegisteredUserInstitutionDetails" runat="server" visible="false" class="dv"
    width="100%">
    <tr>
        <th style="width: 30%">Ίδρυμα:
        </th>
        <td>
            <asp:Label ID="lblRegisteredUserInstitution" runat="server" ForeColor="Blue" />
        </td>
    </tr>
    <tr id="rowRegisteredUserAcademic" runat="server" visible="false">
        <th style="width: 30%">Τμήμα:
        </th>
        <td id="cellRegisteredUserSingleAcademic" runat="server" visible="false">
            <asp:Label ID="lblRegisteredUserAcademic" runat="server" ForeColor="Blue" />
        </td>
        <td id="cellRegisteredUserMultipleAcademic" runat="server" visible="false">
            <asp:Literal ID="litRegisteredUserMultipleAcademic" runat="server" />
        </td>
    </tr>
</table>
<table id="tbRegisteredUserAcademicDetails" runat="server" visible="false" class="dv"
    width="100%">
    <tr>
        <th style="width: 30%">Ίδρυμα:
        </th>
        <td>
            <asp:Label ID="lblInstitution" runat="server" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Σχολή:
        </th>
        <td>
            <asp:Label ID="lblSchool" runat="server" ForeColor="Blue" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Τμήμα:
        </th>
        <td>
            <asp:Label ID="lblDepartment" runat="server" ForeColor="Blue" />
        </td>
    </tr>
</table>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Ατόμου Επικοινωνίας
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Ον/μο:
        </td>
        <td>
            <asp:TextBox ID="txtReporterName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Τηλέφωνο:
        </td>
        <td>
            <asp:TextBox ID="txtReporterPhone" runat="server" MaxLength="10" Width="40%" />
            <asp:RegularExpressionValidator ID="valReporterPhone" runat="server" CssClass="validation"
                ControlToValidate="txtReporterPhone" Display="Dynamic" ValidationGroup="Contact"
                ValidationExpression="^(2[0-9]{9})|(69[0-9]{8})$" ErrorMessage="Πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό">
                Πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">E-mail:
        </td>
        <td>
            <asp:TextBox ID="txtReporterEmail" runat="server" Width="90%" />
            <asp:RegularExpressionValidator ID="revReporterEmail" runat="server" Display="Dynamic"
                ControlToValidate="txtReporterEmail" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το E-mail δεν είναι έγκυρο"><img src="/_img/error.gif" title="Το πεδίο δεν είναι έγκυρο" /></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <th colspan="2" class="header">&raquo; Λεπτομέρειες Συμβάντος
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">Κατάσταση συμβάντος:
        </td>
        <td>
            <asp:DropDownList ID="ddlReportStatus" runat="server" OnInit="ddlReportStatus_Init"
                Width="460px" />
        </td>
    </tr>
    <tr>
        <th style="width: 30%">Πλήρες κείμενο αναφοράς:
        </th>
        <td>
            <asp:TextBox ID="txtReportText" runat="server" TextMode="MultiLine" Rows="4" Width="460px" />
            <asp:RequiredFieldValidator ID="rfvReportText" Display="Dynamic" runat="server" ControlToValidate="txtReportText"
                ErrorMessage="Το πεδίο 'Πλήρες κείμενο αναφοράς' είναι υποχρεωτικό"><img src="/_img/error.gif" title="Το πεδίο είναι υποχρεωτικό" /></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
