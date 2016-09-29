<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchoolInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.GenericControls.SchoolInput" %>

<tr>
    <th style="width: 180px">
        <label for="txtInstitutionName">Ίδρυμα:</label>
    </th>
    <td colspan="3">
        <asp:TextBox ID="txtInstitutionName" runat="server" Width="60%" ClientIDMode="Static" />
        <asp:PlaceHolder runat="server" ID="phSelectAcademic">
            <a href="javascript:void(0);" id="lnkSelectSchool" class="icon-btn bg-selectSchool" onclick="popUp.show('/Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');">Επιλογή Σχολής</a>
            <a href="javascript:void(0);" id="lnkRemoveSchoolSelection" class="icon-btn" onclick="return hd.removeSchoolSelection();" style="display: none;">Αφαίρεση Σχολής</a>
        </asp:PlaceHolder>
        <asp:HiddenField ID="hfSchoolCode" runat="server" />
    </td>
</tr>

<tr>
    <th style="width: 180px">
        <label for="txtSchoolName">Σχολή:</label>
    </th>
    <td>
        <asp:TextBox ID="txtSchoolName" runat="server" Width="60%" Enabled="false" ClientIDMode="Static" />

        <asp:RequiredFieldValidator runat="server" ID="rfvInstitutionName" ControlToValidate="txtInstitutionName"
            Display="Dynamic" ErrorMessage="Το πεδίο 'Ίδρυμα/Σχολή/Τμήμα' είναι υποχρεωτικό">
            <img src="/_img/error.gif" class="errortip" runat="server" title="Το πεδίο 'Ίδρυμα/Σχολή/Τμήμα' είναι υποχρεωτικό" />
        </asp:RequiredFieldValidator>
    </td>
</tr>

<tr>
    <th style="width: 180px">
        <label for="txtDepartmentName">Τμήμα:</label>
    </th>
    <td>
        <asp:TextBox ID="txtDepartmentName" runat="server" Width="60%" Enabled="false" ClientIDMode="Static" />
    </td>
</tr>
