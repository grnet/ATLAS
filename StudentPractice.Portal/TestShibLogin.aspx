<%@ Page Title="" Language="C#" MasterPageFile="~/Portal.Master" AutoEventWireup="true"
    CodeBehind="TestShibLogin.aspx.cs" Inherits="StudentPractice.Portal.TestShibLogin" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <asp:PlaceHolder ID="phPilotSite" runat="server" Visible="false">
        <asp:LinkButton ID="btnCheckPreAssignedPositions" runat="server" CssClass="btn"
            Text="Έλεγχος Προδεσμευμένων Θέσεων" OnClick="btnCheckPreAssignedPositions_Click" />
        <asp:LinkButton ID="btnCheckAssignedPositions" runat="server" CssClass="btn" Text="Έλεγχος Έναρξης Διενέργειας Θέσεων"
            OnClick="btnCheckAssignedPositions_Click" />
        <asp:LinkButton ID="btnCheckBlockedPositions" runat="server" CssClass="btn" Text="Έλεγχος Μπλοκαρισμένων Θέσεων"
            OnClick="btnCheckBlockedPositions_Click" />
        <asp:LinkButton ID="btnSendNewlyPublishedPositions" runat="server" CssClass="btn" Text="Ενημέρωση ΓΠΑ για νέες θέσεις"
            OnClick="btnSendNewlyPublishedPositions_Click" />
        <br />
        <br />
        <table width="750px" class="dv">
            <tr>
                <th colspan="6" class="header">&raquo; Αλλαγή Στοιχείων Προδέσμευσης
                </th>
            </tr>
            <tr>
                <th>Κωδικός Θέσης:
                </th>
                <td>
                    <asp:TextBox ID="txtPositionID" runat="server" Width="80%" />
                    <asp:RequiredFieldValidator ID="rfvPositionID" Display="Dynamic" runat="server" ValidationGroup="vgChangePreAssignedDate"
                        ControlToValidate="txtPositionID" ErrorMessage="Το πεδίο 'Κωδικός Θέσης' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
                <th>Νέα ημ/νία:
                </th>
                <td>
                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" Width="60%" />
                    <asp:HyperLink ID="lnkSelectStartDate" runat="server" NavigateUrl="#">
                    <img runat="server" style="border: none; vertical-align: middle" src="~/_img/iconCalendar.png" />
                    </asp:HyperLink>

                    <act:CalendarExtender ID="ceSelectStartDate" runat="server" PopupButtonID="lnkSelectStartDate" TargetControlID="txtStartDate" Format="<%$ Resources:GlobalProvider, DateFormat %>" />

                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                        ValidationGroup="vgChangePreAssignedDate" Display="Dynamic" ErrorMessage="Το πεδίο 'Νέα ημ/νία' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο 'Νέα ημ/νία' είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>

                    <asp:CompareValidator ID="cvStartDate" runat="server" Display="Dynamic" Type="Date"
                        Operator="DataTypeCheck" ControlToValidate="txtStartDate" ValidationGroup="vgChangePreAssignedDate"
                        ErrorMessage="Το πεδίο 'Νέα ημ/νία' πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο 'Νέα ημ/νία' πρέπει να είναι της μορφής dd/MM/yyyy (π.χ. 02/11/2012)" />
                    </asp:CompareValidator>
                </td>
                <th>Ημέρες που υπολείπονται:
                </th>
                <td>
                    <asp:TextBox ID="txtDaysLeftAssignment" runat="server" Width="80%" />
                    <asp:RequiredFieldValidator ID="rfvDaysLeftAssignment" Display="Dynamic" runat="server"
                        ValidationGroup="vgChangePreAssignedDate" ControlToValidate="txtDaysLeftAssignment"
                        ErrorMessage="Το πεδίο 'Ημέρες που υπολείπονται' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th colspan="6">
                    <asp:LinkButton ID="btnChangePreAssignedDate" runat="server" CssClass="btn"
                        ValidationGroup="vgChangePreAssignedDate" Text="Αλλαγή Στοιχείων Προδέσμευσης"
                        OnClick="btnChangePreAssignedDate_Click" />
                </th>
            </tr>
        </table>
        <br />
        <br />
        <table width="750px" class="dv">
            <tr>
                <th colspan="4" class="header">&raquo; Αλλαγή Στοιχείων Μπλοκαρίσματος
                </th>
            </tr>
            <tr>
                <th>Κωδικός Group:
                </th>
                <td>
                    <asp:TextBox ID="txtBlockedGroupID" runat="server" Width="80%" />
                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ValidationGroup="vgChangeBlockedPositions"
                        ControlToValidate="txtBlockedGroupID" ErrorMessage="Το πεδίο 'Κωδικός Group' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
                <th>Ημέρες Μπλοκαρίσματος
                </th>
                <td>
                    <asp:TextBox ID="txtBlockedDaysLeft" runat="server" Width="80%" />
                    <asp:RequiredFieldValidator Display="Dynamic" runat="server"
                        ValidationGroup="vgChangeBlockedPositions" ControlToValidate="txtBlockedDaysLeft"
                        ErrorMessage="Το πεδίο 'Ημέρες Μπλοκαρίσματος' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th colspan="4">
                    <asp:LinkButton ID="btnChangeBlockedPositions" runat="server" CssClass="btn"
                        ValidationGroup="vgChangeBlockedPositions" Text="Αλλαγή στοιχείων Μπλοκαρίσματος"
                        OnClick="btnChangeBlockedPositions_Click" />
                </th>
            </tr>
        </table>
        <br />
        <br />
        <table width="750px" class="dv">
            <tr>
                <th colspan="2" class="header">&raquo; Υπάρχων Φοιτητής
                </th>
            </tr>
            <tr>
                <th>Αρ. Μητρώου:
                </th>
                <td>
                    <asp:TextBox ID="txtUsername" runat="server" Width="60%" />
                    <asp:RequiredFieldValidator ID="rfvUsername" Display="Static" runat="server" ValidationGroup="vgLogin"
                        ControlToValidate="txtUsername" ErrorMessage="Το πεδίο 'Username' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th colspan="2">
                    <asp:LinkButton ID="btnLogin" runat="server" CssClass="btn" ValidationGroup="vgLogin"
                        Text="Σύνδεση" OnClick="btnLogin_Click" />
                </th>
            </tr>
        </table>
        <br />
        <br />
        <table width="750px" class="dv">
            <tr>
                <th colspan="2" class="header">&raquo; Καινούργιος Φοιτητής
                </th>
            </tr>
            <tr>
                <th>Όνομα:
                </th>
                <td>
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="40" Width="60%" />
                    <asp:RequiredFieldValidator ID="rfvFirstName" Display="Static" runat="server" ValidationGroup="vgRegister"
                        ControlToValidate="txtFirstName" ErrorMessage="Το πεδίο 'Όνομα' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>Επώνυμο:
                </th>
                <td>
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="40" Width="60%" />
                    <asp:RequiredFieldValidator ID="rfvLastName" Display="Static" runat="server" ValidationGroup="vgRegister"
                        ControlToValidate="txtLastName" ErrorMessage="Το πεδίο 'Επώνυμο' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>Αρ. Μητρώου:
                </th>
                <td>
                    <asp:TextBox ID="txtStudentNumber" runat="server" MaxLength="27" Width="60%" />
                    <asp:RequiredFieldValidator ID="rfvStudentNumber" Display="Static" runat="server"
                        ValidationGroup="vgRegister" ControlToValidate="txtStudentNumber" ErrorMessage="Το πεδίο 'Αρ. Μητρώου' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>Ίδρυμα:
                </th>
                <td colspan="3">
                    <asp:TextBox ID="txtInstitutionName" runat="server" Width="60%" ClientIDMode="Static" />
                    <asp:PlaceHolder runat="server" ID="phSelectAcademic">
                        <a href="javascript:void(0);" id="lnkSelectSchool" runat="server" clientidmode="Static" class="icon-btn bg-selectSchool" onclick="popUp.show('/Common/SchoolSelectPopup.aspx', 'Επιλογή Σχολής');">Επιλογή Σχολής
                        </a>
                        <a href="javascript:void(0);" id="lnkRemoveSchoolSelection" class="icon-btn bg-delete" onclick="return hd.removeSchoolSelection();" style="display: none;">Αφαίρεση Σχολής
                        </a>
                    </asp:PlaceHolder>
                    <asp:HiddenField ID="hfSchoolCode" runat="server" />
                </td>
            </tr>
            <tr>
                <th>Σχολή:
                </th>
                <td>
                    <asp:TextBox ID="txtSchoolName" runat="server" Width="60%" Enabled="false" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="rfvInstitutionName" Display="Static" runat="server"
                        ValidationGroup="vgRegister" ControlToValidate="txtInstitutionName" ErrorMessage="Το πεδίο 'Ίδρυμα/Σχολή/Τμήμα' είναι υποχρεωτικό">
                    <img src="/_img/error.gif" class="errortip" title="Το πεδίο είναι υποχρεωτικό" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th>Τμήμα:
                </th>
                <td>
                    <asp:TextBox ID="txtDepartmentName" runat="server" Width="60%" Enabled="false" ClientIDMode="Static" />
                </td>
            </tr>
            <tr>
                <th colspan="2">
                    <asp:LinkButton ID="btnRegister" runat="server" CssClass="btn" ValidationGroup="vgRegister" Text="Εγγραφή" OnClick="btnRegister_Click" />
                </th>
            </tr>
        </table>
        <br />
        <br />
        <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" />
        <dx:ASPxPopupControl ID="dxpcPopup" runat="server" ClientInstanceName="devExPopup"
            Width="900" Height="620" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="true" CloseAction="CloseButton">
        </dx:ASPxPopupControl>
    </asp:PlaceHolder>
</asp:Content>
