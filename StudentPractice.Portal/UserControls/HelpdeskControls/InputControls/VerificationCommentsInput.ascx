<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VerificationCommentsInput.ascx.cs" Inherits="StudentPractice.Portal.UserControls.InternshipPositionControls.InputControls.VerificationCommentsInput" %>

<table width="100%" class="dv">
    <tr>
        <th colspan="2" class="header">&raquo; Στοιχεία Εκτέλεσης Πρακτικής Άσκησης
        </th>
    </tr>
    <tr>
        <th>Θέλει να συνεχίσει να διαχειρίζεται και το Ιδρυματικό τους φοιτητές?
        </th>
        <td>
            <asp:CheckBox ID="chbxCanHandleDepartmentalStudents" runat="server" />

        </td>
    </tr>
    <tr>
        <th>Οι μέχρι σήμερα εξυπηρετούμενοι από το Ιδρυματικό φοιτητές, να μεταφερθούν στο τμηματικό?
        </th>
        <td>
            <asp:CheckBox ID="chbxTransferStudentsToDepartmentalAccount" runat="server" />
        </td>
    </tr>
</table>
