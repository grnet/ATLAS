<%@ Control Language="C#" AutoEventWireup="true" Inherits="StudentPractice.Portal.Secure.Helpdesk.UserControls.ReporterInput"
    CodeBehind="ReporterInput.ascx.cs" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Γενικά Στοιχεία
        </th>
    </tr>
    <tr>
        <th style="width: 30%">
            Κατηγορία αναφέροντος:
        </th>
        <td>
            <asp:Label ID="lblReporterType" runat="server" ForeColor="Blue" />
        </td>
    </tr>
</table>
<table id="tbProviderDetails" runat="server" visible="false" class="dv" width="100%">
    <tr>
        <td class="notRequired" style="width: 30%">
            Επωνυμία:
        </td>
        <td>
            <asp:TextBox ID="txtProviderName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Διακριτικός Τίτλος:
        </td>
        <td>
            <asp:TextBox ID="txtProviderTradeName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Α.Φ.Μ.:
        </td>
        <td>
            <asp:TextBox ID="txtProviderAFM" runat="server" Width="460px" />
            <asp:CustomValidator ID="cvProviderAFM" runat="server" ControlToValidate="txtProviderAFM"
                ErrorMessage="Το πεδίο 'Α.Φ.Μ.' δεν είναι έγκυρο" ValidateEmptyText="false" Display="Dynamic"
                ClientValidationFunction="Imis.Lib.CheckAfm" OnServerValidate="cvAFM_ServerValidate"><img src="/_img/error.gif" title="Το πεδίο 'Α.Φ.Μ.' δεν είναι έγκυρο" /></asp:CustomValidator>
        </td>
    </tr>
</table>
<table id="tbInstitutionDetails" runat="server" visible="false" class="dv" width="100%">
    <tr>
        <td class="notRequired" style="width: 30%">
            Ίδρυμα:
        </td>
        <td>
            <asp:DropDownList ID="ddlInstitution" runat="server" Width="460px" OnInit="ddlInstitution_Init" />
        </td>
    </tr>
</table>
<table id="tbAcademicDetails" runat="server" visible="false" class="dv" width="100%">
    <tr>
        <td class="notRequired" style="width: 30%">
            Ίδρυμα:
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
        <td class="notRequired" style="width: 30%">
            Σχολή:
        </td>
        <td>
            <asp:TextBox ID="txtSchoolName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Τμήμα:
        </td>
        <td>
            <asp:TextBox ID="txtDepartmentName" runat="server" Width="460px" />
        </td>
    </tr>
</table>
<table id="tbOtherDetails" runat="server" visible="false" class="dv" width="100%">
    <tr>
        <td class="notRequired" style="width: 30%">
            Λοιπά Στοιχεία:
        </td>
        <td>
            <asp:TextBox ID="txtDescription" runat="server" Width="460px" />
        </td>
    </tr>
</table>
<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">
            &raquo; Στοιχεία Ατόμου Επικοινωνίας
        </th>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Ον/μο:
        </td>
        <td>
            <asp:TextBox ID="txtContactName" runat="server" Width="460px" />
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            Τηλέφωνο:
        </td>
        <td>
            <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="10" Width="20%" />
            <imis:PhoneValidator ID="valContactPhone" runat="server" ControlToValidate="txtContactPhone"
                PhoneType="FixedOrMobile" ErrorMessage="Το πεδίο 'Τηλέφωνο' πρέπει να αποτελείται από ακριβώς 10 ψηφία και να ξεκινάει από 2 αν πρόκειται για σταθερό ή από 69 αν πρόκειται για κινητό"
                Display="Dynamic"><img src="/_img/error.gif" title="Μη έγκυρος αριθμός τηλεφώνου" /></imis:PhoneValidator>
        </td>
    </tr>
    <tr>
        <td class="notRequired" style="width: 30%">
            E-mail:
        </td>
        <td>
            <asp:TextBox ID="txtContactEmail" runat="server" Width="90%" />
            <asp:RegularExpressionValidator ID="revContactEmail" runat="server" Display="Dynamic"
                ControlToValidate="txtContactEmail" ValidationExpression="^([a-zA-Z0-9_\-])+(\.([a-zA-Z0-9_\-])+)*@((\[(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5])))\.(((([0-1])?([0-9])?[0-9])|(2[0-4][0-9])|(2[0-5][0-5]))\]))|((([a-zA-Z0-9])+(([\-])+([a-zA-Z0-9])+)*\.)+([a-zA-Z])+(([\-])+([a-zA-Z0-9])+)*))$"
                ErrorMessage="Το E-mail δεν είναι έγκυρο"><img src="/_img/error.gif" title="Το πεδίο δεν είναι έγκυρο" /></asp:RegularExpressionValidator>
        </td>
    </tr>
</table>

